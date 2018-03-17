using System;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using DShop.Common.Mongo;
using DShop.Common.Mvc;
using DShop.Common.RabbitMq;
using DShop.Services.Notifications.ServiceForwarders;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DShop.Common.RestEase;
using DShop.Messages.Events.Orders;
using DShop.Common.Handlers;
using DShop.Common.MailKit;
using DShop.Services.Notifications.Templates;

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
            services.AddMvc().AddDefaultJsonOptions();
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyTypes(Assembly.GetEntryAssembly())
                    .AsImplementedInterfaces();

            builder.RegisterServiceForwarder<ICustomersApi>("customers-service");

            builder.Populate(services);
            builder.AddRabbitMq();
            builder.AddMongoDB();
            builder.AddMailKit();
            Container = builder.Build();

            Container.Resolve<IEventHandler<OrderCreated>>().HandleAsync(null, null);

            return new AutofacServiceProvider(Container);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, 
            IApplicationLifetime applicationLifetime)
        {
            if (env.IsDevelopment() || env.EnvironmentName == "local")
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseMvc();
            app.UseErrorHandler();
            app.UseRabbitMq()
                .SubscribeEvent<OrderCreated>();
            applicationLifetime.ApplicationStopped.Register(() => Container.Dispose());
        }
    }
}
