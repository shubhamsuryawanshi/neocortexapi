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

        [TestMethod]
        [Description("Check CalcArraySimilarityOld2 function")]
        public void TestCalcArraySimilarityOld2()
        {
            int[] originArray = { 1, 2, 3, 4, 5 };
            int[] comparingArray = { 6, 7, 3, 4, 5 };
            double result = HomeostaticPlasticityController.CalcArraySimilarityOld2(originArray, comparingArray);
            Assert.AreEqual(0.6, result);
        }

        [TestMethod]
        [Description("Check NeoCortexApi.Entities.HtmSerializer2.IsEqual function to compare HomeostaticPlasticityController objects")]
        public void TestIsEqual()
        {
            HtmConfig prms1 = new HtmConfig(new int[4], new int[4]);
            Connections htmMemory1 = new Connections(prms1);
            double requiredSimilarityThreshold1 = 1.0;
            HomeostaticPlasticityController obj1 = new HomeostaticPlasticityController(htmMemory1, 5, null, 40, requiredSimilarityThreshold1);
            HomeostaticPlasticityController obj2 = new HomeostaticPlasticityController(htmMemory1, 5, null, 40, requiredSimilarityThreshold1);

            bool result = NeoCortexApi.Entities.HtmSerializer2.IsEqual(obj1, obj2);
            Assert.IsTrue(result);
        }

        [TestMethod]
        [Description("Check Serialization and Deserialization for a HomeostaticPlasticityController object")]
        public void TestDeserialize()
        {
            HtmConfig prms = new HtmConfig(new int[4], new int[4]);
            Connections htmMemory = new Connections(prms);
            double requiredSimilarityThreshold = 1.0;
            HomeostaticPlasticityController homeostaticPlasticityController = new HomeostaticPlasticityController(htmMemory, 5, null, 50, requiredSimilarityThreshold);
            string currentBaseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string fileName = System.IO.Path.Combine(currentBaseDirectory, @"TestFiles/testSerialize.txt");
            string filePath = Path.GetFullPath(fileName);

            StreamWriter streamWriter = new StreamWriter(filePath);
            homeostaticPlasticityController.Serialize(streamWriter);
            streamWriter.Close();

            StreamReader streamReader = new StreamReader(filePath);
            HomeostaticPlasticityController deserializedObject = HomeostaticPlasticityController.Deserialize(streamReader, htmMemory);

            Assert.IsTrue(homeostaticPlasticityController.Equals(deserializedObject));
        }
    }
}
