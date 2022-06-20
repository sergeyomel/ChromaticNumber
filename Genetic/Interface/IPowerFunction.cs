using System.Collections.Generic;

namespace Genetic.Interface
{
    public interface IPowerFunction
    {
        public Dictionary<GraphLib.Vertex, int> IndividualPowerFunction(Individual Individ);
    }
}
