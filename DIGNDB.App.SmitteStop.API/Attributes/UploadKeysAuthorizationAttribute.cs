using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.App.SmitteStop.Domain.Configuration;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using DIGNDB.App.SmitteStop.API.Contracts;
using AnonymousTokens.Server.Protocol;
using AnonymousTokens.Core.Services.InMemory;
using Org.BouncyCastle.Crypto.EC;
using Org.BouncyCastle.Asn1.X9;
using System.Security.Cryptography;

namespace DIGNDB.App.SmitteStop.API.Attributes
{
    public class UploadKeysAuthorizationAttribute : ActionFilterAttribute
    {
        private readonly AppSettingsConfig _appSettingsConfig;
        private readonly AuthOptions _authOptions;
        private readonly IJwtValidationService _jwtValidationService;
        private readonly ITokenVerifier _tokenVerifier = new TokenVerifier(new InMemorySeedStore());
        private readonly InMemoryPrivateKeyStore _privateKeyStore = new InMemoryPrivateKeyStore();

        public UploadKeysAuthorizationAttribute(AppSettingsConfig appSettingsConfig, AuthOptions authOptions, IJwtValidationService jwtValidationService)
        {
            _appSettingsConfig = appSettingsConfig;
            _authOptions = authOptions;
            _jwtValidationService = jwtValidationService;
        }

        public override async void OnActionExecuting(ActionExecutingContext context)
        {
            if (!_authOptions.AuthHeaderCheckEnabled) return;

            var logger = context.HttpContext.RequestServices.GetService<ILogger<DiagnosticKeysController>>();
            try
            {
                string authHeader = context.HttpContext.Request.Headers["Authorization"];
                if (authHeader != null && authHeader.Contains("Bearer"))
                {
                    var token = authHeader.Replace("Bearer", string.Empty);
                    token = token.Trim();

                    _jwtValidationService.IsTokenValid(token);
                    return;

                }
                else if (authHeader != null && authHeader.StartsWith("Anonymous ")) 
                {
                    var _ecParameters = CustomNamedCurves.GetByOid(X9ObjectIdentifiers.Prime256v1);
                    string[] anonymousToken = authHeader.Replace("Anonymous ", string.Empty).Split(".");

                    var W = _ecParameters.Curve.DecodePoint(Convert.FromBase64String(anonymousToken[0]));
                    var t = Convert.FromBase64String(anonymousToken[1]);
                    var k = await _privateKeyStore.GetAsync();


                    var isValid = await _tokenVerifier.VerifyTokenAsync(k, _ecParameters.Curve, t, W);
                    if (!isValid)
                    {
                        context.Result = new UnauthorizedObjectResult("Invalid token");
                    }
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
