using API.Data;
using API.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace API.Repositories
{
    public interface IRegistroRepository
    {
        void AgregarRegistro(Registro registro);        
    }

    public class RegistroRepository : IRegistroRepository
    {
        private readonly AppDbContext _context;

        public RegistroRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<Registro> ObtenerRegistros()
        {
            return _context.Registros.Include(r => r.Estudiante).Include(r => r.Materia).ToList();
        }

        public Registro ObtenerRegistroPorId(int id)
        {
            return _context.Registros.Include(r => r.Estudiante).Include(r => r.Materia).FirstOrDefault(r => r.Id == id);
        }

        public void AgregarRegistro(Registro registro)
        {
            _context.Registros.Add(registro);
            _context.SaveChanges();
        }

        public void ActualizarRegistro(Registro registro)
        {
            _context.Entry(registro).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void EliminarRegistro(int id)
        {
            var registro = _context.Registros.Find(id);
            if (registro != null)
            {
                _context.Registros.Remove(registro);
                _context.SaveChanges();
            }
        }
    }
}