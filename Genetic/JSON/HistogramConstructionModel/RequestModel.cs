using System;
using System.Collections.Generic;
using System.Text;

namespace Genetic.JSON.HistogramConstructionModel
{
    public class RequestModel
    {
        /// <summary>
        /// Зависимый параметр гистограммы. (Левая ось).
        /// </summary>
        public DependentElement dependentElement { get; set; }

        /// <summary>
        /// Параметр, на основе которого производится выборка элементов для построения гистограммы. (Нижняя ось).
        /// </summary>
        public RequestParameter requestParameter { get; set; }
        
        /// <summary>
        /// Параметр группировки колонок гистограммы.
        /// </summary>
        public ParameterGroupColumn parameterGroupColumn { get; set;}

        public RequestModel(DependentElement dependentElement,
                            RequestParameter requestParameter, 
                            ParameterGroupColumn parameterGroupColumn)
        {
            this.dependentElement = dependentElement;
            this.requestParameter = requestParameter;
            this.parameterGroupColumn = parameterGroupColumn;
        }

        public List<string> GetProperty()
        {
            var lProperty = new List<string>();
            lProperty.Add(requestParameter.ToString());
            lProperty.Add(dependentElement.ToString());
            lProperty.Add(parameterGroupColumn.ToString());
            return lProperty;
        }
    }
}
