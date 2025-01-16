using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using Talabat.APIs.Errors;
using Talabat.APIs.Extensions;
using Talabat.APIs.Helpers;
using Talabat.APIs.Middlewares;
using Talabat.Core.Entites;
using Talabat.Core.Repositories;
using Talabat.Repository;
using Talabat.Repository.Data;

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

        builder.Services.AddAplicationServices();
 


        #endregion

        var app = builder.Build();

        #region Update-Database
        using var Scope = app.Services.CreateScope();
        var Services = Scope.ServiceProvider;
        var LoggerFactory = Services.GetRequiredService<ILoggerFactory>();
        try
        {

            var dbContext = Services.GetRequiredService<StoreContext>();
            await dbContext.Database.MigrateAsync();
            await StoreContextSeed.SeedAsync(dbContext);
        }
        catch (Exception ex)
        {
            var Logger = LoggerFactory.CreateLogger<Program>();
            Logger.LogError(ex, "an error accured during applying the migration");

        }
        #endregion

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseMiddleware<ExceptionMiddleWare>();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.UseStaticFiles();

        app.MapControllers();
        

        app.Run();

    }
}