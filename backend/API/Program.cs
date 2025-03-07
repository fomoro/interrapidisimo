using System.Text.Json.Serialization;
using API.Data;
using API.Mapping;
using API.Repositories;
using API.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Registrar el DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrar el repositorio
builder.Services.AddScoped<EstudianteRepository, EstudianteRepository>();

// Registrar servicios
builder.Services.AddScoped<IEstudianteService, EstudianteService>();
builder.Services.AddScoped<IMateriaService, MateriaService>();
builder.Services.AddScoped<IProfesorService, ProfesorService>();
builder.Services.AddScoped<IRegistroService, RegistroService>();


var autoMapperProfiles = new[]
{
    typeof(EstudianteProfile),
    typeof(ProfesorProfile),
    typeof(MateriaProfile),
    typeof(RegistroProfile)
};

// Otros servicios
builder.Services.AddAutoMapper(autoMapperProfiles);
builder.Services.AddControllers();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
