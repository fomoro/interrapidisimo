namespace API.ViewModels
{
    public class RegistroViewModel
    {
        public int Id { get; set; }
        public int EstudianteId { get; set; }
        public int MateriaId { get; set; }
    }
    public class CompaneroViewModel
    {
        public string Nombre { get; set; }
        public List<string> Materias { get; set; }
    }
    public class VisibilidadViewModel
    {
        public string Estudiante { get; set; }
        public string Materia { get; set; }
        public string Profesor { get; set; }
    }


}
