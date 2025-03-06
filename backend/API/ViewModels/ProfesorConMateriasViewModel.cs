// API/ViewModels/ProfesorConMateriasViewModel.cs
using System.Collections.Generic;

namespace API.ViewModels
{
    public class ProfesorConMateriasViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public List<string> Materias { get; set; }
    }
}