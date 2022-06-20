using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace GraphLib
{
    public class Graph
    {
        private Dictionary<Vertex, List<VertexConnector>> graph;
        private TypeGraph typeGraph;

        private VCManager VertConnectManager;
        private VManager VertManager;

        private Dictionary<Vertex, Dictionary<Vertex, double>> EvaluationMatrix = null;

        public Graph()
        {
            graph = new Dictionary<Vertex, List<VertexConnector>>();
            typeGraph = TypeGraph.Undirected;

            VertConnectManager = new EdgeManager();
            VertManager = new VManager();
        }

        private Graph(
                        Dictionary<Vertex, List<VertexConnector>> Graph, 
                        TypeGraph TypeGraph,
                        VCManager vertexConnector,
                        VManager vManager)
        {
            graph = Graph;
            typeGraph = TypeGraph;

            VertConnectManager = vertexConnector;
            VertManager = vManager;
        }

        public Dictionary<Vertex, List<VertexConnector>> GetGraph() => graph;
        public Dictionary<Vertex, Dictionary<Vertex, double>> GetEvaluationMatrix()
        {
            if(EvaluationMatrix == null)
                EvaluationMatrix = CalculationEvaluationMatrix();
            return EvaluationMatrix;
        }
        public TypeGraph GetTypeGraph() => typeGraph;
        public Vertex GetLastVertex() => graph.ElementAt(graph.Keys.Count() - 1).Key;
        public Vertex GetVertex(int id)
        {
            foreach (var key in graph.Keys.ToList())
                if (key.GetID() == id)
                    return key;
            return null;
        }

        public Vertex AddVertex()
        {
            var vertex = VertManager.AddVertex();
            graph.Add(vertex, new List<VertexConnector>());
            return vertex;
        }
        public void RemoveVertex(Vertex v)
        {
            if (!graph.ContainsKey(v))
                return;

            graph.Remove(v);
            VertManager.RemoveVertex(v);
            foreach(var key in graph.Keys.ToList())
            {
                var buffVertConnector = new List<VertexConnector>();
                foreach (var vc in graph[key])
                    if (!vc.ContainVertex(v))
                        buffVertConnector.Add(vc);
                graph[key] = buffVertConnector;
            }
        }

        public VertexConnector ContainVertConnector(Vertex start, Vertex end)
        {
            var buffVertConnector = new VertexConnector(start, end);
            foreach (var key in graph.Keys.ToList())
            {
                foreach (var vc in graph[key])
                    if (vc.Equal(buffVertConnector))
                        return vc;
            }
            return null;
        }
        public VertexConnector AddVConnector(Vertex start, Vertex end) 
        {
            var resContain = ContainVertConnector(start, end);
            if(resContain != null)
                return null;
            else {
                var connector = VertConnectManager.AddConnector(start, end, false);
                graph[start].Add(connector);
                if (typeGraph == TypeGraph.Undirected)
                    graph[end].Add(VertConnectManager.AddConnector(end, start, true));
                return connector;
            }
        }
        public void RemoveVConnector(VertexConnector vc) 
        {
            foreach (var key in graph.Keys.ToList())
            {
                if (graph[key].Contains(vc))
                {
                    graph[key].Remove(vc);
                    if (typeGraph == TypeGraph.Directed)
                    {
                        var reversVC = ContainVertConnector(vc.GetEndV(), vc.GetStartV());
                        graph[vc.GetEndV()].Remove(reversVC);
                    }
                    return;
                }
            }
        }

        private void ReplaceVCManager()
        {
            if (VertConnectManager.GetType() == typeof(EdgeManager))
                VertConnectManager = new ArcManager();
            else
                VertConnectManager = new EdgeManager();
        }
        private void ConvertUndirectToDirect()
        {
            foreach (var key in graph.Keys.ToList())
            {
                var buffVertConnector = new List<VertexConnector>();
                foreach (var connector in graph[key])
                {
                    if (!connector.IsBuff)
                        buffVertConnector.Add(VertexConnector.ConvertConnector(connector));
                }
                graph[key] = buffVertConnector;
            }
        }
        private void ConvertDirectToUndirect()
        {
            var buffGraph = new Dictionary<Vertex, List<VertexConnector>>();
            foreach (var key in graph.Keys.ToList())
                buffGraph.Add(key, new List<VertexConnector>());
            foreach (var key in graph.Keys.ToList())
                foreach (var connector in graph[key])
                    buffGraph[connector.GetEndV()].Add(VertConnectManager.AddConnector(connector.GetEndV(), connector.GetStartV(), true));
            foreach (var key in graph.Keys.ToList())
                graph[key].AddRange(buffGraph[key]);
        }

        public void ChangeTypeGraph()
        {
            typeGraph = typeGraph == TypeGraph.Directed ? TypeGraph.Undirected : TypeGraph.Directed;
            ReplaceVCManager();
            if (typeGraph == TypeGraph.Directed)
                ConvertUndirectToDirect();
            else
                ConvertDirectToUndirect();
        }

        /// <summary>
        /// Функция поиска смежных верин для данной вершины.
        /// </summary>
        /// <param name="v"></param>
        /// <param name="graph"></param>
        /// <returns></returns>
        public List<Vertex> FindAdjacencyVertex(Vertex v)
        {
            var lAdjVertex = graph[v].Select(item => item.ReverseVertex(v)).ToList();
            if (typeGraph == TypeGraph.Undirected)
                return lAdjVertex;
            var buffAdjVertex = lAdjVertex
                                            .Select(vertex => graph[vertex]
                                            .Select(connector => connector.ReverseVertex(vertex))
                                            .ToList())
                                            .ToList();
            foreach (var item in buffAdjVertex)
                lAdjVertex.AddRange(item);

            return lAdjVertex.Distinct().ToList();
        }

        /// <summary>
        /// Функция просчёта оценочной матрицы.
        /// </summary>
        private Dictionary<Vertex, Dictionary<Vertex, double>> CalculationEvaluationMatrix()
        {
            EvaluationMatrix = new Dictionary<Vertex, Dictionary<Vertex, double>>();
            int size = graph.Keys.Count;

            foreach (var rowVertex in graph.Keys)
                EvaluationMatrix.Add(rowVertex, new Dictionary<Vertex, double>());

            foreach(var rowVertex in graph.Keys)
            {
                foreach(var columnVertex in graph.Keys)
                {
                    var localDegrVert = graph[rowVertex].Count();
                    var localDegcVert = graph[columnVertex].Count();
                    var sv = FindAdjacencyVertex(rowVertex).Contains(columnVertex) ? 0 : (0.3 * size);
                    EvaluationMatrix[rowVertex][columnVertex] = localDegrVert + localDegcVert + sv;
                }
            }

            return EvaluationMatrix;
        }

        /// <summary>
        /// Функция подсчета локальной степени вершины.
        /// </summary>
        /// <returns></returns>
        public Dictionary<Vertex, int> GetVertexDegrees()
        {
            var dDegrees = new Dictionary<Vertex, int>();
            foreach (var value in graph)
                dDegrees.Add(value.Key, value.Value.Count());
            return dDegrees;
        }

        public Graph Copy()
        {
            return new Graph(new Dictionary<Vertex, List<VertexConnector>>(graph), typeGraph, VertConnectManager, VertManager);
        }

        public override string ToString()
        {
            var sb = new StringBuilder("");
            foreach(var key in graph.Keys)
            {
                sb.Append(key+" |");
                foreach (var vc in graph[key])
                    sb.Append(vc.ToString()+" ");
                sb.Append("\n");
            }
            return sb.ToString();
        }

    }
}
