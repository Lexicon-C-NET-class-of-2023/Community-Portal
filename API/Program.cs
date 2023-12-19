using API;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var serverVersion = new MySqlServerVersion(new Version(8, 0, 34));

IConfigurationRoot configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

//CORS
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var MyAllowAuthorizationHeader = "_myAllowAuthorizationHeader";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins, 
                            builder => builder.WithOrigins("http://127.0.0.1:3000", "http://localhost:3000")
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials());

    options.AddPolicy(MyAllowAuthorizationHeader,
                builder => builder.AllowAnyHeader().WithExposedHeaders("Authorization"));
});


// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options =>
options.UseMySql(configuration.GetConnectionString("Default"), serverVersion));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseStaticFiles();
app.UseRouting();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(MyAllowSpecificOrigins);
app.UseCors(MyAllowAuthorizationHeader);

app.UseAuthorization();

app.MapControllers();

app.Run();
