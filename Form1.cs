using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace ConvertTool
{
    public partial class Form1 : Form
    {
        private string selectedImagePath;
        private string base64ImageString;
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            this.MinimumSize = new System.Drawing.Size(873, 673);
            this.MaximumSize = new System.Drawing.Size(873, 673);
        }
        private void convertStringToBase64(object sender, EventArgs e)
        {
            string originalString = rtOgText.Text;
            if (string.IsNullOrEmpty(originalString))
            {
                MessageBox.Show("Không thể chuyển đổi, vui lòng nhập nội dung!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string encodedString = Convert.ToBase64String(Encoding.UTF8.GetBytes(originalString));
            convertEncodeText.Text = encodedString;
        }
        private void convertBase64ToString(object sender, EventArgs e)
        {
            string encodedString = rtEncTxt.Text;
            if (string.IsNullOrEmpty(encodedString))
            {
                MessageBox.Show("Không thể chuyển đổi, vui lòng nhập nội dung!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            byte[] data = Convert.FromBase64String(encodedString);
            string decodedString = Encoding.UTF8.GetString(data);
            rtConverOg.Text = decodedString;
        }
        private void uploadFileImage(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png, *.bmp)|*.jpg;*.jpeg;*.png;*.bmp|All files (*.*)|*.*";
                openFileDialog.Title = "Chọn một tập tin hình ảnh";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Kiểm tra kích thước của hình ảnh (trong ví dụ này, giới hạn là 2 MB)
                    long maxFileSize = 2 * 1024 * 1024; // 2 MB
                    FileInfo fileInfo = new FileInfo(openFileDialog.FileName);
                    if (fileInfo.Length > maxFileSize)
                    {
                        MessageBox.Show("Hình ảnh quá lớn, vui lòng chọn một hình ảnh nhỏ hơn 2 MB!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    // Lưu đường dẫn của hình ảnh đã chọn
                    selectedImagePath = openFileDialog.FileName;
                    pathImage.Text = selectedImagePath;
                    // Hiển thị hình ảnh đã chọn lên PictureBox
                    var image = Image.FromFile(selectedImagePath);
                    pictureBox.Image = image;
                }
            }
        }
        private void convertImageToBase64(object sender, EventArgs e)
        {
            string imagePath = pathImage.Text;
            if (imagePath.Contains("Tải hình ảnh của bạn lên") || pictureBox.Image == null)
            {
                MessageBox.Show("Vui lòng chọn 1 bức ảnh", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Đọc nội dung của hình ảnh dưới dạng mảng byte
            byte[] imageBytes = File.ReadAllBytes(imagePath);

            // Mã hóa hình ảnh thành Base64
            string base64String = Convert.ToBase64String(imageBytes);
            rtEncodeImage.Text = base64String;
        }

        private void convertBase64ToImage(object sender, EventArgs e)
        {
            try
            {
                base64ImageString = rtEncodeImageText.Text;

                // Giải mã mã Base64 thành hình ảnh
                byte[] imageBytes = Convert.FromBase64String(base64ImageString);
                Image image;
                using (MemoryStream ms = new MemoryStream(imageBytes))
                {
                    image = Image.FromStream(ms);
                }

                // Hiển thị hình ảnh đã giải mã lên PictureBox
                pictureBox1.Image = image;
            }
            catch
            {
                MessageBox.Show("Không thể chuyển đổi, vui lòng nhập nội dung!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void saveimage(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(base64ImageString))
            {
                using (var saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png, *.bmp)|*.jpg;*.jpeg;*.png;*.bmp|All files (*.*)|*.*";
                    saveFileDialog.Title = "Lưu hình ảnh đã giải mã";

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        // Lưu hình ảnh đã giải mã vào máy tính
                        byte[] imageBytes = Convert.FromBase64String(base64ImageString);
                        File.WriteAllBytes(saveFileDialog.FileName, imageBytes);

                        MessageBox.Show("Đã lưu hình ảnh thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            else
            {
                MessageBox.Show("Chưa giải mã hình ảnh hoặc hình ảnh không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        void Copy(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                MessageBox.Show("Văn bản không có dữ liệu, kiểm tra lại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            // Sao chép văn bản từ TextBox vào Clipboard
            Clipboard.SetText(text);

            MessageBox.Show("Văn bản đã được sao chép vào Clipboard!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void copyCodeImageBase64(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Copy(rtEncodeImage.Text);
        }

        private void copyStringDecode(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Copy(rtConverOg.Text);
        }

        private void copyStringEncode(object sender, LinkLabelLinkClickedEventArgs e)
        {            
            Copy(convertEncodeText.Text);
        }
    }
}
