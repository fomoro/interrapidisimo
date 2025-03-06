using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class Estudiante
    {
        public int Id { get; set; }
        public string Nombre { get; set; }        
        public string Carrera { get; set; }
        public ICollection<Registro> Registros { get; set; }
    }
    public class Materia
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int Creditos { get; set; } = 3;

        [Column("profesor_id")]
        public int? ProfesorId { get; set; }
        public Profesor Profesor { get; set; }
        public ICollection<Registro> Registros { get; set; }
    }
    public class Profesor
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public ICollection<Materia> Materias { get; set; }
    }
    public class Registro
    {
        public int Id { get; set; }
        [Column("estudiante_id")]
        public int EstudianteId { get; set; }
        public Estudiante Estudiante { get; set; }
        [Column("materia_id")]
        public int MateriaId { get; set; }
        public Materia Materia { get; set; }
    }
}
