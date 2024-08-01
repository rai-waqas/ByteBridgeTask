using Microsoft.EntityFrameworkCore;
using DataAccess.DataContext;
using DataAccess.Interfaces;
using DataAccess.Repositories;
using Business.Services;
using Business.MappingProfile;
using AutoMapper;

Console.WriteLine("Hello World!");

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddDbContext<DBContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSqlConnection")));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IFilesRepository, FilesRepository>();
builder.Services.AddScoped<IStateRepository, StateRepository>();
builder.Services.AddScoped<IClientDetailsRepository, ClientDetailsRepository>();
builder.Services.AddScoped<IClientDetailsService, ClientDetailsService>();
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IFilesService, FilesService>();
builder.Services.AddScoped<IStateService, StateService>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularDevServer",
        builder =>
        {
            builder.WithOrigins("http://localhost:4200")
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowAngularDevServer");
app.UseHttpsRedirection();
app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
app.Run();