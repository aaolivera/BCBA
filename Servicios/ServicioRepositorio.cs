using Dominio.Entidades;
using Repositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Servicios
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ServicioRepositorio" in both code and config file together.
    public class ServicioRepositorio : IServicioRepositorio
    {
        private readonly IRepositorio repositorio;
        public ServicioRepositorio(IRepositorio repositorio)
        {
            this.repositorio = repositorio;
        }

        public bool InsertarMovimiento(string empresa, Movimiento movimiento, string user)
        {
            if (repositorio.Existe<Jugada>(x => x.Movimiento.Any(y => y.Fecha == movimiento.Fecha && y.Cantidad == movimiento.Cantidad) && x.Empresa == empresa && x.Usuario == user)) 
            {
                return false;
            }
            var jugada = repositorio.Obtener<Jugada>(x => x.Movimiento.Sum(y => y.Cantidad) != 0 && x.Empresa == empresa && x.Usuario == user);
            if (jugada == null) 
            {
                jugada = new Jugada {Movimiento = new List<Movimiento>(), Empresa = empresa, Usuario = user};
                repositorio.Agregar(jugada);
            }
            jugada.Movimiento.Add(movimiento);
            repositorio.GuardarCambios();
            return true;
        }

        public IList<Jugada> ListarJugadas(string usuario)
        {
            return repositorio.Listar<Jugada>(x => x.Usuario == usuario);
        }

        public void BorrarMovimientos(string usuario)
        {
            foreach (var jugada in repositorio.Listar<Jugada>(x => x.Usuario == usuario))
            {
                repositorio.Remover(jugada);
            }
            repositorio.GuardarCambios();
        }

        public void BorrarJugada(string usuario, int id)
        {
            var j = repositorio.Obtener<Jugada>(x => x.Usuario == usuario && x.Id == id);
            repositorio.Remover(j);
            repositorio.GuardarCambios();
        }
    }
}
