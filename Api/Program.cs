using Data;
using Application;
using Domain.DTOs;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

// Загрузка .env
DotNetEnv.Env.TraversePath().Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMapping();
builder.Services.AddContext(builder.Configuration);
builder.Services.AddRepositories();
builder.Services.AddServices();

if (builder.Environment.IsDevelopment())
    builder.Services.AddSwaggerGen();

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var errors = context.ModelState
                .Where(e => e.Value?.Errors.Count > 0)
                .SelectMany(e => e.Value!.Errors.Select(err =>
                    string.IsNullOrEmpty(err.ErrorMessage)
                        ? "Invalid value"
                        : err.ErrorMessage))
                .ToList();
            var message = string.Join("; ", errors);
            return new BadRequestObjectResult(ApiResponse<object>.Error(message));
        };
    });


var app = builder.Build();

app.UseExceptionHandler(appBuilder => appBuilder.Run(async context =>
{
    var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

    var (statusCode, message) = exception switch
    {
        KeyNotFoundException ex => (StatusCodes.Status404NotFound, ex.Message),
        ArgumentException ex => (StatusCodes.Status400BadRequest, ex.Message),
        _ => (StatusCodes.Status500InternalServerError, "Internal server error")
    };

    context.Response.StatusCode = statusCode;
    await context.Response.WriteAsJsonAsync(ApiResponse<object>.Error(message));
}));

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();   
}
app.MapControllers();
app.Run();