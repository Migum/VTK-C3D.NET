using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VTK_C3D.Net
{
    class FileReader
    {
        C3D.C3DFile file;
        System.IO.FileStream filexyz;

        public string c3dFileString(string filePath)
        {
                //Wczytuję plik C3D
                file = C3D.C3DFile.LoadFromFile(filePath);
                //Kod od chińczyka inaczej nie działa.
                UInt16 firstFrameIndex = file.Header.FirstFrameIndex;
                UInt16 pointCount = file.Parameters["POINT:USED"].GetData<UInt16>();
                String[] labels = (file.Parameters.ContainsParameter("POINT", "LABELS") ? file.Parameters["POINT", "LABELS"].GetData<String[]>() : null);
                String allframes = "";

                for (Int32 i = 0; i < file.AllFrames.Count; i++)
                {
                    //you can also use file.AllFrames[i].Point3Ds.Length
                    for (Int32 j = 0; j < pointCount; j++)
                    {

                        //Tworzenie linii  z koordynatów punktów.
                        String line = "Frame " +
                            (firstFrameIndex + i).ToString() +
                            " " +
                            labels[j] +
                            " X = " +
                            file.AllFrames.Get3DPoint(i, j).X.ToString("R") +
                            " Y = " +
                            file.AllFrames.Get3DPoint(i, j).Y.ToString() +
                            " Z = " +
                            file.AllFrames.Get3DPoint(i, j).Z.ToString() + "\n";
                        allframes = allframes + line;
                    }
                }
                return allframes;
        }
        public string xyzfile(string textFrameNr,string filePath)
        {
            String xyzstring = "";
                Int32 framenr = Convert.ToInt32(textFrameNr);
                if (framenr > 0 && framenr <= file.AllFrames.Count)
                {
                    for (int i = 0; i < file.AllFrames[framenr].Point3Ds.Length; i++)
                    {
                        String xyzline = file.AllFrames.Get3DPoint(framenr, i).X.ToString("G", System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")) + " " +
                        file.AllFrames.Get3DPoint(framenr, i).Y.ToString("G", System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")) + " " +
                        file.AllFrames.Get3DPoint(framenr, i).Z.ToString("G", System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")) + "\n";
                        xyzstring = xyzstring + xyzline;
                    }
                }

                filexyz = System.IO.File.Create(filePath+"_frame" + textFrameNr + ".xyz");
                filexyz.Write(Encoding.ASCII.GetBytes(xyzstring), 0, xyzstring.Length);
                filexyz.Close();
                return filePath + "_frame" + textFrameNr + ".xyz";
         }

    }
}