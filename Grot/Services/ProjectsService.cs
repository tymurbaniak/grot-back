using Grot.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Grot.Services
{
    public interface IProjectsService
    {
        List<ProjectContent> GetProjectsList(string userName);
        ProjectContent GetProject(string projectName, string userName);
    }

    public class ProjectsService : IProjectsService
    {
        readonly char separator = Path.DirectorySeparatorChar;

        public List<ProjectContent> GetProjectsList(string userName)
        {
            string current = Directory.GetCurrentDirectory();            
            string projectsPath = $"{current}{separator}projects";
            string userPath = $"{projectsPath}{separator}{userName}";            

            var projects = new List<ProjectContent>();

            if (!Directory.Exists(userPath))
            {
                return projects;
            }

            var projectDirectories = Directory.EnumerateDirectories(userPath);

            foreach(var projectDirectory in projectDirectories)
            {
                string projectName = projectDirectory.Split(separator).Last();
                var projectContent = GetProject(projectName, userName);
                projects.Add(projectContent);
            }

            return projects;
        }

        public ProjectContent GetProject(string projectName, string userName)
        {
            string current = Directory.GetCurrentDirectory();
            string projectsPath = $"{current}{separator}projects";
            string userPath = $"{projectsPath}{separator}{userName}";
            string projectDirectory = $"{userPath}{separator}{projectName}";

            var projectContent = new ProjectContent();
            projectContent.Name = projectName;
            var outputDir = $"{projectDirectory}{separator}output";
            var outputImages = Directory.EnumerateFiles(outputDir);
            projectContent.OutputImageUrls = new List<string>();

            foreach (var outputImage in outputImages)
            {
                string imageName = outputImage.Split(separator).Last();
                string url = $"projects/{userName}/{projectContent.Name}/output/{imageName}";
                projectContent.OutputImageUrls.Add(url);
            }

            projectContent.InputImageUrl = $"projects/{userName}/{projectContent.Name}/input.png";

            return projectContent;
        }
    }
}
