using System.Collections.Generic;

namespace DevopsCommandoLiteLite.Model
{
    public class AppSettings
    {
        public ConnectionStringsSettings ConnectionStrings { get; set; }
        public IList<RepoSettings> Repos { get; set; }
    }

    public class RepoSettings
    {
        public string Repo { get; set; }
        public IList<string> PRs { get; set; }
    }

    public class ConnectionStringsSettings
    {
        public string PersonalAccessToken { get; set; }
        public string InstanceUrl { get; set; }
    }
}
