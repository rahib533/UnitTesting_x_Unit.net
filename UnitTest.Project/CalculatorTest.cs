using System;
using Xunit;
using Calculator;
using Calculator.Concrete;
using Moq;
using Calculator.Abstract;

namespace UnitTest.Project
{
    public class CalculatorTest
    {
        private readonly CalculatorMath _calculator;
        private readonly Mock<ICalculatorService> _mock;
        public CalculatorTest()
        {
            _mock = new Mock<ICalculatorService>();
            _calculator = new CalculatorMath(_mock.Object);
        }

        [Theory]
        [InlineData(4, 5, 9)]
        public void Sum_SimpleValue_ReturnSimpleValue(int a, int b, int exceptedValue)
        {
            _mock.Setup(x => x.Sum(a,b)).Returns(exceptedValue);
            
            var actualValue = _calculator.Sum(a,b);
            _mock.Verify(x => x.Sum(a, b), Times.AtLeast(1));
            Assert.Equal<int>(exceptedValue, actualValue);
        }

        [Theory]
        [InlineData(0, 5)]
        public void Sum_ZeroValueOfFirstParametr_ReturnException(int a, int b)
        {
            _mock.Setup(x => x.Sum(a, b)).Throws(new Exception("a cannot have a zero value"));

            var exception = Assert.Throws<Exception>(() =>
            {
                _calculator.Sum(a, b);
            });

            Assert.Equal("a cannot have a zero value", exception.Message);
        }

        [Theory]
        [InlineData(7, 5,12)]
        public void Test_Callback(int a, int b, int expectedValue)
        {
            int actualData = 0;
            _mock.Setup(x => x.Sum(It.IsAny<int>(), It.IsAny<int>())).Callback<int, int>((int x, int y) => actualData = x + y);

            _calculator.Sum(a, b);

            Assert.Equal(expectedValue,actualData);

            _calculator.Sum(2, 3);
            Assert.Equal(5, actualData);
        }
    }
}
