
using System;
using System.Collections.Generic;
using System.Linq;
using Dominio.Entidades;

namespace Web.Models
{
    public class JugadaModels
    {
        public List<Jugada> Jugadas { get; set; }

        public string[] Empresas
        {
            get { return Jugadas.Select(x => x.Empresa).ToArray(); }
        }


        public List<double> PrecioEntrada
        {
            get {
                return Jugadas.Select(jugada => jugada.Movimiento.Where(x => x.Tipo == Dominio.Enum.Tipo.Entrada).Sum(y => (double) y.PrecioNeto*-1)).Select(movEntrada => Math.Round(movEntrada, 2)).ToList();
            }
        }

        public List<double> PrecioSalida
        {
            get
            {
                return Jugadas.Select(jugada => jugada.Movimiento.Where(x => x.Tipo == Dominio.Enum.Tipo.Salida).Sum(y => (double) y.PrecioNeto)).Select(movSalida => Math.Round(movSalida, 2)).ToList();
            }
        }
    }
}