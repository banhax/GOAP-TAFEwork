using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using GOAP;

public class IntStateTests {

    [Test]
    public void Clone() {
        G_IntState testState = An.IntState().WithName("test").WithValue(5);
        G_State cloneState = testState.Clone();

        Assert.AreEqual(testState.name, cloneState.name);
        Assert.AreEqual((int)testState.GetValue(), (int)cloneState.GetValue());
        Assert.IsTrue(cloneState is G_IntState);
    }

    [TestCase(5, G_StateComparison.equal, 5, true, TestName = "Equals - True")]
    [TestCase(5, G_StateComparison.equal, 0, false, TestName = "Equals - False")]

    [TestCase(5, G_StateComparison.greater, 4, true, TestName = "Greater - Lesser Test Value")]
    [TestCase(5, G_StateComparison.greater, 5, false, TestName = "Greater - Equal Value")]
    [TestCase(5, G_StateComparison.greater, 6, false, TestName = "Greater - Greater Test Value")]
    
    [TestCase(5, G_StateComparison.greater_or_equal, 4, true, TestName = "Greater or Equal - Lesser Test Value")]
    [TestCase(5, G_StateComparison.greater_or_equal, 5, true, TestName = "Greater or Equal - Equal Value")]
    [TestCase(5, G_StateComparison.greater_or_equal, 6, false, TestName = "Greater or Equal - Greater Test Value")]
    
    [TestCase(5, G_StateComparison.lesser, 4, false, TestName = "Lesser - Lesser Test Value")]
    [TestCase(5, G_StateComparison.lesser, 5, false, TestName = "Lesser - Equal Value")]
    [TestCase(5, G_StateComparison.lesser, 6, true, TestName = "Lesser - Greater Test Value")]
    
    [TestCase(5, G_StateComparison.lesser_or_equal, 4, false, TestName = "Lesser or Equal - Lesser Test Value")]
    [TestCase(5, G_StateComparison.lesser_or_equal, 5, true, TestName = "Lesser or Equal - Equal Value")]
    [TestCase(5, G_StateComparison.lesser_or_equal, 6, true, TestName = "Lesser or Equal - Greater Test Value")]


    public void TestState(int stateValue, G_StateComparison comparison, int testValue, bool expectedResult) {
        G_IntState testState = An.IntState().WithName("test").WithValue(stateValue);

        bool result = testState.TestState(testState, comparison, testValue);
        Assert.AreEqual(expectedResult, result);
    }
}
