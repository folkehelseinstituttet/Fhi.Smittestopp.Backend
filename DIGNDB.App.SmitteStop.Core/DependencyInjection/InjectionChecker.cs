using System;
using System.Collections.Generic;
using DIGNDB.App.SmitteStop.Core.Exceptions;
using Microsoft.Extensions.DependencyInjection;

namespace DIGNDB.App.SmitteStop.Core.DependencyInjection
{
    public class InjectionChecker
    {
        public static void CheckIfAreAnyDependenciesAreMissing(IServiceCollection services, IEnumerable<Type> controllers)
        {
            var serviceProvider = services.BuildServiceProvider();
            foreach (var controllerType in controllers)
            {
                try
                {
                    var controller = serviceProvider.GetService(controllerType);
                    if (controller != null)
                        continue;
                }
                catch (InvalidOperationException exception)
                {
                    Console.WriteLine(exception);
                    throw new ServiceNotRegisteredException(exception.Message, exception);
                }
            }
        }
    }
}