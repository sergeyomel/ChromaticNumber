using Genetic.Interface;
using GraphLib;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Genetic
{
    public class Individual
    {
        public readonly Graph Graph;
        public List<Vertex> Genome;
        private IPowerFunction PowerFunction;
        private Dictionary<Vertex, int> dColorVertex;

        public int ValueTargetFunction = -1;

        public Individual(Graph graph, List<Vertex> genome, IPowerFunction powerFunction)
        {
            Graph = graph;
            Genome = genome;
            PowerFunction = powerFunction;
            dColorVertex = PowerFunction.IndividualPowerFunction(this);
        }

        public IPowerFunction GetPowerFunction() => PowerFunction;

        public Dictionary<Vertex, int> GetDColorVertex() => dColorVertex;

        /// <summary>
        /// Функция расчёта значения условной целевой функции.
        /// </summary>
        /// <returns></returns>
        public int TargetFunction()
        {
            /*  if(ValueTargetFunction == -1)
                  ValueTargetFunction = PowerFunction.IndividualPowerFunction(this).Values;*/ //Math.Round((PowerFunction() * 1.0) / Genome.Count(), 5);
            return dColorVertex.Values.ToList().Distinct().Count();
        }

        public override string ToString()
        {
            return String.Join(" ", Genome.Select(item => item.ToString()));
        }

    }
}
