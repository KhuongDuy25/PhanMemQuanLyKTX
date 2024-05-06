using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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

namespace QL_KTX_SV
{
    /// <summary>
    /// Interaction logic for CommonControl.xaml
    /// </summary>
    public partial class CommonControl : UserControl
    {
       

        public CommonControl()
        {
          
            InitializeComponent();
          
        }
        private void MenuItem_Phong_Click(object sender, RoutedEventArgs e)
        {
            Phong phong = new Phong();
            phong.Show();
            Window currentWindow = Window.GetWindow(this);
            currentWindow.Close();
        }



        private void MenuItem_SinhVien_Click(object sender, RoutedEventArgs e)
        {
            sinhvien sv = new sinhvien();
            sv.Show();
            Window currentWindow = Window.GetWindow(this);
            currentWindow.Close();
        }

        private void MenuItem_dmk_Click(object sender, RoutedEventArgs e)
        {
            DMK dmk = new DMK();
            dmk.Show();
            Window currentWindow = Window.GetWindow(this);
            currentWindow.Close();
        }
    }
}
