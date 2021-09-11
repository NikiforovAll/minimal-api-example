var builder = WebApplication.CreateBuilder(args);

builder.AddSerilog();
builder.AddSwagger();
builder.AddAuthentication();
builder.AddAuthorization();
builder.Services.AddCors();
builder.AddStorage();

builder.Services.AddCarter();

var app = builder.Build();
var environment = app.Environment;

app
    .UseExceptionHandling(environment)
    .UseSwaggerEndpoints(routePrefix: string.Empty)
    .UseAppCors()
    .UseAuthentication()
    .UseAuthorization();

app.MapCarter();

app.Run();
