using GraphLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Graphics.Connectors
{
    /// <summary>
    /// Класс отрисовки для ребра в ненаправленном графе.
    /// </summary>
    public class GraphicsEdge : GraphicsVertexConnector
    {
        public GraphicsEdge(VertexConnector vertConnector, GraphicsVertex startV, GraphicsVertex endV) : 
            base(vertConnector, startV, endV){}

        public override List<Line> Draw()
        {
            (double x, double y) p1 = (startVertex.posX, startVertex.posY);
            (double x, double y) p2 = (endVertex.posX, endVertex.posY);

            #region MainLine
            var r1 = startVertex.radius;
            var r2 = endVertex.radius;
            var line = new Line();
            line.X1 = p1.x + r1;
            line.X2 = p2.x + r2;
            line.Y1 = p1.y + r1;
            line.Y2 = p2.y + r2;
            line.Stroke = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            line.StrokeThickness = 2;
            Canvas.SetZIndex(line, 1);
            #endregion

            #region ArrowLine
            /*var coordArrowX = (p1.x + proportionArrow * p2.x) / (1 + proportionArrow);
            var coordArrowY = (p1.y + proportionArrow * p2.y) / (1 + proportionArrow);
            (double x, double y) p3 = (coordArrowX, coordArrowY);
            (double dx, double dy) D = (p3.x - p1.x, p3.y - p1.y);
            var Norm = Math.Sqrt(D.dx * D.dx + D.dy * D.dy);
            (double udx, double udy) uD = (D.dx / Norm, D.dy / Norm);
            var ax = uD.udx * Math.Sqrt(3) / 2 - uD.udy * 1 / 2;
            var ay = uD.udx * 1 / 2 + uD.udy * Math.Sqrt(3) / 2;
            var bx = uD.udx * Math.Sqrt(3) / 2 + uD.udy * 1 / 2;
            var by = -uD.udx * 1 / 2 + uD.udy * Math.Sqrt(3) / 2;

            var length = 20;
            var leftLine = new Line();
            leftLine.X1 = p3.x;
            leftLine.Y1 = p3.y;
            leftLine.X2 = p3.x + length * bx;
            leftLine.Y2 = p3.y + length * by;
            leftLine.Stroke = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            leftLine.StrokeThickness = 2;
            Canvas.SetZIndex(leftLine, 1);

            var rightLine = new Line();
            rightLine.X2 = p3.x;
            rightLine.Y2 = p3.y;
            rightLine.X1 = p3.x + length * ax;
            rightLine.Y1 = p3.y + length * ay;
            rightLine.Stroke = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            rightLine.StrokeThickness = 2;
            Canvas.SetZIndex(rightLine, 1);*/
            #endregion

            return new List<Line>() { line };
        }
    }
}
