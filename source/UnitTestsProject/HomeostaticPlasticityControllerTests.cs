using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
using NeoCortexApi;
using NeoCortexApi.Entities;

namespace UnitTestsProject
{
    /// <summary>
    /// Unit Tests for HomeostaticPlasticityController Class.
    /// </summary>
    [TestClass]
    public class HomeostaticPlasticityControllerTests
    {
        [TestMethod]
        [Description("Check CalcArraySimilarity function with empty arguments")]
        public void TestCalcArraySimilarity1()
        {
            int[] originArray = System.Array.Empty<int>();
            int[] comparingArray = System.Array.Empty<int>();

            double result = HomeostaticPlasticityController.CalcArraySimilarity(originArray, comparingArray);

            Assert.AreEqual(-1.0, result);
        }

        [TestMethod]
        [Description("Check CalcArraySimilarity function")]
        public void TestCalcArraySimilarity2()
        {
            int[] originArray = { 1, 2, 3, 4, 5 };
            int[] comparingArray = { 3, 4, 5, 6, 7 };

            double result = HomeostaticPlasticityController.CalcArraySimilarity(originArray, comparingArray);

            Assert.AreEqual(0.6, result);
        }

        [TestMethod]
        [Description("Check Equals function: when HomeostaticPlasticityController is compared to itself")]
        public void TestEquals1()
        {
            HomeostaticPlasticityController obj = new HomeostaticPlasticityController();

            bool result = obj.Equals(obj);

            Assert.IsTrue(result);
        }

        [TestMethod]
        [Description("Check Equals function: when HomeostaticPlasticityController is compared to null")]
        public void TestEquals2()
        {
            HomeostaticPlasticityController obj = new HomeostaticPlasticityController();

            bool result = obj.Equals(null);

            Assert.IsFalse(result);
        }

        [TestMethod]
        [Description("Check Equals function: when htmMemory for one HomeostaticPlasticityController is null")]
        public void TestEquals3()
        {
            HomeostaticPlasticityController obj1 = new HomeostaticPlasticityController();
            HtmConfig prms = new HtmConfig(new int[4], new int[4]);
            Connections htmMemory = new Connections(prms);
            HomeostaticPlasticityController obj2 = new HomeostaticPlasticityController(htmMemory, 5, null);

            bool result = obj1.Equals(obj2);

            Assert.IsFalse(result);
        }

        [TestMethod]
        [Description("Check Equals function: when htmMemory is different for HomeostaticPlasticityController objects")]
        public void TestEquals4()
        {
            HtmConfig prms1 = new HtmConfig(new int[4], new int[4]);
            Connections htmMemory1 = new Connections(prms1);
            HomeostaticPlasticityController obj1 = new HomeostaticPlasticityController(htmMemory1, 5, null);
            HtmConfig prms2 = new HtmConfig(new int[5], new int[5]);
            Connections htmMemory2 = new Connections(prms2);
            HomeostaticPlasticityController obj2 = new HomeostaticPlasticityController(htmMemory2, 5, null);

            bool result = obj1.Equals(obj2);

            Assert.IsFalse(result);
        }

        [TestMethod]
        [Description("Check Equals function: when requiredSimilarityThreshold is different for HomeostaticPlasticityController objects")]
        public void TestEquals5()
        {
            HtmConfig prms1 = new HtmConfig(new int[4], new int[4]);
            Connections htmMemory1 = new Connections(prms1);
            double requiredSimilarityThreshold1 = 1.0;
            HomeostaticPlasticityController obj1 = new HomeostaticPlasticityController(htmMemory1, 5, null, 50, requiredSimilarityThreshold1);
            double requiredSimilarityThreshold2 = 0.97;
            HomeostaticPlasticityController obj2 = new HomeostaticPlasticityController(htmMemory1, 5, null, 50, requiredSimilarityThreshold2);

            bool result = obj1.Equals(obj2);

            Assert.IsFalse(result);
        }

        [TestMethod]
        [Description("Check Equals function: when minCycles is different for HomeostaticPlasticityController objects")]
        public void TestEquals6()
        {
            HtmConfig prms1 = new HtmConfig(new int[4], new int[4]);
            Connections htmMemory1 = new Connections(prms1);
            double requiredSimilarityThreshold1 = 1.0;
            HomeostaticPlasticityController obj1 = new HomeostaticPlasticityController(htmMemory1, 4, null, 50, requiredSimilarityThreshold1);
            HomeostaticPlasticityController obj2 = new HomeostaticPlasticityController(htmMemory1, 5, null, 50, requiredSimilarityThreshold1);

            bool result = obj1.Equals(obj2);

            Assert.IsFalse(result);
        }

        [TestMethod]
        [Description("Check Equals function: when numOfCyclesToWaitOnChange is different for HomeostaticPlasticityController objects")]
        public void TestEquals7()
        {
            HtmConfig prms1 = new HtmConfig(new int[4], new int[4]);
            Connections htmMemory1 = new Connections(prms1);
            double requiredSimilarityThreshold1 = 1.0;
            HomeostaticPlasticityController obj1 = new HomeostaticPlasticityController(htmMemory1, 5, null, 40, requiredSimilarityThreshold1);
            HomeostaticPlasticityController obj2 = new HomeostaticPlasticityController(htmMemory1, 5, null, 50, requiredSimilarityThreshold1);

            bool result = obj1.Equals(obj2);

            Assert.IsFalse(result);
        }

        [TestMethod]
        [Description("Check GetHash function")]
        public void TestGetHash()
        {
            int[] a1 = { 1, 2, 3, 4, 5, 6 };

            string result = HomeostaticPlasticityController.GetHash(a1);

            Assert.AreEqual("��V�쬐�h���F@B���Wh�/ߌ?)K��I", result);
        }

        [TestMethod]
        [Description("Check TraceState function")]
        public void TestTraceState()
        {
            string currentBaseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string fileName = System.IO.Path.Combine(currentBaseDirectory, @"TestFiles/traceStateTest.txt");
            string filePath = Path.GetFullPath(fileName);

            string traceStateOutput = "";
            HtmConfig prms1 = new HtmConfig(new int[4], new int[4]);
            Connections htmMemory1 = new Connections(prms1);
            double requiredSimilarityThreshold1 = 1.0;
            HomeostaticPlasticityController obj = new HomeostaticPlasticityController(htmMemory1, 10, null, 40, requiredSimilarityThreshold1);

            obj.TraceState(filePath);
            traceStateOutput = File.ReadLines(filePath).First();

            Assert.AreEqual("MinKey=, min stable states=2147483647", traceStateOutput);
        }
    }
}
