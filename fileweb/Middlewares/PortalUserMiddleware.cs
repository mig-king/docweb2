using fileweb.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace fileweb.Middlewares
{
    public class PortalUserMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _portalUrl;
        private readonly EnvOptions _envOptions;
        //private readonly string _appID = "HAH_MANAGE";

        public PortalUserMiddleware(RequestDelegate next, string portalUrl, EnvOptions evnOptions)
        {
            _next = next;
            _portalUrl = portalUrl;
            _envOptions = evnOptions;
        }

        public Task Invoke(HttpContext context)
        {
            // get session if exists
            var session = context.Session.Get(_envOptions.PortalUserSessionOptions.PortalUserSessionKeyName);
            // get user auth token if exists
            var token = context.Request.Query["USER"].ToString();

            if (!string.IsNullOrWhiteSpace(token))
            {
                // clear login time session
                context.Session.Remove(_envOptions.PortalUserSessionOptions.LastLoginTimeSessionKeyName);
            }

            if (session == null && !string.IsNullOrWhiteSpace(token))
            {
                //Get the http request
                HttpWebRequest myReq = (HttpWebRequest)(WebRequest.Create(this._portalUrl + "/GetUserObject.aspx?USER=" + HttpUtility.UrlEncode(token)));

                HttpWebResponse Res = (HttpWebResponse)myReq.GetResponse();

                //Open a stream reader to read the response
                StreamReader r = new StreamReader(Res.GetResponseStream());

                //Read all responses
                string HttpText = r.ReadToEnd();

                Res.Close();

                //If it is not start as an XML string
                if (HttpText[0] != '<')
                {
                    throw new Exception("Wrong Response for requesting User:" + HttpText);
                }
                else
                {
                    System.Text.UTF8Encoding TextConverter = new System.Text.UTF8Encoding();
                    //Parse the XML and get the user object
                    //_CurrentUser = new LoginAPI.UserObject(TextConverter.GetBytes(HttpText));

                    context.Session.Set(_envOptions.PortalUserSessionOptions.PortalUserSessionKeyName, TextConverter.GetBytes(HttpText));
                }
            }

            // check session if it is expired, redirect to login page
            session = context.Session.Get(_envOptions.PortalUserSessionOptions.PortalUserSessionKeyName);
            if (session == null)
                context.Response.Redirect(_portalUrl);
            else
            {
                LoginAPI.UserObject _CurrentUser = new LoginAPI.UserObject(session);

                if (_CurrentUser != null)
                {
                    context.Items["User"] = _CurrentUser.CurrentUser.FullName;
                    //context.Items["UserID"] = _CurrentUser.CurrentUser.UserID;
                    // parent module
                    // portal section
                    //bool hasPortal = false;
                    //if (_CurrentUser.CanUserAccessThisModule(_envOptions.AppModuleOptions.PortalModule.OfficeModule)
                    //    || _CurrentUser.CanUserAccessThisModule(_envOptions.AppModuleOptions.PortalModule.UserModule))
                    //    hasPortal = true;
                    //context.Items["PortalModule"] = hasPortal;
                    //if (hasPortal)
                    //{
                    //    context.Items["OfficeModule"] = _CurrentUser.CanUserAccessThisModule(_envOptions.AppModuleOptions.PortalModule.OfficeModule);
                    //    context.Items["UserModule"] = _CurrentUser.CanUserAccessThisModule(_envOptions.AppModuleOptions.PortalModule.UserModule);
                    //}
                    //// HAH Reporting section
                    //bool hasHAHReporting = false;
                    //if (_CurrentUser.CanUserAccessThisModule(_envOptions.AppModuleOptions.AgingVaAgencyModule.VAModule)
                    //    || _CurrentUser.CanUserAccessThisModule(_envOptions.AppModuleOptions.AgingVaAgencyModule.COMMERCIALModule))
                    //    hasHAHReporting = true;
                    //context.Items["HAHReportingModule"] = hasHAHReporting;
                    //if (hasHAHReporting)
                    //{
                    //    context.Items["AgingVAModule"] = _CurrentUser.CanUserAccessThisModule(_envOptions.AppModuleOptions.AgingVaAgencyModule.VAModule);
                    //    context.Items["AgingCommercialModule"] = _CurrentUser.CanUserAccessThisModule(_envOptions.AppModuleOptions.AgingVaAgencyModule.COMMERCIALModule);
                    //}
                    //// BI Repository section
                    //bool hasBI_Repository = false;
                    //if (_CurrentUser.CanUserAccessThisModule(_envOptions.AppModuleOptions.BIRepositoryModule.ExternalIDMappingModule))
                    //    hasBI_Repository = true;
                    //context.Items["BI_RepositoryModule"] = hasBI_Repository;
                    //if (hasBI_Repository)
                    //{
                    //    context.Items["ExternalIDMappingModule"] = _CurrentUser.CanUserAccessThisModule(_envOptions.AppModuleOptions.BIRepositoryModule.ExternalIDMappingModule);
                    //}
                }
            }

            // Call the next delegate/middleware in the pipeline
            return this._next(context);
        }
    }

    public static class PortalUserMiddlewareExtensions
    {
        public static IApplicationBuilder UsePortalUserMiddleware(this IApplicationBuilder builder, string portalUrl, EnvOptions envOptions)
        {
            return builder.UseMiddleware<PortalUserMiddleware>(portalUrl, envOptions);
        }
    }
}
