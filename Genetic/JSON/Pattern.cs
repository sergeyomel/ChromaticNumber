using Genetic.JSON.HistogramConstructionModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Genetic.JSON
{
    public class Pattern
    {
        public List<string> signatureColumn = null;

        private double TakeObjectValue(object obj)
        {
            if (new List<Type> {typeof(Double), typeof(Int32)}.Contains(obj.GetType()))
                return Convert.ToDouble(obj);
            if (obj.GetType() == typeof(TimeSpan))
                return Convert.ToDouble(((TimeSpan)obj).TotalMilliseconds);
            return 0.0;
        }

        private int DegreesRound(object obj)
        {
            if (new List<Type> { typeof(Double), typeof(Int32) }.Contains(obj.GetType()))
                return 1;
            if (obj.GetType() == typeof(TimeSpan))
                return 3;
            return 1;
        }

        public List<Dictionary<double, List<double>>> GetData(List<ModelJSON> data, RequestModel requestModel)
        {
            foreach(var property in requestModel.GetProperty())
            {
                if (typeof(ModelJSON).GetProperty(property) == null)
                {
                    throw new Exception($"The model does not have the specified property:  {property}.");
                }
            }

            var convertedData = new List<Dictionary<double, List<double>>>();

            if (data == null)
                throw new Exception("Invalid data: no data.");
            if (data.Count == 0)
                throw new Exception("Invalid data: data is empty.");

            var lGroupColumn = data.GroupBy(item => Convert.ToInt32(item.GetType().GetProperty(requestModel.parameterGroupColumn.ToString()).GetValue(item)))
                                   .Select(item => item.Key).ToList();
            lGroupColumn.Sort();
            var lGroupOne = data.GroupBy(item => item.GetType().GetProperty(requestModel.requestParameter.ToString()).GetValue(item)).Select(item => item.Key).ToList();
            lGroupOne.Sort();

            signatureColumn = new List<string>(lGroupOne.Select(item => item.ToString()));

            for (int indexGroupColumn = 0; indexGroupColumn < lGroupColumn.Count; indexGroupColumn++)
            {
                var lModelWitGroupColumnType = data.Where(item => Convert.ToInt32(item.GetType().GetProperty(requestModel.parameterGroupColumn.ToString()).GetValue(item)) == lGroupColumn[indexGroupColumn]);

                var lAverageValueInPopulation = new List<double>();

                for (int indexIndivid = 0; indexIndivid < lGroupOne.Count; indexIndivid++)
                {
                    var lModelValue = lModelWitGroupColumnType
                                                    .Where(item => item.GetType().GetProperty(requestModel.requestParameter.ToString()).GetValue(item).ToString()
                                                                                                          .Equals(lGroupOne[indexIndivid].ToString()))
                                                    .Select(item => item.GetType().GetProperty(requestModel.dependentElement.ToString()).GetValue(item));
                    var averageValue = 0.0;
                    if (lModelValue.Count() != 0)
                        averageValue = Math.Round(lModelValue.Select(item => TakeObjectValue(item)).Average(), DegreesRound(lModelValue.First()));
                    lAverageValueInPopulation.Add(averageValue); 
                }

                var element = new Dictionary<double, List<double>> { { Convert.ToDouble(lGroupColumn[indexGroupColumn]), lAverageValueInPopulation } };

                convertedData.Add(element);
            }

            return convertedData;
        }
    }
}
