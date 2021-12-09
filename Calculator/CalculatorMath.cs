using Calculator.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    public class CalculatorMath
    {
        private readonly ICalculatorService _calculatorService;
        public CalculatorMath(ICalculatorService calculatorService)
        {
            _calculatorService = calculatorService;
        }

        public int Sum(int a, int b)
        {
            return _calculatorService.Sum(a, b);
        }
    }
}
