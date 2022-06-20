using Genetic.JSON.HistogramConstructionModel;

namespace Genetic.JSON.SetBuiltDependencies
{
    public class DependenseManager
    {
        public RequestModel GetModel(DependenseEnum value)
        {
            if (value == DependenseEnum.ChromaticNumberOfCountVertex)
                return new RequestModel(DependentElement.ChromaticNumber,
                                        RequestParameter.CountVertex,
                                        ParameterGroupColumn.CountVertex);

            if (value == DependenseEnum.ChromaticNumberOfCountVertex)
                return new RequestModel(DependentElement.ChromaticNumber,
                                        RequestParameter.CrossoverType,
                                        ParameterGroupColumn.CountVertex);

            if (value == DependenseEnum.ChromaticNumberOfFillingType)
                return new RequestModel(DependentElement.ChromaticNumber,
                                        RequestParameter.FillingType,
                                        ParameterGroupColumn.CountVertex);

            if (value == DependenseEnum.ChromaticNumberOfFillingType)
                return new RequestModel(DependentElement.ChromaticNumber,
                                        RequestParameter.PowerFunction,
                                        ParameterGroupColumn.CountVertex);

            if (value == DependenseEnum.ChromaticNumberOfPowerFunction)
                return new RequestModel(DependentElement.ChromaticNumber,
                                        RequestParameter.PowerFunction,
                                        ParameterGroupColumn.CountVertex);

            if (value == DependenseEnum.TimeOfCountIndividualInGeneration)
                return new RequestModel(DependentElement.LeadTime,
                                        RequestParameter.CountIndividInGeneration,
                                        ParameterGroupColumn.CountVertex);

            if (value == DependenseEnum.TimeOfCountVertex)
                return new RequestModel(DependentElement.LeadTime,
                                        RequestParameter.CountVertex,
                                        ParameterGroupColumn.CountVertex);

            if (value == DependenseEnum.TimeOfCrossing)
                return new RequestModel(DependentElement.LeadTime,
                                        RequestParameter.CrossoverType,
                                        ParameterGroupColumn.CountVertex);

            if (value == DependenseEnum.TimeOfMutation)
                return new RequestModel(DependentElement.LeadTime,
                                        RequestParameter.MutationType,
                                        ParameterGroupColumn.CountVertex);

            return new RequestModel(DependentElement.ChromaticNumber,
                                        RequestParameter.CrossoverType,
                                        ParameterGroupColumn.CountVertex);
        }
    }
}
