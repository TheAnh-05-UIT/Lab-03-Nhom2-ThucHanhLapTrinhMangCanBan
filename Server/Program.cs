using Microsoft.Data.Sqlite;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;

class Server
{
    const int PORT = 3000;
    const string DB_FILE = "HomNayAnGiDay.db";
    static string connectionString = $"Data Source={DB_FILE};";

    static void Main(string[] args)
    {

        // Tạo file SQLite nếu chưa có
        if (!File.Exists(DB_FILE))
            File.Create(DB_FILE).Close();

        // Tạo bảng trong database
        using (var conn = new SqliteConnection(connectionString))
        {
            conn.Open();

            var cmd1 = conn.CreateCommand();
            cmd1.CommandText = @"
                CREATE TABLE IF NOT EXISTS NguoiDung (
                    ID INTEGER PRIMARY KEY AUTOINCREMENT,
                    TenNguoiDung TEXT NOT NULL UNIQUE
                );";
            cmd1.ExecuteNonQuery();

            var cmd2 = conn.CreateCommand();
            cmd2.CommandText = @"
                CREATE TABLE IF NOT EXISTS MonAn (
                    ID INTEGER PRIMARY KEY AUTOINCREMENT,
                    TenMonAn TEXT NOT NULL,
                    IDNguoiDung INTEGER,
                    FOREIGN KEY (IDNguoiDung) REFERENCES NguoiDung(ID)
                );";
            cmd2.ExecuteNonQuery();
        }

        // Khởi động server
        TcpListener listener = new TcpListener(IPAddress.Any, PORT);
        listener.Start();
        Console.WriteLine($"[Server] Listening on port {PORT}...");

        while (true)
        {
            var client = listener.AcceptTcpClient();
            new Thread(() => HandleClient(client)).Start();
        }
    }

    static void HandleClient(TcpClient client)
    {
        try
        {
            using (client)
            using (var ns = client.GetStream())
            {
                byte[] buffer = new byte[4096];
                int bytesRead = ns.Read(buffer, 0, buffer.Length);
                if (bytesRead <= 0) return;

                string reqJson = Encoding.UTF8.GetString(buffer, 0, bytesRead).Trim();
                Console.WriteLine("[Server] Received: " + reqJson);

                JsonDocument doc = JsonDocument.Parse(reqJson);
                var root = doc.RootElement;
                string action = root.GetProperty("action").GetString()?.ToUpperInvariant();

                string response = ProcessCommand(root, action);

                byte[] respBytes = Encoding.UTF8.GetBytes(response);
                ns.Write(respBytes, 0, respBytes.Length);
                Console.WriteLine("[Server] Response sent.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("[Server] Error: " + ex.Message);
        }
    }

    static string ProcessCommand(JsonElement root, string action)
    {
        try
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();

                switch (action)
                {
                    case "ADD":
                        {
                            string username = root.GetProperty("username").GetString();
                            string dish = root.GetProperty("dish").GetString();
                            int id = GetOrCreateUser(conn, username);

                            using (var cmd = conn.CreateCommand())
                            {
                                cmd.CommandText = "INSERT INTO MonAn (TenMonAn, IDNguoiDung) VALUES (@dish, @id)";
                                cmd.Parameters.AddWithValue("@dish", dish);
                                cmd.Parameters.AddWithValue("@id", id);
                                cmd.ExecuteNonQuery();
                            }

                            return JsonSerializer.Serialize(new
                            {
                                status = "OK",
                                message = $"Đã thêm món '{dish}' cho {username}"
                            });
                        }

                    case "LIST":
                        {
                            string username = root.GetProperty("username").GetString();
                            int? id = GetUserId(conn, username);
                            if (id == null)
                                return JsonSerializer.Serialize(new { status = "OK", items = Array.Empty<string>() });

                            using (var cmd = conn.CreateCommand())
                            {
                                cmd.CommandText = "SELECT TenMonAn FROM MonAn WHERE IDNguoiDung = @id";
                                cmd.Parameters.AddWithValue("@id", id.Value);

                                var reader = cmd.ExecuteReader();
                                var list = new System.Collections.Generic.List<string>();
                                while (reader.Read()) list.Add(reader.GetString(0));

                                return JsonSerializer.Serialize(new { status = "OK", items = list });
                            }
                        }

                    case "LISTALL":
                        {
                            using (var cmd = conn.CreateCommand())
                            {
                                cmd.CommandText = @"
                                    SELECT m.TenMonAn, n.TenNguoiDung
                                    FROM MonAn m
                                    LEFT JOIN NguoiDung n ON m.IDNguoiDung = n.ID";

                                var reader = cmd.ExecuteReader();
                                var list = new System.Collections.Generic.List<string>();

                                while (reader.Read())
                                {
                                    string mon = reader.GetString(0);
                                    string nguoi = reader.IsDBNull(1) ? "?" : reader.GetString(1);
                                    list.Add($"{mon} (by {nguoi})");
                                }

                                return JsonSerializer.Serialize(new { status = "OK", items = list });
                            }
                        }

                    case "RANDOM":
                        {
                            string username = root.GetProperty("username").GetString();
                            int? id = GetUserId(conn, username);
                            if (id == null)
                                return JsonSerializer.Serialize(new { status = "ERROR", message = "User has no dishes." });

                            using (var cmd = conn.CreateCommand())
                            {
                                cmd.CommandText = "SELECT TenMonAn FROM MonAn WHERE IDNguoiDung = @id ORDER BY RANDOM() LIMIT 1";
                                cmd.Parameters.AddWithValue("@id", id.Value);
                                var result = cmd.ExecuteScalar();

                                if (result != null)
                                    return JsonSerializer.Serialize(new { status = "OK", result = result.ToString() });
                                else
                                    return JsonSerializer.Serialize(new { status = "ERROR", message = "No dishes for user." });
                            }
                        }

                    case "RANDOMALL":
                        {
                            using (var cmd = conn.CreateCommand())
                            {
                                cmd.CommandText = @"
                                    SELECT m.TenMonAn, n.TenNguoiDung
                                    FROM MonAn m
                                    LEFT JOIN NguoiDung n ON m.IDNguoiDung = n.ID
                                    ORDER BY RANDOM() LIMIT 1";

                                var reader = cmd.ExecuteReader();
                                if (reader.Read())
                                {
                                    string mon = reader.GetString(0);
                                    string nguoi = reader.IsDBNull(1) ? "?" : reader.GetString(1);
                                    return JsonSerializer.Serialize(new { status = "OK", result = mon, by = nguoi });
                                }
                                else
                                    return JsonSerializer.Serialize(new { status = "ERROR", message = "No dishes in community." });
                            }
                        }

                    default:
                        return JsonSerializer.Serialize(new { status = "ERROR", message = "Unknown action." });
                }
            }
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { status = "ERROR", message = ex.Message });
        }
    }

    static int GetOrCreateUser(SqliteConnection conn, string username)
    {
        using (var cmd = conn.CreateCommand())
        {
            cmd.CommandText = "SELECT ID FROM NguoiDung WHERE TenNguoiDung = @user";
            cmd.Parameters.AddWithValue("@user", username);
            var res = cmd.ExecuteScalar();
            if (res != null) return Convert.ToInt32(res);
        }

        using (var cmd = conn.CreateCommand())
        {
            cmd.CommandText = "INSERT INTO NguoiDung (TenNguoiDung) VALUES (@user)";
            cmd.Parameters.AddWithValue("@user", username);
            cmd.ExecuteNonQuery();
        }

        using (var cmd = conn.CreateCommand())
        {
            cmd.CommandText = "SELECT last_insert_rowid()";
            return Convert.ToInt32(cmd.ExecuteScalar());
        }
    }

    static int? GetUserId(SqliteConnection conn, string username)
    {
        using (var cmd = conn.CreateCommand())
        {
            cmd.CommandText = "SELECT ID FROM NguoiDung WHERE TenNguoiDung = @user";
            cmd.Parameters.AddWithValue("@user", username);
            var res = cmd.ExecuteScalar();
            if (res != null) return Convert.ToInt32(res);
            return null;
        }
    }
}
