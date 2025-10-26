using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bai04
{
    public partial class Server : Form
    {
       
        private TcpListener listener;
        private readonly List<TcpClient> clients = new List<TcpClient>();
        private readonly ConcurrentDictionary<string, MovieInfo> danhSachPhim = new ConcurrentDictionary<string, MovieInfo>();
        private readonly Dictionary<string, int> tongSoVe = new Dictionary<string, int>()
        {
            { "Đào, phở và piano", 45 },
            { "Mai", 30 },
            { "Gặp lại chị bầu", 15 },
            { "Tarot", 15 }
        };
        private readonly object _lock = new object();
        private int port = 9000;
        public Server()
        {
            InitializeComponent();
           
        }
       
        private async void Server_Load(object sender, EventArgs e)
        {
            StartServer();
            Log("Server started. Đang chờ file input5.txt...");
            string defaultPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "input5.txt");
            if (File.Exists(defaultPath))
            {
                LoadInputFile(defaultPath);
            }
            else
            {
                Log("Không tìm thấy input5.txt trong thư mục chạy.");
            }
        }

        private void LoadInputFile(string path)
        {
            try
            {
                var lines = File.ReadAllLines(path, Encoding.UTF8)
                                .Select(x => x.Trim())
                                .Where(x => !string.IsNullOrEmpty(x))
                                .ToList();

                if (lines.Count % 4 != 0)
                {
                    Log("File input5.txt có định dạng sai!");
                    return;
                }

                danhSachPhim.Clear();

                for (int i = 0; i < lines.Count; i += 4)
                {
                    string ten = lines[i];
                    if (!double.TryParse(lines[i + 1], out double gia))
                    {
                        Log($"Lỗi: Giá vé của phim {ten} không hợp lệ!");
                        continue;
                    }
                    var phongList = lines[i + 2].Split(',').Select(p => int.Parse(p.Trim())).ToList();
                    var gheList = lines[i + 3].Split(',').Select(g => g.Trim()).ToList();

                    var movie = new MovieInfo
                    {
                        TenPhim = ten,
                        GiaVe = gia,
                        Phong = phongList,
                        Ghe = new Dictionary<string, bool>()
                    };

                    foreach (int phong in phongList)
                    {
                        foreach (string ghe in gheList)
                        {
                            string key = $"{phong}-{ghe}";
                            movie.Ghe[key] = false;
                        }
                    }
                    int expectedTickets = tongSoVe.TryGetValue(ten, out var total) ? total : movie.Ghe.Count;
                    if (movie.Ghe.Count != expectedTickets)
                    {
                        Log($"Cảnh báo: Phim {ten} có {movie.Ghe.Count} vé, nhưng yêu cầu {expectedTickets} vé!");
                    }

                    danhSachPhim[ten] = movie;
                }

                Log($"Đã nạp file: {Path.GetFileName(path)} – {danhSachPhim.Count} phim");
                _ = BroadcastFilmsUpdate();
            }
            catch (Exception ex)
            {
                Log("Lỗi đọc file: " + ex.Message);
            }
        }

        private void StartServer()
        {
            listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            Log($"Đang lắng nghe trên cổng {port}...");

            _ = Task.Run(async () =>
            {
                while (true)
                {
                    try
                    {
                        TcpClient client = await listener.AcceptTcpClientAsync();

                        lock (clients)
                        {
                            clients.Add(client);
                        }

                        Log($"Client kết nối: {client.Client.RemoteEndPoint}");
                        await SendFullUpdate(client);
                        _ = HandleClient(client);
                    }
                    catch (Exception ex)
                    {
                    }
                }
            });
        }

        private async Task HandleClient(TcpClient client)
        {
            var stream = client.GetStream();
            var reader = new StreamReader(stream, Encoding.UTF8);
            var writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };

            try
            {
                while (client.Connected)
                {
                    string line = await reader.ReadLineAsync();
                    if (line == null) break;

                    var doc = JsonDocument.Parse(line);
                    var root = doc.RootElement;
                    string type = root.GetProperty("type").GetString();

                    if (type == "getseats")
                    {
                        string phim = root.GetProperty("phim").GetString().Trim();
                        await SendSeatsUpdate(writer, phim);
                    }
                    else if (type == "getstats")
                    {
                        var stats = new List<object>();
                        foreach (var kv in danhSachPhim)
                        {
                            var movie = kv.Value;
                            int daBan = movie.Ghe.Values.Count(b => b);
                            int conLai = tongSoVe.TryGetValue(movie.TenPhim, out var total) ? total - daBan : movie.Ghe.Count - daBan;
                            double doanhThu = 0;
                            var gheDaMua = movie.Ghe.Where(g => g.Value).Select(g => g.Key).ToList();

                            foreach (var g in movie.Ghe.Where(g => g.Value))
                            {
                                string tenGhe = g.Key.Split('-')[1];
                                double tiLe = 1.0;
                                if (new[] { "A1", "A5", "C1", "C5" }.Contains(tenGhe)) tiLe = 0.25;
                                else if (new[] { "B2", "B3", "B4" }.Contains(tenGhe)) tiLe = 2.0;
                                doanhThu += movie.GiaVe * tiLe;
                            }

                            stats.Add(new
                            {
                                ten = movie.TenPhim,
                                da_ban = daBan,
                                con_lai = conLai,
                                doanh_thu = doanhThu,
                                ghe_da_mua = gheDaMua
                            });
                        }

                        var response = new { type = "getstats_result", data = stats };
                        await writer.WriteLineAsync(JsonSerializer.Serialize(response));
                    }
                    else if (type == "datve")
                    {
                        string phim = root.GetProperty("phim").GetString().Trim();
                        var gheArray = root.GetProperty("ghe").EnumerateArray();

                        List<string> gheThanhCong = new List<string>();
                        List<string> gheThatBai = new List<string>();
                        string lyDo = "";

                        lock (_lock)
                        {
                            if (!danhSachPhim.ContainsKey(phim))
                            {
                                lyDo = "Phim không tồn tại";
                                gheThatBai = gheArray.Select(g =>
                                    $"{g.GetProperty("phong").GetInt32()}-{g.GetProperty("ghe").GetString()}").ToList();
                            }
                            else
                            {
                                var movie = danhSachPhim[phim];
                                foreach (var g in gheArray)
                                {
                                    int phong = g.GetProperty("phong").GetInt32();
                                    string tenGhe = g.GetProperty("ghe").GetString();
                                    string key = $"{phong}-{tenGhe}";

                                    if (!movie.Phong.Contains(phong))
                                    {
                                        gheThatBai.Add(key);
                                        continue;
                                    }

                                    if (movie.Ghe.ContainsKey(key) && !movie.Ghe[key])
                                    {
                                        movie.Ghe[key] = true;
                                        gheThanhCong.Add(key);
                                    }
                                    else
                                    {
                                        gheThatBai.Add(key);
                                    }
                                }
                            }
                        }
                        var result = new
                        {
                            type = "datve_result",
                            success = gheThatBai.Count == 0,
                            ghe_thanh_cong = gheThanhCong,
                            ghe_that_bai = gheThatBai,
                            ly_do = gheThatBai.Count == 0 ? "" : (lyDo != "" ? lyDo : "Ghế đã được đặt trước")
                        };

                        await writer.WriteLineAsync(JsonSerializer.Serialize(result));

                        if (gheThanhCong.Count > 0)
                        {
                            Log($"Đặt vé thành công: {phim} - {string.Join(", ", gheThanhCong)}");
                            await BroadcastSeatsUpdate(phim);
                        }

                        if (gheThatBai.Count > 0)
                        {
                            Log($"Đặt vé thất bại: {string.Join(", ", gheThatBai)} - {result.ly_do}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log("Lỗi client: " + ex.Message);
            }
            finally
            {
                lock (clients) clients.Remove(client);
                client.Close();
            }
        }

        private async Task SendFullUpdate(TcpClient client)
        {
            var phimList = danhSachPhim.Keys.Select(k => k.Trim()).ToList();
            var update = new
            {
                type = "update_films",
                data = phimList
            };

            var writer = new StreamWriter(client.GetStream(), Encoding.UTF8) { AutoFlush = true };
            await writer.WriteLineAsync(JsonSerializer.Serialize(update));
        }

        private async Task BroadcastFilmsUpdate()
        {
            var phimList = danhSachPhim.Keys.Select(k => k.Trim()).ToList();
            var update = new { type = "update_films", data = phimList };
            string json = JsonSerializer.Serialize(update);

            List<TcpClient> currentClients;
            lock (clients)
            {
                currentClients = clients.ToList();
            }

            foreach (var c in currentClients)
            {
                try
                {
                    var w = new StreamWriter(c.GetStream(), Encoding.UTF8) { AutoFlush = true };
                    await w.WriteLineAsync(json);
                }
                catch
                {
                    lock (clients) clients.Remove(c);
                }
            }
        }

        private async Task SendSeatsUpdate(StreamWriter writer, string phim)
        {
            if (!danhSachPhim.TryGetValue(phim, out var movie)) return;

            var gheDaMua = movie.Ghe.Where(kv => kv.Value).Select(kv => kv.Key).ToList();
            var update = new
            {
                type = "update_seats",
                data = new { phim, ghe_da_mua = gheDaMua }
            };

            await writer.WriteLineAsync(JsonSerializer.Serialize(update));
        }

        private async Task BroadcastSeatsUpdate(string phim)
        {
            if (!danhSachPhim.TryGetValue(phim, out var movie)) return;

            var gheDaMua = movie.Ghe.Where(kv => kv.Value).Select(kv => kv.Key).ToList();
            var update = new { type = "update_seats", data = new { phim, ghe_da_mua = gheDaMua } };
            string json = JsonSerializer.Serialize(update);

            List<TcpClient> currentClients;
            lock (clients)
            {
                currentClients = clients.ToList();
            }

            foreach (var c in currentClients)
            {
                try
                {
                    var w = new StreamWriter(c.GetStream(), Encoding.UTF8) { AutoFlush = true };
                    await w.WriteLineAsync(json);
                }
                catch
                {
                    lock (clients) clients.Remove(c);
                }
            }
        }

        private void Log(string msg)
        {
            if (InvokeRequired) { Invoke(new Action(() => Log(msg))); return; }
            txtLog.AppendText($"[{DateTime.Now:T}] {msg}\r\n");
            txtLog.SelectionStart = txtLog.Text.Length;
            txtLog.ScrollToCaret();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            ExportStatistics("output5.txt");
        }

        private void ExportStatistics(string file)
        {
            var list = danhSachPhim.Values.Select(p => new
            {
                p.TenPhim,
                DaBan = p.Ghe.Values.Count(g => g),
                ConLai = p.Ghe.Values.Count(g => !g),
                DoanhThu = p.Ghe.Values.Count(g => g) * p.GiaVe
            }).OrderByDescending(x => x.DoanhThu).ToList();

            var sb = new StringBuilder();
            sb.AppendLine("Tên phim | Vé bán | Vé tồn | Tỉ lệ bán (%) | Doanh thu | Xếp hạng");
            int rank = 1;
            foreach (var item in list)
            {
                double tiLe = item.DaBan + item.ConLai > 0 ? (100.0 * item.DaBan) / (item.DaBan + item.ConLai) : 0;
                sb.AppendLine($"{item.TenPhim} | {item.DaBan} | {item.ConLai} | {tiLe:F1}% | {item.DoanhThu:N0} | {rank++}");
            }

            string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, file);
            File.WriteAllText(fullPath, sb.ToString(), Encoding.UTF8);
            Log($"Đã xuất file: {fullPath}");
        }
        private void txtLog_TextChanged(object sender, EventArgs e)
        {
        }
        private void btnOpenClient_Click(object sender, EventArgs e)

        {
            Form1 clientForm = new Form1();
            clientForm.Show();
        }

    }


    public class MovieInfo
    {
        public string TenPhim { get; set; }
        public double GiaVe { get; set; }
        public List<int> Phong { get; set; } = new List<int>();
        public Dictionary<string, bool> Ghe { get; set; } = new Dictionary<string, bool>();
    }
}
