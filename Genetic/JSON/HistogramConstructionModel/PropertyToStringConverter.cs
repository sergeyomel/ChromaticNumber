using System;
using System.Collections.Generic;
using System.Text;

namespace Genetic.JSON.HistogramConstructionModel
{
    public class PropertyToStringConverter
    {
        private string Convert(string parameter)
        {
            if (parameter.Equals("EliteIndividual"))
                return "ЗАПОЛНЕНИЕ ЭЛИТНЫМИ ОСОБЯМИ";
            if (parameter.Equals("RandomIndividual"))
                return "ЗАПОЛНЕНИЕ СЛУЧАЙНЫМИ ОСОБЯМИ";

            if (parameter.Equals("DoublePointMutation"))
                return "ДВУТОЧЕЧНАЯ МУТАЦИЯ";
            if (parameter.Equals("ThreePointMutation"))
                return "ТРЕХТОЧЕЧНАЯ МУТАЦИЯ";

            if (parameter.Equals("ColoringNonAdjacentVertices"))
                return "РАСКРАСКА АЛГОРИТМА ВОЗЫКИ";
            if (parameter.Equals("NonDecreasingDegrees"))
                return "РАСКРАСКА НЕСМЕЖНЫХ ВЕРШИН";
            if (parameter.Equals("SplittingVerticesIntoSubset"))
                return "РАСКРАСКА РАЗБИЕНИЕМ НА ПОДМНОЖЕСТВА";
            if (parameter.Equals("VozikaAlgorithm"))
                return "АЛГОРИТМ ВОЗЫКИ";

            if (parameter.Equals("MinimumTargetFunctionSelection"))
                return "МИНИМАЛЬНОЕ ЗНАЧЕНИЕ ФУНКЦИИ СИЛЫ";
            
            return parameter;
        }

        public List<string> GetStringParameter(List<string> lProperty)
        {
            var lStringParameter = new List<string>();

            foreach (var property in lProperty)
                lStringParameter.Add(Convert(property));

            return lStringParameter;
        }
    }
}
