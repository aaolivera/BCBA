using Dominio.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entidades
{
    public class Movimiento
    {
        [Key]
        public virtual int Id { get; set; }
        public virtual DateTime Fecha { get; set; }
        public virtual decimal PrecioUnitario { get; set; }
        public virtual decimal PrecioNeto { get; set; }
        public virtual int Cantidad { get; set; }
        public virtual Tipo Tipo { get; set; }
        public virtual Jugada Jugada { get; set; }
    }
}
