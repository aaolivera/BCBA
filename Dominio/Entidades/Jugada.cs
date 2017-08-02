using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entidades
{
    public class Jugada
    {
        [Key]
        public virtual int Id { get; set; }
        public virtual ICollection<Movimiento> Movimiento { get; set; }
        public virtual string Empresa { get; set; }
        public virtual string Usuario { get; set; }

        public bool Cerrada() { return Movimiento.Sum(x => x.Cantidad) == 0; }
        public int Cantidad() { return MovimientosEntrada().Sum(x => x.Cantidad); }
        public List<Movimiento> MovimientosEntrada() { return Movimiento.Where(x => x.Tipo == Enum.Tipo.Entrada).ToList(); }
        public List<Movimiento> MovimientosSalida() { return Movimiento.Where(x => x.Tipo == Enum.Tipo.Salida).ToList(); }

        public string FechaEntrada() { return MovimientosEntrada().Any() ? MovimientosEntrada().Min(x => x.Fecha).ToString("dd/MM/yy"): "?"; }
        public string FechaSalida() { return MovimientosSalida().Any() ? MovimientosSalida().Max(x => x.Fecha).ToString("dd/MM/yy") : "?"; }
        public string Dias()
        {
            return MovimientosEntrada().Any() && MovimientosSalida().Any() ? (MovimientosSalida().Max(x => x.Fecha).DayOfYear - MovimientosEntrada().Min(x => x.Fecha).DayOfYear).ToString() : "?";
        }

        public string PrecioSalida()
        {
            return MovimientosSalida().Any() ? Math.Round(MovimientosSalida().Sum(x => x.PrecioUnitario * x.Cantidad)/ MovimientosSalida().Sum(x => x.Cantidad), 2).ToString(CultureInfo.InvariantCulture) : "-";
        }
        public string PrecioEntrada()
        {
            return MovimientosEntrada().Any() ? Math.Round(MovimientosEntrada().Sum(x => x.PrecioUnitario * x.Cantidad) / MovimientosEntrada().Sum(x => x.Cantidad), 2).ToString(CultureInfo.InvariantCulture) : "-";
        }

        public string Inversion()
        {
            return MovimientosEntrada().Any() ? Math.Round(MovimientosEntrada().Sum(x => x.PrecioNeto) * -1, 2).ToString(CultureInfo.InvariantCulture) : "-";
        }

        public string Ganancia()
        {
            return MovimientosEntrada().Any() && MovimientosSalida().Any() ? Math.Round(MovimientosSalida().Sum(x => x.PrecioNeto) + MovimientosEntrada().Sum(x => x.PrecioNeto), 2).ToString(CultureInfo.InvariantCulture) : "-";
        }

        public decimal Porcentaje()
        {
            return Math.Round((MovimientosEntrada().Any() && MovimientosSalida().Any() ? Math.Round(MovimientosSalida().Sum(x => x.PrecioNeto) + MovimientosEntrada().Sum(x => x.PrecioNeto), 2) : 0) * 100 / (MovimientosEntrada().Any() ? Math.Round(MovimientosEntrada().Sum(x => x.PrecioNeto) * -1, 2) : 0), 2);
        }
    }
}
