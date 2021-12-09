using Calculator.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Concrete
{
    public class CalculatorService : ICalculatorService
    {
        public int Sum(int a, int b)
        {
            if (a == 0)
            {
                throw new Exception("a cannot have a zero value");
            }
            if (b == 0)
            {
                return 0;
            }
            return a + b;
        }
    }
}
