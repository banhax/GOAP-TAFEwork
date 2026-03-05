using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using GOAP;

public class BoolStateTests {
    // A Test behaves as an ordinary method
    [Test]
    public void Construct() {
        G_BoolState testState = ScriptableObject.CreateInstance<G_BoolState>();
        testState.Construct("test", true);

        Assert.AreEqual("test", testState.name);
        Assert.AreEqual(true, (bool)testState.GetValue());
    }

    [Test]
    public void Clone() {
        G_BoolState testState = A.BoolState().WithName("test").WithValue(true);
        G_State cloneState = testState.Clone();

        Assert.AreEqual(testState.name, cloneState.name);
        Assert.AreEqual((bool)testState.GetValue(), (bool)cloneState.GetValue());
        Assert.IsTrue(cloneState is G_BoolState);
    }

    [TestCase(true, G_StateComparison.equal, true, true, TestName = "Equal - True")]
    [TestCase(false, G_StateComparison.equal, true, false, TestName = "Equal - False")]
    [TestCase(true, G_StateComparison.not_equal, false, true, TestName = "Not Equal - True")]
    [TestCase(false, G_StateComparison.not_equal, false, false, TestName = "Not Equal - False")]
    public void TestState(bool actualValue, G_StateComparison comparison, bool expectedValue, bool expectedResult) {
        G_BoolState testState = A.BoolState().WithName("test").WithValue(actualValue);
        
        bool result = testState.TestState(testState, comparison, expectedValue);

        Assert.AreEqual(expectedResult, result);
    }
}
