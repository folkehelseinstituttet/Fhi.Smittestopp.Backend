using Microsoft.AspNetCore.Authorization;

namespace DIGNDB.App.SmitteStop.API.HealthCheckAuthorization
{
    /// <summary>
    /// Authorization requirement for health check endpoints
    /// </summary>
    public class HealthCheckAuthorizationRequirement : IAuthorizationRequirement
    {
    }
}
