using DIGNDB.App.SmitteStop.Core.Helpers;
using DIGNDB.App.SmitteStop.Domain.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace DIGNDB.App.SmitteStop.API.Attributes
{
    public class GitHubAuthorizationAttribute : ActionFilterAttribute
    {
        private readonly AuthOptions _authOptions;
        private readonly ILogger<GitHubAuthorizationAttribute> _logger;

        private readonly string _gitHubTokenEncrypted;

        public GitHubAuthorizationAttribute(AuthOptions authOptions, ILogger<GitHubAuthorizationAttribute> logger, AppSettingsConfig appSettingsConfig)
        {
            _authOptions = authOptions;
            _logger = logger;
            _gitHubTokenEncrypted = appSettingsConfig.AuthorizationGitHub;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!_authOptions.AuthHeaderCheckEnabled)
            {
                return;
            }

            if (context.HttpContext.Request.Headers.TryGetValue("Authorization_GitHub", out var values))
            {
                if (!values.Any())
                {
                    context.Result = new UnauthorizedObjectResult("Missing or invalid token");
                }
                else
                {
                    var token = values.First();
                    string gitHubTokenDecrypted;

                    try
                    {
                        gitHubTokenDecrypted = ConfigEncryptionHelper.UnprotectString(_gitHubTokenEncrypted);
                    }
                    catch (Exception e)
                    {
                        _logger.LogError($"Configuration error. Cannot decrypt the GitHub token from configuration. MobileTokenEncrypted: {_gitHubTokenEncrypted}. Message: {e.Message}");
                        throw;
                    }

                    if (!token.Equals(gitHubTokenDecrypted) || string.IsNullOrEmpty(gitHubTokenDecrypted))
                    {
                        context.Result = new UnauthorizedObjectResult("Missing or invalid token");
                    }
                }
            }
            else
            {
                context.Result = new UnauthorizedObjectResult("Missing or invalid token");
            }
        }
    }
}
