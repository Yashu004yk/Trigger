

//using Microsoft.AspNetCore.Builder;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.EntityFrameworkCore;
//using WebApplication1.IRepository;
//using WebApplication1.Models;
//using WebApplication1.web;
//using WMS;
//using System.Configuration;






//namespace WebApplication1
//{
//    public class Startup
//    {

//        public void ConfigureServices(IServiceCollection services)
//        {
//            services.AddSingleton<NotificationListener>((provider) =>
//            {
//                var config = provider.GetRequiredService<IConfiguration>();
//                var connectionString = config.GetConnectionString("PostgreSQLConnection");
//                var channelName = config["NotificationSettings:ChannelName"];
//                return new NotificationListener(connectionString, channelName);
//            });

//            // Other services...
//        }

//        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
//        {
//            if (env.IsDevelopment())
//            {
//                app.UseSwagger();
//                app.UseSwaggerUI();
//            }

//            app.UseHttpsRedirection();

//            app.UseAuthorization();

//            app.UseRouting();

//            app.UseEndpoints(endpoints =>
//            {
//                endpoints.MapControllers();
//            });
//        }
//    }

//}
