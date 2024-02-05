
using System.Text.Json.Serialization;
using Amazon.Extensions.Configuration.SystemsManager;
using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    ApplicationName = typeof(Program).Assembly.FullName,
    ContentRootPath = Directory.GetCurrentDirectory(),
});

builder.Configuration.AddSystemsManager(
    path: $"/app-settings/api/{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}/",
    reloadAfter: TimeSpan.FromMinutes(10));
builder.Configuration.WaitForSystemsManagerReloadToComplete(TimeSpan.FromSeconds(2));

builder.Services.Configure<FormOptions>(options =>
{
    options.ValueLengthLimit = int.MaxValue;
    options.MultipartBodyLengthLimit = int.MaxValue; // if don't set
    //default value is: 128 MB
    options.MultipartHeadersLengthLimit = int.MaxValue;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

/* Lambda */
builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);

/* HTTP */
builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

// Use CORS policy
app.UseCors(policy => policy.AllowAnyHeader()
                            .AllowAnyMethod()
                            .SetIsOriginAllowed(origin => true)
                            .AllowCredentials());

app.UseAuthentication();

app.MapControllers();

app.UseAuthorization();

app.Run();
