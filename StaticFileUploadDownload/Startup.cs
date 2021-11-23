using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;//�D wwwroot ��Ƨ����d�ҷ|�Ψ�
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;//�D wwwroot ��Ƨ����d�ҷ|�Ψ�
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;//�D wwwroot ��Ƨ����d�ҷ|�Ψ�
using System.Linq;
using System.Threading.Tasks;

namespace StaticFileUploadDownload
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
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles(); // ���F wwwroot ��Ƨ�

            app.UseStaticFiles(new StaticFileOptions() // ���F�D wwwroot ��Ƨ�
            {
                FileProvider = new PhysicalFileProvider(
                            Path.Combine(Directory.GetCurrentDirectory(), @"Download")), //Download ����Ƨ��W��
                RequestPath = new PathString("/app-download")
                // "/app-download" �����}�A�p�n�U�� Download ��Ƨ����� readme.docx �A���}���Ghttps://localhost:<port>/app-download/readme.docx
            });


            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
