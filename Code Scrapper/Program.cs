using System;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using Octokit;

class Program
{
    static async Task Main(string[] args)
    {
        var config = appsettingbuilder();
        var githubToken = config.GetValue(typeof(String), "GitHubPAT").ToString();
        var client = new GitHubClient(new Octokit.ProductHeaderValue("Learning"))
        {
            Credentials = new Credentials(githubToken)
        };
        

        Console.WriteLine("this is PAT " + githubToken);
        Console.ReadKey();
    }

    static async Task SearchGitHub(GitHubClient client)
    {
        var searchRequest = new SearchCodeRequest("[BeforeTestRun] [OneTimeSetUp] public static void trial()")
        {
            Language = Language.CSharp,

            //Repos = new RepositoryCollection { "repository-owner/repository-name" }
        };
        var searchResult = await client.Search.SearchCode(searchRequest);

        foreach (var result in searchResult.Items)
        {
            Console.WriteLine($"{result.Repository.FullName} : {result.Path}");
        }
        }
    static IConfigurationRoot appsettingbuilder()
    {
        var builder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        IConfigurationRoot configuration = builder.Build();
        return configuration;

    }

}