//Create a web app builder
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

//Add configuration from the appsettings.json file
builder.Configuration.AddConfiguration(
    new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build()
);

//Add controllers, return application/json MIME type only
//Ignore properties with null values
builder.Services.AddControllers(options => options.Filters.Add(new ProducesAttribute("application/json")))
    .AddJsonOptions(options =>
    { 
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    }); 

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//Enable Swagger UI
app.UseSwagger();
app.UseSwaggerUI();

//Enable HTTPS
app.UseHttpsRedirection();

//Enable (map) controllers
app.MapControllers();

//Execute the program
app.Run();


