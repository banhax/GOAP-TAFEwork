using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using GOAP;

public class ConditionTests
{
    #region Condition Functions
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
    #endregion

    #region Bool Conditions

    [TestCase(G_StateComparison.equal, true, G_StateComparison.equal, true, true, TestName = "Equal True vs Equal True")]
    [TestCase(G_StateComparison.equal, true, G_StateComparison.equal, false, false, TestName = "Equal True vs Equal False")]
    [TestCase(G_StateComparison.equal, false, G_StateComparison.equal, true, false, TestName = "Equal False vs Equal True")]
    [TestCase(G_StateComparison.equal, false, G_StateComparison.equal, false, true, TestName = "Equal False vs Equal False")]

    [TestCase(G_StateComparison.not_equal, true, G_StateComparison.not_equal, true, true, TestName = "Not Equal True vs Not Equal True")]
    [TestCase(G_StateComparison.not_equal, true, G_StateComparison.not_equal, false, false, TestName = "Not Equal True vs Not Equal False")]
    [TestCase(G_StateComparison.not_equal, false, G_StateComparison.not_equal, true, false, TestName = "Not Equal False vs Not Equal True")]
    [TestCase(G_StateComparison.not_equal, false, G_StateComparison.not_equal, false, true, TestName = "Not Equal False vs Not Equal False")]
    public void CompareConditionToEffect_Bool(G_StateComparison preComparison,
        bool preExpectedValue,
        G_StateComparison effectComparison,
        bool effectExpectedValue,
        bool expectedResult) {

        G_BoolState boolState = A.BoolState().WithName("test").WithValue(true);
        G_Condition preCondition 
            = A.Condition().WithState(boolState).WithComparison(preComparison).WithExpectedValue(preExpectedValue);
        G_Condition effect 
            = A.Condition().WithState(boolState).WithComparison(effectComparison).WithExpectedValue(effectExpectedValue);

        bool result = preCondition.CompareConditionToEffect(effect);

        Assert.AreEqual(expectedResult, result);
    }

    #endregion
}
