using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Serilog;
using ToDoApp.Api.Data;
using ToDoApp.Api.Services.Implementations;
using ToDoApp.Api.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Default")
    ?? throw new InvalidOperationException("Connection string 'Default' is not configured.");

builder.Host.UseSerilog((context, configuration) =>
    {
        configuration.WriteTo.Console();

        var seqUrl = context.Configuration["Seq:ServerUrl"];

        if (string.IsNullOrWhiteSpace(seqUrl) is false)
        {
            configuration.WriteTo.Seq(seqUrl);
        }
    });



builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://vrwhinokcfhaqipqrnqn.supabase.co/auth/v1";
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateAudience = false,
            ValidIssuer = "https://vrwhinokcfhaqipqrnqn.supabase.co/auth/v1"
        };
    });

builder.Services.AddScoped<ITaskService, TaskService>();

builder.Services.AddAuthorization();

var app = builder.Build();

app.Use(async (context, next) =>
    {
        var logger = app.Services.GetRequiredService<ILogger<Program>>();

        using (logger.BeginScope(new Dictionary<string, object> { ["TraceId"] = context.TraceIdentifier}))
        {
            await next();
        }
    });

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
