using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MediatR.HttpBindings.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMediatR(GetType().Assembly);
            services.AddMvc()
                .ConfigureApplicationPartManager(apm =>
                    apm.FeatureProviders.Add(new HttpBindingsProvider(GetType().Assembly)));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }


    [HttpBinding("GET", "users")]
    public class GetUsersRequest : IRequest<GetUsersResponse>
    {

    }

    public class GetUsersResponse
    {
        public List<string> UserNames { get; set; }
    }

    public class GetUsersRequestHandler : AsyncRequestHandler<GetUsersRequest, GetUsersResponse>
    {
        protected override async Task<GetUsersResponse> Handle(GetUsersRequest request)
        {
            return new GetUsersResponse() { UserNames = new List<string> { "Robin" } };
        }
    }

    [HttpBinding("POST", "users")]
    public class AddUserRequest : IRequest<AddUserResponse>
    {
        public string Username { get; set; }
    }

    public class AddUserResponse
    {
        public string Id { get; set; }
    }

    public class AddUserRequestHandler : AsyncRequestHandler<AddUserRequest, AddUserResponse>
    {
        protected override async Task<AddUserResponse> Handle(AddUserRequest request)
        {
            return new AddUserResponse() { Id = request.Username };
        }
    }
}
