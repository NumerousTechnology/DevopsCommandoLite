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
    }
}
