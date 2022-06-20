using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using GraphLib;

namespace Graphics.Connectors
{
    class GraphicsArc : GraphicsVertexConnector
    {
        public GraphicsArc(VertexConnector vertConnector, GraphicsVertex startV, GraphicsVertex endV) : 
            base(vertConnector, startV, endV) { }

        public override List<Line> Draw()
        {
            (double x, double y) p1 = (startVertex.posX, startVertex.posY);
            (double x, double y) p2 = (endVertex.posX, endVertex.posY);

            #region MainLine
            var r1 = startVertex.radius;
            var r2 = endVertex.radius;
            var line = new Line();
            line.X1 = p1.x+r1;
            line.X2 = p2.x+r2;
            line.Y1 = p1.y+r1;
            line.Y2 = p2.y+r2;
            line.Stroke = new SolidColorBrush(Color.FromRgb(255, 35, 35));
            line.StrokeThickness = 2;
            Canvas.SetZIndex(line, 1);
            #endregion

            #region ArrowCoord
            /*(double x, double y) sub((double x, double y) A, (double x, double y) B)
            {
                var x = B.x - A.x;
                var y = B.y - A.y;
                return (x, y);
            }

            double len((double x, double y) p)
            {
                return Math.Sqrt(Math.Pow(p.x, 2) + Math.Pow(p.y, 2));
            }

            List<(double x, double y)> arrowhead((double x, double y) A, (double x, double y) B)
            {
                double h = 10 * Math.Sqrt(3);
                double w = 10;
                (double x, double y) U = (sub(B, A).x / len(sub(B, A)), sub(B, A).y / len(sub(B, A)));
                (double x, double y) V = (-U.y, U.x); ;
                (double x, double y) v1 = (B.x + h * U.x + w * V.x, B.y - h * U.y + w * V.y);
                (double x, double y) v2 = (B.x + h * U.x - w * V.x, B.y - h * U.y - w * V.y);
                return new List<(double x, double y)> { v1, v2 };
            }

            var coordArrowX = (p1.x + proportionArrow * p2.x) / (1 + proportionArrow);
            var coordArrowY = (p1.y + proportionArrow * p2.y) / (1 + proportionArrow);

            var result = arrowhead(p1, (coordArrowX, coordArrowY));
            var coordLeftArrow = result[0];
            var coordRightArrow = result[1];*/
            #endregion

            #region ArrowLine
            /*var leftLine = new Line();
            leftLine.X1 = coordArrowX;
            leftLine.Y1 = coordArrowY;
            leftLine.X2 = coordLeftArrow.x;
            leftLine.Y2 = coordLeftArrow.y;
            leftLine.Stroke = new SolidColorBrush(Color.FromRgb(255, 0, 0));
            leftLine.StrokeThickness = 2;
            Canvas.SetZIndex(leftLine, 1);

            var rightLine = new Line();
            rightLine.X2 = coordArrowX;
            rightLine.Y2 = coordArrowY;
            rightLine.X1 = coordRightArrow.x;
            rightLine.Y1 = coordRightArrow.y;
            rightLine.Stroke = new SolidColorBrush(Color.FromRgb(255, 0, 0));
            rightLine.StrokeThickness = 2;
            Canvas.SetZIndex(rightLine, 1);*/
            #endregion

            return new List<Line>() { line };
        }
    }
}
