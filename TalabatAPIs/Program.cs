using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using Talabat.Core.Data;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Service_Contract;
using Talabat.Repository.Data;
using Talabat.Repository.IdentityStore;
using Talabat.Repository.IdentityStore.IdentityDataSeed;
using Talabat.Repository.Repository;
using Talabat.Service;
using TalabatAPIs.Errors;
using TalabatAPIs.Extensions;
using TalabatAPIs.Helper;
using TalabatAPIs.Middlewares;

namespace TalabatAPIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            #region Configration Services
            builder.Services.AddControllers();

            builder.Services.AddSwaggerServices();

            builder.Services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));

            builder.Services.AddDbContext<StoreContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnetion"));
            });

            builder.Services.AddDbContext<AppIdentityDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnetion"));
            });

            builder.Services.AddIdentityServices(builder.Configuration);
                
            builder.Services.AddSingleton<IConnectionMultiplexer>(S =>
            {

                try
                {
                    var connection = builder.Configuration.GetConnectionString("Redis");
                    return ConnectionMultiplexer.Connect(connection);
                }
                catch (Exception ex)
                {
                    throw new Exception();
                }
            });

            builder.Services.AddApplicationServices();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("MyPolicy", options =>
                {
                    //We should add origin for detecting specific Angular URL
                    //using .WithOrigins();
                    options.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                });
            });

            #endregion

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            #region Cofigration Kestrel and Middlewares

            app.UseMiddleware<ExeptionMiddleware>();

            #region Update Database and Depentancy Injection
            using var Scope = app.Services.CreateScope();
            var Services = Scope.ServiceProvider;
            var _dbContext = Services.GetRequiredService<StoreContext>();
            var _IdentitydbContext = Services.GetRequiredService<AppIdentityDbContext>();
            var Ilogger = Services.GetRequiredService<ILoggerFactory>();

            try
            {
                await _dbContext.Database.MigrateAsync();
                await SeedData.Seedasync(_dbContext, Ilogger);

                var _userManager = Services.GetRequiredService<UserManager<AppUser>>();
                await _IdentitydbContext.Database.MigrateAsync();
                await AppIdentityDbContextSeed.SeedUserAsync(_userManager);
            }
            catch (Exception ex)
            {
                var logger = Ilogger.CreateLogger<Program>();
                logger.LogError(ex, "There is an Error Occured during the Update of the DataBase");
            } 

            #endregion

            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerMiddlewares();
            }

            app.UseStatusCodePagesWithRedirects("/errors/{0}");

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            
            app.MapControllers();

            app.UseCors("MyPolicy");

            app.UseAuthentication();

            app.UseAuthorization();

            #endregion

            app.Run();
        }
    }
}