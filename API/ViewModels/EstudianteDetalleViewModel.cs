namespace API.ViewModels
{
    public class EstudianteDetalleViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Carrera { get; set; }
        public List<string> Materias { get; set; }
    }
}