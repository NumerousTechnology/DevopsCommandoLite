using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevopsCommandoLite.Terminal.Services
{
    public class WorkItemService
    {
        public async Task<DevopsResponse<string>> GetWorkItemFieldValue(string id, string fieldName)
        {
            var api = new ApiService();
            var wiResult =  await api.Get<WorkItem>($"wit/workitems/{id}?fields={fieldName}");

            if (wiResult.IsSuccess)
            {
                return new DevopsResponse<string>(wiResult.IsSuccess, data: wiResult.Data.Fields.Title) ;
            }
            else
            {
                return new DevopsResponse<string>(false, wiResult.ErrorMessage);
            }
        }


        public async Task<DevopsResponse<WorkItems>> GetWorkItems(IList<string> ids, string fieldName)
        {
            var api = new ApiService();
            return await api.Get<WorkItems>($"wit/workitems?ids={string.Join(",", ids)}&fields={fieldName}");
        }
    }
}
