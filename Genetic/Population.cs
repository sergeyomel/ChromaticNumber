using Genetic.Interface;
using GraphLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Genetic.Generations;
using Genetic.Generations.GenerationInterface;

namespace Genetic
{
    /// <summary>
    /// Класс - популяция.
    /// Через него производится настройка 
    /// операций Кроссинговера, Мутаций и Алгоритма отбора особей.
    /// </summary>
    public class Population
    {
        protected Graph graph;

        protected TimeSpan leadTime;

        protected int probabilityCrossover;
        protected int probabilityMutation;
        protected int countIndividualInGenerate;
        protected int countEliteIndividual;
        protected int countRepeatBestGeneration;
        protected int buffCountIndivid;
        protected int totalGeneration;
        protected Individual bestIndividInPopulation;

        protected IFillingInitialIndividuals filling;
        protected ICrossing crossover;
        protected IMutation mutation;
        protected ISelect selector;
        protected IPowerFunction powerFunction;

        protected List<Individual> lStartIndividual;

        public Population(
            int probabilityCrossover,
            int probabilityMutation,
            int countIndividualInGenerate,
            int countEliteIndividual,
            int countRepeatBestGeneration,
            Graph graph, 
            IFillingInitialIndividuals filling,
            ICrossing crossover,
            IMutation mutation,
            ISelect selector,
            IPowerFunction powerFunction)
        {
            this.graph = graph;
            this.probabilityCrossover = probabilityCrossover;
            this.probabilityMutation = probabilityMutation;
            this.countIndividualInGenerate = countIndividualInGenerate;
            this.countEliteIndividual = countEliteIndividual;
            this.countRepeatBestGeneration = countRepeatBestGeneration;
            this.filling = filling;
            this.crossover = crossover;
            this.mutation = mutation;
            this.selector = selector;
            this.powerFunction = powerFunction;
            buffCountIndivid = 0;
            totalGeneration = 0;
        }

        #region Getter/Setter
        public int GetProbabilityCrossover() => probabilityCrossover;
        public int GetProbabilityMutation() => probabilityMutation;
        public Individual GetBestIndividInPopulation() => bestIndividInPopulation;
        public int GetCountIndividInGeneration() => countIndividualInGenerate;
        public int GetCountEliteIndividual() => countEliteIndividual;
        public int GetCountRepeatBestGeneration() => countRepeatBestGeneration;
        public int GetCountGenerationPassed() => totalGeneration;
        public TimeSpan GetLeadTime() => leadTime;
        public IFillingInitialIndividuals GetFillingInitialIndividuals() => filling;
        public ICrossing GetCrossover() => crossover;
        public IMutation GetMutation() => mutation;
        public ISelect GetSelector() => selector;
        public IPowerFunction GetPowerFunction() => powerFunction;
        #endregion

        public Individual FindBestIndivid()
        {
            var sw = new Stopwatch();
            sw.Start();

            var generation = new Generation(null, 
                                            probabilityCrossover,
                                            probabilityMutation,
                                            countIndividualInGenerate, 
                                            countEliteIndividual, 
                                            filling, 
                                            crossover, 
                                            mutation, 
                                            selector);
            generation.FillingInitialGeneration(graph, powerFunction);
            bestIndividInPopulation = generation.GetBestIndividInGeneration();

            ////delete
            int step = 1;
            Debug.WriteLine("\n");
            
            while (buffCountIndivid < countRepeatBestGeneration)
            {
                generation.GenerationIndividual();
                var bestIndividInGeneration = generation.GetBestIndividInGeneration();
                
                Debug.Write($"bestInGeneration - {bestIndividInGeneration.TargetFunction()}, ");

                if (bestIndividInGeneration.TargetFunction() < bestIndividInPopulation.TargetFunction())
                {
                    step = 1;
                    buffCountIndivid = 1;
                    bestIndividInPopulation = bestIndividInGeneration;
                }
                else
                    buffCountIndivid++;

                Debug.WriteLine($"step - {step}, bestInPopulation - {bestIndividInPopulation.TargetFunction()}");
                step++;

                totalGeneration++;
                generation = new Generation(generation.GetListIndividInGeneration(),
                                            probabilityCrossover,
                                            probabilityMutation,
                                            countIndividualInGenerate, 
                                            countEliteIndividual, 
                                            filling, 
                                            crossover, 
                                            mutation, 
                                            selector);
            }

            sw.Stop();
            leadTime = sw.Elapsed;

            var printer = new GraphLib.Writer.WriterFromFile(@"C:\Users\myacc\OneDrive\Рабочий стол\matrix (2).txt");
            printer.WriteAdjancyMatrix(bestIndividInPopulation.Graph);

            return bestIndividInPopulation;
        }

    }
}
