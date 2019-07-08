using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;



namespace GoogleMaps
{
    public partial class Form1 : Form
    {
        GMarkerGoogle marker;
        GMapOverlay markeroverlay;
        DataTable dt;
        private int filaselecionada = 0;
        double latInicial = -23.64339592;
        double lngInicial = -46.63429248;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dt = new DataTable();
            dt.Columns.Add(new DataColumn("Descrição", typeof(string)));
            dt.Columns.Add(new DataColumn("lat:", typeof(double)));
            dt.Columns.Add(new DataColumn("long:", typeof(double)));

            dt.Rows.Add("Marcação 1", latInicial, lngInicial);
            dataGridView1.DataSource = dt;

            dataGridView1.Columns[1].Visible = false;
            dataGridView1.Columns[2].Visible = false;


            gMapControl1.DragButton = MouseButtons.Left;
            gMapControl1.CanDragMap = true;
            gMapControl1.MapProvider = GMapProviders.GoogleMap;
            gMapControl1.Position = new PointLatLng(latInicial, lngInicial);
            gMapControl1.MinZoom = 0;
            gMapControl1.MaxZoom = 24;
            gMapControl1.Zoom = 9;
            gMapControl1.AutoScroll = true;

            //Marcador
            markeroverlay = new GMapOverlay("Marcador");
            marker = new GMarkerGoogle(new PointLatLng(latInicial, lngInicial), GMarkerGoogleType.green);
            markeroverlay.Markers.Add(marker);

            // tooltip
            marker.ToolTipMode = MarkerTooltipMode.Always;
            marker.ToolTipText = string.Format("Marcação: \n Latitud: {0} \n Longitud: {1} ", latInicial, lngInicial);

            gMapControl1.Overlays.Add(markeroverlay);
        }

        private void SelecionarGeoLocalizacao(object sender, DataGridViewCellMouseEventArgs e)
        {

            if (dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].IsNewRow) return;

            filaselecionada   = e.RowIndex;

            txtDescricao.Text = dataGridView1.Rows[filaselecionada].Cells[0].Value.ToString();
            txtLatitude.Text  = dataGridView1.Rows[filaselecionada].Cells[1].Value.ToString();
            txtLongitude.Text = dataGridView1.Rows[filaselecionada].Cells[2].Value.ToString();
            //if (string.IsNullOrEmpty(txtLatitude.Text) || string.IsNullOrEmpty(txtLongitude.Text))
            //{
            //    return;
            //}

            marker.Position = new PointLatLng(Convert.ToDouble(txtLatitude.Text), Convert.ToDouble(txtLongitude.Text));
            gMapControl1.Position = marker.Position;

        }

        private void gMapControl1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            double lat = gMapControl1.FromLocalToLatLng(e.X, e.Y).Lat;
            double lng = gMapControl1.FromLocalToLatLng(e.X, e.Y).Lng;

            txtLatitude.Text = lat.ToString();
            txtLongitude.Text = lng.ToString();

            marker.Position = new PointLatLng(lat, lng);
            marker.ToolTipText = string.Format("Marcação: \n Latitud: {0} \n Longitud: {1} ", lat, lng);
            
        }

        private void btnAdicionar_Click(object sender, EventArgs e)
        {
            dt.Rows.Add(txtDescricao.Text, txtLatitude.Text, txtLongitude.Text);
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.RemoveAt(filaselecionada);
        }

        private void btnPoligono_Click(object sender, EventArgs e)
        {
            GMapOverlay Poligono = new GMapOverlay("Polígonos");
            List<PointLatLng> pontos = new List<PointLatLng>();
            double lat, lng;
            for (int i = 0; i < dataGridView1.Rows.Count-1; i++)
            {
                lat = Convert.ToDouble(dataGridView1.Rows[i].Cells[1].Value);
                lng = Convert.ToDouble(dataGridView1.Rows[i].Cells[2].Value);
                pontos.Add(new PointLatLng(lat, lng));
            }

            GMapPolygon polygonPontos = new GMapPolygon(pontos, "Polígonos");
            Poligono.Polygons.Add(polygonPontos);
            gMapControl1.Overlays.Add(Poligono);

            gMapControl1.Zoom += 1;
            gMapControl1.Zoom -= 1;

        }

        private void btnRotas_Click(object sender, EventArgs e)
        {
            GMapOverlay Rotas = new GMapOverlay("CapaRotas");
            List<PointLatLng> pontos = new List<PointLatLng>();
            double lat, lng;
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                lat = Convert.ToDouble(dataGridView1.Rows[i].Cells[1].Value);
                lng = Convert.ToDouble(dataGridView1.Rows[i].Cells[2].Value);
                pontos.Add(new PointLatLng(lat, lng));
            }

            GMapRoute rotasPontos = new GMapRoute(pontos, "Rotas");
            Rotas.Routes.Add(rotasPontos);
            gMapControl1.Overlays.Add(Rotas);

            gMapControl1.Zoom += 1;
            gMapControl1.Zoom -= 1;
        }
    }
}
