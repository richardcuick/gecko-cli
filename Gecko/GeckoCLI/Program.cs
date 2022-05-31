// See https://aka.ms/new-console-template for more information
using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using LibGit2Sharp;
using LibGit2Sharp.Handlers;
using YamlDotNet.Serialization.NamingConventions;

namespace Gecko
{
    class Configuration
    {
        public string DatabaseConnectionString { get; set; } = string.Empty;
        public string UploadFolder { get; set; } = string.Empty;
        public List<string> ApprovedFileTypes { get; set; } = new List<string>();
    }

    public class AzureUrlInfo
    {
        public string Domain { get; set; } = string.Empty;
        public string Organization { get; set; } = string.Empty;
        public string Project { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Repository { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;

        public string RepositoryUrl
        {
            get
            {
                return $"https://{Domain}/{Organization}/{Project}/{Type}/{Repository}";
            }
        }

        public string FileUrl
        {
            get
            {
                return $"https://{Domain}/{Organization}/{Project}/{Type}/{Repository}?path={Path}";
            }
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {

            Console.WriteLine("Click any key to exit...");
            Console.ReadLine();
        }

        public static void GetUrlInfo()
        {
            string url = "https://dev.azure.com/accenturecio01/SustainabilityHub_110556/_git/tsm-process-report-service";
            Regex rex = new Regex("https://(?<domain>[^/]+)/(?<organization>[^/]+)/(?<project>[^/]+)/(?<type>[^/]+)/(?<repo>[^/?]+)(\\?path=(?<path>.+))?");

            Match match = rex.Match(url);

            AzureUrlInfo info = new AzureUrlInfo()
            {

                Domain = match.Groups["domain"].Value,
                Organization = match.Groups["organization"].Value,
                Project = match.Groups["project"].Value,
                Type = match.Groups["type"].Value,
                Repository = match.Groups["repo"].Value,
                Path = match.Groups["path"].Value,
            };

            Console.WriteLine(info.Domain);
            Console.WriteLine(info.Organization);
            Console.WriteLine(info.Project);
            Console.WriteLine(info.Type);
            Console.WriteLine(info.Repository);
            Console.WriteLine(info.Path);
        }

        public static void ReadYaml()
        {
            var deserializer = new YamlDotNet.Serialization.DeserializerBuilder()
   .WithNamingConvention(CamelCaseNamingConvention.Instance)
   .Build();
            var myConfig = deserializer.Deserialize<Configuration>(File.ReadAllText("Configuration.yaml"));

        }

        public static void RunCommand()
        {
            var psiNpmRunDist = new ProcessStartInfo
            {
                FileName = "cmd",
                RedirectStandardInput = true,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                WorkingDirectory = "."
            };
            var pNpmRunDist = Process.Start(psiNpmRunDist);
            if (pNpmRunDist != null)
            {
                pNpmRunDist.StandardInput.WriteLine("npm run dist & exit");
                string output = pNpmRunDist.StandardOutput.ReadToEnd();
                string error = pNpmRunDist.StandardError.ReadToEnd();

                Console.WriteLine("---");
                Console.WriteLine(error);
                Console.WriteLine("---");
                Console.WriteLine(error);
                pNpmRunDist.WaitForExit();
            }
        }

        public static void CloneRepo()
        {
            var options = new CloneOptions()
            {
                CredentialsProvider = (_url, _user, _cred) => new UsernamePasswordCredentials
                {
                    Username = "b3ydicwhutxk3rfj7elabrqjeunvlvh4iphuxdh7dgs3zxehyw2q",
                    Password = string.Empty
                }
            };

            Repository.Clone("https://accenturecio01@dev.azure.com/accenturecio01/SustainabilityHub_110556/_git/tsm-process-report-service", "TestA", options);


        }
    }
}
