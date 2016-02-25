using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VTK_C3D.Net
{
    public partial class Form1 : Form
    {
        VTKControl VTKControl1;
        FileReader fileReader1;
        public Form1()
        {
            VTKControl1 = new VTKControl(renderWindowControl1);
            fileReader1 = new FileReader();
            InitializeComponent();
        }
        //Odpalenie wyszukiwania pliku
        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }
        //Udane otwarcie pliku
        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            try
            {
                richTextBox1.Text = fileReader1.c3dFileString(openFileDialog1.InitialDirectory + openFileDialog1.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                VTKControl1.VTKRender(fileReader1.xyzfile(textFrameNr.Text, openFileDialog1.InitialDirectory + openFileDialog1.FileName));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
