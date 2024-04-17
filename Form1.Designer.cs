using System.Resources;
using System.Text;
using WinFormsApplication.Properties;

namespace WinFormsApplication

{
    public class Record
    {
        public string ProductCode { get; set; }
        public int Quantity { get; set; }
        public decimal CostPerItem { get; set; }
        public DateTime DateTime { get; set; }
    }
    public partial class Form1 : Form
    {
        private static readonly ResourceManager resources = new ResourceManager(typeof(Resources));
        private DataGridView dataGridView1;
        private readonly HttpClient _httpClient;

        public Form1()
        {
            InitializeComponent();
            _httpClient = new HttpClient();

        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            dataGridView1 = new DataGridView();
            button1 = new Button();
            pictureBox1 = new PictureBox();
            textBox1 = new TextBox();
            listBox1 = new ListBox();
            button2 = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.Location = new Point(78, 56);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(465, 200);
            dataGridView1.TabIndex = 0;
            dataGridView1.Visible = false;
            dataGridView1.CellContentClick += dataGridView1_CellContentClick;
            // 
            // button1
            // 
            button1.BackColor = Color.RosyBrown;
            button1.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            button1.ForeColor = SystemColors.ControlLightLight;
            button1.Location = new Point(354, 262);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 0;
            button1.Text = "UPLOAD";
            button1.UseVisualStyleBackColor = false;
            button1.Click += btnUpload_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = SystemColors.ScrollBar;
            pictureBox1.BackgroundImageLayout = ImageLayout.None;
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(275, 39);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(258, 195);
            pictureBox1.TabIndex = 1;
            pictureBox1.TabStop = false;
            // 
            // textBox1
            // 
            textBox1.BackColor = SystemColors.MenuBar;
            textBox1.BorderStyle = BorderStyle.None;
            textBox1.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point);
            textBox1.Location = new Point(241, 240);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(318, 16);
            textBox1.TabIndex = 2;
            textBox1.Text = "                         Convert Your Visions Into Reality";
            // 
            // listBox1
            // 
            listBox1.BackColor = SystemColors.ButtonHighlight;
            listBox1.ForeColor = Color.Navy;
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 15;
            listBox1.Location = new Point(78, 262);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(295, 94);
            listBox1.TabIndex = 4;
            listBox1.Visible = false;
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;
            // 
            // button2
            // 
            button2.BackColor = Color.Gray;
            button2.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            button2.ForeColor = Color.Azure;
            button2.Location = new Point(12, 12);
            button2.Name = "button2";
            button2.Size = new Size(75, 23);
            button2.TabIndex = 5;
            button2.Text = "Back";
            button2.UseVisualStyleBackColor = false;
            button2.Visible = false;
            button2.Click += backButton_Click;
            // 
            // Form1
            // 
            BackColor = Color.FromArgb(128, 64, 64);
            ClientSize = new Size(814, 483);
            Controls.Add(button2);
            Controls.Add(listBox1);
            Controls.Add(dataGridView1);
            Controls.Add(button1);
            Controls.Add(pictureBox1);
            Controls.Add(textBox1);
            ForeColor = Color.Navy;
            Name = "Form1";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private async void btnUpload_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "CSV Files (*.csv)|*.csv|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.Multiselect = false;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;

                    try
                    {
                        string[] lines = File.ReadAllLines(filePath);

                        var csvContent = string.Join("\n", lines);

                        string apiUrl = "http://localhost:7138/"; 
                        HttpResponseMessage response;
                        using (var client = new HttpClient())
                        {
                            var content = new StringContent(csvContent, Encoding.UTF8, "application/csv");
                            response = await client.PostAsync(apiUrl, content);
                        }
                        if (response.IsSuccessStatusCode)
                        {
                            string jsonResponse = await response.Content.ReadAsStringAsync();
                            MessageBox.Show(jsonResponse, "Analysis Results", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Failed to upload CSV file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void backButton_Click(object sender, EventArgs e)
        {
            // Hide the DataGridView and show the upload button, picture box, and text box
            dataGridView1.Visible = false;
            button1.Visible = true;
            pictureBox1.Visible = true;
            textBox1.Visible = true;
            listBox1.Visible = false;
            button2.Visible = false;
        }
        private void PopulateDataGridView(List<string[]> csvData)
        {
            // Clear existing rows and columns
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();

            // Add columns to DataGridView
            dataGridView1.Columns.Add("ProductCode", "Product Code");
            dataGridView1.Columns.Add("Quantity", "Quantity");
            dataGridView1.Columns.Add("CostPerItem", "Cost per item");
            dataGridView1.Columns.Add("DateTime", "Date Time");

            // Add rows to DataGridView
            foreach (var dataRow in csvData)
            {
                dataGridView1.Rows.Add(dataRow);
            }
            button1.Visible = false;
            pictureBox1.Visible = false;
            textBox1.Visible = false;
            button2.Visible = true;
        }
        private void ProcessCSVFile(string filePath)
        {
            try
            {
                // Read CSV file contents
                string[] lines = File.ReadAllLines(filePath);

                // Parse CSV data
                var records = ParseCSVData(lines);

                // Calculate metrics
                DateTime earliestDate = records.Min(r => r.DateTime);
                DateTime latestDate = records.Max(r => r.DateTime);
                decimal totalCost = records.Sum(r => r.Quantity * r.CostPerItem);
                var highestTotalCost = records.GroupBy(r => r.ProductCode)
                                               .Select(group => new
                                               {
                                                   ProductCode = group.Key,
                                                   TotalCost = group.Sum(r => r.Quantity * r.CostPerItem)
                                               })
                                               .OrderByDescending(x => x.TotalCost)
                                               .First();
                double averageQuantity = records.Average(r => r.Quantity);

                // Display the metrics
                DisplayMetrics(earliestDate, latestDate, totalCost, highestTotalCost, averageQuantity);
                listBox1.Items.Clear();
                listBox1.Items.AddRange(GetMetricsText(earliestDate, latestDate, totalCost, highestTotalCost, averageQuantity).Split('\n'));

                button1.Visible = false;
                pictureBox1.Visible = false;
                textBox1.Visible = false;


            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error processing CSV file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GetMetricsText(DateTime earliestDate, DateTime latestDate, decimal totalCost,
                                dynamic highestTotalCost, double averageQuantity)
        {
            return $"1) Earliest date - {earliestDate:dd/MM/yyyy HH:mm:ss}\r\n" +
                   $"2) Latest date - {latestDate:dd/MM/yyyy HH:mm:ss}\r\n" +
                   $"3) Total cost of the upload – R {totalCost:F}\r\n" +
                   $"4) Highest total cost per Name - {highestTotalCost.ProductCode} (R {highestTotalCost.TotalCost:F})\r\n" +
                   $"5) Average quantity – {averageQuantity:F}\r\n";
        }
        private void DisplayMetrics(DateTime earliestDate, DateTime latestDate, decimal totalCost,
                               dynamic highestTotalCost, double averageQuantity)
        {
            listBox1.Items.Clear();
            listBox1.Items.Add($"1) Earliest date - {earliestDate:dd/MM/yyyy HH:mm:ss}");
            listBox1.Items.Add($"2) Latest date - {latestDate:dd/MM/yyyy HH:mm:ss}");
            listBox1.Items.Add($"3) Total cost of the upload – R {totalCost:F}");
            listBox1.Items.Add($"4) Highest total cost per Name - {highestTotalCost.ProductCode} (R {highestTotalCost.TotalCost:F})");
            listBox1.Items.Add($"5) Average quantity – {averageQuantity:F}");

        }

        private List<Record> ParseCSVData(string[] lines)
        {
            var records = new List<Record>();
            foreach (string line in lines.Skip(1)) 
            {
                try
                {
                    string[] cells = line.Split(',');

                    string productCode = cells[0].Trim();
                    int quantity;
                    if (!int.TryParse(cells[1].Trim(), out quantity))
                    {
                        throw new FormatException($"Invalid quantity format: {cells[1]}");
                    }

                    decimal costPerItem;
                    if (!decimal.TryParse(cells[2].Trim().Replace("R", "").Replace(",", "").Trim(),
                        System.Globalization.NumberStyles.Currency,
                        System.Globalization.CultureInfo.InvariantCulture,
                        out costPerItem))
                    {
                        throw new FormatException($"Invalid cost per item format: {cells[2]}");
                    }


                    DateTime dateTime;
                    if (!DateTime.TryParseExact(cells[3].Trim(), "dd/MM/yyyy HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out dateTime))
                    {
                        throw new FormatException($"Invalid DateTime format: {cells[3]}");
                    }

                    records.Add(new Record
                    {
                        ProductCode = productCode,
                        Quantity = quantity,
                        CostPerItem = costPerItem,
                        DateTime = dateTime
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error parsing CSV line: {line}\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            return records;
        }



        private List<string[]> ParseCSV(string filePath)
        {
            List<string[]> csvData = new List<string[]>();

            try
            {
                using (var reader = new StreamReader(filePath))
                {
                    while (!reader.EndOfStream)
                    {
                        string[] line = reader.ReadLine().Split(',');
                        csvData.Add(line);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error reading CSV file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return csvData;
        }

        private Button button1;
        private PictureBox pictureBox1;
        private TextBox textBox1;
        private System.ComponentModel.IContainer components;
        private ListBox listBox1;
        private Button button2;
    }
}