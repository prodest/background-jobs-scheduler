using System.Collections.Generic;

namespace BackgroundJobsScheduler
{
    public class AutenticacaoIdentityServer
    {
        public string Authority { get; set; }
        public bool RequireHttpsMetadata { get; set; }
        public List<string> AllowedScopes { get; set; }
        public bool AutomaticAuthenticate { get; set; }
    }
}
