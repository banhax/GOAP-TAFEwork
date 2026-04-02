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

    [TestCase(5f, G_StateComparison.EqualTo, 5, true, TestName = "Equals - True")]
    [TestCase(5f, G_StateComparison.EqualTo, 0, false, TestName = "Equals - False")]

    [TestCase(5f, G_StateComparison.GreaterThan, 4, true, TestName = "Greater - Lesser Test Value")]
    [TestCase(5f, G_StateComparison.GreaterThan, 5, false, TestName = "Greater - Equal Value")]
    [TestCase(5f, G_StateComparison.GreaterThan, 6, false, TestName = "Greater - Greater Test Value")]
    
    [TestCase(5f, G_StateComparison.GreaterThanOrEqualTo, 4, true, TestName = "Greater or Equal - Lesser Test Value")]
    [TestCase(5f, G_StateComparison.GreaterThanOrEqualTo, 5, true, TestName = "Greater or Equal - Equal Value")]
    [TestCase(5f, G_StateComparison.GreaterThanOrEqualTo, 6, false, TestName = "Greater or Equal - Greater Test Value")]
    
    [TestCase(5f, G_StateComparison.LessThan, 4, false, TestName = "Lesser - Lesser Test Value")]
    [TestCase(5f, G_StateComparison.LessThan, 5, false, TestName = "Lesser - Equal Value")]
    [TestCase(5f, G_StateComparison.LessThan, 6, true, TestName = "Lesser - Greater Test Value")]
    
    [TestCase(5f, G_StateComparison.LessThanOrEqualTo, 4, false, TestName = "Lesser or Equal - Lesser Test Value")]
    [TestCase(5f, G_StateComparison.LessThanOrEqualTo, 5, true, TestName = "Lesser or Equal - Equal Value")]
    [TestCase(5f, G_StateComparison.LessThanOrEqualTo, 6, true, TestName = "Lesser or Equal - Greater Test Value")]


    public void TestState(float stateValue, G_StateComparison comparison, float testValue, bool expectedResult) {
        G_FloatState testState = A.FloatState("test").WithValue(stateValue);

        bool result = testState.TestState(testState, comparison, testValue);
        Assert.AreEqual(expectedResult, result);
    }
}
