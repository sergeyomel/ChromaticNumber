using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Genetic.JSON
{
    public class ModelJSON
    {
        [JsonProperty("fill")]
        public string FillingType { get; set; }

        [JsonProperty("cros")]
        public string CrossoverType { get; set; }

        [JsonProperty("mut")]
        public string MutationType { get; set; }

        [JsonProperty("select")]
        public string SelectorType { get; set; }

        [JsonProperty("powerFunc")]
        public string PowerFunction { get; set; }

        [JsonProperty("propCros")]
        public int ProbabilityCrossover { get; set; }

        [JsonProperty("propMut")]
        public int ProbabilityMutation { get; set; }

        [JsonProperty("cVert")]
        public int CountVertex { get; set; }

        [JsonProperty("cIndivid")]
        public int CountIndividInGeneration { get; set; }

        [JsonProperty("cEliteInd")]
        public int CountEliteIndividInGeneration { get; set; }

        [JsonProperty("chromNum")]
        public int ChromaticNumber { get; set; }

        [JsonProperty("rBestGen")]
        public int RepeatsBestGeneration { get; set; }

        [JsonProperty("cGenPas")]
        public int CountGenerationPassed { get; set; }

        [JsonProperty("lT")]
        public TimeSpan LeadTime { get; set; }

        public ModelJSON(string fillingType,
                         string crossoverType,
                         string mutationType,
                         string selectorType,
                         string powerFunction,
                         int probabilityCrossover,
                         int probabilityMutation,
                         int countVertex,
                         int countIndividInGeneration,
                         int countEliteIndividInGeneration,
                         int chromaticNumber,
                         int repeatsBestGeneration,
                         int countGenerationPassed,
                         TimeSpan leadTime)
        {
            FillingType = fillingType;
            CrossoverType = crossoverType;
            MutationType = mutationType;
            SelectorType = selectorType;
            PowerFunction = powerFunction;
            ProbabilityCrossover = probabilityCrossover;
            ProbabilityMutation = probabilityMutation;
            CountVertex = countVertex;
            CountIndividInGeneration = countIndividInGeneration;
            CountEliteIndividInGeneration = countEliteIndividInGeneration;
            ChromaticNumber = chromaticNumber;
            RepeatsBestGeneration = repeatsBestGeneration;
            CountGenerationPassed = countGenerationPassed;
            LeadTime = leadTime;
        }

    }
}
