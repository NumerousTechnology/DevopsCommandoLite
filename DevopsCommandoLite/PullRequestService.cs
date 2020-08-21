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
            return await _api.Get<WorkItemReferenceParent>($"git/repositories/{repositoryId}/pullRequests/{pullRequestId}/workitems?fields=System.Title,System.WorkItemType");
        }
    }
}
