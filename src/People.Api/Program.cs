using System.Net;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.OpenApi.Models;
using People.Application;
using People.Application.Exceptions;
using People.Application.Models;
using People.Infrastructure.Persistence;
using People.Infrastructure.Shared;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddSharedInfrastructure(builder.Configuration);
builder.Services.AddApplication(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(configurePolicy =>
    {
        configurePolicy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    options.JsonSerializerOptions.DictionaryKeyPolicy = null;
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    
    options.SwaggerDoc("v1", new OpenApiInfo()
    {
        Title = "People Api",
        Version = "v1",
        Description = "This Api will be responsible for overall data distribution and authorization.",
        Contact = new OpenApiContact
        {
            Name = "Contact Name",
            Email = "contact@mail.com",
            Url = new Uri("https://localhost/contact"),
        }
    });
});

var app = builder.Build();

app.UseCors();

if (!app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Extensions
app.UseGlobalException();

app.MapControllers();

app.Run();

public static class Extensions
{
    public static void UseGlobalException(this WebApplication app)
    {
        _ = app.UseExceptionHandler(exceptionHandlerApp =>
        {
            exceptionHandlerApp.Run(async context =>
            {
                var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                
                context.Response.ContentType = "application/json";
                ApiResponse responseModel;

                var error = exceptionHandlerPathFeature?.Error;

                if (error is CustomException ce)
                {
                    context.Response.StatusCode = (int) ce.StatusCode;
                    
                    responseModel = ApiResponse.Error(ce.ResponseCode, ce.Message, errorMessages: ce.ErrorMessages?.ToArray());
                }
                else
                {

                    if (!app.Environment.IsProduction())
                    {
                        responseModel = ApiResponse.Error(error?.Message ?? string.Empty);
                    }
                    else
                    {
                        // TODO log
                        responseModel = ApiResponse.Error("Please, contact the support service.");
                    }
                
                    switch (error)
                    {
                        case KeyNotFoundException e:
                            // not found error
                            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                            break;
                        default:
                            // unhandled error
                            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            break;
                    }
                }

                var jsonSerializerOptions = new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                    DictionaryKeyPolicy = null,
                    PropertyNamingPolicy = null
                };

                await context.Response.WriteAsJsonAsync(responseModel, jsonSerializerOptions);
            });
        });
    }
}