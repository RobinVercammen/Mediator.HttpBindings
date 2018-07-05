using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace MediatR.HttpBindings
{
    internal class RequestModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null) throw new ArgumentNullException(nameof(bindingContext));

            var jsonData = new StreamReader(bindingContext.HttpContext.Request.Body).ReadToEnd();

            object request;
            if (string.IsNullOrEmpty(jsonData))
                request = Activator.CreateInstance(bindingContext.ModelType);
            else
                request = JsonConvert.DeserializeObject(jsonData, bindingContext.ModelType);

            foreach (var propertyInfo in bindingContext.ModelType.GetProperties())
            {
                var name = propertyInfo.Name;
                var lowercaseName = char.ToLowerInvariant(name[0]) + name.Substring(1);


                var nameValue = bindingContext.ValueProvider.GetValue(name);
                var lowerNameValue = bindingContext.ValueProvider.GetValue(lowercaseName);

                if (nameValue.Length == 1) propertyInfo.SetValue(request, nameValue.FirstValue);

                if (lowerNameValue.Length == 1) propertyInfo.SetValue(request, lowerNameValue.FirstValue);
            }

            bindingContext.Result = ModelBindingResult.Success(request);
            return Task.CompletedTask;
        }
    }
}