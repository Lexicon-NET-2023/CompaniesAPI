﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Companies.API.Data;
using Companies.API.Extensions;
using Companies.API.Mappings;
using Companies.API.Middleware;
using Companies.API.Repositorys;

namespace Companies.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            // Add services to the container.

            builder.Services.AddControllers(options =>
            {
                options.ReturnHttpNotAcceptable = true;
            })
            .AddNewtonsoftJson();
                //.AddXmlDataContractSerializerFormatters();

            builder.Services.AddDbContext<APIContext>(options =>
              options.UseSqlServer(builder.Configuration.GetConnectionString("APIContext") 
              ?? throw new InvalidOperationException("Connection string 'APIContext' not found.")));

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddAutoMapper(typeof(CompanyMappings));
            builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
            builder.Services.AddScoped(provider => new Lazy<ICompanyRepository>(() => provider.GetRequiredService<ICompanyRepository>()));
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();    

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                await app.SeedDataAsync();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseDemo();

            //app.Map("/hej", builder =>
            //{
            //    builder.Run(async context =>
            //    {
            //        await context.Response.WriteAsync("Application return if route was hej!");
            //    });
            //});


            app.MapControllers();

            app.Run();
        }
    }
}
