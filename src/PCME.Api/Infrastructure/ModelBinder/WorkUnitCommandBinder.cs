using Microsoft.AspNetCore.Mvc.ModelBinding;
using PCME.Api.Application.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PCME.Api.Infrastructure.ModelBinder
{
    public class WorkUnitCommandBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }
            var id = bindingContext.HttpContext;
            //CreateOrUpdateWorkUnitCommand command = new CreateOrUpdateWorkUnitCommand(
                
            //    );

            // Specify a default argument name if none is set by ModelBinderAttribute
            var modelName = bindingContext.BinderModelName;
            if (string.IsNullOrEmpty(modelName))
            {
                modelName = "WorkUnitNature.Id";
            }

            var valueProviderResult = bindingContext.ValueProvider.GetValue("WorkUnitNature.Id");

            return Task.CompletedTask;
        }
    }
}
