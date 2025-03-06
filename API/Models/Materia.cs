using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
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
}
