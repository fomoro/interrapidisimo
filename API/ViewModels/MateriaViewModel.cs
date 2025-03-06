// API/ViewModels/MateriaViewModel.cs
namespace API.ViewModels
{
    public class MateriaViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int Creditos { get; set; }
        public string Profesor { get; set; } // Nombre del profesor
    }
}