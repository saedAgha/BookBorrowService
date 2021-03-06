using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using BookBorrowService.Repositiories;
using BookBorrowService.Services;
using BookBorrowService.Services.Validators;
using Microsoft.EntityFrameworkCore;

namespace BookBorrowService
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
            services.AddDbContext<BookRequestContext>(opt =>
                opt.UseInMemoryDatabase("BooksBorrow"));

            services.AddTransient<IUserServiceValidator, UserServiceValidator>();
            services.AddTransient<IBookRequestRepository, BookRequestRepository>();
            services.AddTransient<IUserService,UserService>();
            services.AddTransient<IBookService, Services.BookService>();
            services.AddTransient<IBookRequestValidator, BookRequestValidator>();
            services.AddTransient<IBookRequestService, BookRequestService>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "BookBorrowService", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BookBorrowService v1"));
            }

            app.UseHttpsRedirection();
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
