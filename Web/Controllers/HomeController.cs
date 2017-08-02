using Dominio.Entidades;
using Servicios;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Dominio.Enum;
using GoogleApi;
using Web.Models;
using static System.String;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        IServicioRepositorio servicio;
        public HomeController(IServicioRepositorio servicio)
        {
            this.servicio = servicio;
        }
        public ActionResult Index(string mensaje)
        {
            var jugadas = new List<Jugada>();
            if (User.Identity.IsAuthenticated)
            {
                jugadas = servicio.ListarJugadas(User.Identity.Name).ToList();
            }

            if (!IsNullOrEmpty(mensaje))
            {
                ViewBag.Mensaje = mensaje;
            }
            
            return View(new JugadaModels {Jugadas = jugadas });
        }


        public ActionResult ObtenerUltimoValor(string empresa)
        {
            return Json(new { data = Google.GetLastValue(empresa) }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BorrarJugadas()
        {
            if (User.Identity.IsAuthenticated)
            {
                servicio.BorrarMovimientos(User.Identity.Name);
            }
            return RedirectToAction("Index");
        }

        public ActionResult BorrarJugada(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                servicio.BorrarJugada(User.Identity.Name, id);
            }
            return RedirectToAction("Index");
        }

        public ActionResult DescargarJugadas()
        {
            var archivo = string.Empty;
            if (User.Identity.IsAuthenticated)
            {
                archivo = servicio.ListarJugadas(User.Identity.Name)
                    .Aggregate(archivo,
                        (current1, jugada) =>
                            jugada.Movimiento.Aggregate(current1,
                                (current, m) =>
                                    current +
                                    $"{m.Fecha}\t{(m.Tipo == Tipo.Entrada ? "Compra" : "Venta")}\t{m.Jugada.Empresa}\t{(m.Cantidad * (m.Tipo == Tipo.Salida ? -1 : 1))}\t{m.PrecioUnitario}\tMERVAL\t0\t0\t0\t{(m.PrecioNeto * (m.Tipo == Tipo.Entrada ? -1 : 1))}\n"));
            }
            return File(Encoding.UTF8.GetBytes(archivo),
                 "text/plain","bak"+DateTime.Now.ToString("d") + ".txt");
        }

        [HttpPost]
        public ActionResult IngresarDatos(HttpPostedFileBase file)
        {
            var mensaje = "";
            if (file != null && User.Identity.IsAuthenticated)
            {
                try
                {
                    var cambios = 0;
                    if (file.InputStream != null)
                    {
                        var reader = new StreamReader(file.InputStream);
                        while (!reader.EndOfStream)
                        {
                            var line = reader.ReadLine();
                            var values = line.Split('\t');
                            DateTime fecha;
                            if (values.Length > 0 && DateTime.TryParse(values[0], CultureInfo.GetCultureInfo("es-AR"),DateTimeStyles.None, out fecha))
                            {
                                var tipo = (values[1] == "Venta" ? Tipo.Salida : Dominio.Enum.Tipo.Entrada);
                                var cantidad = int.Parse(values[3].Split(',')[0], NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands, CultureInfo.GetCultureInfo("es-AR")) * (tipo == Dominio.Enum.Tipo.Entrada ? 1 : -1);
                                var precioUnitario = decimal.Parse(values[4].Replace("$", ""), CultureInfo.GetCultureInfo("es-AR"));
                                var precioNeto = decimal.Parse(values[9].Replace("$", ""), CultureInfo.GetCultureInfo("es-AR")) * (tipo == Tipo.Entrada ? -1 : 1);
                                var movimiento = new Movimiento { Fecha = fecha, Cantidad = cantidad, PrecioUnitario = precioUnitario, PrecioNeto = precioNeto, Tipo = tipo };
                                cambios += servicio.InsertarMovimiento(values[2], movimiento, User.Identity.Name) ? 1 : 0;
                            }
                        }
                    }
                    mensaje += $"Se insertaron {cambios} movimientos";
                }
                catch (Exception ex)
                {
                    mensaje = ex.Message;
                }
            }
            
            return RedirectToAction("Index", "Home", new { mensaje });
        }
        
    }
}