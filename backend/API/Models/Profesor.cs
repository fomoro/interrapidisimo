namespace API.Models
{
    public class Profesor
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public ICollection<Materia> Materias { get; set; }
    }
}
