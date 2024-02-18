using budgetManager.Data;
using budgetManager.Repositories.Interfaces;
using budgetManager.Repositories;
using budgetManager.Services.Interfaces;
using budgetManager.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var MyTestingPolicy = "_testingPolicy";

// Add services to the container.


// Cors policies
builder.Services.AddCors(options =>
{
    options.AddPolicy("_serverPolicy",
        policy =>
        {
            policy.WithOrigins("http://www.budgetManager.com");
        });

    options.AddPolicy(MyTestingPolicy,
        policy =>
        {
            policy.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

// Conexion with Sql Server 
builder.Services.AddDbContext<DataContext>(x =>
{
    x.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerConnection"));
});

builder.Services.AddAutoMapper(typeof(AuthService).Assembly);
LoadServices(builder.Services);
LoadRepositories(builder.Services);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(MyTestingPolicy);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

void LoadServices(IServiceCollection services)
{
    services.AddScoped<IAuthService, AuthService>();
    services.AddScoped<IMailService, MailService>();
    services.AddScoped<IUserService, UserService>();
}

void LoadRepositories(IServiceCollection services)
{
    services.AddScoped<IUserRepository, UserRepository>();
}
