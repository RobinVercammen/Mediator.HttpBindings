using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Reflection.Emit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace MediatR.HttpBindings
{
    internal class HttpBindingsProvider : IApplicationFeatureProvider<ControllerFeature>
    {
        private readonly string _prefix;
        private readonly IEnumerable<Type> requests;

        public HttpBindingsProvider(string prefix = null, params Assembly[] requestAssemblies)
        {
            _prefix = prefix??string.Empty;
            requests = requestAssemblies.SelectMany(ra => ra.GetTypes())
                .Where(t => t.GetCustomAttribute<HttpBindingAttribute>() != null);
        }

        public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
        {
            var classAttrBuilder = CreateClassAttribute();
            foreach (var requestType in requests)
            {
                var controllerName = requestType.Name + "Controller";
                if (!feature.Controllers.Any(t => t.Name == controllerName))
                {
                    var url = _prefix + requestType.Name;
                    var responseType = GetReturnTypeFromIRequest(requestType);
                    var controllerType =
                        typeof(RequestResponseController<,>).MakeGenericType(requestType, responseType);

                    var typeBuilder = CreateControllerTypeBuilder(controllerName, controllerType);
                    CallBaseController(typeBuilder, controllerType);
                    typeBuilder.SetCustomAttribute(classAttrBuilder);

                    var executeAsyncMethodInfo = GetBaseMethod(controllerType, requestType);
                    var methodBuilder = CreateHandleMethod(typeBuilder, executeAsyncMethodInfo);
                    var parameters = executeAsyncMethodInfo.GetParameters();
                    var parameter = parameters.First();
                    var paramBuilder = methodBuilder.DefineParameter(1, parameter.Attributes, parameter.Name);

                    CallBaseMethod(methodBuilder, parameters, executeAsyncMethodInfo);
                    var attrBuilder = CreateMethodAttribute(url);
                    methodBuilder.SetCustomAttribute(attrBuilder);

                    var type = typeBuilder.CreateTypeInfo();

                    feature.Controllers.Add(type);
                }
            }
        }

        private static Type GetReturnTypeFromIRequest(Type t)
        {
            return t.GetInterfaces().Single(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequest<>))
                .GetGenericArguments()[0];
        }

        private static TypeBuilder CreateControllerTypeBuilder(string controllerName, Type controllerType)
        {
            var assemblyName = new AssemblyName("RequestResponseController");
            var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            var moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName.Name);
            var typeBuilder = moduleBuilder.DefineType(controllerName, TypeAttributes.Public, controllerType);
            return typeBuilder;
        }

        private static CustomAttributeBuilder CreateClassAttribute()
        {
            var classAttrCtorParams = new[] {typeof(string)};
            var classAttrCtorInfo = typeof(RouteAttribute).GetConstructor(classAttrCtorParams);
            var classAttrBuilder = new CustomAttributeBuilder(classAttrCtorInfo, new object[] {string.Empty});
            return classAttrBuilder;
        }

        private static ConstructorBuilder CallBaseController(TypeBuilder typeBuilder, Type controllerType)
        {
            var cbBuilder = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard,
                new[] {typeof(IMediator)});
            var constructor = cbBuilder.GetILGenerator();
            constructor.Emit(OpCodes.Ldarg_0);
            constructor.Emit(OpCodes.Ldarg_1);
            constructor.Emit(OpCodes.Call,
                controllerType.GetConstructor(
                    BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.Instance, null,
                    new[] {typeof(IMediator)}, null));
            constructor.Emit(OpCodes.Nop);
            constructor.Emit(OpCodes.Nop);
            constructor.Emit(OpCodes.Ret);
            return cbBuilder;
        }

        private static CustomAttributeBuilder CreateMethodAttribute(string url)
        {
            var attrCtorParams = new[] {typeof(string)};
            var attrCtorInfo = typeof(HttpPostAttribute).GetConstructor(attrCtorParams);
            var attrBuilder = new CustomAttributeBuilder(attrCtorInfo, new object[] {url});
            return attrBuilder;
        }

        private static MethodInfo GetBaseMethod(Type controllerType, Type requestType)
        {
            return controllerType.GetMethod("ExecuteAsync", BindingFlags.Instance | BindingFlags.NonPublic,
                Type.DefaultBinder, new[] {requestType}, null);
        }

        private static MethodBuilder CreateHandleMethod(TypeBuilder typeBuilder, MethodInfo method)
        {
            var parameters = method.GetParameters().Select(p => p.ParameterType).ToArray();
            return typeBuilder.DefineMethod("Handle", MethodAttributes.Public, method.CallingConvention,
                method.ReturnType, parameters);
        }

        private static void CallBaseMethod(MethodBuilder methodBuilder, ParameterInfo[] parameters,
            MethodInfo baseMethod)
        {
            var ilCode = methodBuilder.GetILGenerator();
            ilCode.Emit(OpCodes.Ldarg_0);

            for (var i = 0; i < parameters.Length; i++)
            {
                var parameter = parameters[i];
                ilCode.Emit(OpCodes.Ldarg_S, i + 1);
            }

            ilCode.Emit(OpCodes.Call, baseMethod);
            ilCode.Emit(OpCodes.Ret);
        }
    }
}