using System;
using System.Security.Cryptography.X509Certificates;
using DIGNDB.App.SmitteStop.Domain.Configuration;
using JWT.Algorithms;
using JWT.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DIGNDB.App.SmitteStop.API.Attributes
{
    public class AuthorizationAttribute : ActionFilterAttribute
    {
        private readonly IAppSettingsConfig _appSettingsConfig;
        private readonly AuthOptions _authOptions;

        public AuthorizationAttribute(IAppSettingsConfig appSettingsConfig, AuthOptions authOptions)
        {
            _appSettingsConfig = appSettingsConfig;
            _authOptions = authOptions;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!_authOptions.AuthHeaderCheckEnabled) return;

            var logger = context.HttpContext.RequestServices.GetService<ILogger<DiagnosticKeysController>>();
            try
            {
                string authHeader = context.HttpContext.Request.Headers["Authorization"];
                if (authHeader != null && authHeader.Contains("Bearer"))
                {
                    var secret = _appSettingsConfig.Configuration.GetValue<string>("publicKey");
                    byte[] publicKeyDecoded = Convert.FromBase64String(secret);
                    var token = authHeader.Replace("Bearer", "");
                    string jsonPayload = new JwtBuilder()
                        .WithAlgorithm(new RS256Algorithm(new X509Certificate2(publicKeyDecoded)))
                        .MustVerifySignature()
                        .Decode(token.Trim());
                    return;

                }
                else
                {
                    logger.LogWarning("Missing token or invalid scheme. Header value:"+authHeader);
                    context.Result = new UnauthorizedObjectResult("Missing token or invalid scheme.");
                }
            }
            catch (Exception e)
            {
                logger.LogError("Error on authorization:" + e);
                context.Result = new UnauthorizedObjectResult(e.Message);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
