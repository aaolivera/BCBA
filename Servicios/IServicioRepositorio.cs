using Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Servicios
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IServicioRepositorio" in both code and config file together.
    [ServiceContract]
    public interface IServicioRepositorio
    {
        [OperationContract]
        bool InsertarMovimiento(string empresa, Movimiento movimiento, string user);

        [OperationContract]
        IList<Jugada> ListarJugadas(string usuario);

        [OperationContract]
        void BorrarMovimientos(string usuario);

        [OperationContract]
        void BorrarJugada(string usuario, int id);
    }
}
