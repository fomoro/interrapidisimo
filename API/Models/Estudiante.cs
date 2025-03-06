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
}
