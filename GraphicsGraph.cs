using System;
using System.Collections.Generic;
using System.Text;
using GraphLib;
using Graphics.Connectors;
using System.Windows.Controls;
using System.Diagnostics;
using System.Linq;
using System.Windows.Media;

namespace Graphics
{
    public class GraphicsGraph
    {
        private Canvas canvas;
        private int radiusVertex = 25;

        public GraphicsGraph(Canvas c)
        {
            canvas = c;
            graph = new Graph();
        }

        public Graph graph;
        public List<GraphicsVertex> lGraphVertex = new List<GraphicsVertex>();
        public List<GraphicsVertexConnector> lGraphicsVertexConnectors = new List<GraphicsVertexConnector>();

        public GraphicsVertex ContainVertex(double x, double y, int length = 60)
        {
            var nx = x - radiusVertex;
            var ny = y - radiusVertex;
            foreach (var item in lGraphVertex)
                if (Math.Sqrt(Math.Pow((nx - item.posX), 2) + Math.Pow((ny - item.posY), 2)) <= length)
                    return item;
            return null;
        }
        public GraphicsVertex AddGraphVertex(double x, double y)
        {
            var nx = x - radiusVertex;
            var ny = y - radiusVertex;
            var vertex = graph.AddVertex();
            lGraphVertex.Add(new GraphicsVertex(vertex, nx, ny));
            Draw();
            return lGraphVertex.Last();
        }
        public void RemoveGraphVertex(double x, double y)
        {
            var vertex = ContainVertex(x, y, radiusVertex - 1);
            if (vertex != null)
            {
                var lGV = new List<GraphicsVertexConnector>(lGraphicsVertexConnectors);
                foreach (var connector in lGraphicsVertexConnectors)
                    if (connector.startVertex.vertex.Equal(vertex.vertex) ||
                        connector.endVertex.vertex.Equal(vertex.vertex))
                        lGV.Remove(connector);
                lGraphicsVertexConnectors = lGV;

                lGraphVertex.Remove(vertex);
                graph.RemoveVertex(vertex.vertex);

                Draw();
            }
        }

        private GraphicsVertexConnector FindGraphVertConnector(double x, double y)
        {
            foreach(var connector in lGraphicsVertexConnectors)
            {
                (double x, double y) p1 = (connector.startVertex.posX, connector.startVertex.posY);
                (double x, double y) p2 = (connector.endVertex.posX, connector.endVertex.posY);
                (double x, double y) p3 = (x, y);

                if ((1 / 2) * Math.Abs((p2.x - p1.x) * (p3.y - p1.y) - (p3.x - p1.x) * (p2.y - p1.y)) < 2)
                    return connector;
            }
            return null;
        }
        public void AddVertConnector(GraphicsVertex startVert, GraphicsVertex endVert)
        {
            var resAddVertConnector = graph.AddVConnector(startVert.vertex, endVert.vertex);
            if(resAddVertConnector != null)
            {
                if (graph.GetTypeGraph() == GraphLib.TypeGraph.Undirected)
                    lGraphicsVertexConnectors.Add(new GraphicsEdge(resAddVertConnector, startVert, endVert));
                else
                    lGraphicsVertexConnectors.Add(new GraphicsArc(resAddVertConnector, startVert, endVert));
                Draw();
            }
        }
        public void RemoveVertConnector(double x, double y)
        {
            var resContainVC = FindGraphVertConnector(x, y);
            if (resContainVC == null)
                return;
            lGraphicsVertexConnectors.Remove(resContainVC);
            graph.RemoveVConnector(resContainVC.vertConnector);
            Draw();
        }

        public void ChangeTypeGraphicsGraph()
        {
            graph.ChangeTypeGraph();
            var buffGraphVertConnector = new List<GraphicsVertexConnector>();
            foreach (var connector in lGraphicsVertexConnectors)
                buffGraphVertConnector.Add(GraphicsVertexConnector.ConvertConnector(connector));
            lGraphicsVertexConnectors = buffGraphVertConnector;
            Draw();
        }

        public void Draw()
        {
            canvas.Children.Clear();
            foreach (var vertex in lGraphVertex)
            {
                var tupleDraw = vertex.Draw();
                canvas.Children.Add(tupleDraw.Item1);
                canvas.Children.Add(tupleDraw.Item2);
            }
            foreach (var connector in lGraphicsVertexConnectors)
            {
                foreach (var item in connector.Draw())
                    canvas.Children.Add(item);
            }

            var Writer = new GraphLib.Writer.WriterFromFile(@"D:\Input.txt");
            Writer.WriteAdjancyMatrix(graph);
        }

        public void ColoringGraph(Genetic.Individual individ)
        {
            var dColorVert = individ.GetDColorVertex();
            var sColor = new HashSet<Color>();
            var lColor = ColorSet.GetSetColor(dColorVert.GroupBy(item => item.Value).Count()); //Цвета не меняются, потому что генерятся в количестве вершин графа.
            foreach (var gVert in lGraphVertex)
            {
                foreach (var vert in dColorVert.Keys.ToList())
                {
                    if (gVert.vertex.Equal(vert))
                    {
                        gVert.SetFillColor(lColor[dColorVert[vert]]);
                        if (!sColor.Contains(lColor[dColorVert[vert]]))
                            sColor.Add(lColor[dColorVert[vert]]);
                    }
                        
                }
            }
            Draw();
        }

        public void ReserGraphicsVertColor()
        {
            foreach (var gVert in lGraphVertex)
                gVert.SetFillColor(Color.FromRgb(35, 255, 35));
            Draw();
        }

    }
}
