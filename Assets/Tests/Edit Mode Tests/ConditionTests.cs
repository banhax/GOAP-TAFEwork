using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using GOAP;

public class ConditionTests
{
    // A Test behaves as an ordinary method
    [TestCase(false, TestName = "Use Internal State")]
    [TestCase(true, TestName = "Use Parameter State")]
    public void DoesStateMeetCondition(bool useParameter)
    {
        G_BoolState boolState = A.BoolState().WithName("test").WithValue(true);

        G_Condition condition = A.Condition()
        .WithState(boolState)
        .WithComparison(G_StateComparison.equal)
        .WithExpectedValue(true);
        
        bool result = false;

        if (useParameter) {
            result = condition.DoesStateMeetCondition(boolState);
        }
        else {
            result = condition.DoesStateMeetCondition();
        }

        Assert.IsTrue(result);
    }
}
