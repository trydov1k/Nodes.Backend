using Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddContext();
builder.Services.AddRepositories();
builder.Services.AddServices();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapControllers();

app.UseSwagger();
app.UseSwaggerUI();
app.Run();