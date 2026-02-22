using GOAP;
using UnityEngine;

public static class G_NumberConditionComparer
{
    #region Compare Conditions


    public static bool CompareNumberCondition(float preValue,
        G_StateComparison preCompare,
        float effectValue,
        G_StateComparison effectCompare) {

        bool success = false;

        switch (preCompare) {
            case G_StateComparison.equal:
                success = TestEqual(preValue, effectValue, effectCompare);
                break;
            case G_StateComparison.greater:
                success = TestGreater(preValue, effectValue, effectCompare);
                break;
            case G_StateComparison.greater_or_equal:
                success = TestGreaterOrEqual(preValue, effectValue, effectCompare);
                break;
            case G_StateComparison.lesser:
                success = TestLesser(preValue, effectValue, effectCompare);
                break;
            case G_StateComparison.lesser_or_equal:
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
            case G_StateComparison.equal:
                success = TestEqual(preValue, effectValue, effectCompare);
                break;
            case G_StateComparison.greater:
                success = TestGreater(preValue, effectValue, effectCompare);
                break;
            case G_StateComparison.greater_or_equal:
                success = TestGreaterOrEqual(preValue, effectValue, effectCompare);
                break;
            case G_StateComparison.lesser:
                success = TestLesser(preValue, effectValue, effectCompare);
                break;
            case G_StateComparison.lesser_or_equal:
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
        return effectCompare == G_StateComparison.equal
            && preValue == effectValue;
    }
    static bool TestEqual(int preValue, int effectValue, G_StateComparison effectCompare) {
        return effectCompare == G_StateComparison.equal
            && preValue == effectValue;
    }
    #endregion

    #region Test Greater
    static bool TestGreater(float preValue, float effectValue, G_StateComparison effectCompare) {
        bool success = false;
        
        switch (effectCompare) {
            case G_StateComparison.equal:
                success = effectValue > preValue;
                break;
            case G_StateComparison.greater:
                success = effectValue >= preValue;
                break;
            case G_StateComparison.greater_or_equal:
                success = effectValue > preValue;
                break;
        }

        return success;
    }

    static bool TestGreater(int preValue, int effectValue, G_StateComparison effectCompare) {
        bool success = false;
        
        switch (effectCompare) {
            case G_StateComparison.equal:
                success = effectValue > preValue;
                break;
            case G_StateComparison.greater:
                success = effectValue >= preValue;
                break;
            case G_StateComparison.greater_or_equal:
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
            case G_StateComparison.equal:
                success = effectValue < preValue;
                break;
            case G_StateComparison.lesser:
                success = effectValue <= preValue;
                break;
            case G_StateComparison.lesser_or_equal:
                success = effectValue < preValue;
                break;
        }

        return success;
    }

    static bool TestLesser(int preValue, int effectValue, G_StateComparison effectCompare) {
        bool success = false;

        switch (effectCompare) {
            case G_StateComparison.equal:
                success = effectValue < preValue;
                break;
            case G_StateComparison.lesser:
                success = effectValue <= preValue;
                break;
            case G_StateComparison.lesser_or_equal:
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
            case G_StateComparison.equal:
                success = effectValue >= preValue;
                break;
            case G_StateComparison.greater:
                success = effectValue >= preValue;
                break;
            case G_StateComparison.greater_or_equal:
                success = effectValue >= preValue;
                break;
        }

        return success;
    }
    static bool TestGreaterOrEqual(int preValue, int effectValue, G_StateComparison effectCompare) {
        bool success = false;

        switch (effectCompare) {
            case G_StateComparison.equal:
                success = effectValue >= preValue;
                break;
            case G_StateComparison.greater:
                success = effectValue >= preValue - 1;
                break;
            case G_StateComparison.greater_or_equal:
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
            case G_StateComparison.equal:
                success = effectValue <= preValue;
                break;
            case G_StateComparison.lesser:
                success = effectValue <= preValue;
                break;
            case G_StateComparison.lesser_or_equal:
                success = effectValue <= preValue;
                break;
        }

        return success;
    }

    static bool TestLesserOrEqual(int preValue, int effectValue, G_StateComparison effectCompare) {
        bool success = false;

        switch (effectCompare) {
            case G_StateComparison.equal:
                success = effectValue <= preValue;
                break;
            case G_StateComparison.lesser:
                success = effectValue <= preValue + 1;
                break;
            case G_StateComparison.lesser_or_equal:
                success = effectValue <= preValue;
                break;
        }

        return success;
    }
    #endregion
}
