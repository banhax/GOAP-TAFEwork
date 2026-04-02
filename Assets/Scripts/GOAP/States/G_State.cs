using UnityEngine;
using GOAP;
using UnityEditor;

namespace GOAP {

    public class G_State : ScriptableObject {
        // the value we are storing
        object value;
        public bool isLocal = false;

        #region Basic Controls

        public virtual void Construct(string name, object value, bool isLocal) {
            this.name = name;
            this.isLocal = isLocal;
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
            clone.Construct(this.name, this.value, this.isLocal);
            return clone;
        }

        public virtual object ConvertSerializedStringToValue(string serializedString) {
            return null;
        }

        #endregion

        #region Testing Controls

        /// <summary>
        /// Tests the given state against the expectedValue using the chosen comparison, returning true if the comparison
        /// is correct and false if not
        /// </summary>
        /// <param name="state"></param>
        /// <param name="expectedValue"></param>
        /// <param name="comparison"></param>
        /// <returns></returns>
        public virtual bool TestState(G_State state, G_StateComparison comparison, object expectedValue) {
            Debug.LogWarning($"Base class G_State has no State testing implemented - returning false for {state.name}");
            return false;
        }

        /// <summary>
        /// This function returns true if the two given conditions match their states, expected values, and comparisons
        /// </summary>
        /// <param name="preCondition"></param>
        /// <param name="effect"></param>
        /// <returns></returns>
        public virtual bool TestStateConditionMatch(G_Condition preCondition, G_Condition effect) {
            Debug.LogWarning($"Base class G_State has no condition comparisons implemented - returning false");
            return false;
        }
         
        /// <summary>
        /// Returns true if the state type has an implementation for the given comparison type
        /// </summary>
        /// <param name="comparison"></param>
        /// <returns></returns>
        public virtual bool StateSupportsComparison(G_StateComparison comparison) {
            Debug.LogWarning($"Base class G_State doesn't support any comparisons - returning false");
            return false;
        }

        /// <summary>
        /// Tests if the given value is of the same type as the value stored in this state and returns true if it is
        /// </summary>
        /// <param name="testValue"></param>
        /// <returns></returns>
        public virtual bool TestValueMatch(object testValue) {
            Debug.LogWarning($"Base class G_State doesn't support value matching - returning false");
            return false;
        }

        #endregion

#if UNITY_EDITOR
        #region Editor

        public virtual int GetEditorHeight() {
            return 3;
        }

        public virtual void Editor(G_ConditionEditor propertyDrawer,
            ref float height,
            Rect position,
            SerializedProperty property,
            GUIContent label) {

            EditorGUI.LabelField(position, "No GUI Implemented for this State type");
            propertyDrawer.IncrementHeight(out height, property, label);
        }

        #endregion
#endif
    }
}