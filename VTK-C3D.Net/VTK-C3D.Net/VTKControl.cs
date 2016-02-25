using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VTK_C3D.Net
{
    class VTKControl
    {
        Kitware.VTK.vtkSimplePointsReader reader;
        Kitware.VTK.vtkPolyDataMapper mapper;
        Kitware.VTK.vtkRenderWindow renderwindow;
        Kitware.VTK.vtkRenderer renderer;
        Kitware.VTK.vtkActor actor;
        Kitware.VTK.RenderWindowControl renderWindowControl1;

        public VTKControl(Kitware.VTK.RenderWindowControl _renderWindowControl1)
        {
            renderWindowControl1 = _renderWindowControl1;
        }

        public void VTKRender(string filePath)
        {
            if (renderer != null)
            {
                renderer.Clear();
                mapper.RemoveAllInputs();
            }

            renderWindowControl1.Refresh();
            reader = new Kitware.VTK.vtkSimplePointsReader();
            reader.SetFileName(filePath);
            reader.Update();

            mapper = Kitware.VTK.vtkPolyDataMapper.New();
            mapper.SetInputConnection(reader.GetOutputPort());
            actor = Kitware.VTK.vtkActor.New();
            actor.SetMapper(mapper);
            actor.SetScale(0.01, 0.01, 0.01);
            actor.GetProperty().SetPointSize(1);


            renderwindow = renderWindowControl1.RenderWindow;
            renderer = renderwindow.GetRenderers().GetFirstRenderer();
            renderer.SetBackground(0.3, 0.6, 0.3);
            renderer.AddActor(actor);
            renderer.Clear();
            renderWindowControl1.Refresh();

        }
    }
}
