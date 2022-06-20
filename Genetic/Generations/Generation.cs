using System;
using System.Collections.Generic;
using System.Linq;
using Genetic;
using Genetic.Generations.GenerationInterface;
using Genetic.Interface;
using GraphLib;

namespace Genetic.Generations
{
    /// <summary>
    /// Класс - поколение особей.
    /// </summary>
    public class Generation
    {
        protected List<Individual> lNewIndividIndividual;
        protected List<Individual> lCurrentIndividual;

        protected int countIndividualInGeneration;
        protected int countEliteIndividual;

        private int probabilityCrossover = 50;
        private int probabilityMutation = 0;
        protected Random rnd = new Random();

        protected Individual bestIndividInGeneration;

        private IFillingInitialIndividuals filling;
        private ICrossing crossover;
        private IMutation mutation;
        private ISelect selector;

        public Generation(
            List<Individual> lIndivid,
            int probabilityCrossover,
            int probabilityMutation,
            int countIndividualInGeneration,
            int countEliteIndividual,
            IFillingInitialIndividuals filling,
            ICrossing crossover,
            IMutation mutation,
            ISelect selector)
        {
            lNewIndividIndividual = new List<Individual>();
            lCurrentIndividual = lIndivid;

            this.probabilityCrossover = probabilityCrossover;
            this.probabilityMutation = probabilityMutation;
            this.countIndividualInGeneration = countIndividualInGeneration;
            this.countEliteIndividual = countEliteIndividual;

            this.filling = filling;
            this.crossover = crossover;
            this.mutation = mutation;
            this.selector = selector;
            bestIndividInGeneration = lCurrentIndividual is null ? null : lCurrentIndividual.First();
        }
        
        public List<Individual> GetListIndividInGeneration() => lNewIndividIndividual;
        public Individual GetBestIndividInGeneration() => bestIndividInGeneration;

        public void FillingInitialGeneration(Graph graph, IPowerFunction powerFunction)
        {
            lCurrentIndividual = filling.FillingInitialIndivdual(graph, powerFunction, countIndividualInGeneration, countEliteIndividual);
            bestIndividInGeneration = lCurrentIndividual.First();
        }

        /// <summary>
        /// Функция нахождения пары для скрещивания с особью, не равная данной особи.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private int ParentSecondIndex(int index)
        {
            int indexParent = index;
            while (indexParent == index)
                indexParent = rnd.Next(0, countIndividualInGeneration);
            return indexParent;
        }

        /// <summary>
        /// Функция скрещивания.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private (Individual childOne, Individual childTwo) Crossover(int index)
        {
            int indexParentTwo = ParentSecondIndex(index);
            if (rnd.Next(0, 100) < probabilityCrossover)
            {
                var result = crossover.CrossingIndividual(lCurrentIndividual[index], lCurrentIndividual[indexParentTwo]);
                return (result.childOne, result.childTwo);
            }
            return (lCurrentIndividual[index], null);
        }

        private (Individual mutIndividOne, Individual mutIndividTwo) Mutation(Individual individOne, Individual individTwo)
        {
            if(rnd.Next(0, 100) < probabilityMutation)
            {
                individOne =  mutation.MutatingIndividual(individOne) ?? individOne;
                individTwo = individTwo != null ? mutation.MutatingIndividual(individTwo) : null;
                return (individOne, individTwo);
            }
            return (individOne, individTwo);
        }

        public void GenerationIndividual()
        {
            for(int index = 0; index < countIndividualInGeneration; index++)
            {
                var resultCrossover = Crossover(index);
                var resultMutation = Mutation(resultCrossover.childOne, resultCrossover.childTwo);
                var bestIndivid = selector.SelectIndividual(lCurrentIndividual[index], resultMutation.mutIndividOne);
                if (resultMutation.mutIndividTwo != null)
                    bestIndivid = selector.SelectIndividual(bestIndivid, resultMutation.mutIndividTwo);

                if (bestIndivid.TargetFunction() < bestIndividInGeneration.TargetFunction())
                    bestIndividInGeneration = bestIndivid;

                lNewIndividIndividual.Add(bestIndivid);
            }
        }

    }
}
