using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Estudiante> Estudiantes { get; set; }
        public DbSet<Profesor> Profesores { get; set; }
        public DbSet<Materia> Materias { get; set; }
        public DbSet<Registro> Registros { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configurar relaciones y restricciones
            modelBuilder.Entity<Registro>()
                .HasOne(r => r.Estudiante)
                .WithMany(e => e.Registros)
                .HasForeignKey(r => r.EstudianteId);

            modelBuilder.Entity<Registro>()
                .HasOne(r => r.Materia)
                .WithMany(m => m.Registros)
                .HasForeignKey(r => r.MateriaId);

            modelBuilder.Entity<Materia>()
                .HasOne(m => m.Profesor)
                .WithMany(p => p.Materias)
                .HasForeignKey(m => m.ProfesorId);
        }
    }
}
