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
            var list = new List<string>();
            Console.WriteLine("");
            foreach (var r in repos)
            {
                Console.Write($"Looking in {r.Repo}");
                var ret = await GetUseStoreis(r.Repo, r.PRs);
                if (ret.IsSuccess)
                {
                    Console.WriteLine($" - Found {ret.Messages.Count} work items");
                    list.AddRange(ret.Messages);
                }
                else
                {
                    Console.WriteLine("! ERROR IN " + r.Repo);
                    foreach (var m in ret.Messages)
                        Console.WriteLine(m);
                }
                
            }
            Console.WriteLine("");


            foreach (var m in list.Distinct())
                Console.WriteLine($"- {m}{Environment.NewLine}");
            Console.ReadLine();
        }

        static async Task<CommandResult> GetUseStoreis(string repoId, IEnumerable<string> prIds)
        {
            var prService = new PullRequestService(new ApiService());
            
            var requests = prIds.Select(prId => new { Task = prService.GetWorkItemList(repoId, prId), Id = prId });
            await Task.WhenAll(requests.Select(p => p.Task));

            var workItemService = new WorkItemService();
            var resp = new CommandResult { IsSuccess = true };
            foreach (var request in requests)
            {
                var result = await request.Task;

                if (result.IsSuccess)
                {
                    foreach(var wi in result.Data.Value)
                        resp.Messages.Add($"• #{wi.Id}");

                    // Version that can get more workitem details
                    //var listt = await workItemService.GetWorkItems(result.Data.Value.Select(p => p.Id).ToList(), "System.Title,System.WorkItemType");

                    //foreach (var wi in listt.Data.Value)
                    //{
                    //    //var isBug = wi.Fields.Type == "Bug" ? "[Bug]" : "";
                    //    //resp.Messages.Add($"• {isBug} {wi.Fields.Title} - #{wi.Id}");
                    //}
                }
                else
                {
                    resp.Messages.Add($"Request for PR {request.Id} failed: {result.ErrorMessage}");
                }
            }

            return resp;
        }
    }
}
