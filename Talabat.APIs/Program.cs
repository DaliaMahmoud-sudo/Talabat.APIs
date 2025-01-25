using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System.ComponentModel;
using Talabat.APIs.Errors;
using Talabat.APIs.Extensions;
using Talabat.APIs.Helpers;
using Talabat.APIs.Middlewares;
using Talabat.Core.Entites;
using Talabat.Core.Entites.Identity;
using Talabat.Core.Repositories;
using Talabat.Repository;
using Talabat.Repository.Data;
using Talabat.Repository.Identity;

public class Program {
    public static async Task Main(string[] args) {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        #region Configure Service
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddDbContext<StoreContext>(Options =>
        {
            Options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
        });

        builder.Services.AddDbContext<AppIdentityDbContext>(Options =>
        {
            Options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));   
        });
        builder.Services.AddSingleton<IConnectionMultiplexer>(Options =>
        {
            var Connection = builder.Configuration.GetConnectionString("RedisConnection");
            return ConnectionMultiplexer.Connect(Connection);
        });
        builder.Services.AddAplicationServices();
       
        builder.Services.AddIdentityServices(builder.Configuration);
        #endregion

        var app = builder.Build();

        #region Update-Database
        using var Scope = app.Services.CreateScope();
        var Services = Scope.ServiceProvider;
        var LoggerFactory = Services.GetRequiredService<ILoggerFactory>();
        try
        {

            var DbContext = Services.GetRequiredService<StoreContext>();
            await DbContext.Database.MigrateAsync();

            var IdentityDbContext = Services.GetRequiredService<AppIdentityDbContext>();

            await IdentityDbContext.Database.MigrateAsync();
            var UserManager = Services.GetRequiredService<UserManager<AppUser>>();
            await AppIdentityDbContextSeed.SeedUserAsync(UserManager);
            await StoreContextSeed.SeedAsync(DbContext);
        }
        catch (Exception ex)
        {
            var Logger = LoggerFactory.CreateLogger<Program>();
            Logger.LogError(ex, "an error accured during applying the migration");

        }
        #endregion

        #region Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseMiddleware<ExceptionMiddleWare>();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseStatusCodePagesWithReExecute("/errors/{0}");
        
        app.UseHttpsRedirection();

        app.UseStaticFiles();

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();
        #endregion

        app.Run();

    }
}