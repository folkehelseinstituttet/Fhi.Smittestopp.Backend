using DIGNDB.App.SmitteStop.Core.Helpers;
using DIGNDB.App.SmitteStop.Domain.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DIGNDB.App.SmitteStop.API.HealthCheckAuthorization
{
    /// <summary>
    /// Handles authorization for health check endpoints by inspecting header value for key "Authorization_HealthCheck"
    /// </summary>
    public class HealthCheckAuthorizationHandler : AuthorizationHandler<HealthCheckAuthorizationRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthOptions _authOptions;
        private readonly ILogger<HealthCheckAuthorizationHandler> _logger;

        private static readonly string _tokenJsonKey = "Authorization_HealthCheck";
        private readonly string _tokenEncrypted;

        /// <summary>
        /// Ctor for HealthCheckAuthorizationHandler
        /// </summary>
        /// <param name="config"></param>
        /// <param name="httpContextAccessor"></param>
        /// <param name="authOptions"></param>
        /// <param name="logger"></param>
        public HealthCheckAuthorizationHandler(AppSettingsConfig config, IHttpContextAccessor httpContextAccessor, AuthOptions authOptions, ILogger<HealthCheckAuthorizationHandler> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _authOptions = authOptions;
            _logger = logger;

            _tokenEncrypted = config.HealthCheckSettings.AuthorizationHealthCheck; 
        }

        /// <summary>
        /// Handles authorization by inspecting token in header for value "Authorization_HealthCheck" <seealso cref="Attributes.MobileAuthorizationAttribute"/>
        /// </summary>
        /// <param name="context"></param>
        /// <param name="requirement"></param>
        /// <returns></returns>
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HealthCheckAuthorizationRequirement requirement)
        {
            // Used in, e.g., development, see StartUp
            if (!_authOptions.AuthHeaderCheckEnabled)
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            var httpContext = _httpContextAccessor.HttpContext;
            var headers = httpContext.Request.Headers;

            if (headers.TryGetValue(_tokenJsonKey, out var values))
            {
                if (!values.Any())
                {
                    _logger.LogWarning($"Access to health check denied. Missing value for {_tokenJsonKey} token");
                    context.Fail();
                }
                else
                {
                    var token = values.First();
                    string healthTokenDecrypted;

                    try
                    {
                        // Use [Authorization_HealthCheck: 8AKYHjvzDQ] for development and test
                        healthTokenDecrypted = ConfigEncryptionHelper.UnprotectString(_tokenEncrypted);
                    }
                    catch (Exception e)
                    {
                        _logger.LogError($"Configuration error. Cannot decrypt the {_tokenJsonKey}. Encrypted token: {_tokenEncrypted}. Message: {e.Message}");
                        throw;
                    }

                    if (!token.Equals(healthTokenDecrypted) || string.IsNullOrEmpty(healthTokenDecrypted))
                    {
                        _logger.LogWarning($"Invalid {_tokenJsonKey} token");
                        context.Fail();
                    }
                    else
                    {
                        // Token value for header validated
                        context.Succeed(requirement);
                    }
                }
            }
            else
            {
                _logger.LogWarning($"Missing {_tokenJsonKey} token");
                context.Fail();
            }
            
            return Task.CompletedTask;
        }
    }
}
