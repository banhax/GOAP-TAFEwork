using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using GOAP;

public class FloatStateTests {

    [Test]
    public void Clone() {
        G_FloatState testState = A.FloatState("test").WithValue(5);
        G_State cloneState = testState.Clone();

        Assert.AreEqual(testState.name, cloneState.name);
        Assert.AreEqual((float)testState.GetValue(), (float)cloneState.GetValue());
        Assert.IsTrue(cloneState is G_FloatState);
    }

    [TestCase(5f, G_StateComparison.equal, 5, true, TestName = "Equals - True")]
    [TestCase(5f, G_StateComparison.equal, 0, false, TestName = "Equals - False")]

    [TestCase(5f, G_StateComparison.greater, 4, true, TestName = "Greater - Lesser Test Value")]
    [TestCase(5f, G_StateComparison.greater, 5, false, TestName = "Greater - Equal Value")]
    [TestCase(5f, G_StateComparison.greater, 6, false, TestName = "Greater - Greater Test Value")]
    
    [TestCase(5f, G_StateComparison.greater_or_equal, 4, true, TestName = "Greater or Equal - Lesser Test Value")]
    [TestCase(5f, G_StateComparison.greater_or_equal, 5, true, TestName = "Greater or Equal - Equal Value")]
    [TestCase(5f, G_StateComparison.greater_or_equal, 6, false, TestName = "Greater or Equal - Greater Test Value")]
    
    [TestCase(5f, G_StateComparison.lesser, 4, false, TestName = "Lesser - Lesser Test Value")]
    [TestCase(5f, G_StateComparison.lesser, 5, false, TestName = "Lesser - Equal Value")]
    [TestCase(5f, G_StateComparison.lesser, 6, true, TestName = "Lesser - Greater Test Value")]
    
    [TestCase(5f, G_StateComparison.lesser_or_equal, 4, false, TestName = "Lesser or Equal - Lesser Test Value")]
    [TestCase(5f, G_StateComparison.lesser_or_equal, 5, true, TestName = "Lesser or Equal - Equal Value")]
    [TestCase(5f, G_StateComparison.lesser_or_equal, 6, true, TestName = "Lesser or Equal - Greater Test Value")]


    public void TestState(float stateValue, G_StateComparison comparison, float testValue, bool expectedResult) {
        G_FloatState testState = A.FloatState("test").WithValue(stateValue);

        bool result = testState.TestState(testState, comparison, testValue);
        Assert.AreEqual(expectedResult, result);
    }
}
