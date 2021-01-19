using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using DIGNDB.App.SmitteStop.API.Contracts;
using AnonymousTokens.Server.Protocol;
using AnonymousTokens.Core.Services.InMemory;

namespace DIGNDB.App.SmitteStop.API.Attributes
{
    public class UploadKeysAuthorizationAttribute : ActionFilterAttribute
    {
        private readonly IJwtValidationService _jwtValidationService;
        private readonly IAnonymousTokenKeySource _anonymousTokenKeySource;
        private readonly ITokenVerifier _tokenVerifier = new TokenVerifier(new InMemorySeedStore());

        public UploadKeysAuthorizationAttribute(IJwtValidationService jwtValidationService, IAnonymousTokenKeySource anonymousTokenKeySource)
        {
            _jwtValidationService = jwtValidationService;
            _anonymousTokenKeySource = anonymousTokenKeySource;
        }

        public override async void OnActionExecuting(ActionExecutingContext context)
        {
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

                if (authHeader != null && authHeader.StartsWith("Anonymous "))
                {
                    string[] anonymousToken = authHeader.Replace("Anonymous ", string.Empty).Split(".");

                    var submittedPoint = _anonymousTokenKeySource.ECParameters.Curve.DecodePoint(Convert.FromBase64String(anonymousToken[0]));
                    var tokenSeed = Convert.FromBase64String(anonymousToken[1]);
                    var keyId = anonymousToken[2];

                    var privateKey = _anonymousTokenKeySource.GetPrivateKey(keyId);

                    var isValid = await _tokenVerifier.VerifyTokenAsync(privateKey, _anonymousTokenKeySource.ECParameters.Curve, tokenSeed, submittedPoint);
                    if (!isValid)
                    {
                        context.Result = new UnauthorizedObjectResult("Invalid token");
                    }
                    return;
                }
                logger.LogWarning("Missing token or invalid scheme. Header value:" + authHeader);
                context.Result = new UnauthorizedObjectResult("Missing token or invalid scheme.");
            }
            catch (Exception e)
            {
                logger.LogError("Error on authorization:" + e);
                context.Result = new UnauthorizedObjectResult(e.Message);
            }
        }
    }
}
