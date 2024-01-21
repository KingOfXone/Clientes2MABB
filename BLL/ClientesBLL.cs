using ClientesMABB.DAL;
using ClientesMABB.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ClientesMABB.BLL
{
    public class ClientesBLL
    {
        private Contexto Contexto;

        public ClientesBLL(Contexto contexto)
        {
            Contexto = contexto;
        }

        public bool Existe(int id)
        {
            return Contexto.Clientes.Any(o => o.ClienteId == id);
        }
        private bool Insertar(Cliente clientes)
        {
            Contexto.Clientes.Add(clientes);
            return Contexto.SaveChanges() > 0;
        }

        private bool Modificar(Cliente cliente)
        {
            var PrioridadADesechar = Contexto.Clientes.Find(cliente.ClienteId);
            if (cliente != null)
            {
                Contexto.Entry(PrioridadADesechar).State = EntityState.Detached;
                Contexto.Entry(cliente).State = EntityState.Modified;
                return Contexto.SaveChanges() > 0;
            }
            return false;

        }

        public bool Guardar(Cliente cliente)
        {
            if (Contexto.Clientes.Any(p => p.ClienteId != cliente.ClienteId && p.Rnc == cliente.Rnc || p.Nombres == cliente.Nombres))
            {
                return false;
            }
            if (!Existe(cliente.ClienteId))
                return Insertar(cliente);
            else
                return Modificar(cliente);
        }

        public bool Eliminar(Cliente cliente)
        {


            if (cliente != null)
            {
                Contexto.Entry(cliente).State = EntityState.Deleted;
                return Contexto.SaveChanges() > 0;
            }
            return false;

        }

        public Cliente? Buscar(int id)
        {
            return Contexto.Clientes.Where(o => o.ClienteId == id).AsNoTracking().SingleOrDefault(); ;
        }

        public List<Cliente> BuscarPorId(int id)
        {
            return Contexto.Clientes.AsNoTracking().Where(c => c.ClienteId == id).ToList();
        }

        public List<Cliente> GetList(Expression<Func<Cliente, bool>> criterio)
        {
            return Contexto.Clientes.AsNoTracking().Where(criterio).ToList();
        }
        public List<Cliente> GetList()
        {
            return Contexto.Clientes.AsNoTracking().ToList();
        }
    }
}
