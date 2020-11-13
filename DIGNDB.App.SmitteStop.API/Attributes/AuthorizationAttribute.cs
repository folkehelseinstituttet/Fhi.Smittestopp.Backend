using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.App.SmitteStop.Domain.Configuration;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace DIGNDB.App.SmitteStop.API.Attributes
{
    public class AuthorizationAttribute : ActionFilterAttribute
    {
        private readonly AuthOptions _authOptions;
        private readonly IJwtValidationService _jwtValidationService;

        public AuthorizationAttribute(AuthOptions authOptions, IJwtValidationService jwtValidationService)
        {
            _authOptions = authOptions;
            _jwtValidationService = jwtValidationService;
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
                    var token = authHeader.Replace("Bearer", "");

                    _jwtValidationService.IsTokenValid(token);
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
