using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using GOAP;

public class IntStateTests {

    [Test]
    public void Clone() {
        G_IntState testState = An.IntState("test").WithValue(5);
        G_State cloneState = testState.Clone();

        Assert.AreEqual(testState.name, cloneState.name);
        Assert.AreEqual((int)testState.GetValue(), (int)cloneState.GetValue());
        Assert.IsTrue(cloneState is G_IntState);
    }

    [TestCase(5, G_StateComparison.EqualTo, 5, true, TestName = "Equals - True")]
    [TestCase(5, G_StateComparison.EqualTo, 0, false, TestName = "Equals - False")]

    [TestCase(5, G_StateComparison.GreaterThan, 4, true, TestName = "Greater - Lesser Test Value")]
    [TestCase(5, G_StateComparison.GreaterThan, 5, false, TestName = "Greater - Equal Value")]
    [TestCase(5, G_StateComparison.GreaterThan, 6, false, TestName = "Greater - Greater Test Value")]
    
    [TestCase(5, G_StateComparison.GreaterThanOrEqualTo, 4, true, TestName = "Greater or Equal - Lesser Test Value")]
    [TestCase(5, G_StateComparison.GreaterThanOrEqualTo, 5, true, TestName = "Greater or Equal - Equal Value")]
    [TestCase(5, G_StateComparison.GreaterThanOrEqualTo, 6, false, TestName = "Greater or Equal - Greater Test Value")]
    
    [TestCase(5, G_StateComparison.LessThan, 4, false, TestName = "Lesser - Lesser Test Value")]
    [TestCase(5, G_StateComparison.LessThan, 5, false, TestName = "Lesser - Equal Value")]
    [TestCase(5, G_StateComparison.LessThan, 6, true, TestName = "Lesser - Greater Test Value")]
    
    [TestCase(5, G_StateComparison.LessThanOrEqualTo, 4, false, TestName = "Lesser or Equal - Lesser Test Value")]
    [TestCase(5, G_StateComparison.LessThanOrEqualTo, 5, true, TestName = "Lesser or Equal - Equal Value")]
    [TestCase(5, G_StateComparison.LessThanOrEqualTo, 6, true, TestName = "Lesser or Equal - Greater Test Value")]


    public void TestState(int stateValue, G_StateComparison comparison, int testValue, bool expectedResult) {
        G_IntState testState = An.IntState("test").WithValue(stateValue);

        bool result = testState.TestState(testState, comparison, testValue);
        Assert.AreEqual(expectedResult, result);
    }
}
