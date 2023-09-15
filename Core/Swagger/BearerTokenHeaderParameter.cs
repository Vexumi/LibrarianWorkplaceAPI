using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace LibrarianWorkplaceAPI.Core.Swagger
{
    public class BearerTokenHeaderParameter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var filterPipeline = context.ApiDescription.ActionDescriptor.FilterDescriptors;
            var isAuthorized = filterPipeline.Select(filterInfo => filterInfo.Filter).Any(filter => filter is AuthorizeFilter);
            var allowAnonymous = filterPipeline.Select(filterInfo => filterInfo.Filter).Any(filter => filter is IAllowAnonymousFilter);
            var hasAuthorizeAttribute = context.MethodInfo.DeclaringType.GetCustomAttributes(true).OfType<Microsoft.AspNetCore.Authorization.AuthorizeAttribute>().Any() || 
                context.MethodInfo.GetCustomAttributes(true).OfType<Microsoft.AspNetCore.Authorization.AuthorizeAttribute>().Any();

            if (hasAuthorizeAttribute || isAuthorized && !allowAnonymous)
            {
                if (operation.Parameters == null)
                    operation.Parameters = new List<OpenApiParameter>();

                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Description = "Access token",
                    Required = true,
                    Schema = new OpenApiSchema
                    {
                        Type = "string",
                        Default = new OpenApiString("Bearer ")
                    }
                });
            }
        }
    }
}
