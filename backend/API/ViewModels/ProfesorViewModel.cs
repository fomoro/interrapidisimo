// API/ViewModels/ProfesorViewModel.cs
namespace API.ViewModels
{
    public class ProfesorViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
    }

    public class ProfesorConMateriasViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public List<string> Materias { get; set; }
    }
}