using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace QL_KTX_SV
{
    /// <summary>
    /// Interaction logic for traphong.xaml
    /// </summary>
    public partial class traphong : Window
    {
        Connect conn;
        public traphong(string maSinhVien, string tenSinhVien, string ngaySinh,string gioitinh, string que, string khoa, string maLop)
        {
            InitializeComponent();
            conn = new Connect();
            // Gán giá trị cho các TextBlock tương ứng
            tb_ma.Text = maSinhVien;
            tb_ten.Text = tenSinhVien;
            tb_ns.Text = ngaySinh;
            cb_que.Text = que;
            cb_khoa.Text = khoa;
            cb_lop.Text = maLop;
           if(gioitinh == "Nam")
            {
                rd_nam.IsChecked = true;
            }
            else
            {
                rd_nu.IsChecked = true;
            }
            DataTable dt = conn.LayThongTinPhong(tb_ma.Text);

            // Kiểm tra nếu DataTable không rỗng
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0]; // Lấy dòng đầu tiên của DataTable

                // Lấy giá trị từ cột Tiennha và gán vào tb_tienha.Text
                tb_tienha.Text = row["Tiennha"].ToString();

                // Lấy giá trị từ cột Tiendien và gán vào tb_tiendien.Text
                tb_tiendien.Text = row["Tiendien"].ToString();

                // Lấy giá trị từ cột Tiennuoc và gán vào tb_nuoc.Text
                tb_nuoc.Text = row["Tiennuoc"].ToString();

                // Lấy giá trị từ cột Tienvesinh và gán vào tb_vs.Text
                tb_vs.Text = row["Tienvesinh"].ToString();

                // Kiểm tra nếu cột "TienPhat" không rỗng và không là DBNull.Value, thì gán giá trị
                if (!row.IsNull("TienPhat") && row["TienPhat"] != DBNull.Value)
                {
                    tb_tp.Text = row["TienPhat"].ToString();
                }
                else
                {
                    // Nếu cột "TienPhat" là rỗng hoặc DBNull.Value, gán giá trị là 0
                    tb_tp.Text = "0";
                }

                // Lấy giá trị từ cột Tenphong và gán vào tb_phong.Text
                tb_phong.Text = row["Tenphong"].ToString();

                // Lấy giá trị từ cột NgayBdau và gán vào tb_nbd.Text
                tb_nbd.Text = row["NgayBdau"].ToString();

                // Lấy giá trị từ cột Ngaykt và gán vào tb_nkt.Text
                tb_nkt.Text = row["Ngaykt"].ToString();

                // Tính tổng tiền
                int tienNha = int.Parse(tb_tienha.Text);
                int tienDien = int.Parse(tb_tiendien.Text);
                int tienNuoc = int.Parse(tb_nuoc.Text);
                int tienVeSinh = int.Parse(tb_vs.Text);
                int tienPhat = int.Parse(tb_tp.Text);

                int tongTien = tienNha + tienDien + tienNuoc + tienVeSinh + tienPhat;

                // Hiển thị tổng tiền
                tb_total.Text = tongTien.ToString();

            }
            else
            {
                MessageBoxResult result = MessageBox.Show("Không tìm thấy thông tin phòng cho sinh viên này! Bạn có muốn đăng kí không?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Information);

                // Kiểm tra xem người dùng đã chọn Yes hay không
                if (result == MessageBoxResult.Yes)
                {
                  Phong phong = new Phong();
                  phong.Show();
                    this.Close();
                }
                
            }



        }

        private void btn_ql_Click(object sender, RoutedEventArgs e)
        {
            sinhvien sv = new sinhvien();
            sv.Show();
            this.Close();
        }

        private void btn_Them_Click(object sender, RoutedEventArgs e)
        {
            conn.XoaSinhVien(tb_ma.Text);
            Phong phong = new Phong();
            phong.Show();
            this.Close();

        }
    }
}
