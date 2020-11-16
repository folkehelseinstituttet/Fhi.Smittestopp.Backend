using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DIGNDB.App.SmitteStop.Core.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace DIGNDB.App.SmitteStop.Core.DependencyInjection
{
    public class InjectionChecker
    {
        public static void CheckIfAreAnyDependenciesAreMissing(
            IServiceCollection services,
            Assembly executingAssembly,
            Type baseType)
        {
            var servicesToBeCreated = executingAssembly.GetTypes()
                .Where(baseType.IsAssignableFrom)
                .ToList();

            var serviceProvider = services.BuildServiceProvider();
            foreach (var controllerType in servicesToBeCreated)
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