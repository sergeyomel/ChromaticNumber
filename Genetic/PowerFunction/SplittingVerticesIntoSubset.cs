using Genetic.Interface;
using GraphLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Genetic.PowerFunction
{
    public class SplittingVerticesIntoSubset : IPowerFunction
    {
        /// <summary>
        /// Проверка на то, что максимальная степень у всех вершин равна 0 (все вершины стоят по одной)
        /// </summary>
        /// <param name="graph"></param>
        /// <returns></returns>
        private bool IsMaximallySimplified(Graph graph)
        {
            var lVertexDegrees = graph.GetVertexDegrees();
            foreach (var item in lVertexDegrees.Values)
                if (item != 0)
                    return false;
            return true;
        }

        public Dictionary<Vertex, int> IndividualPowerFunction(Individual Individual)
        {
            var graph = Individual.Graph.Copy();
            var lPaintedVertex = new List<Vertex>();
            var dColorVertex = new Dictionary<Vertex, int>();
            int CurrentColor = 0;

            while (lPaintedVertex.Count() != Individual.Graph.GetGraph().Keys.Count())
            {
                //Разбиваем на одиночные вершины
                while (!IsMaximallySimplified(graph))
                {
                    var vertexWithMaxDegrees = graph
                                            .GetVertexDegrees()
                                            .OrderBy(pair => pair.Value)
                                            .ToDictionary(pair => pair.Key, pair => pair.Value)
                                            .Last()
                                            .Key;
                    graph.RemoveVertex(vertexWithMaxDegrees);
                }

                //Добавляем одиночные вершины с список окрашенных
                foreach (var item in graph.GetGraph().Keys)
                {
                    lPaintedVertex.Add(item);
                    dColorVertex[item] = CurrentColor;
                }
                CurrentColor++;

                //Удаляем из начального графа посещенные вершины
                graph = Individual.Graph.Copy();
                foreach (var item in lPaintedVertex)
                    graph.RemoveVertex(item);
            }
            CurrentColor += 1;

            return dColorVertex;
        }

        public override string ToString()
        {
            return "SplittingVerticesIntoSubset";
        }
    }
}
