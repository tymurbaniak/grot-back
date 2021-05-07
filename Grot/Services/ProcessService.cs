using Grot.Hubs;
using Grot.ViewModels;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using UserManagement.Interfaces;

namespace Grot.Services
{
    public interface IProcessService
    {
        public ProcessStartedResponse RequestProcess(List<InputParameterValue> parameterValues, string imageDataString, IUser user);
    }

    public class ProcessService : IProcessService
    {
        private const string inputSettingsName = "input.txt";
        private const string inputImageName = "input.png";
        private readonly IHubContext<GrotHub> grotHub;

        public ProcessService(IHubContext<GrotHub> grotHub)
        {
            this.grotHub = grotHub;
        }

        public ProcessStartedResponse RequestProcess(List<InputParameterValue> parameterValues, string imageDataString, IUser user)
        {
            string projectName = parameterValues.Single(p => p.Name.Equals("project")).Values[0];
            DirectoryInfo projectDir = CreateProjectDirectory(user, projectName);
            CreateParametersInputFile(parameterValues, projectDir);
            CreateImage(imageDataString, projectDir);
            DirectoryInfo outputDir = CreateOutputDirectory(projectDir);
            return ExecuteGrot(projectDir, outputDir, user, projectName);
        }

        private DirectoryInfo CreateOutputDirectory(DirectoryInfo projectDir)
        {
            string outputPath = $"{projectDir.FullName}{Path.DirectorySeparatorChar}output";
            return Directory.CreateDirectory(outputPath);
        }

        private ProcessStartedResponse ExecuteGrot(DirectoryInfo projectDir, DirectoryInfo outputDir, IUser user, string projectName)
        {            
            char sep = Path.DirectorySeparatorChar;
            string arguments = $"{sep}app{sep}script{sep}grot{sep}run.py -i {projectDir.FullName} -o {outputDir.FullName}";
            var process = Process.Start("python3", arguments);
            process.EnableRaisingEvents = true;
            process.Exited += new EventHandler(async (object sender, EventArgs e) =>
            {
                await this.grotHub.Clients.User(user.Name)
                    .SendCoreAsync("processingDoneReceived", new string[] { $"Processing done for: {projectName}" });
            });

            var response = new ProcessStartedResponse
            {
                Started = true,
                DateTime = process.StartTime.ToString(),
                Id = process.Id.ToString()
            };

            return response;
        }

        private void CreateImage(string imageDataString, DirectoryInfo projectDir)
        {
            var imageParts = imageDataString.Split(',');
            if (imageParts.Length == 2)
            {
                byte[] imageContent = Convert.FromBase64String(imageParts[1]);
                char separator = Path.DirectorySeparatorChar;
                string imagePath = $"{projectDir.FullName}{separator}{inputImageName}";
                using FileStream fs = File.Create(imagePath);
                fs.Write(imageContent, 0, imageContent.Length);
            }

        }

        private void CreateParametersInputFile(List<InputParameterValue> parameterValues, DirectoryInfo projectDir)
        {
            var paramFileCreator = new InputParametersCreator();
            string inputFileContentString = paramFileCreator.CreateInputParametersString(parameterValues);
            char separator = Path.DirectorySeparatorChar;
            string inputFilePath = $"{projectDir.FullName}{separator}{inputSettingsName}";

            using FileStream fs = File.Create(inputFilePath);
            byte[] inputContent = Encoding.UTF8.GetBytes(inputFileContentString);
            fs.Write(inputContent, 0, inputContent.Length);
        }

        private DirectoryInfo CreateProjectDirectory(IUser user, string projectName)
        {
            string current = Directory.GetCurrentDirectory();
            char separator = Path.DirectorySeparatorChar;
            string projectsPath = $"{current}{separator}projects";

            if (!Directory.Exists(projectsPath))
            {
                Directory.CreateDirectory(projectsPath);
            }

            string userPath = $"{projectsPath}{separator}{user.Name}";

            if (!Directory.Exists(userPath))
            {
                Directory.CreateDirectory(userPath);
            }

            string newProjectName = projectName;
            string newProjectDirectory = $"{userPath}{separator}{newProjectName}";
            return Directory.CreateDirectory(newProjectDirectory);
        }
    }
}
