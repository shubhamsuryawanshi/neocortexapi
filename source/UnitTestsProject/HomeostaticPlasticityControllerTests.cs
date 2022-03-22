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
        [Description("Check CalcArraySimilarity function under different scenarios")]
        [DataRow(-1.0, new int[] {}, new int[] {})]
        [DataRow(0.6, new int[] {1,2,3,4,5}, new int[] {3,4,5,6,7})]
        public void CalcArraySimilarityTest(double expectedResult, int[] arrayOne, int[] arrayTwo)
        {
            double result = HomeostaticPlasticityController.CalcArraySimilarity(arrayOne, arrayTwo);
            Assert.AreEqual(expectedResult, result);
        }
        
        [TestMethod]
        [Description("Check GetHash function")]
        [DataRow("��V�쬐�h���F@B���Wh�/ߌ?)K��I", new int[] { 1, 2, 3, 4, 5, 6 })]
        [DataRow("Oj��e�o��Kf���*��n�?�>�e(�H��", new int[] { 1, 2, 3, 4, 5})]
        public void GetHashTest(string hashValue, int[] obj)
        {
            string result = HomeostaticPlasticityController.GetHash(obj);
            Assert.AreEqual(hashValue, result);
        }

        [TestMethod]
        [Description("Check TraceState function")]
        public void TraceStateTest()
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

        [TestMethod]
        [Description("Check CalcArraySimilarityOld2 function")]
        [DataRow(0.6, new int[] {1,2,3,4,5}, new int[] {6,7,3,4,5})]
        public void CalcArraySimilarityOld2Test(double expectedResult, int[] arrayOne, int[] arrayTwo)
        {
            double result = HomeostaticPlasticityController.CalcArraySimilarityOld2(arrayOne, arrayTwo);
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        [Description("Check Serialization and Deserialization for a HomeostaticPlasticityController object")]
        public void DeserializeTest()
        {
            HtmConfig prms = new HtmConfig(new int[4], new int[4]);
            Connections htmMemory = new Connections(prms);
            double requiredSimilarityThreshold = 1.0;
            HomeostaticPlasticityController homeostaticPlasticityController = new HomeostaticPlasticityController(htmMemory, 5, null, 50, requiredSimilarityThreshold);
            string currentBaseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string fileName = System.IO.Path.Combine(currentBaseDirectory, @"TestFiles/testSerialize.txt");
            string filePath = Path.GetFullPath(fileName);
            HomeostaticPlasticityController deserializedObject = null;

            using (StreamWriter streamWriter = new StreamWriter(filePath))
            {
                homeostaticPlasticityController.Serialize(streamWriter);
            }

            using (StreamReader streamReader = new StreamReader(filePath))
            {
                deserializedObject = HomeostaticPlasticityController.Deserialize(streamReader, htmMemory);
            }

            Assert.IsTrue(homeostaticPlasticityController.Equals(deserializedObject));
        }

        [TestMethod]
        [Description("HomeostaticPlasticityController with unrealistic requiredSimilarityThreshold, compute method output expected is False as m_IsStable is False")]
        public void ComputeTest()
        {
            HtmConfig prms = new HtmConfig(new int[4], new int[4]);
            Connections htmMemory = new Connections(prms);
            double requiredSimilarityThreshold = -1;
            HomeostaticPlasticityController homeostaticPlasticityController = new HomeostaticPlasticityController(htmMemory, 5, null, 50, requiredSimilarityThreshold);

            bool res = homeostaticPlasticityController.Compute(new int[4], new int[4]);
            // In the second run of HomeostaticPlasticityController.Compute, condition m_InOutMap.ContainsKey(inpHash) is True
            res = homeostaticPlasticityController.Compute(new int[4], new int[4]);

            Assert.IsFalse(res);
        }

        [TestMethod]
        [Description("Check Equals function: when HomeostaticPlasticityController is compared to itself")]
        public void EqualsTest1()
        {
            HomeostaticPlasticityController obj = new HomeostaticPlasticityController();

            bool result = obj.Equals(obj);

            Assert.IsTrue(result);
        }

        [TestMethod]
        [Description("Check Equals function: when HomeostaticPlasticityController is compared to null")]
        public void EqualsTest2()
        {
            HomeostaticPlasticityController obj = new HomeostaticPlasticityController();

            bool result = obj.Equals(null);

            Assert.IsFalse(result);
        }

        [TestMethod]
        [Description("Check Equals function: when htmMemory for one HomeostaticPlasticityController is null")]
        public void EqualsTest3()
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
        public void EqualsTest4()
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
        public void EqualsTest5()
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
        public void EqualsTest6()
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
        public void EqualsTest7()
        {
            HtmConfig prms1 = new HtmConfig(new int[4], new int[4]);
            Connections htmMemory1 = new Connections(prms1);
            double requiredSimilarityThreshold1 = 1.0;
            HomeostaticPlasticityController obj1 = new HomeostaticPlasticityController(htmMemory1, 5, null, 40, requiredSimilarityThreshold1);
            HomeostaticPlasticityController obj2 = new HomeostaticPlasticityController(htmMemory1, 5, null, 50, requiredSimilarityThreshold1);

            bool result = obj1.Equals(obj2);

            Assert.IsFalse(result);
        }
    }
}
