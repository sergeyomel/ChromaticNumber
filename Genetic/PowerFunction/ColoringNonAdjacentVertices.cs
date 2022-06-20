using Genetic.Interface;
using GraphLib;
using System.Collections.Generic;
using System.Linq;

namespace Genetic.PowerFunction
{
    public class ColoringNonAdjacentVertices : IPowerFunction
    {
        Dictionary<Vertex, int> IPowerFunction.IndividualPowerFunction(Individual Individual)
        {
            int CurrentColor = 0;
            var dColorVertex = new Dictionary<Vertex, int>();

            var lNonVisitedVertex = new List<Vertex>();

            var lGenome = new List<Vertex>();
            foreach(var group in Individual.Graph
                                                 .GetVertexDegrees()
                                                 .OrderBy(v => v.Value)
                                                 .Reverse()
                                                 .GroupBy(v => v.Value))
                    lGenome.AddRange(group
                        .OrderBy(v => v.Key.GetID())
                        .Select(v => v.Key));

            dColorVertex[lGenome.First()] = CurrentColor;
            lNonVisitedVertex = new List<Vertex>(lGenome);
            lNonVisitedVertex.Remove(lGenome.First());

            while(lNonVisitedVertex.Count != 0)
            {
                var lNonPaintedVertex = new List<Vertex>();
                foreach(var vertex in lNonVisitedVertex)
                {
                    var lAdjVertex = Individual.Graph.FindAdjacencyVertex(vertex);
                    var lVertCurrentColor = dColorVertex
                        .Where(item => item.Value == CurrentColor)
                        .Select(item => item.Key)
                        .ToList();
                    if (lAdjVertex.Intersect(lVertCurrentColor).Count() != 0)
                        lNonPaintedVertex.Add(vertex);
                    else
                        dColorVertex[vertex] = CurrentColor;
                }
                lNonVisitedVertex = new List<Vertex>(lNonPaintedVertex);
                CurrentColor += 1;
            }
            CurrentColor += 1;

            return dColorVertex;
        }

        public override string ToString()
        {
            return "ColoringNonAdjacentVertices";
        }

    }
}
