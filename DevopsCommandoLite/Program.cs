using DevopsCommandoLite.Terminal;
using DevopsCommandoLite.Terminal.Commands;
using DevopsCommandoLite.Terminal.Services;
using DevopsCommandoLiteLite.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DevopsCommandoLiteLite
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            var configuration = builder.Build();

            var settings = new AppSettings();            
            configuration.Bind(settings);

            Settings.Token = settings.ConnectionStrings.PersonalAccessToken;
            Settings.InstanceUrl = settings.ConnectionStrings.InstanceUrl;

            if (string.IsNullOrWhiteSpace(Settings.Token))
            {
                Console.WriteLine("Personal access token is empty");
                return;
            }

            var repos = settings.Repos.Where(p => p.PRs != null && p.PRs.Count > 0).ToList();
            if (!repos.Any())
            {
                Console.WriteLine("Repos and Pull request ids are not set");
                return;
            }

            Console.WriteLine("");
            var reposSummary = "";

            var workItems = new List<WorkItem>();

            foreach (var r in repos)
            {
                Console.Write($"Looking in {r.Repo}");
                var wi = await GetWorkItems(r.Repo, r.PRs);                
                Console.WriteLine($" - Found {wi.Count} work items");
                workItems.AddRange(wi);

                reposSummary += $"{r.Repo}({wi.Count}), ";
            }
            Console.WriteLine("");

            foreach (var createdBy in workItems.Select(p => p.Fields.CreatedBy.DisplayName).Distinct())
            {
                Console.WriteLine($"## Work items created by @<{createdBy}> :");

                foreach (var wiId in workItems
                    .Where(p => p.Fields.CreatedBy.DisplayName == createdBy)
                    .OrderBy(p=>p.Id)
                    .Select(p=>p.Id)
                    .Distinct())
                {
                    Console.WriteLine($"- #{wiId}");
                }
            }

            Console.WriteLine("");
            Console.WriteLine("Repos to deploy: " + reposSummary);
            Console.ReadLine();
        }

        static async Task<IList<WorkItem>> GetWorkItems(string repoId, IEnumerable<string> prIds)
        {
            var prService = new PullRequestService(new ApiService());
            
            var requests = prIds.Select(prId => new { Task = prService.GetWorkItemList(repoId, prId), Id = prId });
            await Task.WhenAll(requests.Select(p => p.Task));

            var workItemService = new WorkItemService();
            var workItems = new List<WorkItem>();

            var workItemIds = new List<string>();
            foreach (var request in requests)
            {
                var result = await request.Task;

                if (!result.IsSuccess)
                {
                    throw new Exception($"Request for PR {request.Id} failed: {result.ErrorMessage}");   
                }
                workItemIds.AddRange(result.Data.Value.Select(p => p.Id).ToList());
            }

            var workItemResponse = await workItemService.GetWorkItems(workItemIds, "System.Title,System.WorkItemType,System.CreatedBy");
            if (!workItemResponse.IsSuccess)
            { 
                throw new Exception($"Request for WorkItemsfailed: {workItemResponse.ErrorMessage}");
            }
            
            return workItemResponse.Data.Value;
        }
    }
}
