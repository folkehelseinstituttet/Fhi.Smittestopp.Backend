using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DIGNDB.App.SmitteStop.Core.Helpers
{
    public class ModelValidator
    {
        public static bool TryValidate(object @object, out ICollection<ValidationResult> results)
        {
            var context = new ValidationContext(@object, serviceProvider: null, items: null);
            results = new List<ValidationResult>();
            return Validator.TryValidateObject(
                @object, context, results,
                validateAllProperties: true
            );
        }

        public static void ValidateContract(object instance)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));

            var result = new List<ValidationResult>();
            var context = new ValidationContext(instance);
            if (!Validator.TryValidateObject(instance, context, result,true))
            {
                throw new ValidationException(string.Join(Environment.NewLine, result.Select(r => r.ErrorMessage)));
            }
        }
    }
}
