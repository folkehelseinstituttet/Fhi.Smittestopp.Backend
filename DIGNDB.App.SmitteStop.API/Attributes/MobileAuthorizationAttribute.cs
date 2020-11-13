using DIGNDB.App.SmitteStop.Core.Helpers;
using DIGNDB.App.SmitteStop.Domain.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System;
using System.Linq;

namespace DIGNDB.App.SmitteStop.API.Attributes
{
    public class MobileAuthorizationAttribute : ActionFilterAttribute
    {
        private readonly AuthOptions _authOptions;
        private readonly ILogger<MobileAuthorizationAttribute> _logger;
        private readonly AppSettingsConfig _appSettingsConfig;

        private readonly string _mobileTokenEncrypted;

        public MobileAuthorizationAttribute(AuthOptions authOptions, ILogger<MobileAuthorizationAttribute> logger, AppSettingsConfig appSettingsConfig)
        {
            _authOptions = authOptions;
            _logger = logger;
            _mobileTokenEncrypted = appSettingsConfig.AuthorizationMobile;
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
                    string mobileTokenDecrypted;

                    try
                    {
                        mobileTokenDecrypted = ConfigEncryptionHelper.UnprotectString(_mobileTokenEncrypted);
                    }
                    catch (Exception e)
                    {
                        _logger.LogError($"Configuration error. Cannot decrypt the mobileToken from configuration. MobileTokenEncrypted: {_mobileTokenEncrypted}. Message: {e.Message}");
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
