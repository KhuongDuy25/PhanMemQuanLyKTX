using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Runtime.Remoting.Contexts;
using System.Windows;

namespace QL_KTX_SV
{
    internal class Connect
    {
        private string conncet = "Data Source=data.db";

        public bool Check_Mk(string srtpass)
        {
            using (SQLiteConnection connection = new SQLiteConnection(conncet))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand("SELECT pass FROM mk", connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string hashedPasswordFromDB = reader.GetString(0);
                            string hashedInputPassword =(srtpass);

                            if (hashedPasswordFromDB == hashedInputPassword)
                            {
                                return true; // Mật khẩu hợp lệ
                            }
                        }
                    }
                }
            }
            return false; // Mật khẩu không hợp lệ hoặc không tìm thấy mật khẩu trong cơ sở dữ liệu
        }

        public Dictionary<string, string> LayMaTenThietBi()
        {
            Dictionary<string, string> maTenThietBi = new Dictionary<string, string>();

            using (SQLiteConnection connection = new SQLiteConnection(conncet))
            {
                connection.Open();

                string query = "SELECT mathietbi, tenthietbi FROM Thietbi";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string maThietBi = reader["mathietbi"].ToString();
                            string tenThietBi = reader["tenthietbi"].ToString();
                            maTenThietBi.Add(maThietBi, tenThietBi);
                        }
                    }
                }
            }

            return maTenThietBi;
        }

        public bool UpdatePassword(string newHashedPassword)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(conncet))
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand("UPDATE mk SET pass = @NewPassword", connection))
                    {
                        command.Parameters.AddWithValue("@NewPassword", newHashedPassword);
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            return true; // Cập nhật mật khẩu thành công
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi cập nhật mật khẩu: " + ex.Message);
            }

            return false; // Cập nhật mật khẩu thất bại
        }

        public DataTable GetPhongDataWithRoomTypes()
        {
            DataTable dataTable = new DataTable();

            using (SQLiteConnection connection = new SQLiteConnection(conncet))
            {
                connection.Open();

                string query = @"
            SELECT
                Phong.MaPhong,
                Phong.Tenphong,
                Khunha.Tennha,
                Phong.Loaiphong,
                Phong.Songuoitoida,
                Phong.Songuoidango,
                Thietbi.tenthietbi,
                Thietbi_phong.soluong,
                Thietbi_phong.Tinhtrang,
                Phong_chiphi.Tiennha,
                Phong_chiphi.Tiendien,
                Phong_chiphi.Tiennuoc,
                Phong_chiphi.Tienvesinh,
                Phong_chiphi.TienPhat,
                Phong.Ghichu
            FROM
                Phong
            LEFT JOIN Khunha ON Phong.Manha = Khunha.Manha
            LEFT JOIN Thietbi_phong ON Phong.MaPhong = Thietbi_phong.MaPhong
            LEFT JOIN Thietbi ON Thietbi_phong.Mathietbi = Thietbi.mathietbi
            LEFT JOIN Phong_chiphi ON Phong.MaPhong = Phong_chiphi.Maphong";

                SQLiteCommand command = new SQLiteCommand(query, connection);
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                adapter.Fill(dataTable);
            }

            return dataTable;
        }





        public DataTable danhsachphongtrong()
        {
            DataTable dataTable = new DataTable();

            using (SQLiteConnection connection = new SQLiteConnection(conncet))
            {
                connection.Open();

                string query = @"
            SELECT
                Phong.MaPhong,
                Phong.Tenphong,
                Khunha.Tennha,
                Phong.Loaiphong,
                Phong.Songuoitoida,
                Phong.Songuoidango,
                Thietbi.tenthietbi,
                Thietbi_phong.soluong,
                Thietbi_phong.Tinhtrang,
                Phong_chiphi.Tiennha,
                Phong_chiphi.Tiendien,
                Phong_chiphi.Tiennuoc,
                Phong_chiphi.Tienvesinh,
                Phong_chiphi.TienPhat,
                Phong.Ghichu
            FROM
                Phong
            LEFT JOIN Khunha ON Phong.Manha = Khunha.Manha
            LEFT JOIN Thietbi_phong ON Phong.MaPhong = Thietbi_phong.MaPhong
            LEFT JOIN Thietbi ON Thietbi_phong.Mathietbi = Thietbi.mathietbi
            LEFT JOIN Phong_chiphi ON Phong.MaPhong = Phong_chiphi.Maphong
            WHERE Phong.Songuoidango < Phong.Songuoitoida"; // Thêm điều kiện WHERE vào câu truy vấn

                SQLiteCommand command = new SQLiteCommand(query, connection);
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                adapter.Fill(dataTable);
            }

            return dataTable;
        }





        public DataTable Search(string searchText)
        {
            DataTable dataTable = new DataTable();

            using (SQLiteConnection connection = new SQLiteConnection(conncet))
            {
                connection.Open();
                string query = @"
                                SELECT
                                    Phong.MaPhong,
                                    Phong.Tenphong,
                                    Khunha.Tennha,
                                    Phong.Loaiphong,
                                    Phong.Songuoitoida,
                                    Phong.Songuoidango,
                                    Thietbi.tenthietbi,
                                    Thietbi_phong.soluong,
                                    Thietbi_phong.Tinhtrang,
                                    Phong_chiphi.Tiennha,
                                    Phong_chiphi.Tiendien,
                                    Phong_chiphi.Tiennuoc,
                                    Phong_chiphi.Tienvesinh,
                                   
                                    Phong_chiphi.TienPhat,
                                    Phong.Ghichu
                                FROM
                                    Phong
                                LEFT JOIN Khunha ON Phong.Manha = Khunha.Manha
                                LEFT JOIN Thietbi_phong ON Phong.MaPhong = Thietbi_phong.MaPhong
                                LEFT JOIN Thietbi ON Thietbi_phong.Mathietbi = Thietbi.mathietbi
                                LEFT JOIN Phong_chiphi ON Phong.MaPhong = Phong_chiphi.Maphong 
                                WHERE Phong.MaPhong LIKE @SearchText OR Phong.TenPhong LIKE @SearchText";
                                            SQLiteCommand command = new SQLiteCommand(query, connection);
                command.Parameters.AddWithValue("@SearchText", "%" + searchText + "%");

                SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                adapter.Fill(dataTable);
            }

            return dataTable;
        }
        public DataTable LayThongTinPhong(string maSinhVien)
        {
            DataTable dataTable = new DataTable();

            // Kết nối đến cơ sở dữ liệu SQLite
            using (SQLiteConnection connection = new SQLiteConnection(conncet))
            {
                connection.Open();

                // Truy vấn SQL để lấy thông tin từ hai bảng "SV-Phong" và "Phong_chiphi"
                string query = @"SELECT p.Tenphong, pc.Tiennha, pc.Tiendien, pc.Tiennuoc, pc.Tienvesinh, pc.TienPhat,
                                sp.NgayBdau, sp.Ngaykt
                             FROM 'SV-Phong' sp
                             INNER JOIN Phong_chiphi pc ON sp.Maphong = pc.Maphong
                             INNER JOIN Phong p ON sp.Maphong = p.MaPhong
                             WHERE sp.Masv = @MaSinhVien";

                // Tạo Adapter và thực hiện truy vấn
                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, connection))
                {
                    adapter.SelectCommand.Parameters.AddWithValue("@MaSinhVien", maSinhVien);
                    adapter.Fill(dataTable);
                }
            }

            return dataTable;
        }

        public void XoaSinhVien(string maSinhVien)
        {
            using (SQLiteConnection connection = new SQLiteConnection(conncet))
            {
                connection.Open();

                // Bắt đầu một giao dịch để đảm bảo tính nhất quán
                using (SQLiteTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Xóa sinh viên có mã sinh viên truyền vào trong bảng sinhvien
                        string queryDeleteSV = "DELETE FROM sinhvien WHERE Masinhvien = @MaSinhVien";
                        using (SQLiteCommand commandDeleteSV = new SQLiteCommand(queryDeleteSV, connection, transaction))
                        {
                            commandDeleteSV.Parameters.AddWithValue("@MaSinhVien", maSinhVien);
                            commandDeleteSV.ExecuteNonQuery();
                        }

                        // Xóa sinh viên có mã sinh viên truyền vào trong bảng sinhvien_phòng
                        string queryDeleteSV_Phong = "DELETE FROM 'SV-Phong' WHERE Masv = @MaSinhVien";
                        using (SQLiteCommand commandDeleteSV_Phong = new SQLiteCommand(queryDeleteSV_Phong, connection, transaction))
                        {
                            commandDeleteSV_Phong.Parameters.AddWithValue("@MaSinhVien", maSinhVien);
                            commandDeleteSV_Phong.ExecuteNonQuery();
                        }

                        // Giảm số người đang ở trong bảng phòng đi 1
                        string queryUpdateSoNguoi = "UPDATE Phong SET Songuoidango = Songuoidango - 1 WHERE MaPhong IN (SELECT Maphong FROM 'SV-Phong' WHERE Masv = @MaSinhVien)";
                        using (SQLiteCommand commandUpdateSoNguoi = new SQLiteCommand(queryUpdateSoNguoi, connection, transaction))
                        {
                            commandUpdateSoNguoi.Parameters.AddWithValue("@MaSinhVien", maSinhVien);
                            commandUpdateSoNguoi.ExecuteNonQuery();
                        }

                        // Commit giao dịch nếu mọi thao tác thành công
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        // Nếu có lỗi, rollback giao dịch
                        transaction.Rollback();
                        Console.WriteLine("Lỗi: " + ex.Message);
                    }
                }
            }
        }

        public void CapNhatThietBiPhong(string maPhong, string maThietBi, int soLuong, string tinhTrang)
        {
            using (SQLiteConnection connection = new SQLiteConnection(conncet))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    // Kiểm tra xem mã phòng đã tồn tại trong bảng Thietbi_phong hay chưa
                    command.CommandText = @"SELECT COUNT(*) FROM Thietbi_phong WHERE MaPhong = @MaPhong";
                    command.Parameters.AddWithValue("@MaPhong", maPhong);

                    int count = Convert.ToInt32(command.ExecuteScalar());

                    if (count == 0)
                    {
                        // Nếu mã phòng chưa tồn tại, thực hiện thêm thông tin mới
                        command.CommandText = @"INSERT INTO Thietbi_phong (MaPhong, Mathietbi, soluong, Tinhtrang) VALUES (@MaPhong, @MaThietBi, @SoLuong, @TinhTrang)";
                        command.Parameters.AddWithValue("@MaThietBi", maThietBi);
                        command.Parameters.AddWithValue("@SoLuong", soLuong);
                        command.Parameters.AddWithValue("@TinhTrang", tinhTrang);
                    }
                    else
                    {
                        // Nếu mã phòng đã tồn tại, thực hiện cập nhật thông tin
                        command.CommandText = @"UPDATE Thietbi_phong SET soluong = @SoLuong, Tinhtrang = @TinhTrang WHERE MaPhong = @MaPhong";
                        command.Parameters.AddWithValue("@SoLuong", soLuong);
                        command.Parameters.AddWithValue("@TinhTrang", tinhTrang);
                    }

                    // Thực thi câu lệnh
                    command.ExecuteNonQuery();
                }
            }
        }

    }
}
