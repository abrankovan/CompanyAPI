using CompanyAPI;
using CompanyAPI.DbContexts;
using CompanyAPI.Services;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<CompanyContext>(
	dbContextOptions => dbContextOptions.UseSqlite(
	builder.Configuration["ConnectionStrings:CompanyAPIDBConnectionString"]));
builder.Services.AddScoped<IEmployeeInfoRepository, EmployeeInfoRepository>();
builder.Services.AddScoped<ITaskInfoRepository, TaskInfoRepository>();
builder.Services.AddScoped<IDepartmentInfoRepository, DepartmentInfoRepository>();
builder.Services.AddScoped<IStatisticsService, StatisticsService>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
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

app.Run();
