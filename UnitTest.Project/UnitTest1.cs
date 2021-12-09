using System;
using Xunit;
using Calculator;
using Calculator.Concrete;

namespace UnitTest.Project
{
    public class UnitTest1
    {
        private readonly CalculatorMath _calculator;
        public UnitTest1()
        {
            _calculator = new CalculatorMath(new CalculatorService());
        }

        [Theory]
        [InlineData(2,4,6)]
        [InlineData(0,9,9)]
        public void Test2(int a, int b, int exceptedValue)
        {
            var actualValue = a + b;
            Assert.Equal<int>(exceptedValue, actualValue);
        }
    }
}
