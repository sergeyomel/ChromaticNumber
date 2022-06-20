namespace Genetic.Generations.GenerationInterface
{
    public interface IFillingInitialIndividuals
    {
        public System.Collections.Generic.List<Individual> FillingInitialIndivdual(GraphLib.Graph graph, 
                                                                                   Interface.IPowerFunction powerFunction, 
                                                                                   int countIndividualInGeneration,
                                                                                   int countEliteIndividual);
    }
}