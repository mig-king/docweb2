using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fileweb.Models
{
    public class EnvOptions
    {
        public EnvOptions()
        {
            this.AppModuleOptions = new AppModuleOptions();
            this.PortalUserSessionOptions = new PortalUserSessionOptions();
            this.PortalUserCacheOptions = new PortalUserCacheOptions();
        }

        public string PortalUrl { get; set; }
        public AppModuleOptions AppModuleOptions { get; set; }
        public PortalUserSessionOptions PortalUserSessionOptions { get; set; }
        public PortalUserCacheOptions PortalUserCacheOptions { get; set; }
    }

    public class AppModuleOptions
    {
        public AppModuleOptions() { }

        public ReportingModule ReportingModule { get; set; }
        public TrainingModule TrainingModule { get; set; }
        public AdminModule AdminModule { get; set; }
    }

    public class AdminModule
    {
        public AdminModule() { }

        public string VAModule { get; set; }
        public string COMMERCIALModule { get; set; }
    }

    public class ReportingModule
    {
        public ReportingModule() { }

        public string OfficeModule { get; set; }
        public string UserModule { get; set; }
    }

    public class TrainingModule
    {
        public TrainingModule() { }

        public string ExternalIDMappingModule { get; set; }
    }

    public class PortalUserSessionOptions
    {
        public PortalUserSessionOptions() { }

        public string PortalUserSessionKeyName { get; set; }
        public string PortalUserSessionCookieName { get; set; }
        public int SessionExpiredMinutes { get; set; }
        public int SessionExpireNotificationMinutes { get; set; }
        public string LastLoginTimeSessionKeyName { get; set; }
    }

    public class PortalUserCacheOptions
    {
        public PortalUserCacheOptions() { }

        public string PortalUserCacheKeyName { get; set; }
        public double CacheExpiredHours { get; set; }
    }
}
