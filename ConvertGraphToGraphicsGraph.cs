using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace Graphics
{
    public class ConverterGraphToGraphicsGraph
    {
        public GraphicsGraph graphicsGraph;

        int startAngle = -160;
        int incStartAngle = 43;
        int radius = 100;
        int incrementRadiusAtiteration = 50;

        int startX;
        int startY;

        int coordX = 0;
        int coordY = 0;

        int countVertex;
        int countVertOnCircle = 15;
        int buffCountVert = 0;
        int countDrawVertex = 0;
        int incrAngleRotationAtIter;

        public ConverterGraphToGraphicsGraph(Canvas canvas)
        {
            graphicsGraph = new GraphicsGraph(canvas);

            startX = (int)canvas.Width / 2;
            startY = (int)canvas.Height / 2;
        }

        private void FillingGraphVertex(GraphLib.Graph graph)
        {
            foreach (var key in graph.GetGraph().Keys.ToList())
            {
                if (buffCountVert % countVertOnCircle == 0)
                {
                    startAngle += incStartAngle;
                    incrAngleRotationAtIter = (countVertex - countDrawVertex) <= countVertOnCircle ? 360 / (countVertex - countDrawVertex) : 360 / countVertOnCircle;
                    radius += incrementRadiusAtiteration;
                    buffCountVert = 0;
                }

                var radian = Convert.ToDouble(startAngle) * Math.PI / 180;
                coordX = startX + (int)(radius * Math.Cos(radian));
                coordY = startY + (int)(radius * Math.Sin(radian));
                graphicsGraph.AddGraphVertex(coordX, coordY);

                startAngle += incrAngleRotationAtIter;
                buffCountVert += 1;
                countDrawVertex += 1;
            }
        }

        private GraphicsVertex FindGraphicsVertex(GraphLib.Vertex vertex)
        {
            foreach (var item in graphicsGraph.lGraphVertex)
                if (item.vertex.Equal(vertex))
                    return item;
            return null;
        }

        private void FillingGraphConnector(GraphLib.Graph graph)
        {
            foreach(var lVertConnect in graph.GetGraph().Values)
            {
                foreach(var item in lVertConnect)
                {
                    var start = FindGraphicsVertex(item.GetStartV());
                    var end = FindGraphicsVertex(item.GetEndV());
                    graphicsGraph.AddVertConnector(start, end);
                }
            }
        }

        public GraphicsGraph ConvertGraph(GraphLib.Graph graph)
        {
            countVertex = graph.GetGraph().Keys.Count();
            incrAngleRotationAtIter = countVertex <= countVertOnCircle ? 360 / countVertex : 360 / countVertOnCircle;

            FillingGraphVertex(graph);
            FillingGraphConnector(graph);

            return graphicsGraph;

        }


    }
}
