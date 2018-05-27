using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace MediatR.HttpBindings
{
    public class HttpBindingAttribute : Attribute
    {
        public string Method { get; private set; }
        public string Url { get; private set; }

        public HttpBindingAttribute(string method, string url)
        {
            Method = method;
            Url = url;
        }
    }

    public class HttpBindingsProvider : IApplicationFeatureProvider<ControllerFeature>
    {
        private readonly IEnumerable<Type> requests;

        public HttpBindingsProvider(params Assembly[] requestAssemblies)
        {
            this.requests = requestAssemblies.SelectMany(ra => ra.GetTypes()).Where(t => t.GetCustomAttribute<HttpBindingAttribute>() != null);
        }
        public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
        {

            foreach (var entityType in requests)
            {
                var typeName = entityType.Name + "Controller";
                if (!feature.Controllers.Any(t => t.Name == typeName))
                {
                    var httpBinding = entityType.GetCustomAttribute<HttpBindingAttribute>();
                    var method = httpBinding.Method;
                    var url = httpBinding.Url;

                    var returnType = entityType.GetInterfaces().Single(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequest<>)).GetGenericArguments()[0];
                    var controllerType = typeof(RequestResponseController<,>)
                        .MakeGenericType(entityType, returnType);

                    var assemblyName = new System.Reflection.AssemblyName("RequestResponseController");
                    var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
                    var moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName.Name);
                    var typeBuilder = moduleBuilder.DefineType(typeName, System.Reflection.TypeAttributes.Public, controllerType);

                    var classAttrCtorParams = new Type[] { typeof(string) };
                    var classAttrCtorInfo = typeof(RouteAttribute).GetConstructor(classAttrCtorParams);
                    var classAttrBuilder = new CustomAttributeBuilder(classAttrCtorInfo, new object[] { url });
                    typeBuilder.SetCustomAttribute(classAttrBuilder);

                    var cbBuilder = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, new[] { typeof(IMediator) });
                    var constructor = cbBuilder.GetILGenerator();
                    constructor.Emit(OpCodes.Ldarg_0);                // push &quot;this&quot;
                    constructor.Emit(OpCodes.Ldarg_1);                // push the 1. parameter
                    constructor.Emit(OpCodes.Call, controllerType.GetConstructor(BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.Instance, null, new Type[] { typeof(IMediator) }, null));
                    constructor.Emit(OpCodes.Nop);
                    constructor.Emit(OpCodes.Nop);
                    constructor.Emit(OpCodes.Ret);

                    var attrCtorParams = new Type[] { };
                    var attrCtorInfo = GetTypeFromMethod(method).GetConstructor(attrCtorParams);
                    var attrBuilder = new CustomAttributeBuilder(attrCtorInfo, new object[] { });

                    var executeAsyncMethodInfo = controllerType.GetMethod("ExecuteAsync", BindingFlags.Instance | BindingFlags.NonPublic, Type.DefaultBinder, new[] { entityType }, null);
                    var methodBuilder = typeBuilder.DefineMethod("Handle", executeAsyncMethodInfo.Attributes, executeAsyncMethodInfo.CallingConvention, executeAsyncMethodInfo.ReturnType, executeAsyncMethodInfo.GetParameters().Select(p => p.ParameterType).ToArray());
                    methodBuilder.DefineParameter(1, executeAsyncMethodInfo.GetParameters().First().Attributes, executeAsyncMethodInfo.GetParameters().First().Name);
                    var ilCode = methodBuilder.GetILGenerator();
                    ilCode.Emit(OpCodes.Ldarg_0);
                    ilCode.Emit(OpCodes.Ldarg_1); // load array argument

                    // get element at index
                    ilCode.Emit(OpCodes.Ldc_I4_S, 0); // specify index
                    ilCode.Emit(OpCodes.Ldelem_Ref); // get element

                    if (entityType.IsPrimitive)
                    {
                        ilCode.Emit(OpCodes.Unbox_Any, entityType);
                    }
                    else if (entityType == typeof(object))
                    {
                        // do nothing
                    }
                    else
                    {
                        ilCode.Emit(OpCodes.Castclass, entityType);
                    }
                    ilCode.EmitCall(OpCodes.Call, executeAsyncMethodInfo, null);
                    ilCode.Emit(OpCodes.Ret);

                    methodBuilder.SetCustomAttribute(attrBuilder);

                    var type = typeBuilder.CreateTypeInfo();

                    feature.Controllers.Add(type);
                }
            }
        }

        public static Type GetTypeFromMethod(string method)
        {
            if (method == HttpMethod.Get.Method)
            {
                return typeof(HttpGetAttribute);
            }
            if (method == HttpMethod.Post.Method)
            {
                return typeof(HttpPostAttribute);
            }
            if (method == HttpMethod.Put.Method)
            {
                return typeof(HttpPutAttribute);
            }
            if (method == HttpMethod.Delete.Method)
            {
                return typeof(HttpDeleteAttribute);
            }
            throw new InvalidOperationException($"Method {method} not supported");
        }

        public class RequestResponseController<T, U> : Controller where T : IRequest<U>
        {
            private readonly IMediator mediator;

            public RequestResponseController(IMediator mediator)
            {
                this.mediator = mediator;
            }
            protected async Task<U> ExecuteAsync(T request)
            {
                return await mediator.Send(request);
            }
        }
    }
}
