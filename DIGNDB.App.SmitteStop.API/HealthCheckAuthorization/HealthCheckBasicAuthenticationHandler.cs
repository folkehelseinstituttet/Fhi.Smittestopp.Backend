using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace DIGNDB.App.SmitteStop.API.HealthCheckAuthorization
{
    /// <summary>
    /// Handler for authentication used for health checks
    /// </summary>
    public class HealthCheckBasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        /// <summary>
        /// Name of scheme used for health check authentication
        /// </summary>
        public const string HealthCheckBasicAuthenticationScheme = "HealthCheckBasicAuthenticationScheme";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <param name="encoder"></param>
        /// <param name="logger"></param>
        /// <param name="clock"></param>
        public HealthCheckBasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        /// <summary>
        /// Returns success for authentication at all times since no user authentication is needed for health checks
        /// </summary>
        /// <returns></returns>
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var ticket = new AuthenticationTicket(Context.User, HealthCheckBasicAuthenticationScheme);
            var success = Task.FromResult(AuthenticateResult.Success(ticket));
            return success;
        }
    }
    
    /// <summary>
    /// Extension class for adding scheme
    /// </summary>
    public static class AddNoOperationAuthenticationExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static AuthenticationBuilder AddNoOperationAuthentication(this AuthenticationBuilder builder) =>
            builder.AddNoOperationAuthentication(HealthCheckBasicAuthenticationHandler.HealthCheckBasicAuthenticationScheme, _ => { });

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configureOptions"></param>
        /// <returns></returns>
        public static AuthenticationBuilder AddNoOperationAuthentication(this AuthenticationBuilder builder,
            Action<AuthenticationSchemeOptions> configureOptions) =>
            builder.AddNoOperationAuthentication(HealthCheckBasicAuthenticationHandler.HealthCheckBasicAuthenticationScheme, configureOptions);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="authenticationScheme"></param>
        /// <param name="configureOptions"></param>
        /// <returns></returns>
        public static AuthenticationBuilder AddNoOperationAuthentication(this AuthenticationBuilder builder, string authenticationScheme,
            Action<AuthenticationSchemeOptions> configureOptions) =>
            builder.AddNoOperationAuthentication(authenticationScheme, null, configureOptions);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="authenticationScheme"></param>
        /// <param name="displayName"></param>
        /// <param name="configureOptions"></param>
        /// <returns></returns>
        public static AuthenticationBuilder AddNoOperationAuthentication(this AuthenticationBuilder builder, string authenticationScheme,
            string displayName, Action<AuthenticationSchemeOptions> configureOptions)
        {
            return builder.AddScheme<AuthenticationSchemeOptions, HealthCheckBasicAuthenticationHandler>(authenticationScheme,
                displayName, configureOptions);
        }
    }
}
