using UnityEditor;
using UnityEngine;

namespace GOAP {
    [CreateAssetMenu(fileName = "New Bool State", menuName = "GOAP/States/Bool State")]
    public class G_BoolState : G_State {
        bool value;


        #region Basic Controls

        public override void Construct(string name, object value, bool isLocal) {
            this.name = name;
            this.isLocal = isLocal;
            SetValue(value);
        }

        public override object GetValue() {
            return value;
        }

        public override void SetValue(object value) {
            if (TestValueMatch(value)) {
                this.value = (bool)value;   
            }
        }

        public override G_State Clone() {
            return A.BoolState(name).WithValue(value).IsLocal(isLocal);
        }

        #endregion

        #region Testing Controls
        
        /// <summary>
        /// Returns true if the value entered is a bool
        /// </summary>
        /// <param name="testValue"></param>
        /// <returns></returns>
        public override bool TestValueMatch(object testValue) {
            return testValue is bool;
        }

        /// <summary>
        /// This returns true if the comparison type is either equal or not_equal
        /// </summary>
        /// <param name="comparison"></param>
        /// <returns></returns>
        public override bool StateSupportsComparison(G_StateComparison comparison) {
            return comparison == G_StateComparison.equal
                || comparison == G_StateComparison.not_equal;
        }

        /// <summary>
        /// Test the given state's value against the expected value using the given comparison
        /// and return true if the result matches expectations
        /// </summary>
        /// <param name="state"></param>
        /// <param name="comparison"></param>
        /// <param name="expectedValue"></param>
        /// <returns></returns>
        public override bool TestState(G_State state, G_StateComparison comparison, object expectedValue) {
            bool stateValue = (bool)state.GetValue();
            bool testValue = (bool)expectedValue;
            bool result = false;

            if (comparison == G_StateComparison.equal) {
                result = stateValue == testValue;
            }
            else if (comparison == G_StateComparison.not_equal) {
                result = stateValue != testValue;
            }
            else {
                Debug.LogWarning($"Bool state does not support {comparison} comparisons");
            }

            return result;
        }

        /// <summary>
        /// Return true if both conditions are referring to the same state, have the same comparison, and expected value
        /// </summary>
        /// <param name="precondition"></param>
        /// <param name="effect"></param>
        /// <returns></returns>
        public override bool TestStateConditionMatch(G_Condition precondition, G_Condition effect) {
            bool preValue = (bool)precondition.ExpectedValue;
            bool effectValue = (bool)effect.ExpectedValue;
            bool preCompareValid = StateSupportsComparison(precondition.Comparison);
            bool effectCompareValid = StateSupportsComparison(effect.Comparison);
            bool result = false;

            // avoiding errors due to invalid comparisons
            if (!preCompareValid || !effectCompareValid) {
                Debug.LogWarning($"Invalid comparison found when comparing one or more conditions for bool state");
            }
            else if (CompareConditions(precondition.Comparison, effect.Comparison, preValue, effectValue)) {
                result = true;
            }
            
            return result;
        }

        #endregion

        #region Conditions

        bool CompareConditions(G_StateComparison preconditionCompare,
            G_StateComparison effectCompare,
            bool preValue,
            bool effectValue) {

            bool result = false;

            if (CompareSameCondition(preconditionCompare, effectCompare, preValue, effectValue)
                || CompareOppositeCondition(preconditionCompare, effectCompare, preValue, effectValue)) {
                result = true;
            }
            
            return result;
        }

        bool CompareSameCondition(G_StateComparison preconditionCompare,
            G_StateComparison effectCompare,
            bool preValue,
            bool effectValue) {
            
            return preconditionCompare == effectCompare && preValue == effectValue;
        }

        bool CompareOppositeCondition(G_StateComparison preconditionCompare,
            G_StateComparison effectCompare,
            bool preValue,
            bool effectValue) {
            
            return preconditionCompare != effectCompare && preValue != effectValue;
        }

        #endregion

#if UNITY_EDITOR
        #region Editor

        public override int GetEditorHeight() {
            return 3;
        }

        public override void Editor(G_ConditionEditor propertyDrawer,
            ref float height,
            Rect position,
            SerializedProperty property,
            GUIContent label) {

            position = propertyDrawer.GetFormattedRect(position, property, label);
            EditorGUI.BeginChangeCheck();

            SerializedProperty expectedValue = property.FindPropertyRelative("expectedValue");
            SerializedProperty comparison = property.FindPropertyRelative("comparison");

            if (expectedValue.managedReferenceValue == null
                || !(expectedValue.managedReferenceValue is bool)) {

                expectedValue.managedReferenceValue = null;
                expectedValue.managedReferenceValue = false;
                property.FindPropertyRelative("expectedReference").objectReferenceValue = null;
                property.FindPropertyRelative("useExpectedReference").boolValue = true;
            }

            Rect comparisonRect = new Rect(position.x,
                position.y,
                position.width * 0.75f,
                position.height);

            comparison.enumValueIndex = (int)(G_StateComparison)EditorGUI.EnumPopup(comparisonRect,
                new GUIContent("is"),
                (G_StateComparison)comparison.enumValueIndex,
                (option) => StateSupportsComparison((G_StateComparison)option));

            Rect toggleRect = new Rect(position.x + position.width * 0.75f,
                position.y,
                position.width * 0.25f,
                position.height);

            bool toggleValue = EditorGUI.Toggle(toggleRect, (bool)expectedValue.managedReferenceValue);

            if (toggleValue != (bool)expectedValue.managedReferenceValue) {
                expectedValue.managedReferenceValue = toggleValue;
            }

            propertyDrawer.IncrementHeight(out height, property, label);
;
            if (EditorGUI.EndChangeCheck()) {
                property.serializedObject.ApplyModifiedProperties();
            }
        }

        #endregion
#endif
    }
}
