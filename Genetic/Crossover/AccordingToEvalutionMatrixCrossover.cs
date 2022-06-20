using Genetic.Interface;
using GraphLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Genetic.Crossover
{
    public class AccordingToEvalutionMatrixCrossover : ICrossing
    {
        private List<Vertex> AdditionGenotypeBasedEvaluationMatrix(List<Vertex> child, int pointCross, Individual individOne, Individual individTwo)
        {
            var EvaluationMatrixIndivideOne = individOne.Graph.GetEvaluationMatrix();
            Vertex CurrentVertex = child.Last();
            for (int index = pointCross; index < individOne.Genome.Count; index++)
            {
                var valueOneEM = EvaluationMatrixIndivideOne[CurrentVertex][individOne.Genome[index]];
                var valueTwoEM = EvaluationMatrixIndivideOne[CurrentVertex][individTwo.Genome[index]];
                if (valueOneEM >= valueTwoEM)
                    CurrentVertex = individOne.Genome[index];
                else
                    CurrentVertex = individTwo.Genome[index];
                child.Add(CurrentVertex);
            }
            return child;
        }

        public (Individual childOne, Individual childTwo) CrossingIndividual(Individual individOne, Individual individTwo)
        {
            var rnd = new Random();
            var pointCross = rnd.Next(1, individOne.Genome.Count);
            var fullGenome = new List<Vertex>(individOne.Genome);

            var newIndividOne = AdditionGenotypeBasedEvaluationMatrix(individOne.Genome.Take(pointCross).ToList(),
                                                                      pointCross,
                                                                      individOne, 
                                                                      individTwo)
                                .Distinct().ToList();
            var newIndividTwo = AdditionGenotypeBasedEvaluationMatrix(individTwo.Genome.Take(pointCross).ToList(),
                                                                      pointCross,
                                                                      individOne,
                                                                      individTwo)
                                .Distinct().ToList();

            newIndividOne.AddRange(fullGenome.Except(newIndividOne).ToList().Shuffle());
            newIndividTwo.AddRange(fullGenome.Except(newIndividTwo).ToList().Shuffle());

            return (
               new Individual(individOne.Graph, newIndividOne, individOne.GetPowerFunction()),
               new Individual(individOne.Graph, newIndividTwo, individOne.GetPowerFunction()));
        }

        public override string ToString()
        {
            return "ACC. EVA. MX.";
        }
    }
}
