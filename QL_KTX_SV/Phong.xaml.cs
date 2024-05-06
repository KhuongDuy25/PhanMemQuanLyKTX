using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;

namespace QL_KTX_SV
{
  
    public partial class Phong : Window
    {
        Connect connect;
       
        public Phong()
        {
            InitializeComponent();
            connect = new Connect();
           FillTable();
            
        }
    
        private void FillTable()
        {
            // Lấy dữ liệu từ phương thức GetPhongDataWithRoomTypes
            DataTable dataTable = connect.GetPhongDataWithRoomTypes();

            // Đặt dữ liệu vào ItemsSource của datagrid
            dataGrid.ItemsSource = dataTable.DefaultView;
            Dictionary<string, string> maTenThietBi = connect.LayMaTenThietBi();

            // Chuyển danh sách thành ICollectionView
            ICollectionView view = CollectionViewSource.GetDefaultView(maTenThietBi);
            // Thiết lập thuộc tính hiển thị của ComboBox
            tb_tenthietbi.ItemsSource = view;
            // Thiết lập đường dẫn thuộc tính hiển thị
            tb_tenthietbi.DisplayMemberPath = "Value";
            // Thiết lập đường dẫn giá trị
            tb_tenthietbi.SelectedValuePath = "Key";


        }



        private void tb_search_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btn_tk_Click(sender, e);
            }
        }

        private void btn_tk_Click(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = connect.Search(tb_search.Text).DefaultView;
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (dataGrid.SelectedItem != null)
            {
                // Lấy hàng được chọn
                DataRowView selectedRow = (DataRowView)dataGrid.SelectedItem;

                tb_tenthietbi.Text = selectedRow["tenthietbi"].ToString();
                tb_soluong.Text = selectedRow["soluong"].ToString();
                tb_tinhtrang.Text = selectedRow["Tinhtrang"].ToString() ;

            }
        }

        private void btn_dangky_Click(object sender, RoutedEventArgs e)
        {
            // Check if an item is selected in the DataGrid
            if (dataGrid.SelectedItem != null)
            {
                // Cast the selected item to a DataRowView
                DataRowView selectedRow = (DataRowView)dataGrid.SelectedItem;
                if (int.Parse(selectedRow["Songuoidango"].ToString()) < int.Parse(selectedRow["Songuoitoida"].ToString()))
                {
                    string maphong = selectedRow["MaPhong"].ToString();
                    string tienNha = selectedRow["Tiennha"].ToString();
                    string tienDien = selectedRow["Tiendien"].ToString();
                    string tienNuoc = selectedRow["Tiennuoc"].ToString();
                    string tienVeSinh = selectedRow["Tienvesinh"].ToString();
                    string soNguoiDangO = selectedRow["Songuoidango"].ToString();
                    string phong = selectedRow["Tenphong"].ToString();
                    string loaiPhong = selectedRow["Loaiphong"].ToString(); // Lấy thông tin loại phòng
                    string tenNha = selectedRow["Tennha"].ToString(); // Lấy thông tin tên nhà
                    string thietBi = selectedRow["tenthietbi"].ToString(); // Lấy thông tin thiết bị
                    string soLuongThietBi = selectedRow["soluong"].ToString(); // Lấy thông tin số lượng thiết bị
                    string tinhTrang = selectedRow["Tinhtrang"].ToString(); // Lấy thông tin tình trạng
                    string songuoitoida = selectedRow["Songuoitoida"].ToString();
                    string tenphong = selectedRow["Tenphong"].ToString();
                    // Set the retrieved values to the corresponding TextBlocks or TextBoxes in the Traphong.xaml
                    Datphong dp = new Datphong(tienNha, tienDien, tienNuoc, tienVeSinh, soNguoiDangO, songuoitoida, phong, maphong,
                         loaiPhong, tenNha, thietBi, soLuongThietBi, tinhTrang, tenphong);
                    dp.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Phòng đã đầy!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                }


                // Retrieve the values from the selected row

            }
        }



        private void btn_soPhongConTrong_Click(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = connect.danhsachphongtrong().DefaultView;
        }

        private void CommonControl_Loaded(object sender, RoutedEventArgs e)
        {
            btn_dangky_Click(sender, e);
        }

        private void btn_sua_Click(object sender, RoutedEventArgs e)
        {

                // Lấy hàng được chọn
                DataRowView selectedRow = (DataRowView)dataGrid.SelectedItem;

                string maphong = selectedRow["MaPhong"].ToString();
                string maThietBi = ((KeyValuePair<string, string>)tb_tenthietbi.SelectedItem).Key;

                int sl;
                string tinhtrang = tb_tinhtrang.Text;
               
            try
            {
                sl = Convert.ToInt32( tb_soluong.Text);
                connect.CapNhatThietBiPhong(maphong,maThietBi, sl,tinhtrang);
                FillTable();
                tb_tenthietbi.Text = "";
                tb_soluong.Text = "";
                tb_tinhtrang.Text ="";

            }
            catch
            {
                MessageBox.Show(" Số lượng phải là giá trị nguyên", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

           
            
        }

        private void tb_search_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void tb_tenthietbi_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void tb_soluong_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
