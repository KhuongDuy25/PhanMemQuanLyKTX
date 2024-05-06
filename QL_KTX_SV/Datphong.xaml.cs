using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Interaction logic for Datphong.xaml
    /// </summary>
    public partial class Datphong : Window
    {
        string maphong;
        
        
        Connect_SinhVien consv;
        public Datphong(string tienNha, string tienDien, string tienNuoc, string tienVeSinh, string soNguoiDangO, string songuoitoida, string phong, string maphong,
                    string loaiPhong, string tenNha, string thietBi, string soLuongThietBi, string tinhTrang,string tenphong)
        {
            consv = new Connect_SinhVien();
            InitializeComponent();
            filldata(tienNha, tienDien, tienNuoc, tienVeSinh, soNguoiDangO, songuoitoida, phong, maphong,
                     loaiPhong, tenNha, thietBi, soLuongThietBi, tinhTrang,tenphong);
            this.maphong = maphong;
           
            
        }

        private void filldata(string tienNha, string tienDien, string tienNuoc, string tienVeSinh, string soNguoiDangO,string songuoitoida, string phong, string maphong,
                    string loaiPhong, string tenNha, string thietBi, string soLuongThietBi, string tinhTrang,string tenphong)
        {
            tb_tenphong.Text = tenphong;
            tb_tiennha.Text = tienNha;
            tb_tiendien.Text = tienDien;
            tb_tiennuoc.Text = tienNuoc;
            tb_vesinh.Text = tienVeSinh;
            tb_min.Text = soNguoiDangO;
            tb_loaiphong.Text = loaiPhong;
            tb_tennha.Text = tenNha;
            tb_thietbi.Text = thietBi;
            tb_sl.Text = soLuongThietBi;
            tb_tinhtrang.Text = tinhTrang;
            tb_max.Text = songuoitoida;
            tb_maphong.Text = maphong;
            cb_khoa.ItemsSource = consv.LayDanhSachKhoaMaTen();

            // Hiển thị giá trị key cho ComboBox của Khoa
            cb_khoa.DisplayMemberPath = "Value";

            // Cập nhật danh sách từ dữ liệu vào ComboBox cho Que
            cb_que.ItemsSource = consv.LayDanhSachQueMaTen();

            // Hiển thị giá trị key cho ComboBox của Que
            cb_que.DisplayMemberPath = "Value";

        }

        private void btn_Them_Click(object sender, RoutedEventArgs e)
        {
            if (cb_khoa.SelectedItem == null || cb_que.SelectedItem == null || cb_lop.SelectedItem == null || cb_ngay.SelectedItem == null || cb_thang.SelectedItem == null || cb_nam.SelectedItem == null || tb_msv.Text == "" || tb_ten.Text == "")
            {
                if (rd_nam.IsChecked == false && rd_nu.IsChecked == false)
                         MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {

                MessageBoxResult result = MessageBox.Show("Bạn có chắc muốn đăng kí phòng này?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);

                // Kiểm tra kết quả của hộp thoại
                if (result == MessageBoxResult.Yes)
                {
                    string gt = "Nam";
                    if (rd_nu.IsChecked == true)
                    {
                        gt = "Nữ";
                    }
                    string nam = cb_nam.Text;
                    string thang = cb_thang.Text;
                    string ngay = cb_ngay.Text;

                    // Tạo chuỗi ngày tháng từ các giá trị combobox
                    string ns = nam + "-" + thang.PadLeft(2, '0') + "-" + ngay.PadLeft(2, '0');
                    KeyValuePair<string, string> selectedque = (KeyValuePair<string, string>)cb_que.SelectedItem;

                    KeyValuePair<string, string> selectedkhoa = (KeyValuePair<string, string>)cb_khoa.SelectedItem;

                    KeyValuePair<string, string> selectedlop = (KeyValuePair<string, string>)cb_lop.SelectedItem;


                    consv.ThemSinhVienVaTangSoNguoiDangO(tb_msv.Text, tb_ten.Text, ns, gt, selectedque.Key, selectedkhoa.Key, selectedlop.Key, maphong,tb_nbd.Text,tb_nd.Text);
                    sinhvien sv = new sinhvien();
                    sv.Show();
                    this.Close();




                }
                else
                {
                    Phong phong = new Phong();
                    phong.Show();
                    this.Close();
                }
            }
        }

        private void cb_khoa_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cb_khoa.SelectedItem != null)
            {
                KeyValuePair<string, string> selectedKhoa = (KeyValuePair<string, string>)cb_khoa.SelectedItem;
                MessageBox.Show(selectedKhoa.Key);

                cb_lop.ItemsSource = consv.LayDanhSachTenVaMaLopTheoKhoa(selectedKhoa.Key);
                cb_lop.DisplayMemberPath = "Value";
            }
        }

        private void btn_quaylai_Click(object sender, RoutedEventArgs e)
        {
            Phong phong = new Phong();
            phong.Show();   
            this.Close();
        }

        private void tb_nbd_Copy_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }

}
