using Genetic.Interface;
using GraphLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Genetic.Crossover
{
    public class RoadCrossover : ICrossing
    {
        private Individual CrossIndividual(int pOne, int pTwo, Individual one, Individual two)
        {
            var resultingGenotype = new List<Vertex>();
            for (int pos = 0; pos < one.Genome.Count; pos++)
                resultingGenotype.Add(null);
            var lMiddleVertex = one.Genome.Skip(pOne).Take(pTwo - pOne).ToList();
            var buffPOne = pOne;
            foreach (var item in lMiddleVertex)
                resultingGenotype[buffPOne++] = item;

            var lShakeVertexSecondParent = two.Genome.Skip(pTwo).Take(two.Genome.Count - pTwo).ToList();
                lShakeVertexSecondParent.AddRange(two.Genome.Take(pTwo));
            foreach (var item in lMiddleVertex)
                lShakeVertexSecondParent.Remove(item);

            int posInGenotype = pTwo;
            foreach(var item in lShakeVertexSecondParent)
                resultingGenotype[(posInGenotype++) % one.Genome.Count] = item;

            return new Individual(one.Graph, resultingGenotype, one.GetPowerFunction());
        }

        public (Individual childOne, Individual childTwo) CrossingIndividual(Individual individOne, Individual individTwo)
        {
            var rnd = new Random();
            var pointCrossStart = rnd.Next(0, individOne.Genome.Count / 2);
            var pointCrossEnd = rnd.Next(individOne.Genome.Count / 2, individOne.Genome.Count);

            return (CrossIndividual(pointCrossStart, pointCrossEnd, individOne, individTwo),
                    CrossIndividual(pointCrossStart, pointCrossEnd, individTwo, individOne));
        }

        public override string ToString()
        {
            return "ROAD CROSS";
        }
    }
}
