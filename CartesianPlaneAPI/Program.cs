//Create a web app builder
var builder = WebApplication.CreateBuilder(args);

//Add configuration from the appsettings.json file
builder.Configuration.AddConfiguration(
    new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build()
);

//Add controllers
builder.Services.AddControllers();
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

//Execute the API
app.Run();


