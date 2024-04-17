using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace WinFormsApplication
{
    public partial class Form1 : Form
    {
        //public Form1()
        //{
        //    InitializeComponent();
        //}

        // Other methods...

        private void DisplayAnalysisResult(List<string[]> csvData)
        {
            // Display basic analysis result
            int totalLines = csvData.Count;
            int totalColumns = csvData[0].Length;

            MessageBox.Show($"CSV file contains {totalLines} lines and {totalColumns} columns.", "Analysis Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
