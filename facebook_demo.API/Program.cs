using System.Reflection.Metadata;
using facebook_demo.API.Extensions;
using facebook_demo.API.Middleware;
using facebook_demo.Repo;
using Microsoft.EntityFrameworkCore;
using MailService = facebook_demo.Service.MailService;
using JwtService = facebook_demo.Service.JwtService;
using CloudinaryService = facebook_demo.Service.CloudinaryService;
using MediaService = facebook_demo.Service.MediaService;
using FacebookService = facebook_demo.Service.FacebookService;


    var builder = WebApplication.CreateBuilder(args);
    
    builder.Services.AddControllers();
    // Add services to the container.
    builder.Services.AddEndpointsApiExplorer();

    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql(
            builder.Configuration.GetConnectionString("DefaultConnection")
        )
    );

    builder.Services.ConfigureRateLimiter();
    builder.Services.AddJwtServices(builder.Configuration);
    builder.Services.AddSwaggerServices();
    builder.Services.AddHttpContextAccessor();

    builder.Services.AddScoped<MailService.IService, MailService.Service>();
    builder.Services.AddScoped<JwtService.IService, JwtService.Service>();
    builder.Services.AddScoped<MediaService.IService, CloudinaryService.Service>();
    builder.Services.AddHttpClient<FacebookService.IService, FacebookService.Service>();
    builder.Services.AddTransient<GlobalExceptionHandlerMiddleware>();

    //builder.Services.AddValidatorsFromAssembly(AssemblyReference.Assembly);

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowFrontend", policy =>
        {
            policy
                .WithOrigins("http://localhost:4200")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
    });


    var app = builder.Build();
    app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwaggerAPI();
    }

    app.UseCors("AllowFrontend");

    app.UseRateLimiter();

    app.UseAuthentication();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
