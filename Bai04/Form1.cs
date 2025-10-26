using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.IO;
using System.Text.Json;
using System.Threading;

namespace Bai04
{
    public partial class Form1 : Form
    {
        public class ThongKePhim
        {
            public string TenPhim { get; set; }
            public int DaBan { get; set; }
            public int ConLai { get; set; }
            public double DoanhThu { get; set; }
        }
        private BackgroundWorker bwExport;
        private TaskCompletionSource<(bool success, List<string> thanhCong, List<string> thatBai, string lyDo)> datVeTcs;
        Dictionary<Button, (int phong, string ghe)> danhSachGhe = new Dictionary<Button, (int, string)>();
        HashSet<string> gheDaMua = new HashSet<string>();
        HashSet<string> gheDangChon = new HashSet<string>();
        private readonly Dictionary<string, double> giaPhim = new Dictionary<string, double>()
        {
            {"Đào, phở và piano", 45000},
            { "Mai", 100000 },
            { "Gặp lại chị bầu", 70000 },
            { "Tarot", 90000 }
        };
        private TcpClient tcp;
        private StreamReader reader;
        private StreamWriter writer;
        private CancellationTokenSource clientCts;

        public Form1()
        {
            InitializeComponent();
            bwExport = new BackgroundWorker();
            bwExport.WorkerReportsProgress = true;
            bwExport.DoWork += BwExport_DoWork;
            bwExport.ProgressChanged += BwExport_ProgressChanged;
            bwExport.RunWorkerCompleted += BwExport_RunWorkerCompleted;
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            await ConnectServer("127.0.0.1", 9000);

            try
            {
                for (int i = 0; i < tcSelectedChair.TabPages.Count; i++)
                {
                    int phong = i + 1;
                    foreach (Control c in tcSelectedChair.TabPages[i].Controls)
                    {
                        if (c is Button btn && btn.Text.Length >= 2)
                        {
                            danhSachGhe[btn] = (phong, btn.Text);
                            btn.BackColor = Color.LightGray;
                            btn.Click += XuLyChonGhe;
                        }
                    }
                }
                await writer.WriteLineAsync(JsonSerializer.Serialize(new { type = "getfilms" }));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khởi tạo: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task ConnectServer(string host, int port)
        {
            try
            {
                tcp = new TcpClient();
                await tcp.ConnectAsync(host, port);
                reader = new StreamReader(tcp.GetStream(), Encoding.UTF8);
                writer = new StreamWriter(tcp.GetStream(), Encoding.UTF8) { AutoFlush = true };
                clientCts = new CancellationTokenSource();
                _ = Task.Run(() => ListenFromServer(clientCts.Token));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối: " + ex.Message);
            }
        }
        private void TaoHoaDon(string tenKhach, string phim, double giaChuan, List<string> danhSachGhe)
        {
            var sb = new StringBuilder();
            sb.AppendLine("=================================");
            sb.AppendLine("        HÓA ĐƠN ĐẶT VÉ PHIM");
            sb.AppendLine("=================================");
            sb.AppendLine($"Khách hàng: {tenKhach}");
            sb.AppendLine($"Phim: {phim}");
            sb.AppendLine($"Giá vé chuẩn: {giaChuan:N0} đ");
            sb.AppendLine("---------------------------------");
            sb.AppendLine("STT | Ghế   | Phòng | Loại vé     | Giá vé");

            double tongTien = 0;
            int stt = 1;

            foreach (string key in danhSachGhe)
            {
                var parts = key.Split('-');
                int phong = int.Parse(parts[0]);
                string ghe = parts[1];

                string loaiVe = "Vé thường";
                double tiLe = 1.0;

                if (new[] { "A1", "A5", "C1", "C5" }.Contains(ghe))
                {
                    tiLe = 0.25; loaiVe = "Vé vớt (25%)";
                }
                else if (new[] { "B2", "B3", "B4" }.Contains(ghe))
                {
                    tiLe = 2.0; loaiVe = "Vé VIP (200%)";
                }

                double giaVe = giaChuan * tiLe;
                tongTien += giaVe;

                sb.AppendLine($"{stt++,-3} | {ghe,-5} | {phong,-5} | {loaiVe,-11} | {giaVe,10:N0} đ");
            }

            sb.AppendLine("---------------------------------");
            sb.AppendLine($"TỔNG TIỀN: {tongTien,25:N0} đ");
            sb.AppendLine("=================================");
            sb.AppendLine($"Thời gian: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            sb.AppendLine("Cảm ơn quý khách!");

            txtBill.Text = sb.ToString();
        }

        private double TinhTongTien(double giaChuan, List<string> danhSachGhe)
        {
            double tong = 0;
            foreach (string key in danhSachGhe)
            {
                var ghe = key.Split('-')[1];
                double tiLe = 1.0;
                if (new[] { "A1", "A5", "C1", "C5" }.Contains(ghe)) tiLe = 0.25;
                else if (new[] { "B2", "B3", "B4" }.Contains(ghe)) tiLe = 2.0;
                tong += giaChuan * tiLe;
            }
            return tong;
        }
        private async Task ListenFromServer(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested && tcp?.Connected == true)
                {
                    string line = await reader.ReadLineAsync();
                    if (line == null)
                    {
                        // Kết nối bị ngắt
                        break;
                    }

                    try
                    {
                        var doc = JsonDocument.Parse(line);
                        var root = doc.RootElement;
                        string type = root.GetProperty("type").GetString();

                        if (type == "update_films")
                        {
                            var data = root.GetProperty("data").EnumerateArray();
                            this.Invoke(new Action(() =>
                            {
                                cboSelectedFilm.Items.Clear();
                                foreach (var item in data)
                                {
                                    string tenPhim = item.GetString().Trim();
                                    cboSelectedFilm.Items.Add(tenPhim);
                                }
                            }));
                        }
                        else if (type == "update_seats")
                        {
                            var data = root.GetProperty("data");
                            this.Invoke(new Action(() =>
                            {
                                string phim = data.GetProperty("phim").GetString().Trim();
                                if (cboSelectedFilm.SelectedItem?.ToString() != phim) return;

                                // Cập nhật ghế đã mua
                                foreach (var ghe in data.GetProperty("ghe_da_mua").EnumerateArray())
                                {
                                    string gheStr = ghe.GetString();
                                    var parts = gheStr.Split('-');
                                    int phong = int.Parse(parts[0]);
                                    string tenGhe = parts[1];
                                    string key = $"{phong}-{tenGhe}";

                                    if (!gheDaMua.Contains(key))
                                    {
                                        gheDaMua.Add(key);
                                        var btn = danhSachGhe.FirstOrDefault(kvp => $"{kvp.Value.phong}-{kvp.Value.ghe}" == key).Key;
                                        if (btn != null)
                                        {
                                            btn.BackColor = Color.IndianRed;
                                            btn.Enabled = false;
                                        }
                                    }
                                }

                                // Xóa ghế đang chọn nếu đã bị mua
                                gheDangChon.ExceptWith(gheDaMua);
                                CapNhatMauGheDangChon();
                            }));
                        }
                        else if (type == "getstats_result")
                        {
                            var data = root.GetProperty("data").EnumerateArray();
                            var stats = new List<ThongKePhim>();

                            foreach (var item in data)
                            {
                                var gheDaMua = item.GetProperty("ghe_da_mua").EnumerateArray().Select(x => x.GetString()).ToList();
                                double giaChuan = giaPhim.TryGetValue(item.GetProperty("ten").GetString(), out var gia) ? gia : 0;
                                double doanhThuTinhToan = TinhTongTien(giaChuan, gheDaMua);

                                stats.Add(new ThongKePhim
                                {
                                    TenPhim = item.GetProperty("ten").GetString(),
                                    DaBan = item.GetProperty("da_ban").GetInt32(),
                                    ConLai = item.GetProperty("con_lai").GetInt32(),
                                    DoanhThu = doanhThuTinhToan
                                });
                            }

                            bwExport.RunWorkerAsync(stats);
                        }
                        else if (type == "datve_result")
                        {
                            bool success = root.GetProperty("success").GetBoolean();
                            var thanhCong = root.GetProperty("ghe_thanh_cong").EnumerateArray().Select(x => x.GetString()).ToList();
                            var thatBai = root.GetProperty("ghe_that_bai").EnumerateArray().Select(x => x.GetString()).ToList();
                            string lyDo = root.TryGetProperty("ly_do", out var l) ? l.GetString() : "Không xác định";

                            // GỬI KẾT QUẢ VỀ btnPay_Click ĐANG CHỜ
                            datVeTcs?.TrySetResult((success, thanhCong, thatBai, lyDo));

                            // CẬP NHẬT GIAO DIỆN (ghế đỏ, xóa ghế đang chọn)
                            this.Invoke(new Action(() =>
                            {
                                foreach (string key in thanhCong)
                                {
                                    gheDaMua.Add(key);
                                    var btn = danhSachGhe.FirstOrDefault(kvp => $"{kvp.Value.phong}-{kvp.Value.ghe}" == key).Key;
                                    if (btn != null)
                                    {
                                        btn.BackColor = Color.IndianRed;
                                        btn.Enabled = false;
                                    }
                                }
                                gheDangChon.ExceptWith(thanhCong);
                                CapNhatMauGheDangChon();
                            }));
                        }
                    }
                    catch (Exception parseEx)
                    {
                        // JSON lỗi → bỏ qua, không crash
                        continue;
                    }
                }
            }
            catch (ObjectDisposedException) { /* Stream đã đóng */ }
            catch (IOException) { /* Mất kết nối */ }
            catch (Exception ex)
            {
                // Chỉ hiện 1 lần khi thật sự mất kết nối
                if (!token.IsCancellationRequested)
                {
                    this.Invoke((MethodInvoker)(() =>
                    {
                        if (!this.IsDisposed && !this.Disposing)
                        {
                            MessageBox.Show("Mất kết nối với server!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }));
                }
            }
            finally
            {
                // Dọn dẹp
                this.Invoke((MethodInvoker)(() =>
                {
                    if (!this.IsDisposed)
                    {
                        cboSelectedFilm.Enabled = false;
                        btnPay.Enabled = false;
                    }
                }));
            }
        }
        private void BwExport_DoWork(object sender, DoWorkEventArgs e)
        {
            var stats = (List<ThongKePhim>)e.Argument;
            stats = stats.OrderByDescending(x => x.DoanhThu).ToList();

            var sb = new StringBuilder();
            sb.AppendLine("Tên phim | Vé bán | Vé tồn | Tỉ lệ bán (%) | Doanh thu |### Xếp hạng");

            int total = stats.Count;
            int processed = 0;

            int rank = 1;
            foreach (var s in stats)
            {
                double tiLe = (s.DaBan + s.ConLai) > 0 ? (100.0 * s.DaBan) / (s.DaBan + s.ConLai) : 0;
                sb.AppendLine($"{s.TenPhim} | {s.DaBan,6} | {s.ConLai,6} | {tiLe,11:F1}% | {s.DoanhThu,12:N0} | {rank++,8}");
                processed++;
                int progress = (int)((processed / (double)total) * 100);
                bwExport.ReportProgress(progress);
            }
            string projectDir = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName; 
            string filePath = Path.Combine(projectDir, "output5.txt");

            File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);

            e.Result = $"Xuất thành công!";
        }
        private void BwExport_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pgbExport.Value = Math.Min(e.ProgressPercentage, 100); 
        }

        private void BwExport_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            pgbExport.Visible = false;
            btnExport.Enabled = true;

            if (e.Error != null)
            {
                MessageBox.Show("Lỗi: " + e.Error.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show(e.Result.ToString(), "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private async void btnPay_Click(object sender, EventArgs e)
        {
            if (tcp?.Connected != true)
            {
                MessageBox.Show("Không có kết nối đến server!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Vui lòng nhập tên khách hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cboSelectedFilm.SelectedItem == null || gheDangChon.Count == 0)
            {
                MessageBox.Show("Chưa chọn phim hoặc ghế!");
                return;
            }

            string tenKhach = txtName.Text.Trim();
            string phim = cboSelectedFilm.SelectedItem.ToString().Trim();

            if (!giaPhim.TryGetValue(phim, out double giaChuan))
            {
                MessageBox.Show("Không tìm thấy giá phim!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            datVeTcs = new TaskCompletionSource<(bool, List<string>, List<string>, string)>();

            var danhSachGheGui = gheDangChon.Select(k =>
            {
                var parts = k.Split('-');
                return new { phong = int.Parse(parts[0]), ghe = parts[1] };
            }).ToList();

            var json = JsonSerializer.Serialize(new
            {
                type = "datve",
                phim = phim,
                ghe = danhSachGheGui
            });

            try
            {
                await writer.WriteLineAsync(json);
                var result = await datVeTcs.Task.TimeoutAfter(TimeSpan.FromSeconds(10));
                if (result.success && result.thanhCong.Count > 0)
                {
                    TaoHoaDon(tenKhach, phim, giaChuan, result.thanhCong);
                    MessageBox.Show($"Đặt vé thành công! Tổng tiền: {TinhTongTien(giaChuan, result.thanhCong):N0} đ",
                                   "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show($"Đặt vé thất bại!\nLý do: {result.lyDo}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (TimeoutException)
            {
                MessageBox.Show("Server không phản hồi! Vui lòng thử lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            finally
            {
                datVeTcs = null;
            }
        }

        
        private void XuLyChonGhe(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null) return;

            var (phong, ghe) = danhSachGhe[btn];
            string key = $"{phong}-{ghe}";

            if (gheDaMua.Contains(key))
            {
                MessageBox.Show($"Ghế {ghe} ở phòng {phong} đã được mua!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (gheDangChon.Contains(key))
            {
                gheDangChon.Remove(key);
                btn.BackColor = Color.LightGray;
            }
            else
            {
                var phongDangChon = gheDangChon.Select(k => int.Parse(k.Split('-')[0])).Distinct().ToList();
                if (phongDangChon.Count >= 2 && !phongDangChon.Contains(phong))
                {
                    MessageBox.Show("Không thể chọn ghế từ hơn 2 phòng chiếu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (gheDangChon.Count >= 2 && phongDangChon.Count > 1)
                {
                    MessageBox.Show("Không thể chọn quá 2 ghế khi ở 2 phòng chiếu khác nhau!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                gheDangChon.Add(key);
                btn.BackColor = Color.Yellow;
            }
        }
        private void button19_Click(object sender, EventArgs e)
        {

        }

        private void button22_Click(object sender, EventArgs e)
        {

        }

        private void button29_Click(object sender, EventArgs e)
        {

        }

        private void button23_Click(object sender, EventArgs e)
        {

        }
        private void CapNhatMauGheDangChon()
        {
            foreach (var kvp in danhSachGhe)
            {
                var (phong, ghe) = kvp.Value;
                string key = $"{phong}-{ghe}";
                if (gheDangChon.Contains(key))
                    kvp.Key.BackColor = Color.Yellow;
                else if (!gheDaMua.Contains(key))
                    kvp.Key.BackColor = Color.LightGray;
            }
        }

        private void ResetMauTatCaGhe()
        {
            foreach (var kvp in danhSachGhe)
            {
                var (phong, ghe) = kvp.Value;
                string key = $"{phong}-{ghe}";
                if (gheDaMua.Contains(key))
                {
                    kvp.Key.BackColor = Color.IndianRed;
                    kvp.Key.Enabled = false;
                }
                else
                {
                    kvp.Key.BackColor = Color.LightGray;
                    kvp.Key.Enabled = true;
                }
            }
        }

        private async void cboSelectedFilm_SelectedIndexChanged(object sender, EventArgs e)
        {
            await ChonPhimAsync();
        }

        private async Task ChonPhimAsync()
        {
            if (cboSelectedFilm.SelectedItem == null) return;

            string phim = cboSelectedFilm.SelectedItem.ToString().Trim();
            gheDangChon.Clear();
            CapNhatMauGheDangChon();
            ResetMauTatCaGhe();
            tabPage1.Enabled = true;
            tabPage2.Enabled = true;
            tabPage3.Enabled = true;

            if (phim == "Mai")
                tabPage1.Enabled = false;
            else if (phim == "Gặp lại chị bầu")
                tabPage2.Enabled = tabPage3.Enabled = false;
            else if (phim == "Tarot")
                tabPage1.Enabled = tabPage2.Enabled = false;
            if (tcp?.Connected != true || writer == null)
            {
                MessageBox.Show("Không có kết nối đến server!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                var request = new { type = "getseats", phim = phim };
                await writer.WriteLineAsync(JsonSerializer.Serialize(request));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi gửi yêu cầu ghế: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pgbExport_Click(object sender, EventArgs e)
        {

        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (tcp?.Connected != true)
            {
                MessageBox.Show("Chưa kết nối server!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            pgbExport.Value = 0;
            pgbExport.Visible = true;
            btnExport.Enabled = false;
            var request = new { type = "getstats" };
            writer.WriteLineAsync(JsonSerializer.Serialize(request));
        }
    }
    public class ThongKePhim
    {
        public string TenPhim { get; set; }
        public int DaBan { get; set; }
        public int ConLai { get; set; }
        public double DoanhThu { get; set; }
    }
    public static class TaskExtensions
    {
        public static async Task<T> TimeoutAfter<T>(this Task<T> task, TimeSpan timeout)
        {
            using (var cts = new CancellationTokenSource())
            {
                var delayTask = Task.Delay(timeout, cts.Token);
                var resultTask = await Task.WhenAny(task, delayTask);
                if (resultTask == delayTask)
                    throw new TimeoutException();
                cts.Cancel();
                return await task;
            }
        }
    }
}