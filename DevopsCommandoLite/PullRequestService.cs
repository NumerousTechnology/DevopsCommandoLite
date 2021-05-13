using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevopsCommandoLite.Terminal.Services
{
    public class PullRequestService
    {
        private readonly ApiService _api;

        public PullRequestService(ApiService api)
        {
            _api = api;
        }

        public async Task<DevopsResponse<WorkItemReferenceParent>> GetWorkItemList(string repositoryId, string pullRequestId)
        {
            // https://docs.microsoft.com/en-us/rest/api/azure/devops/git/pull%20request%20work%20items/list?view=azure-devops-rest-6.0
            return await _api.Get<WorkItemReferenceParent>($"git/repositories/{repositoryId}/pullRequests/{pullRequestId}/workitems");
        }

        public async Task<DevopsResponse<CreatePrResult>> Create(string repositoryId, CreatePrModel data)
        {
            // https://docs.microsoft.com/en-us/rest/api/azure/devops/git/pull%20request%20work%20items/list?view=azure-devops-rest-6.0

            return await _api.Post<CreatePrResult>($"git/repositories/{repositoryId}/pullRequests?api-version=6.0", data);
        }


        public async Task<DevopsResponse<PullRequestModelParent>> GetCompleted(string repositoryId, int top)
        {
            return await _api.Get<PullRequestModelParent>($"git/repositories/{repositoryId}/pullRequests?searchCriteria.status=completed&$top={top}");
        }
        
    }

    public class CreatePrModel
    {
        public string sourceRefName { get; set; }
        public string targetRefName { get; set; }
        public string title { get; set; }
        public string description { get; set; }

    }

    public class CreatePrResult
    {
        public string pullRequestId { get; set; }
        public string status { get; set; }
        public string title { get; set; }
        public string url { get; set; }

    }


    public class PullRequestModelParent
    {
        public IEnumerable<PullRequestModel> Value { get; set; }
        public int Count { get; set; }
    }
    public class PullRequestModel
    {
        public string pullRequestId { get; set; }
        public string status { get; set; }
        public string title { get; set; }
        public string sourceRefName { get; set; }
        public string targetRefName { get; set; }
    }


}
