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

namespace View_Map_on_Winform
{
    public partial class Form1 : Form
    {
        private GMapControl map;
        private int defaultZoom = 10;
        public Form1()
        {
            InitializeComponent();

            // Khởi tạo control GMapControl
            map = new GMapControl();

            // Đặt thuộc tính Dock của GMapControl thành Fill để nó lấp đầy Panel1
            map.Dock = DockStyle.Fill;

            // Thêm GMapControl vào Panel1
            panel1.Controls.Add(map);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Thiết lập nhà cung cấp bản đồ và tọa độ ban đầu của GMapControl
            //map.MapProvider = GMapProviders.GoogleMap;
            // Đặt kiểu bản đồ thành ảnh vệ tinh
            // map.MapProvider = GMapProviders.GoogleSatelliteMap;
            // Đặt kiểu bản đồ thành giao thông
            map.MapProvider = GMapProviders.GoogleMap;
            map.Manager.Mode = AccessMode.ServerOnly; // Đảm bảo rằng bạn sử dụng chế độ server để có thể hiển thị bản đồ giao thông


            map.Position = new PointLatLng(21.0285, 105.8542); // Set initial position
            map.MinZoom = 1;
            map.MaxZoom = 20;
            map.Zoom = defaultZoom;

            latitude.Text = (21.0285).ToString();
            longitude.Text = (105.8542).ToString();
        }

        private void goCordinates_Click(object sender, EventArgs e)
        {
            map.Position = new PointLatLng(Convert.ToDouble(latitude.Text), Convert.ToDouble(longitude.Text));
            map.Zoom = defaultZoom;
            map.Update();
            map.Refresh();
        }


        private void zoomLevel_ValueChanged(object sender, EventArgs e)
        {
            map.Zoom = Convert.ToDouble(zoomLevel.Value);
            map.Update();
            map.Refresh();
        }

        void gmap_MouseClick(object sender, MouseEventArgs e)
        {
            // Lấy tọa độ của điểm được nhấp chuột trên bản đồ
            PointLatLng point = map.FromLocalToLatLng(e.X, e.Y);

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
                map.MouseClick += gmap_MouseClick;
                btnReturnCordinate.Text = "Unactive Cordinates Click";
                isActive = true;
            }
            else
            {
                // Hủy bỏ sự kiện MouseClick từ control GMapControl
                map.MouseClick -= gmap_MouseClick;
                btnReturnCordinate.Text = "Active Cordinates Click";
                isActive = false;
            }
        }

        void map_AddMarker_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // Lấy tọa độ của điểm được nhấp chuột trên bản đồ
                PointLatLng point = map.FromLocalToLatLng(e.X, e.Y);

                double currentZoom = map.Zoom;

                map.Zoom = currentZoom;

                GMapMarker marker = new GMarkerGoogle(point, GMarkerGoogleType.blue_dot);

                // 1. Tạo 1 lớp phủ
                GMapOverlay markers = new GMapOverlay("markers");

                // 2. Thêm tất cả các điểm đánh dấu có sẵn vào Lớp phủ đó
                markers.Markers.Add(marker);

                // 3. Che phủ bản đồ bằng Overlay
                map.Overlays.Add(markers);

                // Đảm bảo rằng marker được hiển thị ở vị trí được click
                PointLatLng center = map.Position;
                map.Position = center;
            }
        }


        private bool isActiveAddMarker = false;

        private void btnFlagThePoint_Click(object sender, EventArgs e)
        {
            if (!isActiveAddMarker)
            {
                // Gắn sự kiện MouseClick cho control GMapControl
                map.MouseClick += map_AddMarker_MouseClick;
                btnFlagThePoint.Text = "Unactive Add Markers";
                isActiveAddMarker = true;
            }
            else
            {
                // Hủy bỏ sự kiện MouseClick từ control GMapControl
                map.MouseClick -= map_AddMarker_MouseClick;
                btnFlagThePoint.Text = "Flag the point";
                isActiveAddMarker = false;
                map.Overlays.Clear(); // Xóa toàn bộ các lớp phủ, bao gồm cả các điểm đánh dấu
                map.Refresh();
            }
        }

        bool isCreatingLine = false;
        PointLatLng startPoint = new PointLatLng();
        List<PointLatLng> linePoints = new List<PointLatLng>();
        private void btnCreateLine_Click(object sender, EventArgs e)
        {
            // Chuyển đổi trạng thái của biến isCreatingLine giữa true và false
            isCreatingLine = !isCreatingLine;

            // Cập nhật văn bản của nút tùy thuộc vào trạng thái của biến isCreatingLine
            btnCreateLine.Text = isCreatingLine ? "Unactive Draw the Line" : "Draw the Line";

            if (isCreatingLine)
            {
                // Bắt đầu xử lý sự kiện MouseClick khi chức năng vẽ đường được kích hoạt
                map.MouseClick += map_AddMarker_MouseClick;
                map.MouseClick += map_AddLine_MouseClick;
            }
            else
            {
                // Ngừng xử lý sự kiện MouseClick khi chức năng vẽ đường được tắt
                map.MouseClick -= map_AddMarker_MouseClick;
                map.MouseClick -= map_AddLine_MouseClick;
                // Reset danh sách các điểm và điểm bắt đầu
                linePoints = null;
                startPoint = new PointLatLng();
                map.Overlays.Clear(); // Xóa toàn bộ các lớp phủ, bao gồm cả các điểm đánh dấu
                map.Refresh();
            }
        }

        private void map_AddLine_MouseClick(object sender, MouseEventArgs e)
        {
            // Lấy tọa độ của điểm được nhấp chuột trên bản đồ
            PointLatLng point = map.FromLocalToLatLng(e.X, e.Y);

            // Nếu danh sách các điểm tạo thành đường chưa được khởi tạo, khởi tạo nó
            if (linePoints == null)
            {
                linePoints = new List<PointLatLng>();
            }

            // Thêm điểm vào danh sách
            linePoints.Add(point);


            // Nếu chỉ có một điểm, lưu lại điểm đó và không vẽ đường
            if (linePoints.Count == 1)
            {
                startPoint = point;
                return;
            }

            // Tạo đường từ danh sách các điểm
            GMapRoute route = new GMapRoute(linePoints, "MyRoute");

            // Tạo lớp phủ để chứa đường
            GMapOverlay routesOverlay = new GMapOverlay("routes");
            routesOverlay.Routes.Add(route);

            // Thêm lớp phủ vào bản đồ
            map.Overlays.Add(routesOverlay);

            // Đảm bảo rằng marker được hiển thị ở vị trí được click
            PointLatLng center = map.Position;
            map.Position = center;
        }

        private List<PointLatLng> polygonPoints = new List<PointLatLng>();

        private void map_AddPolygon_MouseClick(object sender, MouseEventArgs e)
        {
            // Lấy tọa độ của điểm được click trên bản đồ
            PointLatLng clickedPoint = map.FromLocalToLatLng(e.X, e.Y);

            // Thêm điểm vào danh sách các điểm của đa giác
            polygonPoints.Add(clickedPoint);

            // Kiểm tra nếu đã có đủ điểm để tạo đa giác
            if (polygonPoints.Count >= 3)
            {
                // Tạo đa giác mới từ danh sách các điểm đã chọn
                var polygon = new GMapPolygon(polygonPoints, "My Area")
                {
                    Stroke = new Pen(Color.DarkGreen, 2),
                    Fill = new SolidBrush(Color.BurlyWood)
                };

                // Tạo một Overlay mới để chứa đa giác và thêm nó vào bản đồ
                var polygonsOverlay = new GMapOverlay("polygons");
                polygonsOverlay.Polygons.Add(polygon);
                map.Overlays.Add(polygonsOverlay);

            }
            // Đảm bảo rằng marker được hiển thị ở vị trí được click
            PointLatLng center = map.Position;
            map.Position = center;
        }

        bool isCreatingPolygon = false;

        private void btnAddPolygon_Click(object sender, EventArgs e)
        {
            isCreatingPolygon = !isCreatingPolygon;
            btnAddPolygon.Text = isCreatingPolygon ? "Unactive Draw the Polygon" : "Draw the Polygon";


            if (isCreatingPolygon)
            {
                // Bắt đầu xử lý sự kiện MouseClick khi chức năng vẽ đường được kích hoạt
                map.MouseClick += map_AddPolygon_MouseClick;
            }
            else
            {
                // Ngừng xử lý sự kiện MouseClick khi chức năng vẽ đường được tắt
                map.MouseClick -= map_AddPolygon_MouseClick;
                // Reset danh sách các điểm và điểm bắt đầu
                polygonPoints.Clear();
                map.Overlays.Clear(); // Xóa toàn bộ các lớp phủ, bao gồm cả các điểm đánh dấu
                map.Refresh();
            }
            // MessageBox.Show("Nhấp chuột trên bản đồ để chọn các điểm của đa giác. Khi hoàn thành, nhấn nút Lưu.", "Hướng dẫn", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }




        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Giải phóng tài nguyên của GMapControl
            map.Dispose();
        }
    }
}
