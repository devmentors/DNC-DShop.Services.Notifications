using System;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Consul;
using DShop.Common.Consul;
using DShop.Common.Mongo;
using DShop.Common.Mvc;
using DShop.Common.RabbitMq;
using DShop.Services.Notifications.ServiceForwarders;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DShop.Common.RestEase;
using DShop.Common.Handlers;
using DShop.Common.MailKit;
using DShop.Services.Notifications.Templates;
using DShop.Services.Notifications.Events;
using DShop.Common.Dispatchers;

namespace DShop.Services.Notifications
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IContainer Container { get; private set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddCustomMvc();
            services.AddConsul();
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyTypes(Assembly.GetEntryAssembly())
                    .AsImplementedInterfaces();
            builder.Populate(services);
            builder.RegisterServiceForwarder<ICustomersApi>("customers-service");
            builder.AddDispatchers();
            builder.AddRabbitMq();
            builder.AddMongoDB();
            builder.AddMailKit();
            Container = builder.Build();

            return new AutofacServiceProvider(Container);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, 
            IApplicationLifetime applicationLifetime, IConsulClient client)
        {
            if (env.IsDevelopment() || env.EnvironmentName == "local")
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseErrorHandler();
            app.UseServiceId();
            app.UseMvc();
            app.UseRabbitMq()
                .SubscribeEvent<OrderCreated>();
            var consulServiceId = app.UseConsul();
            applicationLifetime.ApplicationStopped.Register(() => 
            { 
                client.Agent.ServiceDeregister(consulServiceId); 
                Container.Dispose(); 
            });
        }
    }
}
