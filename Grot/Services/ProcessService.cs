using Grot.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UserManagement.Interfaces;
using System.Drawing;

namespace Grot.Services
{
    public interface IProcessService
    {
        public void RequestProcess(List<InputParameterValue> parameterValues, string imageDataString, IUser user);
    }

    public class ProcessService : IProcessService
    {
        private const string inputSettingsName = "input.txt";
        private const string inputImageName = "inputpng";

        public void RequestProcess(List<InputParameterValue> parameterValues, string imageDataString, IUser user)
        {
            DirectoryInfo projectDir = CreateProjectDirectory(user);
            CreateParametersInputFile(parameterValues, projectDir);
            CreateImage(imageDataString, projectDir);
            ExecuteGrot();
        }

        private void ExecuteGrot()
        {
            string grotCommand = $"python3 ";
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

        private DirectoryInfo CreateProjectDirectory(IUser user)
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

            string newProjectName = Guid.NewGuid().ToString();
            string newProjectDirectory = $"{userPath}{separator}{newProjectName}";
            return Directory.CreateDirectory(newProjectDirectory);
        }
    }
}
