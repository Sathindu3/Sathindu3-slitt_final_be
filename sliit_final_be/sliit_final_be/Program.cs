var builder = WebApplication.CreateBuilder(args);

// Read configuration
var roboflowConfig = builder.Configuration.GetSection("Roboflow");
string apiKey = roboflowConfig["ApiKey"];
string modelEndpoint = roboflowConfig["ModelEndpoint"];

// Add services
builder.Services.AddControllers();
builder.Services.AddHttpClient();

// Register RoboflowService with required parameters
builder.Services.AddScoped<RoboflowService>(_ => new RoboflowService(apiKey, modelEndpoint));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
