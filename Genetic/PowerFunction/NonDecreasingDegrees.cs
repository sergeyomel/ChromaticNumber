using Genetic.Interface;
using GraphLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Genetic.PowerFunction
{
    /// <summary>
    /// Упорядочивание вершин по неубыванию степеней и постепенной их вычеркивание.
    /// https://www.youtube.com/watch?v=iaZVzsu3eyw
    /// </summary>
    public class NonDecreasingDegrees : IPowerFunction
    {
        public Dictionary<Vertex, int> IndividualPowerFunction(Individual Individual)
        {
            int CurrentColor = 0;
            var dColorVertex = new Dictionary<Vertex, int>();
            var nonDecDeg = Individual.Graph
                                            .GetVertexDegrees()
                                            .OrderBy(pair => pair.Value)
                                            .Reverse()
                                            .ToDictionary(pair => pair.Key, pair => pair.Value);

            var lPaintedVertex = new List<Vertex>();
            var lNonpaintedVertex = new List<Vertex>(nonDecDeg.Keys);

            dColorVertex[lNonpaintedVertex.First()] = CurrentColor;
            lPaintedVertex.Add(lNonpaintedVertex.First());
            lNonpaintedVertex.Remove(lNonpaintedVertex.First());

            while(lNonpaintedVertex.Count() != 0)
            {
                foreach(var item in lNonpaintedVertex)
                {
                    var lAdjVertex = Individual.Graph.FindAdjacencyVertex(item);
                    var CurrentColorVertex = dColorVertex
                                                        .Where(item => item.Value == CurrentColor)
                                                        .Select(item => item.Key)
                                                        .Intersect(lPaintedVertex)
                                                        .ToList();
                    if(lAdjVertex.Intersect(CurrentColorVertex).Count() == 0)
                    {
                        lPaintedVertex.Add(item);
                        dColorVertex[item] = CurrentColor;
                    }
                }
                CurrentColor += 1;
                lNonpaintedVertex = lNonpaintedVertex.Except(lNonpaintedVertex.Intersect(lPaintedVertex)).ToList();
            }
            CurrentColor += 1;

            return dColorVertex;
        }

        public override string ToString()
        {
            return "NonDecreasingDegrees";
        }
    }
}
