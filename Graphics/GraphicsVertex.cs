using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using GraphLib;

namespace Graphics
{
    public class GraphicsVertex
    {
        public Vertex vertex;
        public Color fillColor = Color.FromRgb(35, 255, 35);
        public int StrokeTh = 2;
        public double posX;
        public double posY;
        public int radius = 25;

        public GraphicsVertex(Vertex v, double x, double y)
        {
            vertex = v;
            posX = x;
            posY = y;
        }

        public void SetFillColor(Color color)
        {
            fillColor = color;
        }
        public int GetStrokeTh() { return StrokeTh; }
        public void SetStrokeTh(int value) { StrokeTh = value; }

        public (Ellipse, TextBlock) Draw()
        {
            var circle = new Ellipse();
            circle.Width = radius * 2;
            circle.Height = radius * 2;
            circle.Fill = new SolidColorBrush(fillColor);
            circle.Stroke = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            circle.StrokeThickness = StrokeTh;
            Canvas.SetZIndex(circle, 2);
            Canvas.SetLeft(circle, posX);
            Canvas.SetTop(circle, posY);

            var tb = new TextBlock();
            tb.Text = vertex.GetID().ToString();
            tb.FontSize = 18;
            tb.FontWeight = System.Windows.FontWeights.Bold;
            Canvas.SetZIndex(tb, 3);
            Canvas.SetLeft(tb, posX + 15);
            Canvas.SetTop(tb, posY + 10);

            return (circle, tb);
        }
    }
}
