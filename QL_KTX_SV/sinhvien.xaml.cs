using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace QL_KTX_SV
{
    /// <summary>
    /// Interaction logic for sinhvien.xaml
    /// </summary>
    public partial class sinhvien : Window
    {
        Connect_SinhVien connect;

        public sinhvien()
        {
            InitializeComponent();
            connect = new Connect_SinhVien();
            FillTable();
        }

        private void FillTable()
        {
            // Lấy dữ liệu từ phương thức GetSinhVienData của đối tượng connect
            (DataTable dataTable, List<string> lop, List<string> que, List<string> khoa) = connect.GetSinhVienData();

            // Đặt dữ liệu vào ItemsSource của DataGrid
            dataGrid1.ItemsSource = dataTable.DefaultView;

            cb_tp.ItemsSource = connect.LayDanhSachTenVaMaPhong();
            cb_tp.DisplayMemberPath = "Value";

            cb_khoa.ItemsSource = connect.LayDanhSachKhoaMaTen();

            // Hiển thị giá trị key cho ComboBox của Khoa
            cb_khoa.DisplayMemberPath = "Value";

            // Cập nhật danh sách từ dữ liệu vào ComboBox cho Que
            cb_que.ItemsSource = connect.LayDanhSachQueMaTen();

            // Hiển thị giá trị key cho ComboBox của Que
            cb_que.DisplayMemberPath = "Value";


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            dataGrid1.ItemsSource = connect.LayThongTinSinhVienTrongPhong(tb_search.Text).DefaultView;
        }

        

        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (dataGrid1.SelectedItem != null)
            {
                // Lấy hàng được chọn
                DataRowView selectedRow = (DataRowView)dataGrid1.SelectedItem;

                // Lấy thông tin của sinh viên từ hàng được chọn
                tb_msv.Text = selectedRow["Masinhvien"].ToString();
                tb_ten.Text = selectedRow["Tensinhvien"].ToString();
                string ngaySinh = selectedRow["Ngaysinh"].ToString();
                string format = "yyyy-MM-dd";
                DateTime dateTime = DateTime.ParseExact(ngaySinh, format, System.Globalization.CultureInfo.InvariantCulture);

                // Lấy ra năm, tháng và ngày
                int year = dateTime.Year;
                int month = dateTime.Month;
                int day = dateTime.Day;
                cb_ngay.Text = day.ToString();
                cb_thang.Text = month.ToString();
                cb_nam.Text = year.ToString();
                if ( selectedRow["gioitinh"].ToString() == "Nam")
                {
                    rd_nam.IsChecked = true;
                }
                else
                {
                    rd_nu.IsChecked = true;
                }
                cb_que.Text = selectedRow["TenQue"].ToString();
                cb_khoa.Text = selectedRow["TenKhoa"].ToString();
                cb_lop.Text = selectedRow["TenLop"].ToString();

                
            }

        }

        private void cb_khoa_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (cb_khoa.SelectedItem != null)
            {
                KeyValuePair<string, string> selectedKhoa = (KeyValuePair<string, string>)cb_khoa.SelectedItem;
                

                cb_lop.ItemsSource = connect.LayDanhSachTenVaMaLopTheoKhoa(selectedKhoa.Key);
                cb_lop.DisplayMemberPath = "Value";
            }
        }

        private void btn_traphong_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid1.SelectedItem != null)
            {
                // Lấy hàng được chọn
                DataRowView selectedRow = (DataRowView)dataGrid1.SelectedItem;

                // Lấy thông tin của sinh viên từ hàng được chọn
                string maSinhVien = selectedRow["Masinhvien"].ToString();
                string tenSinhVien = selectedRow["Tensinhvien"].ToString();
                string ngaySinh = selectedRow["Ngaysinh"].ToString();
                string gioiTinh = selectedRow["gioitinh"].ToString();
                string tenQue = selectedRow["TenQue"].ToString();
                string tenKhoa = selectedRow["TenKhoa"].ToString();
                string tenLop = selectedRow["TenLop"].ToString();
                traphong tp = new traphong(maSinhVien,tenSinhVien,ngaySinh,gioiTinh, tenQue, tenKhoa,tenLop);
                tp.Show();
                this.Close();
            }
    }

   

        private void tb_search_TextChanged(object sender, TextChangedEventArgs e)
        {
            dataGrid1.ItemsSource = connect.LayThongTinSinhVienTrongPhong(tb_search.Text).DefaultView;
        }

        private void btn_sua_Click(object sender, RoutedEventArgs e)
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


                    connect.SuaThongTinSinhVien(tb_msv.Text, tb_ten.Text, ns, gt, selectedque.Key, selectedkhoa.Key, selectedlop.Key);

                    FillTable();



                }
                else
                {
                    Phong phong = new Phong();
                    phong.Show();
                    this.Close();
                }
               
            }

        }

        private void bt_tkp_Click(object sender, RoutedEventArgs e)
        {
            KeyValuePair<string, string> selectedPhong = (KeyValuePair<string, string>)cb_tp.SelectedItem;
            dataGrid1.ItemsSource = connect.LayThongTinSinhVienCungPhong(selectedPhong.Key).DefaultView;
        }

        private void search_Click(object sender, RoutedEventArgs e)
        {
            dataGrid1.ItemsSource = connect.LayThongTinSinhVienTrongPhong(tb_search.Text).DefaultView;
        }
    }
}
