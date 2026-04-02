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
        G_BoolState boolState = A.BoolState("test").WithValue(true);

        G_Condition condition = A.Condition().State(boolState).IsEqualTo(true);
        
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

    // Equals
    [TestCase(G_StateComparison.EqualTo, true, G_StateComparison.EqualTo, true, true, TestName = "Equal True vs Equal True")]
    [TestCase(G_StateComparison.EqualTo, true, G_StateComparison.EqualTo, false, false, TestName = "Equal True vs Equal False")]
    [TestCase(G_StateComparison.EqualTo, false, G_StateComparison.EqualTo, true, false, TestName = "Equal False vs Equal True")]
    [TestCase(G_StateComparison.EqualTo, false, G_StateComparison.EqualTo, false, true, TestName = "Equal False vs Equal False")]
    
    // Not Equals
    [TestCase(G_StateComparison.NotEqualTo, true, G_StateComparison.NotEqualTo, true, true, TestName = "Not Equal True vs Not Equal True")]
    [TestCase(G_StateComparison.NotEqualTo, true, G_StateComparison.NotEqualTo, false, false, TestName = "Not Equal True vs Not Equal False")]
    [TestCase(G_StateComparison.NotEqualTo, false, G_StateComparison.NotEqualTo, true, false, TestName = "Not Equal False vs Not Equal True")]
    [TestCase(G_StateComparison.NotEqualTo, false, G_StateComparison.NotEqualTo, false, true, TestName = "Not Equal False vs Not Equal False")]
    
    // Equal vs Not Equal
    [TestCase(G_StateComparison.EqualTo, true, G_StateComparison.NotEqualTo, true, false, TestName = "Equal True vs Not Equal True")]
    [TestCase(G_StateComparison.EqualTo, true, G_StateComparison.NotEqualTo, false, true, TestName = "Equal True vs Not Equal False")]
    [TestCase(G_StateComparison.EqualTo, false, G_StateComparison.NotEqualTo, true, true, TestName = "Equal False vs Not Equal True")]
    [TestCase(G_StateComparison.EqualTo, false, G_StateComparison.NotEqualTo, false, false, TestName = "Equal False vs Not Equal False")]

    // Not Equal vs Equal
    [TestCase(G_StateComparison.NotEqualTo, true, G_StateComparison.EqualTo, true, false, TestName = "Not Equal True vs Equal True")]
    [TestCase(G_StateComparison.NotEqualTo, true, G_StateComparison.EqualTo, false, true, TestName = "Not Equal True vs Equal False")]
    [TestCase(G_StateComparison.NotEqualTo, false, G_StateComparison.EqualTo, true, true, TestName = "Not Equal False vs Equal True")]
    [TestCase(G_StateComparison.NotEqualTo, false, G_StateComparison.EqualTo, false, false, TestName = "Not Equal False vs Equal False")]
    public void CompareConditionToEffect_Bool(G_StateComparison preComparison,
        bool preExpectedValue,
        G_StateComparison effectComparison,
        bool effectExpectedValue,
        bool expectedResult) {

        // Arrange
        G_BoolState boolState = A.BoolState("test").WithValue(true);
        G_Condition preCondition 
            = A.Condition().State(boolState).WithComparison(preComparison).WithExpectedValue(preExpectedValue);
        G_Condition effect 
            = A.Condition().State(boolState).WithComparison(effectComparison).WithExpectedValue(effectExpectedValue);

        // Act
        bool result = preCondition.CompareConditionToEffect(effect);

        // Assert
        Assert.AreEqual(expectedResult, result);
    }

    #endregion

    #region Float Conditions

    // Equals
    [TestCase(G_StateComparison.EqualTo, 5, G_StateComparison.EqualTo, 5, true, TestName = "Pre == 5 vs Effect == 5")]
    [TestCase(G_StateComparison.EqualTo, 5, G_StateComparison.EqualTo, 4, false, TestName = "Pre == 5 vs Effect == 4")]



    // Greater Vs Equal
    [TestCase(G_StateComparison.GreaterThan, 5, G_StateComparison.EqualTo, 4, false, TestName = "Pre > 5 vs Effect == 4")]
    [TestCase(G_StateComparison.GreaterThan, 5, G_StateComparison.EqualTo, 5, false, TestName = "Pre > 5 vs Effect == 5")]
    [TestCase(G_StateComparison.GreaterThan, 5, G_StateComparison.EqualTo, 6, true, TestName = "Pre > 5 vs Effect == 6")]

    // Greater Vs Greater
    [TestCase(G_StateComparison.GreaterThan, 5, G_StateComparison.GreaterThan, 4, false, TestName = "Pre > 5 vs Effect > 4")]
    [TestCase(G_StateComparison.GreaterThan, 5, G_StateComparison.GreaterThan, 5, true, TestName = "Pre > 5 vs Effect > 5")]
    [TestCase(G_StateComparison.GreaterThan, 5, G_StateComparison.GreaterThan, 6, true, TestName = "Pre > 5 vs Effect > 6")]

    // Greater Vs Greater or Equal
    [TestCase(G_StateComparison.GreaterThan, 5, G_StateComparison.GreaterThanOrEqualTo, 4, false, TestName = "Pre > 5 vs Effect >= 4")]
    [TestCase(G_StateComparison.GreaterThan, 5, G_StateComparison.GreaterThanOrEqualTo, 5, false, TestName = "Pre > 5 vs Effect >= 5")]
    [TestCase(G_StateComparison.GreaterThan, 5, G_StateComparison.GreaterThanOrEqualTo, 6, true, TestName = "Pre > 5 vs Effect >= 6")]



    // Lesser Vs Equal
    [TestCase(G_StateComparison.LessThan, 5, G_StateComparison.EqualTo, 4, true, TestName = "Pre < 5 vs Effect == 4")]
    [TestCase(G_StateComparison.LessThan, 5, G_StateComparison.EqualTo, 5, false, TestName = "Pre < 5 vs Effect == 5")]
    [TestCase(G_StateComparison.LessThan, 5, G_StateComparison.EqualTo, 6, false, TestName = "Pre < 5 vs Effect == 6")]

    // Lesser Vs Lesser
    [TestCase(G_StateComparison.LessThan, 5, G_StateComparison.LessThan, 4, true, TestName = "Pre < 5 vs Effect < 4")]
    [TestCase(G_StateComparison.LessThan, 5, G_StateComparison.LessThan, 5, true, TestName = "Pre < 5 vs Effect < 5")]
    [TestCase(G_StateComparison.LessThan, 5, G_StateComparison.LessThan, 6, false, TestName = "Pre < 5 vs Effect < 6")]

    // Lesser Vs Lesser or Equal
    [TestCase(G_StateComparison.LessThan, 5, G_StateComparison.LessThanOrEqualTo, 4, true, TestName = "Pre < 5 vs Effect <= 4")]
    [TestCase(G_StateComparison.LessThan, 5, G_StateComparison.LessThanOrEqualTo, 5, false, TestName = "Pre < 5 vs Effect <= 5")]
    [TestCase(G_StateComparison.LessThan, 5, G_StateComparison.LessThanOrEqualTo, 6, false, TestName = "Pre < 5 vs Effect <= 6")]



    // Greater or Equal Vs Equal
    [TestCase(G_StateComparison.GreaterThanOrEqualTo, 5, G_StateComparison.EqualTo, 4, false, TestName = "Pre >= 5 vs Effect == 4")]
    [TestCase(G_StateComparison.GreaterThanOrEqualTo, 5, G_StateComparison.EqualTo, 5, true, TestName = "Pre >= 5 vs Effect == 5")]
    [TestCase(G_StateComparison.GreaterThanOrEqualTo, 5, G_StateComparison.EqualTo, 6, true, TestName = "Pre >= 5 vs Effect == 6")]

    // Greater or Equal Vs Greater
    [TestCase(G_StateComparison.GreaterThanOrEqualTo, 5, G_StateComparison.GreaterThan, 4, false, TestName = "Pre >= 5 vs Effect > 4")]
    [TestCase(G_StateComparison.GreaterThanOrEqualTo, 5, G_StateComparison.GreaterThan, 5, true, TestName = "Pre >= 5 vs Effect > 5")]
    [TestCase(G_StateComparison.GreaterThanOrEqualTo, 5, G_StateComparison.GreaterThan, 6, true, TestName = "Pre >= 5 vs Effect > 6")]

    // Greater or Equal Vs Greater or Equal
    [TestCase(G_StateComparison.GreaterThanOrEqualTo, 5, G_StateComparison.GreaterThanOrEqualTo, 4, false, TestName = "Pre >= 5 vs Effect >= 4")]
    [TestCase(G_StateComparison.GreaterThanOrEqualTo, 5, G_StateComparison.GreaterThanOrEqualTo, 5, true, TestName = "Pre >= 5 vs Effect >= 5")]
    [TestCase(G_StateComparison.GreaterThanOrEqualTo, 5, G_StateComparison.GreaterThanOrEqualTo, 6, true, TestName = "Pre >= 5 vs Effect >= 6")]



    // Lesser or Equal Vs Equal
    [TestCase(G_StateComparison.LessThanOrEqualTo, 5, G_StateComparison.EqualTo, 4, true, TestName = "Pre <= 5 vs Effect == 4")]
    [TestCase(G_StateComparison.LessThanOrEqualTo, 5, G_StateComparison.EqualTo, 5, true, TestName = "Pre <= 5 vs Effect == 5")]
    [TestCase(G_StateComparison.LessThanOrEqualTo, 5, G_StateComparison.EqualTo, 6, false, TestName = "Pre <= 5 vs Effect == 6")]

    // Lesser or Equal Vs Lesser
    [TestCase(G_StateComparison.LessThanOrEqualTo, 5, G_StateComparison.LessThan, 4, true, TestName = "Pre <= 5 vs Effect < 4")]
    [TestCase(G_StateComparison.LessThanOrEqualTo, 5, G_StateComparison.LessThan, 5, true, TestName = "Pre <= 5 vs Effect < 5")]
    [TestCase(G_StateComparison.LessThanOrEqualTo, 5, G_StateComparison.LessThan, 6, false, TestName = "Pre <= 5 vs Effect < 6")]

    // Lesser or Equal Vs Lesser or Equal
    [TestCase(G_StateComparison.LessThanOrEqualTo, 5, G_StateComparison.LessThanOrEqualTo, 4, true, TestName = "Pre <= 5 vs Effect <= 4")]
    [TestCase(G_StateComparison.LessThanOrEqualTo, 5, G_StateComparison.LessThanOrEqualTo, 5, true, TestName = "Pre <= 5 vs Effect <= 5")]
    [TestCase(G_StateComparison.LessThanOrEqualTo, 5, G_StateComparison.LessThanOrEqualTo, 6, false, TestName = "Pre <= 5 vs Effect <= 6")]

    public void CompareConditionToEffect_Float(G_StateComparison preComparison,
        float preExpectedValue,
        G_StateComparison effectComparison,
        float effectExpectedValue,
        bool expectedResult) {

        // Arrange
        G_FloatState floatState = A.FloatState("test").WithValue(5);
        G_Condition preCondition
            = A.Condition().State(floatState).WithComparison(preComparison).WithExpectedValue(preExpectedValue);
        G_Condition effect
            = A.Condition().State(floatState).WithComparison(effectComparison).WithExpectedValue(effectExpectedValue);

        // Act
        bool result = preCondition.CompareConditionToEffect(effect);

        // Assert
        Assert.AreEqual(expectedResult, result);
    }

    #endregion

    #region Int Conditions

    // Equals
    [TestCase(G_StateComparison.EqualTo, 5, G_StateComparison.EqualTo, 5, true, TestName = "Pre == 5 vs Effect == 5")]
    [TestCase(G_StateComparison.EqualTo, 5, G_StateComparison.EqualTo, 4, false, TestName = "Pre == 5 vs Effect == 4")]



    // Greater Vs Equal
    [TestCase(G_StateComparison.GreaterThan, 5, G_StateComparison.EqualTo, 4, false, TestName = "Pre > 5 vs Effect == 4")]
    [TestCase(G_StateComparison.GreaterThan, 5, G_StateComparison.EqualTo, 5, false, TestName = "Pre > 5 vs Effect == 5")]
    [TestCase(G_StateComparison.GreaterThan, 5, G_StateComparison.EqualTo, 6, true, TestName = "Pre > 5 vs Effect == 6")]

    // Greater Vs Greater
    [TestCase(G_StateComparison.GreaterThan, 5, G_StateComparison.GreaterThan, 4, false, TestName = "Pre > 5 vs Effect > 4")]
    [TestCase(G_StateComparison.GreaterThan, 5, G_StateComparison.GreaterThan, 5, true, TestName = "Pre > 5 vs Effect > 5")]
    [TestCase(G_StateComparison.GreaterThan, 5, G_StateComparison.GreaterThan, 6, true, TestName = "Pre > 5 vs Effect > 6")]

    // Greater Vs Greater or Equal
    [TestCase(G_StateComparison.GreaterThan, 5, G_StateComparison.GreaterThanOrEqualTo, 4, false, TestName = "Pre > 5 vs Effect >= 4")]
    [TestCase(G_StateComparison.GreaterThan, 5, G_StateComparison.GreaterThanOrEqualTo, 5, false, TestName = "Pre > 5 vs Effect >= 5")]
    [TestCase(G_StateComparison.GreaterThan, 5, G_StateComparison.GreaterThanOrEqualTo, 6, true, TestName = "Pre > 5 vs Effect >= 6")]



    // Lesser Vs Equal
    [TestCase(G_StateComparison.LessThan, 5, G_StateComparison.EqualTo, 4, true, TestName = "Pre < 5 vs Effect == 4")]
    [TestCase(G_StateComparison.LessThan, 5, G_StateComparison.EqualTo, 5, false, TestName = "Pre < 5 vs Effect == 5")]
    [TestCase(G_StateComparison.LessThan, 5, G_StateComparison.EqualTo, 6, false, TestName = "Pre < 5 vs Effect == 6")]

    // Lesser Vs Lesser
    [TestCase(G_StateComparison.LessThan, 5, G_StateComparison.LessThan, 4, true, TestName = "Pre < 5 vs Effect < 4")]
    [TestCase(G_StateComparison.LessThan, 5, G_StateComparison.LessThan, 5, true, TestName = "Pre < 5 vs Effect < 5")]
    [TestCase(G_StateComparison.LessThan, 5, G_StateComparison.LessThan, 6, false, TestName = "Pre < 5 vs Effect < 6")]

    // Lesser Vs Lesser or Equal
    [TestCase(G_StateComparison.LessThan, 5, G_StateComparison.LessThanOrEqualTo, 4, true, TestName = "Pre < 5 vs Effect <= 4")]
    [TestCase(G_StateComparison.LessThan, 5, G_StateComparison.LessThanOrEqualTo, 5, false, TestName = "Pre < 5 vs Effect <= 5")]
    [TestCase(G_StateComparison.LessThan, 5, G_StateComparison.LessThanOrEqualTo, 6, false, TestName = "Pre < 5 vs Effect <= 6")]



    // Greater or Equal Vs Equal
    [TestCase(G_StateComparison.GreaterThanOrEqualTo, 5, G_StateComparison.EqualTo, 4, false, TestName = "Pre >= 5 vs Effect == 4")]
    [TestCase(G_StateComparison.GreaterThanOrEqualTo, 5, G_StateComparison.EqualTo, 5, true, TestName = "Pre >= 5 vs Effect == 5")]
    [TestCase(G_StateComparison.GreaterThanOrEqualTo, 5, G_StateComparison.EqualTo, 6, true, TestName = "Pre >= 5 vs Effect == 6")]

    // Greater or Equal Vs Greater
    [TestCase(G_StateComparison.GreaterThanOrEqualTo, 5, G_StateComparison.GreaterThan, 3, false, TestName = "Pre >= 5 vs Effect > 3")]
    [TestCase(G_StateComparison.GreaterThanOrEqualTo, 5, G_StateComparison.GreaterThan, 4, true, TestName = "Pre >= 5 vs Effect > 4")]
    [TestCase(G_StateComparison.GreaterThanOrEqualTo, 5, G_StateComparison.GreaterThan, 5, true, TestName = "Pre >= 5 vs Effect > 5")]
    [TestCase(G_StateComparison.GreaterThanOrEqualTo, 5, G_StateComparison.GreaterThan, 6, true, TestName = "Pre >= 5 vs Effect > 6")]

    // Greater or Equal Vs Greater or Equal
    [TestCase(G_StateComparison.GreaterThanOrEqualTo, 5, G_StateComparison.GreaterThanOrEqualTo, 4, false, TestName = "Pre >= 5 vs Effect >= 4")]
    [TestCase(G_StateComparison.GreaterThanOrEqualTo, 5, G_StateComparison.GreaterThanOrEqualTo, 5, true, TestName = "Pre >= 5 vs Effect >= 5")]
    [TestCase(G_StateComparison.GreaterThanOrEqualTo, 5, G_StateComparison.GreaterThanOrEqualTo, 6, true, TestName = "Pre >= 5 vs Effect >= 6")]



    // Lesser or Equal Vs Equal
    [TestCase(G_StateComparison.LessThanOrEqualTo, 5, G_StateComparison.EqualTo, 4, true, TestName = "Pre <= 5 vs Effect == 4")]
    [TestCase(G_StateComparison.LessThanOrEqualTo, 5, G_StateComparison.EqualTo, 5, true, TestName = "Pre <= 5 vs Effect == 5")]
    [TestCase(G_StateComparison.LessThanOrEqualTo, 5, G_StateComparison.EqualTo, 6, false, TestName = "Pre <= 5 vs Effect == 6")]

    // Lesser or Equal Vs Lesser
    [TestCase(G_StateComparison.LessThanOrEqualTo, 5, G_StateComparison.LessThan, 4, true, TestName = "Pre <= 5 vs Effect < 4")]
    [TestCase(G_StateComparison.LessThanOrEqualTo, 5, G_StateComparison.LessThan, 5, true, TestName = "Pre <= 5 vs Effect < 5")]
    [TestCase(G_StateComparison.LessThanOrEqualTo, 5, G_StateComparison.LessThan, 6, true, TestName = "Pre <= 5 vs Effect < 6")]
    [TestCase(G_StateComparison.LessThanOrEqualTo, 5, G_StateComparison.LessThan, 7, false, TestName = "Pre <= 5 vs Effect < 7")]

    // Lesser or Equal Vs Lesser or Equal
    [TestCase(G_StateComparison.LessThanOrEqualTo, 5, G_StateComparison.LessThanOrEqualTo, 4, true, TestName = "Pre <= 5 vs Effect <= 4")]
    [TestCase(G_StateComparison.LessThanOrEqualTo, 5, G_StateComparison.LessThanOrEqualTo, 5, true, TestName = "Pre <= 5 vs Effect <= 5")]
    [TestCase(G_StateComparison.LessThanOrEqualTo, 5, G_StateComparison.LessThanOrEqualTo, 6, false, TestName = "Pre <= 5 vs Effect <= 6")]

    public void CompareConditionToEffect_Int(G_StateComparison preComparison,
        int preExpectedValue,
        G_StateComparison effectComparison,
        int effectExpectedValue,
        bool expectedResult) {

        // Arrange
        G_IntState intState = An.IntState("test").WithValue(5);
        G_Condition preCondition
            = A.Condition().State(intState).WithComparison(preComparison).WithExpectedValue(preExpectedValue);
        G_Condition effect
            = A.Condition().State(intState).WithComparison(effectComparison).WithExpectedValue(effectExpectedValue);

        // Act
        bool result = preCondition.CompareConditionToEffect(effect);

        // Assert
        Assert.AreEqual(expectedResult, result);
    }

    #endregion

    #region At Location Compares

    [TestCase(G_StateComparison.EqualTo, true, G_StateComparison.EqualTo, true, true, TestName = "Equals tree vs Equals tree")]
    [TestCase(G_StateComparison.EqualTo, false, G_StateComparison.EqualTo, false, true, TestName = "Equals null vs Equals null")]
    [TestCase(G_StateComparison.EqualTo, false, G_StateComparison.EqualTo, true, false, TestName = "Equals null vs Equals tree")]
    [TestCase(G_StateComparison.EqualTo, true, G_StateComparison.EqualTo, false, false, TestName = "Equals tree vs Equals null")]

    [TestCase(G_StateComparison.GreaterThan, true, G_StateComparison.EqualTo, true, false, TestName = "Greater tree vs Equals tree")]
    [TestCase(G_StateComparison.GreaterThan, false, G_StateComparison.EqualTo, false, false, TestName = "Greater null vs Equals null")]
    public void AtLocationConditionCompare(G_StateComparison preCompare,
    bool validPreExpectedValue,
    G_StateComparison effectCompare,
    bool validEffectExpectedValue,
    bool expectedResult) {

        LocationType tree = A.LocationType("tree");
        G_AtLocation atLocation = An.AtLocation("atLocation").WithLocationType(tree);
        LocationType preExpectedValue = null;
        LocationType effectExpectedValue = null;

        if (validPreExpectedValue) {
            preExpectedValue = tree;
        }
        if (validEffectExpectedValue) {
            effectExpectedValue = tree;
        }

        G_Condition precondition =
            A.Condition().State(atLocation).WithComparison(preCompare).WithExpectedReference(preExpectedValue);
        G_Condition effect =
            A.Condition().State(atLocation).WithComparison(effectCompare).WithExpectedReference(effectExpectedValue);

        bool result = precondition.CompareConditionToEffect(effect);
        Assert.AreEqual(expectedResult, result);
    }

    #endregion

    #region Inventory Conditions

    [TestCase(true, G_StateComparison.EqualTo, 1, G_StateComparison.EqualTo, 1, true, TestName = "Both equal to 1 item")]
    [TestCase(true, G_StateComparison.EqualTo, 1, G_StateComparison.EqualTo, 0, false, TestName = "Not equal to Same item")]
    [TestCase(false, G_StateComparison.EqualTo, 1, G_StateComparison.EqualTo, 1, false, TestName = "Different Items")]
    [TestCase(true, G_StateComparison.GreaterThan, 0, G_StateComparison.EqualTo, 1, true, TestName = "Equal 1 vs Greater 0")]
    public void InventoryConditionCompare(bool useSameItem,
    G_StateComparison preCompare,
    int preQuantity,
    G_StateComparison effectCompare,
    int effectQuantity,
    bool expectedResult) {

        GameObject go = new GameObject();
        Inventory inventory = go.AddComponent<Inventory>();
        Item axe = An.Item("axe").isStackable(false);
        Item wood = An.Item("wood").isStackable(true);

        G_Inventory testState = An.InventoryState("test").WithInventory(inventory);

        ItemStack preExpectedValue = new ItemStack(axe, preQuantity);
        ItemStack effectExpectedValue = useSameItem ? new ItemStack(axe, effectQuantity) : new ItemStack(wood, effectQuantity);

        G_Condition precondition
            = A.Condition().State(testState).WithComparison(preCompare).WithExpectedValue(preExpectedValue);
        G_Condition effect
            = A.Condition().State(testState).WithComparison(effectCompare).WithExpectedValue(effectExpectedValue);

        bool result = precondition.CompareConditionToEffect(effect);

        Assert.AreEqual(expectedResult, result);

    }

    #endregion
}
