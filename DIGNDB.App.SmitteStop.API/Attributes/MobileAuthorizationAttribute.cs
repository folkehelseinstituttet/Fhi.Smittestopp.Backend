using DIGNDB.App.SmitteStop.Core.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System.Linq;
using DIGNDB.App.SmitteStop.Domain.Configuration;
using Microsoft.Extensions.Logging;
using System;

namespace DIGNDB.App.SmitteStop.API.Attributes
{
    public class MobileAuthorizationAttribute : ActionFilterAttribute
    {
        private readonly AuthOptions _authOptions;
        private readonly ILogger<MobileAuthorizationAttribute> _logger;
        private readonly IAppSettingsConfig _appSettingsConfig;

        public MobileAuthorizationAttribute(AuthOptions authOptions, ILogger<MobileAuthorizationAttribute> logger, IAppSettingsConfig appSettingsConfig)
        {
            _authOptions = authOptions;
            _logger = logger;
            _appSettingsConfig = appSettingsConfig;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!_authOptions.AuthHeaderCheckEnabled) return;

            StringValues values = new StringValues();
            if (context.HttpContext.Request.Headers.TryGetValue("authorization_mobile", out values))
            {
                if (!values.Any())
                {
                    context.Result = new UnauthorizedObjectResult("Missing or invalid token");
                }
                else
                {
                    var token = values.First();
                    string mobileTokenEncrypted = _appSettingsConfig.Configuration.GetValue<string>("authorizationMobile");
                    string mobileTokenDecrypted;

                    try
                    {
                        mobileTokenDecrypted = ConfigEncryptionHelper.UnprotectString(mobileTokenEncrypted);
                    }
                    catch (Exception e)
                    {
                        _logger.LogError($"Configuration error. Cannot decrypt the mobileToken from configuration. MobileTokenEncrypted: {mobileTokenEncrypted}. Message: {e.Message}");
                        throw;
                    }

                    if (!token.Equals(mobileTokenDecrypted) || string.IsNullOrEmpty(mobileTokenDecrypted))
                        context.Result = new UnauthorizedObjectResult("Missing or invalid token");
                    else
                        return;
                }
            }
            else
            {
                context.Result = new UnauthorizedObjectResult("Missing or invalid token");
            }
        }
    }
}
