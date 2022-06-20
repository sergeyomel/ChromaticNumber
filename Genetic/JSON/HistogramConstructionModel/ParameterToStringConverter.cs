using System;
using System.Collections.Generic;
using System.Text;

namespace Genetic.JSON.HistogramConstructionModel
{
    public class ParameterToStringConverter
    {
        private string Convert(string parameter)
        {
            if (parameter.Equals("FillingType"))
                return "ТИП ЗАПОЛНЕНИЯ НАЧАЛЬНОГО ПОКОЛЕНИЯ";
            if (parameter.Equals("CrossoverType"))
                return "ТИП КРОССОВЕРА";
            if (parameter.Equals("MutationType"))
                return "ТИП МУТАЦИИ";
            if (parameter.Equals("SelectorType"))
                return "ТИП ОТБОРА ЛУЧШЕЙ ОСОБИ";
            if (parameter.Equals("PowerFunction"))
                return "ТИП ФУНКЦИИ СИЛЫ";
            if (parameter.Equals("ProbabilityCrossover"))
                return "ВЕРОЯТНОСТЬ КРОССОВЕРА";
            if (parameter.Equals("ProbabilityMutation"))
                return "ВЕРОЯТНОСТЬ МУТАЦИИ";
            if (parameter.Equals("CountVertex"))
                return "КОЛИЧЕСТВО ВЕРШИН";
            if (parameter.Equals("ChromaticNumber"))
                return "ХРОМАТИЧЕСКОЕ ЧИСЛО";
            if (parameter.Equals("CountIndividInGeneration"))
                return "КОЛИЧЕСТВО ОСОБЕЙ В ПОКОЛЕНИИ";
            if (parameter.Equals("CountEliteIndividInGeneration"))
                return "КОЛИЧЕСТВО ЭЛИТНЫХ ОСОБЕЙ В ПОКОЛЕНИИ";
            if (parameter.Equals("RepeatsBestGeneration"))
                return "КОЛИЧЕСТВО ПОВТОРЕНИЙ ЭЛИТНОЙ ОСОБИ";
            if (parameter.Equals("CountGenerationPassed"))
                return "КОЛИЧЕСТВО ПРОЙДЕННЫХ ПОКОЛЕНИЙ";
            if (parameter.Equals("LeadTime"))
                return "ЗАТРАЧЕННОЕ ВРЕМЯ";
            return "НЕ ОПРЕДЕЛЕННЫЙ ПАРАМЕТР";
        }

        public List<string> GetStringParameter(RequestModel model)
        {
            var lStringParameter = new List<string>();

            lStringParameter.Add(Convert(model.dependentElement.ToString()));
            lStringParameter.Add(Convert(model.requestParameter.ToString()));
            lStringParameter.Add(Convert(model.parameterGroupColumn.ToString()));

            return lStringParameter;
        }
    }
}
