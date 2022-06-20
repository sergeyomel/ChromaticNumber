using System;
using System.Collections.Generic;

namespace Genetic.JSON
{
    public static class JSONValidator
    {
        public static bool IsCorrectModel(ModelJSON model)
        {
            List<string> lPropertiesNotBeChecked = new List<string> { "probabilitycrossover", "probabilitymutation", "counteliteindividingeneration" };
            Type type = typeof(ModelJSON);
            foreach(var property in type.GetProperties())
            {
                switch (property.PropertyType.Name.ToLower())
                {
                    case "string":
                        if (property.GetValue(model) == null)
                            return false;
                        break;
                    case "int32":
                        if (!lPropertiesNotBeChecked.Contains(property.Name.ToLower()) &&
                            (int)property.GetValue(model) == default(Int32))
                            return false;
                        break;
                    case "double":
                        if ((double)property.GetValue(model) == default(Double))
                            return false;
                        break;
                    case "timespan":
                        if ((TimeSpan)property.GetValue(model) == default(TimeSpan))
                            return false;
                        break;
                }
            }
            return true;
        }
    }
}
