using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace QL_KTX_SV
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Connect conn;
        public MainWindow()
        {
            InitializeComponent();
            conn = new Connect();
        }

        

        private void tb_mk_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btn_dn_Click_1(sender, e);
            }
        }

        private void btn_dn_Click_1(object sender, RoutedEventArgs e)
        {
            string pass = tb_mk.Password;
            string strpass = "";
            using (MD5 md5 = MD5.Create())
            {
                // Chuyển đổi chuỗi thành mảng byte và tính toán MD5 hash
                byte[] inputBytes = Encoding.UTF8.GetBytes(pass);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Chuyển đổi mảng byte thành chuỗi hexa
                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    stringBuilder.Append(hashBytes[i].ToString("x2"));
                }
                strpass = stringBuilder.ToString();
            }

            if (conn.Check_Mk(strpass))
            {
                Phong w1 = new Phong();
                w1.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Nhập sai mật khẩu. Nhập lại", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void ForgotPassword_Click(object sender, RoutedEventArgs e)
        {
            Random random = new Random();

            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()-_+=[]{}|;:',.<>?/";
            chars = chars.Replace(" ", "");
            StringBuilder passwordBuilder = new StringBuilder(); // Đổi tên biến này
            for (int i = 0; i < 8; i++)
            {
                passwordBuilder.Append(chars[random.Next(chars.Length)]);
            }
            string strpass = "";
            string randomPassword = passwordBuilder.ToString();
            using (MD5 md5 = MD5.Create())
            {
                // Chuyển đổi chuỗi thành mảng byte và tính toán MD5 hash
                byte[] inputBytes = Encoding.UTF8.GetBytes(randomPassword);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Chuyển đổi mảng byte thành chuỗi hexa
                StringBuilder hashedStringBuilder = new StringBuilder(); // Đổi tên biến này
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    hashedStringBuilder.Append(hashBytes[i].ToString("x2"));
                }
                strpass = hashedStringBuilder.ToString();
                Connect conn = new Connect();
                conn.UpdatePassword(strpass);
            }
            MessageBox.Show($"Mật khẩu mới của bạn là: {randomPassword}", "Quên mật khẩu", MessageBoxButton.OK, MessageBoxImage.Information);
        }



    }
}
