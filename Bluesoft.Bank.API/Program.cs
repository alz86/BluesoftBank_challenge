using Bluesoft.Bank.API.Middlewares;
using Bluesoft.Bank.Business;
using Bluesoft.Bank.DataAccess.EF;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//API Versioning
builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.DefaultApiVersion = new ApiVersion(1, 0); // Set the default API version
    options.AssumeDefaultVersionWhenUnspecified = true; // Assume default version when none specified
});

//business module configuration
builder.Services.UseBusinessModule();
builder.Services.UseEFModule(builder.Configuration.GetConnectionString("MainDb"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseMiddleware<SaveChangesMiddleware>();

app.Run();
