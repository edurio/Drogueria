using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;

namespace Reportes
{
    public partial class rptSolicitud : DevExpress.XtraReports.UI.XtraReport
    {
        public rptSolicitud()
        {
            InitializeComponent();
        }
        public void Cargar(Entidades.Solicitud solicitud, List<Entidades.DetalleSolicitud> detalleSolicitud)
        {
            Entidades.Filtro filtro = new Entidades.Filtro();
            filtro.Id = solicitud.Id;
            var lista = DAL.SolicitudDAL.ObtenerSolicitud(filtro);

            txtFechaIngreso.Text = lista[0].Fecha_Ingreso.ToString();
            txtFolio.Text = lista[0].Folio.ToString();
            txtSolicitante.Text = lista[0].UsuarioCreador;
            txtEstablecimiento.Text = " ";
            txtPrioridad.Text = lista[0].Prioridad;
            txtEstado.Text = lista[0].Estado;
            txtObservacion.Text = lista[0].Observacion_Solicitud;
            txtTipo.Text = lista[0].Tipo;

            DataSet dataSet1 = new DataSet();
            dataSet1.DataSetName = "dataSet";
            DataTable dataTable1 = new DataTable();

            dataSet1.Tables.Add(dataTable1);
            dataTable1.TableName = "Table";

            
            dataTable1.Columns.Add("Articulo", typeof(string));
            dataTable1.Columns.Add("Consumo", typeof(string));
            dataTable1.Columns.Add("Factor", typeof(string));
            dataTable1.Columns.Add("Cantidad", typeof(string));
            dataTable1.Columns.Add("Unidad", typeof(string));
            dataTable1.Columns.Add("Observación", typeof(string));

            foreach (var d in detalleSolicitud)
            {
                dataTable1.Rows.Add(new object[] {  d.ProductoStr, d.Consumo, d.Factor, d.Cantidad, d.Unidad, d.Observacion });
            }

            this.DataSource = dataSet1;
            this.DataMember = dataTable1.TableName;

            
            this.tcArticulo.DataBindings.Add("Text", null, dataTable1.Columns[0].Caption);
            this.tcConsumo.DataBindings.Add("Text", null, dataTable1.Columns[1].Caption);
            this.tcFactor.DataBindings.Add("Text", null, dataTable1.Columns[2].Caption);
            this.tcCantidad.DataBindings.Add("Text", null, dataTable1.Columns[3].Caption);
            this.tcUnidad.DataBindings.Add("Text", null, dataTable1.Columns[4].Caption);
            this.tcObservación.DataBindings.Add("Text", null, dataTable1.Columns[5].Caption);
        }
    }
}
