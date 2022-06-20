using System;
using System.Collections.Generic;
using System.Text;
using Genetic.Interface;

namespace Genetic.Selection
{
    public class MinimumTargetFunctionSelection : ISelect
    {
        public Individual SelectIndividual(Individual individOne, Individual individTwo)
        {
            return individOne.TargetFunction() < individTwo.TargetFunction() ? individTwo : individTwo;
        }

        public override string ToString()
        {
            return "MINIMUM TARGET FUNCTION";
        }
    }
}
