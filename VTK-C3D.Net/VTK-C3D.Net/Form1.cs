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
        //Zmienne potrzebne do obsługi plików
        Kitware.VTK.vtkSimplePointsReader reader;
        Kitware.VTK.vtkPolyDataMapper mapper;
        Kitware.VTK.vtkRenderWindow renderwindow;
        Kitware.VTK.vtkRenderer renderer;
        Kitware.VTK.vtkActor actor;
        C3D.C3DFile file;
        System.IO.FileStream filexyz;


        //Funkcje dopisane
        //Funkcja aktualizacji okna(Do poprawy oraz dodanie animacji).
        private void aktualizacja()
        {

            renderWindowControl1.Refresh();

            reader = new Kitware.VTK.vtkSimplePointsReader();
            reader.SetFileName(openFileDialog1.InitialDirectory + openFileDialog1.FileName+".xyz");
            reader.Update();

            mapper = Kitware.VTK.vtkPolyDataMapper.New();
            mapper.SetInputConnection(reader.GetOutputPort());
            actor = Kitware.VTK.vtkActor.New();
            actor.SetMapper(mapper);
            actor.SetScale(0.01, 0.01, 0.01);
            actor.GetProperty().SetPointSize(1);
            actor.SetOrigin(0, 0, 0);


            renderwindow = renderWindowControl1.RenderWindow;
            renderer = renderwindow.GetRenderers().GetFirstRenderer();
            renderer.SetBackground(0.3, 0.6, 0.3);
            renderer.AddActor(actor);
            renderer.Clear();
            renderWindowControl1.Refresh();

        }
        public Form1()
        {
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
                //Wczytuję plik C3D
                file = C3D.C3DFile.LoadFromFile(openFileDialog1.InitialDirectory + openFileDialog1.FileName);
                //Kod od chińczyka inaczej nie działa.
                UInt16 firstFrameIndex = file.Header.FirstFrameIndex;
                UInt16 pointCount = file.Parameters["POINT:USED"].GetData<UInt16>();
                String allframes = "";
                String xyzstring = "";

                for (Int32 i = 0; i < file.AllFrames.Count; i++)
                {
                    //you can also use file.AllFrames[i].Point3Ds.Length
                    for (Int32 j = 0; j < pointCount; j++)
                    {
                        //Tworzenie linii  z koordynatów punktów.
                        String line = "Frame " +
                            (firstFrameIndex + i).ToString() +
                            " " +
                            " X = " +
                            file.AllFrames.Get3DPoint(i, j).X.ToString("R") +
                            " Y = " +
                            file.AllFrames.Get3DPoint(i, j).Y.ToString() +
                            " Z = " +
                            file.AllFrames.Get3DPoint(i, j).Z.ToString() + "\n";
                        if (i == 0)
                        {
                            //Zapis pierwszej klatki do pliku XYZ
                            String xyzline = file.AllFrames.Get3DPoint(i, j).X.ToString("G", System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")) + " " +
                                file.AllFrames.Get3DPoint(i, j).Y.ToString("G", System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")) + " " +
                                file.AllFrames.Get3DPoint(i, j).Z.ToString("G", System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")) + "\n";

                            xyzstring = xyzstring + xyzline;
                        }
                        //Zapis wygenerowanej linii
                        allframes = allframes + line;
                        /*Console.WriteLine("Frame {0} : X = {1}, Y = {2}, Z = {3}",
                            firstFrameIndex + i,
                            file.AllFrames.Get3DPoint(i, j).X,
                            file.AllFrames.Get3DPoint(i, j).Y,
                            file.AllFrames.Get3DPoint(i, j).Z);
                         */
                    }
                }

               //Zapis pliku XYZ
                filexyz = System.IO.File.Create(openFileDialog1.InitialDirectory + openFileDialog1.FileName+".xyz");
                filexyz.Write(Encoding.ASCII.GetBytes(xyzstring), 0, xyzstring.Length);
                filexyz.Close();
                //Wyświetlenie zawartości klatek.
                richTextBox1.Text = allframes;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            aktualizacja();
        }

        private void process1_Exited(object sender, EventArgs e)
        {

        }
    }
}
