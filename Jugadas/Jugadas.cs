using Dominio.Entidades;
using Servicios;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Jugadas
{
    public partial class Jugadas : Form
    {
        private readonly IServicioRepositorio repositorio;
        public Jugadas(IServicioRepositorio repositorio)
        {
            this.repositorio = repositorio;
            InitializeComponent();
        }

        private void cargarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stream myStream = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "xls files (*.xls)|*.xls";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var cambios = 0;
                    if ((myStream = openFileDialog1.OpenFile()) != null)
                    {
                        var reader = new StreamReader(myStream);
                        while (!reader.EndOfStream)
                        {
                            var line = reader.ReadLine();
                            var values = line.Split('\t');
                            DateTime fecha;
                            if (values.Length > 0 && DateTime.TryParse(values[0], out fecha))
                            {
                                var tipo = (values[1] == "Venta" ? Dominio.Enum.Tipo.Salida : Dominio.Enum.Tipo.Entrada);
                                var cantidad = int.Parse(values[3].Split(',')[0].Replace(".", "")) * (tipo == Dominio.Enum.Tipo.Entrada ? 1 : -1);
                                var precioUnitario = decimal.Parse(values[4].Replace("$", ""));
                                var precioNeto = decimal.Parse(values[8].Replace("$", "")) * (tipo == Dominio.Enum.Tipo.Entrada ? -1 : 1);
                                var movimiento = new Movimiento { Fecha = fecha,Cantidad = cantidad, PrecioUnitario = precioUnitario, PrecioNeto = precioNeto, Tipo = tipo };
                                cambios = repositorio.InsertarMovimiento(values[2],movimiento) ? 1 : 0;
                            }
                        }
                    }
                    MessageBox.Show(String.Format("Se insertaron {0} movimientos",cambios.ToString()));
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void Jugadas_Load(object sender, EventArgs e)
        {
            var jugadas = repositorio.ListarJugadas();
            foreach (var jugada in jugadas)
            {
                var linea = new System.Windows.Forms.DataVisualization.Charting.Series();
                linea.Name = jugada.Empresa;
                linea.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bar;

                chart1.Series[jugada.Empresa] = linea;
                var entrada = jugada.Movimiento.Where(x => x.Tipo == Dominio.Enum.Tipo.Entrada);
                var salida = jugada.Movimiento.Where(x => x.Tipo == Dominio.Enum.Tipo.Salida);

                if (entrada.Any())
                {
                    linea.Points.AddXY(entrada.First().Fecha, entrada.Sum(x => x.PrecioNeto));
                }
                if (salida.Any())
                {
                    linea.Points.AddXY(salida.Last().Fecha, salida.Sum(x => x.PrecioNeto));
                }
            }
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
    }
}
