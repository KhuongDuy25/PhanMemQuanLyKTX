using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for DMK.xaml
    /// </summary>
    public partial class DMK : Window
    {
        public DMK()
        {
            InitializeComponent();
        }

        private void ok_Click(object sender, RoutedEventArgs e)
        {
            string pass = pb_mkm.Password;

            string strpass = "";
            bool acc = true;
            if (pass.Length < 8)
                acc = false;

            // Kiểm tra chứa ít nhất một chữ số, một chữ cái hoa, một chữ cái thường và một ký tự đặc biệt
            bool hasDigit = pass.Any(char.IsDigit);
            bool hasUpper = pass.Any(char.IsUpper);
            bool hasLower = pass.Any(char.IsLower);
            bool hasSpecialChar = Regex.IsMatch(pass, @"[!@#$%^&*()_+{}\[\]:;<>,.?/\|\\-]");
            if (!(hasDigit && hasUpper && hasSpecialChar))
            {
                acc = false;
            }

            if (pass == pb_nlmk.Password== true && acc == true)
            {
                    
                strpass = Hask(pass);
                Connect connect = new Connect();

                if (connect.UpdatePassword(strpass))
                {
                   MainWindow w1 = new MainWindow();
                    w1.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Không thể cập nhật mật khẩu", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                }

            }
            else
            {
                if (acc == false)
                {
                    MessageBox.Show("Mật Khẩu quá yếu, Có ít nhất 8 Kí tự chữa chữ số, in hoa và kí tự đặc biệt !");
                }
                MessageBox.Show("Nhập mật khẩu lại không khớp. Nhập lại", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ql_Click(object sender, RoutedEventArgs e)
        {
            Phong phong = new Phong();
            phong.Show();
            this.Close();   
        }
        private string Hask(string text)
        {
            string strpass = "";

            using (MD5 md5 = MD5.Create())
            {
                // Chuyển đổi chuỗi thành mảng byte và tính toán MD5 hash
                byte[] inputBytes = Encoding.UTF8.GetBytes(text);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Chuyển đổi mảng byte thành chuỗi hexa
                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    stringBuilder.Append(hashBytes[i].ToString("x2"));
                }
                strpass = stringBuilder.ToString();
            }
            return strpass;
        }
    }
}
