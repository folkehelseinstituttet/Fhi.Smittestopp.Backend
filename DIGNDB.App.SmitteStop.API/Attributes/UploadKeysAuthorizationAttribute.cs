using DIGNDB.App.SmitteStop.API.Contracts;
using DIGNDB.App.SmitteStop.Domain.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;

namespace DIGNDB.App.SmitteStop.API.Attributes
{
    public class UploadKeysAuthorizationAttribute : ActionFilterAttribute
    {
        private readonly IJwtValidationService _jwtValidationService;
        private readonly IAnonymousTokenValidationService _anonymousTokenValidationService;
        private readonly AnonymousTokenKeyStoreConfiguration _anonymousTokenConfig;

        public UploadKeysAuthorizationAttribute(IJwtValidationService jwtValidationService,
            IAnonymousTokenValidationService anonymousTokenValidationService,
            IOptions<AnonymousTokenKeyStoreConfiguration> anonymousTokenConfig)
        {
            _jwtValidationService = jwtValidationService;
            _anonymousTokenValidationService = anonymousTokenValidationService;
            _anonymousTokenConfig = anonymousTokenConfig.Value;
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

                if (_anonymousTokenConfig.Enabled)
                {
                    if (authHeader != null && authHeader.StartsWith("Anonymous "))
                    {
                        var anonymousToken = authHeader.Replace("Anonymous ", string.Empty);
                        var isValid = await _anonymousTokenValidationService.IsTokenValid(anonymousToken);
                        if (!isValid)
                        {
                            context.Result = new UnauthorizedObjectResult("Invalid token");
                        }

                        return;
                    }
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
