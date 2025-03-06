using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
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
