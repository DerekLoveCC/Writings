using System;
using System.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UTDemo.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var s1 = Environment.GetEnvironmentVariables();

            using (var s = ShimsContext.Create())
            {
                ShimDateTime.NowGet = () => new DateTime(2020, 10, 10);
                var now = DateTime.Now;
            }

            var now1 = DateTime.Now;
        }

        [TestMethod]
        public void TestMethod2()
        {
           

            var now1 = DateTime.Now;
        }
    }
}
