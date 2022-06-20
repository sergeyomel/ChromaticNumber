using Genetic.Interface;
using GraphLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Genetic.Generations.GenerationInterface
{
    public class FillingRandomIndividuals: IFillingInitialIndividuals
    {
        public List<Individual> FillingInitialIndivdual(Graph graph, IPowerFunction powerFunction, int countIndividualInGeneration,  int countEliteIndividual)
        {
            var lIndividual = new List<Individual>();

            var lVertex = graph.GetGraph().Select(item => item.Key).ToList();
            for (int i = 0; i < countIndividualInGeneration; i++)
                lIndividual.Add(new Individual(graph, lVertex.Shuffle(), powerFunction));

            return lIndividual;
        }

        public override string ToString()
        {
            return "RandomIndividual";
        }
    }
}
