using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using SportsPro.Models;

namespace SportsProTest
{
    public class NavTests
    {
        [Fact]
        public void ActiveMethod_ReturnsAString()
        {
            string s1 = "Product";                 //arrange
            string s2 = "Customer";

            var result = Nav.Active(s1, s2);    //act

            Assert.IsType<string>(result);      //assert
        }

        [Theory]
        [InlineData("Product", "Product")]
        [InlineData("Customer", "Customer")]
        public void ActiveMethod_ReturnsValueActiveIfMatch(string s1, string s2)
        {
            string expected = "active";     //arrange

            var result = Nav.Active(s1, s2);    //act

            Assert.Equal(expected, result);     //assert
        }

        [Theory]
        [InlineData("Product", "Customer")]
        [InlineData("Customer", "Product")]
        public void ActiveMethod_ReturnsEmptyStringIfNoMatch(string s1, string s2)
        {
            // act
            string active = Nav.Active(s1, s2);

            // assert
            Assert.Equal("", active);
        }

    }
}
