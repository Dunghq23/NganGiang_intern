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

namespace View_Map_on_Winform
{
    public partial class Form1 : Form
    {
        private GMapControl gMapControl1;
        private int defaultZoom = 10;
        public Form1()
        {
            InitializeComponent();

            // Khởi tạo control GMapControl
            gMapControl1 = new GMapControl();

            // Đặt thuộc tính Dock của GMapControl thành Fill để nó lấp đầy Panel1
            gMapControl1.Dock = DockStyle.Fill;

            // Thêm GMapControl vào Panel1
            panel1.Controls.Add(gMapControl1);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Thiết lập nhà cung cấp bản đồ và tọa độ ban đầu của GMapControl
            gMapControl1.MapProvider = GMapProviders.GoogleMap;
            gMapControl1.Position = new PointLatLng(21.0285, 105.8542); // Set initial position
            gMapControl1.MinZoom = 1;
            gMapControl1.MaxZoom = 20;
            gMapControl1.Zoom = defaultZoom;

        }

        private void goCordinates_Click(object sender, EventArgs e)
        {
            gMapControl1.Position = new PointLatLng(Convert.ToDouble(latitude.Text), Convert.ToDouble(longitude.Text));
            gMapControl1.Zoom = defaultZoom;
            gMapControl1.Update();
            gMapControl1.Refresh();
        }


        private void zoomLevel_ValueChanged(object sender, EventArgs e)
        {
            gMapControl1.Zoom = Convert.ToDouble(zoomLevel.Value);
            gMapControl1.Update();
            gMapControl1.Refresh();
        }

        void gmap_MouseClick(object sender, MouseEventArgs e)
        {
            // Lấy tọa độ của điểm được nhấp chuột trên bản đồ
            PointLatLng point = gMapControl1.FromLocalToLatLng(e.X, e.Y);

            // Hiển thị tọa độ trên TextBox latitude và longitude
            latitude.Text = point.Lat.ToString();
            longitude.Text = point.Lng.ToString();
        }

        private bool isActive = false;
        private void btnReturnCordinate_Click(object sender, EventArgs e)
        {
            if (!isActive)
            {
                // Gắn sự kiện MouseClick cho control GMapControl
                gMapControl1.MouseClick += gmap_MouseClick;
                btnReturnCordinate.Text = "Unactive Cordinates Click";
                isActive = true;
            }
            else
            {
                // Hủy bỏ sự kiện MouseClick từ control GMapControl
                gMapControl1.MouseClick -= gmap_MouseClick;
                btnReturnCordinate.Text = "Active Cordinates Click";
                isActive = false;
            }
        }

    }
}
