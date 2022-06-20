using System;
using System.IO;

namespace Genetic.JSON
{
    public static class JSONWriter
    {
        // public static string path = Path.Combine(Environment.CurrentDirectory, "JSONINFO.txt");
        public static string path = @"C:\Users\myacc\Desktop\JSONEvristicAlgorithm.txt";

        public static void Write(Population population)
        {
            var model = new ModelJSON(
                population.GetFillingInitialIndividuals().ToString(),
                population.GetCrossover().ToString(),
                population.GetMutation().ToString(),
                population.GetSelector().ToString(),
                population.GetPowerFunction().ToString(),
                population.GetProbabilityCrossover(),
                population.GetProbabilityMutation(),
                population.GetBestIndividInPopulation().Genome.Count,
                population.GetCountIndividInGeneration(),
                population.GetCountEliteIndividual(),
                population.GetBestIndividInPopulation().TargetFunction(),
                population.GetCountRepeatBestGeneration(),
                population.GetCountGenerationPassed(),
                population.GetLeadTime());

            var JSONpopulation = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            using (StreamWriter stream = File.AppendText(path))
                stream.WriteLine(JSONpopulation);
        }
    }
}
