using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using GOAP;

public class FloatStateTests {

    // A Test behaves as an ordinary method
    [TestCase(5f, G_StateComparison.equal, 5, true)]
    [TestCase(5f, G_StateComparison.equal, 0, false)]
    public void TestState(float stateValue, G_StateComparison comparison, float testValue, bool expectedResult) {
        G_FloatState testState = A.G_FloatState().WithName("test").WithValue(stateValue);

        bool result = testState.TestState(testState, comparison, testValue);
        Assert.AreEqual(expectedResult, result);
    }
}
