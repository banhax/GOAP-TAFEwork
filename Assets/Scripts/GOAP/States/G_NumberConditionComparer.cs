using GOAP;
using UnityEngine;

public static class G_NumberConditionComparer
{
    #region Test Values

    public static bool TestValues(int stateValue, G_StateComparison comparison, int expectedValue) {
        bool result = false;

        switch (comparison) {
            case G_StateComparison.EqualTo:
                result = (stateValue == expectedValue);
                break;
            case G_StateComparison.GreaterThan:
                result = (stateValue > expectedValue);
                break;
            case G_StateComparison.GreaterThanOrEqualTo:
                result = (stateValue >= expectedValue);
                break;
            case G_StateComparison.LessThan:
                result = (stateValue < expectedValue);
                break;
            case G_StateComparison.LessThanOrEqualTo:
                result = (stateValue <= expectedValue);
                break;
        }

        return result;
    }

    public static bool TestValues(float stateValue, G_StateComparison comparison, float expectedValue) {
        bool result = false;

        switch (comparison) {
            case G_StateComparison.EqualTo:
                result = (stateValue == expectedValue);
                break;
            case G_StateComparison.GreaterThan:
                result = (stateValue > expectedValue);
                break;
            case G_StateComparison.GreaterThanOrEqualTo:
                result = (stateValue >= expectedValue);
                break;
            case G_StateComparison.LessThan:
                result = (stateValue < expectedValue);
                break;
            case G_StateComparison.LessThanOrEqualTo:
                result = (stateValue <= expectedValue);
                break;
        }

        return result;
    }

    #endregion

    #region Compare Conditions


    public static bool CompareNumberCondition(float preValue,
        G_StateComparison preCompare,
        float effectValue,
        G_StateComparison effectCompare) {

        bool success = false;

        switch (preCompare) {
            case G_StateComparison.EqualTo:
                success = TestEqual(preValue, effectValue, effectCompare);
                break;
            case G_StateComparison.GreaterThan:
                success = TestGreater(preValue, effectValue, effectCompare);
                break;
            case G_StateComparison.GreaterThanOrEqualTo:
                success = TestGreaterOrEqual(preValue, effectValue, effectCompare);
                break;
            case G_StateComparison.LessThan:
                success = TestLesser(preValue, effectValue, effectCompare);
                break;
            case G_StateComparison.LessThanOrEqualTo:
                success = TestLesserOrEqual(preValue, effectValue, effectCompare);
                break;
            default:
                Debug.Log($"Number condition comparer does not support {preCompare} comparison");
                break;
        }

        return success;
    }

    public static bool CompareNumberCondition(int preValue,
        G_StateComparison preCompare,
        int effectValue,
        G_StateComparison effectCompare) {

        bool success = false;

        switch (preCompare) {
            case G_StateComparison.EqualTo:
                success = TestEqual(preValue, effectValue, effectCompare);
                break;
            case G_StateComparison.GreaterThan:
                success = TestGreater(preValue, effectValue, effectCompare);
                break;
            case G_StateComparison.GreaterThanOrEqualTo:
                success = TestGreaterOrEqual(preValue, effectValue, effectCompare);
                break;
            case G_StateComparison.LessThan:
                success = TestLesser(preValue, effectValue, effectCompare);
                break;
            case G_StateComparison.LessThanOrEqualTo:
                success = TestLesserOrEqual(preValue, effectValue, effectCompare);
                break;
            default:
                Debug.Log($"Number condition comparer does not support {preCompare} comparison");
                break;
        }

        return success;
    }

    #endregion

    #region Test Equal
    static bool TestEqual(float preValue, float effectValue, G_StateComparison effectCompare) {
        return effectCompare == G_StateComparison.EqualTo
            && preValue == effectValue;
    }
    static bool TestEqual(int preValue, int effectValue, G_StateComparison effectCompare) {
        return effectCompare == G_StateComparison.EqualTo
            && preValue == effectValue;
    }
    #endregion

    #region Test Greater
    static bool TestGreater(float preValue, float effectValue, G_StateComparison effectCompare) {
        bool success = false;
        
        switch (effectCompare) {
            case G_StateComparison.EqualTo:
                success = effectValue > preValue;
                break;
            case G_StateComparison.GreaterThan:
                success = effectValue >= preValue;
                break;
            case G_StateComparison.GreaterThanOrEqualTo:
                success = effectValue > preValue;
                break;
        }

        return success;
    }

    static bool TestGreater(int preValue, int effectValue, G_StateComparison effectCompare) {
        bool success = false;
        
        switch (effectCompare) {
            case G_StateComparison.EqualTo:
                success = effectValue > preValue;
                break;
            case G_StateComparison.GreaterThan:
                success = effectValue >= preValue;
                break;
            case G_StateComparison.GreaterThanOrEqualTo:
                success = effectValue > preValue;
                break;
        }

        return success;
    }
    #endregion

    #region Test Lesser
    static bool TestLesser(float preValue, float effectValue, G_StateComparison effectCompare) {
        bool success = false;

        switch (effectCompare) {
            case G_StateComparison.EqualTo:
                success = effectValue < preValue;
                break;
            case G_StateComparison.LessThan:
                success = effectValue <= preValue;
                break;
            case G_StateComparison.LessThanOrEqualTo:
                success = effectValue < preValue;
                break;
        }

        return success;
    }

    static bool TestLesser(int preValue, int effectValue, G_StateComparison effectCompare) {
        bool success = false;

        switch (effectCompare) {
            case G_StateComparison.EqualTo:
                success = effectValue < preValue;
                break;
            case G_StateComparison.LessThan:
                success = effectValue <= preValue;
                break;
            case G_StateComparison.LessThanOrEqualTo:
                success = effectValue < preValue;
                break;
        }

        return success;
    }
    #endregion

    #region Greater Or Equal
    static bool TestGreaterOrEqual(float preValue, float effectValue, G_StateComparison effectCompare) {
        bool success = false;

        switch (effectCompare) {
            case G_StateComparison.EqualTo:
                success = effectValue >= preValue;
                break;
            case G_StateComparison.GreaterThan:
                success = effectValue >= preValue;
                break;
            case G_StateComparison.GreaterThanOrEqualTo:
                success = effectValue >= preValue;
                break;
        }

        return success;
    }
    static bool TestGreaterOrEqual(int preValue, int effectValue, G_StateComparison effectCompare) {
        bool success = false;

        switch (effectCompare) {
            case G_StateComparison.EqualTo:
                success = effectValue >= preValue;
                break;
            case G_StateComparison.GreaterThan:
                success = effectValue >= preValue - 1;
                break;
            case G_StateComparison.GreaterThanOrEqualTo:
                success = effectValue >= preValue;
                break;
        }

        return success;
    }
    #endregion

    #region Lesser Or Equal
    static bool TestLesserOrEqual(float preValue, float effectValue, G_StateComparison effectCompare) {
        bool success = false;

        switch (effectCompare) {
            case G_StateComparison.EqualTo:
                success = effectValue <= preValue;
                break;
            case G_StateComparison.LessThan:
                success = effectValue <= preValue;
                break;
            case G_StateComparison.LessThanOrEqualTo:
                success = effectValue <= preValue;
                break;
        }

        return success;
    }

    static bool TestLesserOrEqual(int preValue, int effectValue, G_StateComparison effectCompare) {
        bool success = false;

        switch (effectCompare) {
            case G_StateComparison.EqualTo:
                success = effectValue <= preValue;
                break;
            case G_StateComparison.LessThan:
                success = effectValue <= preValue + 1;
                break;
            case G_StateComparison.LessThanOrEqualTo:
                success = effectValue <= preValue;
                break;
        }

        return success;
    }
    #endregion
}
