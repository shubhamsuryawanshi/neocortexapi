using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeoCortexApi;

namespace UnitTestsProject
{
    /// <summary>
    /// Unit Tests for HomeostaticPlasticityController.
    /// </summary>
    [TestClass]
    public class HomeostaticPlasticityControllerTests
    {
        [TestMethod, Description("Check CalcArraySimilarity with empty arguments")]
        public void TestCalcArraySimilarity1()
        {
            int[] originArray = System.Array.Empty<int>();
            int[] comparingArray = System.Array.Empty<int>();
            double result = HomeostaticPlasticityController.CalcArraySimilarity(originArray, comparingArray);
            Assert.AreEqual(-1.0, result);
        }

        [TestMethod, Description("Check CalcArraySimilarity functionality")]
        public void TestCalcArraySimilarity2()
        {
            int[] originArray = {1, 2, 3, 4, 5};
            int[] comparingArray = {3, 4, 5, 6, 7};
            double result = HomeostaticPlasticityController.CalcArraySimilarity(originArray, comparingArray);
            Assert.AreEqual(0.6, result);
        }
    }
}
