using GOAP;
using UnityEngine;

public static class G_NumberConditionComparer
{
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

    static bool TestEqual(float preValue, float effectValue, G_StateComparison effectCompare) {
        return effectCompare == G_StateComparison.equal
            && preValue == effectValue;
    }
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
    static bool TestLesserOrEqual(float preValue, float effectValue, G_StateComparison effectCompare) {
        bool success = false;

        switch (effectCompare) {
            case G_StateComparison.equal:
                success = effectValue < preValue;
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
}
