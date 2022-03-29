﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
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
        [Description("Check CalcArraySimilarity method in different scenarios")]
        [DataRow(-1.0, new int[] {}, new int[] {})]
        [DataRow(0.6, new int[] {1,2,3,4,5}, new int[] {3,4,5,6,7})]
        [DataRow(0.0, new int[] {1,2,3}, new int[] {4,5,6})]
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
        [DataRow("[0 - stable cycles: 0,len = 0] 	 ", "MinKey=,4��;��Z�*d7̣�~�	�%���X+Q�, min stable states=0", 10)]
        [DataRow("[0 - stable cycles: 0,len = 0] 	 ", "MinKey=[o��a�GY9v}h�F����Z>��Q�QW��, min stable states=0", 20)]
        public void TraceStateTest(string expectedTraceState1, string expectedTraceState2, int elements)
        {
            string currentBaseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string fileName = System.IO.Path.Combine(currentBaseDirectory, @"TestFiles/traceStateTest.txt");
            string filePath = Path.GetFullPath(fileName);

            HtmConfig prms1 = new HtmConfig(new int[40], new int[40]);
            Connections htmMemory1 = new Connections(prms1);
            double requiredSimilarityThreshold1 = 0.6;
            HomeostaticPlasticityController homeostaticPlasticityController = 
                new HomeostaticPlasticityController(htmMemory1, 10, null, 40, requiredSimilarityThreshold1);
            homeostaticPlasticityController.Compute(new int[elements], new int[elements]);
            homeostaticPlasticityController.TraceState(filePath);
            string traceStateOutput1 = File.ReadLines(filePath).First();
            string traceStateOutput2 = File.ReadLines(filePath).Last();
            Assert.AreEqual(expectedTraceState1, traceStateOutput1);
            Assert.AreEqual(expectedTraceState2, traceStateOutput2);
        }

        [TestMethod]
        [Description("Check CalcArraySimilarityOld2 function")]
        [DataRow(0.6, new int[] {1,2,3,4,5}, new int[] {6,7,3,4,5})]
        [DataRow(0.0, new int[] {1,2,3,4,5}, new int[] {6,7,8,9,10})]
        public void CalcArraySimilarityOld2Test(double expectedResult, int[] arrayOne, int[] arrayTwo)
        {
            double result = HomeostaticPlasticityController.CalcArraySimilarityOld2(arrayOne, arrayTwo);
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        [Description("Validate Serialization and Deserialization operation for a HomeostaticPlasticityController object")]
        public void DeserializeTest()
        {
            HtmConfig prms = new HtmConfig(new int[4], new int[4]);
            Connections htmMemory = new Connections(prms);
            double requiredSimilarityThreshold = 1.0;
            HomeostaticPlasticityController homeostaticPlasticityController = 
                new HomeostaticPlasticityController(htmMemory, 5, null, 50, requiredSimilarityThreshold);
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
        [Description("HomeostaticPlasticityController with unrealistic parameters, compute method output expected is False as m_IsStable is False")]
        public void ComputeTest()
        {
            int[] inputArray = new int[4];
            int[] outputArray = new int[4];
            HtmConfig prms = new HtmConfig(inputArray, outputArray);
            Connections htmMemory = new Connections(prms);
            double requiredSimilarityThreshold = -1;
            HomeostaticPlasticityController homeostaticPlasticityController =
                new HomeostaticPlasticityController(htmMemory, 5, null, 15, requiredSimilarityThreshold);

            bool res = homeostaticPlasticityController.Compute(new int[4], new int[4]);
            // In the second run of HomeostaticPlasticityController.Compute, condition m_InOutMap.ContainsKey(inpHash) is True
            res = homeostaticPlasticityController.Compute(new int[4], new int[4]);

            Assert.IsFalse(res);
        }

        [TestMethod]
        [Description("Using HomeostaticPlasticityController in simulated training conditions where stable state is achieved")]
        public void ComputeTest2()
        {
            int[] inputArray = new int[4];
            int[] outputArray = new int[4];
            HtmConfig prms = new HtmConfig(inputArray, outputArray);
            Connections htmMemory = new Connections(prms);
            double requiredSimilarityThreshold = -1;
            Action<bool, int, double, int> OnStabilityStatusUpdate = (a,b,c,d) => Console.WriteLine("Write {0}, {1}, {2}, {3}", a, b, c, d);
            HomeostaticPlasticityController homeostaticPlasticityController =
                new HomeostaticPlasticityController(htmMemory, 5, OnStabilityStatusUpdate, 10, requiredSimilarityThreshold);
            bool res = false;

            for (int i=0; i<30; i++)
            {
                res = homeostaticPlasticityController.Compute(inputArray, outputArray);
            }
            Assert.IsTrue(res);
        }
        
        [TestMethod]
        [Description("Using HomeostaticPlasticityController in simulated training conditions to check the state of private variables")]
        public void ComputeTest3()
        {
            int[] inputArray = new int[4];
            int[] outputArray = new int[4];
            HtmConfig prms = new HtmConfig(inputArray, outputArray);
            Connections htmMemory = new Connections(prms);
            double requiredSimilarityThreshold = -1;
            Dictionary<string, int> stableCyclesForInput = null;
            int requiredNumOfStableCycles = 0;
            bool isStable = false;
            Action<bool, int, double, int> OnStabilityStatusUpdate = (a,b,c,d) => Console.WriteLine("Write {0}, {1}, {2}, {3}", a, b, c, d);
            HomeostaticPlasticityController homeostaticPlasticityController =
                new HomeostaticPlasticityController(htmMemory, 5, OnStabilityStatusUpdate, 15, requiredSimilarityThreshold);

            for (int i=0; i<20; i++)
            {
                homeostaticPlasticityController.Compute(inputArray, outputArray);
            }

            stableCyclesForInput = homeostaticPlasticityController.Get_m_NumOfStableCyclesForInput();
            requiredNumOfStableCycles = homeostaticPlasticityController.Get_m_RequiredNumOfStableCycles();
            isStable = homeostaticPlasticityController.Get_m_IsStable();
            
            Assert.IsTrue(stableCyclesForInput[HomeostaticPlasticityController.GetHash(inputArray)] > requiredNumOfStableCycles);
            Assert.IsTrue(isStable);
        }

        [TestMethod]
        [Description("Check Equals method under different scenarios")]
        public void EqualsTest()
        {
            // Comparing a HomeostaticPlasticityController to itself
            HomeostaticPlasticityController obj = new HomeostaticPlasticityController();
            bool result = obj.Equals(obj);
            Assert.IsTrue(result);
            
            // Comparing a HomeostaticPlasticityController to null
            result = obj.Equals(null);
            Assert.IsFalse(result);
            
            // Comparing HomeostaticPlasticityController objects with different htmMemory
            HtmConfig prms1 = new HtmConfig(new int[4], new int[4]);
            Connections htmMemory1 = new Connections(prms1);
            HomeostaticPlasticityController obj1 = new HomeostaticPlasticityController(htmMemory1, 5, null);
            HtmConfig prms2 = new HtmConfig(new int[5], new int[5]);
            Connections htmMemory2 = new Connections(prms2);
            HomeostaticPlasticityController obj2 = new HomeostaticPlasticityController(htmMemory2, 5, null);
            result = obj1.Equals(obj2);
            Assert.IsFalse(result);
            
            // Comparing HomeostaticPlasticityController objects with different requiredSimilarityThreshold
            prms1 = new HtmConfig(new int[4], new int[4]);
            htmMemory1 = new Connections(prms1);
            double requiredSimilarityThreshold1 = 1.0;
            obj1 = new HomeostaticPlasticityController(htmMemory1, 5, null, 50, requiredSimilarityThreshold1);
            double requiredSimilarityThreshold2 = 0.97;
            obj2 = new HomeostaticPlasticityController(htmMemory1, 5, null, 50, requiredSimilarityThreshold2);
            result = obj1.Equals(obj2);
            Assert.IsFalse(result);
            
            // Comparing HomeostaticPlasticityController objects with different minCycles
            prms1 = new HtmConfig(new int[4], new int[4]);
            htmMemory1 = new Connections(prms1);
            requiredSimilarityThreshold1 = 1.0;
            obj1 = new HomeostaticPlasticityController(htmMemory1, 4, null, 50, requiredSimilarityThreshold1);
            obj2 = new HomeostaticPlasticityController(htmMemory1, 5, null, 50, requiredSimilarityThreshold1);
            result = obj1.Equals(obj2);
            Assert.IsFalse(result);
            
            // Comparing HomeostaticPlasticityController objects with different numOfCyclesToWaitOnChange
            prms1 = new HtmConfig(new int[4], new int[4]);
            htmMemory1 = new Connections(prms1);
            requiredSimilarityThreshold1 = 1.0;
            obj1 = new HomeostaticPlasticityController(htmMemory1, 5, null, 40, requiredSimilarityThreshold1);
            obj2 = new HomeostaticPlasticityController(htmMemory1, 5, null, 50, requiredSimilarityThreshold1);
            result = obj1.Equals(obj2);
            Assert.IsFalse(result);
        }
    }
}
