using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QL_KTX_SV
{
    internal class Connect_SinhVien
    {
            private string connect = "Data Source=data.db";

        public (DataTable, List<string>, List<string>, List<string>) GetSinhVienData()
        {
            DataTable dataTable = new DataTable();
            List<string> maLopList = new List<string>();
            List<string> maQueList = new List<string>();
            List<string> maKhoaList = new List<string>();

            using (SQLiteConnection connection = new SQLiteConnection(connect))
            {
                connection.Open();

                string query = @"SELECT s.Masinhvien, s.Tensinhvien, s.Ngaysinh, s.gioitinh, q.Tenque AS TenQue, k.Tenkhoa AS TenKhoa, l.Tenlop AS TenLop
                        FROM sinhvien s
                        INNER JOIN que q ON s.Maque = q.Maque
                        INNER JOIN khoa k ON s.Makhoa = k.Makhoa
                        INNER JOIN lop l ON s.Malop = l.Malop";

                SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, connection);
                adapter.Fill(dataTable);
            }

            // Lấy danh sách mã lớp từ bảng lop
            using (SQLiteConnection connection = new SQLiteConnection(connect))
            {
                connection.Open();

                string query = "SELECT Tenlop FROM lop";
                SQLiteCommand command = new SQLiteCommand(query, connection);
                SQLiteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    maLopList.Add(reader["Tenlop"].ToString());
                }
            }

            // Lấy danh sách mã quê từ bảng que
            using (SQLiteConnection connection = new SQLiteConnection(connect))
            {
                connection.Open();

                string query = "SELECT Tenque FROM que";
                SQLiteCommand command = new SQLiteCommand(query, connection);
                SQLiteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    maQueList.Add(reader["Tenque"].ToString());
                }
            }

            // Lấy danh sách mã khoa từ bảng khoa
            using (SQLiteConnection connection = new SQLiteConnection(connect))
            {
                connection.Open();

                string query = "SELECT Tenkhoa FROM khoa";
                SQLiteCommand command = new SQLiteCommand(query, connection);
                SQLiteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    maKhoaList.Add(reader["Tenkhoa"].ToString());
                }
            }

            return (dataTable, maLopList, maQueList, maKhoaList);
        }


        public void ThemSinhVienVaTangSoNguoiDangO(string maSV, string tenSV, string ngaySinh, string gioiTinh, string maQue, string maKhoa, string maLop, string maPhong, string nbd, string nd)
        {
            bool maSVTonTai = false; // Biến để kiểm tra mã sinh viên đã tồn tại hay chưa

            using (SQLiteConnection connection = new SQLiteConnection(connect))
            {
                connection.Open();
                using (SQLiteTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Kiểm tra xem mã sinh viên đã tồn tại chưa
                        string queryKiemTraSV = @"SELECT COUNT(*) FROM sinhvien WHERE Masinhvien = @MaSV";
                        using (SQLiteCommand commandKiemTraSV = new SQLiteCommand(queryKiemTraSV, connection, transaction))
                        {
                            commandKiemTraSV.Parameters.AddWithValue("@MaSV", maSV);
                            int count = Convert.ToInt32(commandKiemTraSV.ExecuteScalar());
                            if (count > 0)
                            {
                                // Mã sinh viên đã tồn tại, đặt biến maSVTonTai thành true
                                maSVTonTai = true;
                            }
                        }

                        if (!maSVTonTai) // Nếu mã sinh viên chưa tồn tại, thực hiện thêm sinh viên và tăng số người đang ở
                        {
                            // Thêm sinh viên vào bảng 'sinhvien'
                            string queryThemSV = @"INSERT INTO sinhvien (Masinhvien, Tensinhvien, Ngaysinh, gioitinh, Maque, Makhoa, Malop) 
                     VALUES (@MaSV, @TenSV, @NgaySinh, @GioiTinh, @MaQue, @MaKhoa, @MaLop)";

                            using (SQLiteCommand commandThemSV = new SQLiteCommand(queryThemSV, connection, transaction))
                            {
                                commandThemSV.Parameters.AddWithValue("@MaSV", maSV);
                                commandThemSV.Parameters.AddWithValue("@TenSV", tenSV);
                                commandThemSV.Parameters.AddWithValue("@NgaySinh", ngaySinh);
                                commandThemSV.Parameters.AddWithValue("@GioiTinh", gioiTinh);
                                commandThemSV.Parameters.AddWithValue("@MaQue", maQue);
                                commandThemSV.Parameters.AddWithValue("@MaKhoa", maKhoa);
                                commandThemSV.Parameters.AddWithValue("@MaLop", maLop);

                                commandThemSV.ExecuteNonQuery();
                            }

                            // Tăng số người đang ở trong phòng
                            string queryTangSoNguoi = @"UPDATE Phong 
                             SET Songuoidango = Songuoidango + 1
                             WHERE MaPhong = @MaPhong";

                            using (SQLiteCommand commandTangSoNguoi = new SQLiteCommand(queryTangSoNguoi, connection, transaction))
                            {
                                commandTangSoNguoi.Parameters.AddWithValue("@MaPhong", maPhong);
                                commandTangSoNguoi.ExecuteNonQuery();
                            }

                            // Thêm sinh viên vào bảng 'SV-Phong'
                            string queryThemSVPhong = @"INSERT INTO 'SV-Phong' (Mathue, Masv, Maphong, NgayBdau, Ngaykt)
                             VALUES (@MaThue, @MaSV, @MaPhong, @NgayBD, @NgayKT)";

                            using (SQLiteCommand commandThemSVPhong = new SQLiteCommand(queryThemSVPhong, connection, transaction))
                            {
                                commandThemSVPhong.Parameters.AddWithValue("@MaThue", Guid.NewGuid().ToString()); // Sử dụng Guid cho Mathue
                                commandThemSVPhong.Parameters.AddWithValue("@MaSV", maSV);
                                commandThemSVPhong.Parameters.AddWithValue("@MaPhong", maPhong);
                                commandThemSVPhong.Parameters.AddWithValue("@NgayBD", nbd);
                                commandThemSVPhong.Parameters.AddWithValue("@NgayKT", nd);

                                commandThemSVPhong.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        // Xử lý ngoại lệ
                        transaction.Rollback();
                        Console.WriteLine("Lỗi khi thêm sinh viên và tăng số người đang ở: " + ex.Message);
                    }
                }
            }

            // Hiển thị cảnh báo nếu mã sinh viên đã tồn tại
            if (maSVTonTai)
            {
                MessageBox.Show("Sinh viên đã tồn tại!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        public Dictionary<string, string> LayDanhSachKhoaMaTen()
        {
            Dictionary<string, string> khoaDictionary = new Dictionary<string, string>();

            using (SQLiteConnection connection = new SQLiteConnection(connect))
            {
                connection.Open();

                string query = "SELECT makhoa, Tenkhoa FROM Khoa";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string maKhoa = reader["makhoa"].ToString();
                            string tenKhoa = reader["Tenkhoa"].ToString();
                            khoaDictionary.Add(maKhoa, tenKhoa);
                        }
                    }
                }
            }

            return khoaDictionary;
        }


        public Dictionary<string, string> LayDanhSachQueMaTen()
        {
            Dictionary<string, string> queDictionary = new Dictionary<string, string>();

            using (SQLiteConnection connection = new SQLiteConnection(connect))
            {
                connection.Open();

                string query = "SELECT Maque, Tenque FROM Que";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string maQue = reader["Maque"].ToString();
                            string tenQue = reader["Tenque"].ToString();
                            queDictionary.Add(maQue, tenQue);
                        }
                    }
                }
            }

            return queDictionary;
        }
        public Dictionary<string, string> LayDanhSachTenVaMaLopTheoKhoa(string maKhoa)
        {
            Dictionary<string, string> tenMaLopDict = new Dictionary<string, string>();

            using (SQLiteConnection connection = new SQLiteConnection(connect))
            {
                connection.Open();

                string query = "SELECT Tenlop, Malop FROM Lop WHERE Makhoa = @Makhoa";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Makhoa", maKhoa);

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string tenLop = reader["Tenlop"].ToString();
                            string maLop = reader["Malop"].ToString();
                            tenMaLopDict.Add(maLop, tenLop);
                        }
                    }
                }
            }

            return tenMaLopDict;
        }

        public DataTable LayThongTinSinhVienCungPhong(string maPhong)
        {
            DataTable dataTable = new DataTable();

            // Kết nối đến cơ sở dữ liệu SQLite
            using (SQLiteConnection connection = new SQLiteConnection(connect))
            {
                connection.Open();

                // Truy vấn SQL để lấy thông tin từ bảng "SV-Phong" và "Phong"
                string query = @"SELECT s.Masinhvien, s.Tensinhvien, s.Ngaysinh, s.gioitinh, q.Tenque AS TenQue, k.Tenkhoa AS TenKhoa, l.Tenlop AS TenLop
                        FROM sinhvien s
                        INNER JOIN que q ON s.Maque = q.Maque
                        INNER JOIN khoa k ON s.Makhoa = k.Makhoa
                        INNER JOIN lop l ON s.Malop = l.Malop
                        INNER JOIN 'SV-Phong' sp ON s.Masinhvien = sp.Masv
                        INNER JOIN Phong p ON sp.Maphong = p.MaPhong
                        WHERE p.MaPhong = @MaPhong";

                // Tạo Adapter và thực hiện truy vấn
                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, connection))
                {
                    adapter.SelectCommand.Parameters.AddWithValue("@MaPhong", maPhong);
                    adapter.Fill(dataTable);
                }
            }

            return dataTable;
        }

        public Dictionary<string, string> LayDanhSachTenVaMaPhong()
        {
            Dictionary<string, string> danhSachPhong = new Dictionary<string, string>();

            // Kết nối đến cơ sở dữ liệu SQLite
            using (SQLiteConnection connection = new SQLiteConnection(connect))
            {
                connection.Open();

                // Truy vấn SQL để lấy danh sách tên và mã phòng từ bảng "Phong"
                string query = "SELECT Tenphong, MaPhong FROM Phong";

                // Tạo Command và thực hiện truy vấn
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        // Đọc từng dòng dữ liệu và thêm vào từ điển
                        while (reader.Read())
                        {
                            string tenPhong = reader["Tenphong"].ToString();
                            string maPhong = reader["MaPhong"].ToString();
                            danhSachPhong.Add(maPhong, tenPhong);
                        }
                    }
                }
            }

            return danhSachPhong;
        }

        public DataTable LayThongTinSinhVienTrongPhong(string maHoacTen)
        {
            DataTable dataTable = new DataTable();

            // Kết nối đến cơ sở dữ liệu SQLite
            using (SQLiteConnection connection = new SQLiteConnection(connect))
            {
                connection.Open();

                // Truy vấn SQL để lấy thông tin sinh viên từ bảng "sinhvien" và "Phong"
                string query = @"SELECT s.Masinhvien, s.Tensinhvien, s.Ngaysinh, s.gioitinh, q.Tenque AS TenQue, k.Tenkhoa AS TenKhoa, l.Tenlop AS TenLop, p.Tenphong
                         FROM sinhvien s
                         INNER JOIN que q ON s.Maque = q.Maque
                         INNER JOIN khoa k ON s.Makhoa = k.Makhoa
                         INNER JOIN lop l ON s.Malop = l.Malop
                         INNER JOIN 'SV-Phong' sp ON s.Masinhvien = sp.Masv
                         INNER JOIN Phong p ON sp.Maphong = p.MaPhong
                         WHERE s.Masinhvien = @MaHoacTen OR s.Tensinhvien LIKE @TenHoacMa";

                // Tạo Adapter và thực hiện truy vấn
                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, connection))
                {
                    adapter.SelectCommand.Parameters.AddWithValue("@MaHoacTen", maHoacTen);
                    adapter.SelectCommand.Parameters.AddWithValue("@TenHoacMa", "%" + maHoacTen + "%"); // Sử dụng LIKE để tìm kiếm tên gần đúng
                    adapter.Fill(dataTable);
                }
            }

            return dataTable;
        }

        public void SuaThongTinSinhVien(string maSV, string tenSV, string ngaySinh, string gioiTinh, string maQue, string maKhoa, string maLop)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connect))
            {
                connection.Open();
                using (SQLiteTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Cập nhật thông tin của sinh viên trong bảng 'sinhvien'
                        string querySuaSV = @"UPDATE sinhvien 
                                      SET Tensinhvien = @TenSV, Ngaysinh = @NgaySinh, gioitinh = @GioiTinh, 
                                          Maque = @MaQue, Makhoa = @MaKhoa, Malop = @MaLop
                                      WHERE Masinhvien = @MaSV";

                        using (SQLiteCommand commandSuaSV = new SQLiteCommand(querySuaSV, connection, transaction))
                        {
                            commandSuaSV.Parameters.AddWithValue("@TenSV", tenSV);
                            commandSuaSV.Parameters.AddWithValue("@NgaySinh", ngaySinh);
                            commandSuaSV.Parameters.AddWithValue("@GioiTinh", gioiTinh);
                            commandSuaSV.Parameters.AddWithValue("@MaQue", maQue);
                            commandSuaSV.Parameters.AddWithValue("@MaKhoa", maKhoa);
                            commandSuaSV.Parameters.AddWithValue("@MaLop", maLop);
                            commandSuaSV.Parameters.AddWithValue("@MaSV", maSV);

                            commandSuaSV.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        // Xử lý ngoại lệ
                        transaction.Rollback();
                        Console.WriteLine("Lỗi khi sửa thông tin sinh viên: " + ex.Message);
                    }
                }
            }
        }

    }
}
