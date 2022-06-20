using Genetic.Interface;
using Genetic.PowerFunction;
using GraphLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Genetic.Generations.GenerationInterface
{
    public class FillingEliteIndividuals : IFillingInitialIndividuals
    {
        public List<Individual> FillingInitialIndivdual(Graph graph, IPowerFunction powerFunction, int countIndividualInGeneration, int countEliteIndividual)
        {
            var lIndividuals = new List<Individual>();

            var lVertex = graph.GetGraph().Select(item => item.Key).ToList();
            var dColorVertex = new Individual(graph, lVertex, new NonDecreasingDegrees()).GetDColorVertex();
            var eliteGenome = new List<Vertex>();
            foreach (var lGroupVertex in dColorVertex
                                                       .GroupBy(item => item.Value)
                                                       .Select(color => new
                                                       {
                                                           lVertexForColor = color.Select(vertex => vertex.Key)
                                                       }))
                eliteGenome.AddRange(lGroupVertex.lVertexForColor.ToList());

            if (countEliteIndividual > countIndividualInGeneration)
                countEliteIndividual = countIndividualInGeneration;
            for (int i = 0; i < countEliteIndividual; i++)
                lIndividuals.Add(new Individual(graph, eliteGenome, powerFunction));
            if (countEliteIndividual < countIndividualInGeneration)
            {
                for (int i = 0; i < countIndividualInGeneration - countEliteIndividual; i++)
                    lIndividuals.Add(new Individual(graph, lVertex.Shuffle(), powerFunction));
            }

            return lIndividuals;
        }

        public override string ToString()
        {
            return "EliteIndividual";
        }
    }
}
