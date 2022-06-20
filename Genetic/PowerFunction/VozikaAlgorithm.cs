using Genetic.Interface;
using GraphLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Genetic.PowerFunction
{
    public class VozikaAlgorithm : IPowerFunction
    {
        Dictionary<Vertex, int> IPowerFunction.IndividualPowerFunction(Individual Individual)
        {
            int CurrentColor = 0;
            var dColorVertex = new Dictionary<Vertex, int>();
            dColorVertex[Individual.Genome.First()] = CurrentColor;

            foreach (var vertex in Individual.Genome.Skip(1))
            {
                var lAdjVertex = Individual.Graph.FindAdjacencyVertex(vertex);
                var lVertCurrentColor = dColorVertex
                    .Where(item => item.Value == CurrentColor)
                    .Select(item => item.Key)
                    .ToList();
                if (lAdjVertex.Intersect(lVertCurrentColor).Count() != 0)
                    CurrentColor++;
                dColorVertex[vertex] = CurrentColor;
            }
            CurrentColor += 1;

            return dColorVertex;
        }

        public override string ToString()
        {
            return "VozikaAlgorithm";
        }

    }
}
