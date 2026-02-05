using UnityEngine;

public class G_State : ScriptableObject
{
    // the value we are storing
    object value;

    #region Basic Controls

    public virtual void Construct(string name, object value) {
        this.name = name;
        SetValue(value);
    }

    public virtual void SetValue(object value) {
        this.value = value;
    }

    public virtual object GetValue() {
        return value;
    }

    public virtual G_State Clone() {
        G_State clone = ScriptableObject.CreateInstance<G_State>();
        clone.Construct(this.name, this.value);
        return clone;
    }

    #endregion

    #region Testing Controls

    public virtual bool TestState(G_State state, object expectedValue/*, G_StateComparison comparison*/) {
        Debug.LogWarning($"Base class G_State has no State testing implemented - returning false for {state.name}");
        return false;
    }

    public virtual bool TestStateConditionMatch(/*G_Condition preCondition, G_Condition effect*/) {
        Debug.LogWarning($"Base class G_State has no condition comparisons implemented - returning false");
        return false;
    }

    public virtual bool StateSupportsComparison(/*G_StateComparison comparison*/) {
        Debug.LogWarning($"Base class G_State doesn't support any comparisons - returning false");
        return false;
    }

    public virtual bool TestValueMatch(object testValue) {
        Debug.LogWarning($"Base class G_State doesn't support value matching - returning false");
        return false;
    }

    #endregion
}
