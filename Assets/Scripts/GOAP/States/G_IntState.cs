using UnityEditor;
using UnityEngine;

namespace GOAP {
    [CreateAssetMenu(fileName = "New Int State", menuName = "GOAP/States/Int State")]
    public class G_IntState : G_State {
        int value;
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
                this.value = (int)value;   
            }
        }

        public override G_State Clone() {
            return An.IntState(name).WithValue(value).IsLocal(isLocal);
        }

        public override object ConvertSerializedStringToValue(string serializedString) {
            char type = serializedString[0];
            if (type == 'i') {
                return int.Parse(serializedString.Substring(1));
            }
            else {
                return null;
            }
        }

        #endregion

        #region Testing Controls

        /// <summary>
        /// Returns true if the value entered is a bool
        /// </summary>
        /// <param name="testValue"></param>
        /// <returns></returns>
        public override bool TestValueMatch(object testValue) {
            return testValue is int;
        }

        /// <summary>
        /// This returns true if the comparison type is either equal or not_equal
        /// </summary>
        /// <param name="comparison"></param>
        /// <returns></returns>
        public override bool StateSupportsComparison(G_StateComparison comparison) {
            return comparison != G_StateComparison.NotEqualTo;
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
            bool result = false;
            result = G_NumberConditionComparer.TestValues((int)state.GetValue(), comparison, (int)expectedValue);
            return result;
        }

        /// <summary>
        /// Return true if both conditions are referring to the same state, have the same comparison, and expected value
        /// </summary>
        /// <param name="precondition"></param>
        /// <param name="effect"></param>
        /// <returns></returns>
        public override bool TestStateConditionMatch(G_Condition precondition, G_Condition effect) {
            bool result = false;

            result = G_NumberConditionComparer.CompareNumberCondition((int)precondition.ExpectedValue,
                precondition.Comparison,
                (int)effect.ExpectedValue,
                effect.Comparison);

            return result;
        }

        #endregion

        #region Conditions

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
                || !(expectedValue.managedReferenceValue is int)) {

                expectedValue.managedReferenceValue = null;
                expectedValue.managedReferenceValue = 0;
                property.FindPropertyRelative("serializedExpectedValue").stringValue = "b" + 0.ToString();
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

            Rect intFieldRect = new Rect(position.x + position.width * 0.75f,
                position.y,
                position.width * 0.25f,
                position.height);
            int editorValue = EditorGUI.IntField(intFieldRect, (int)expectedValue.managedReferenceValue);

            if (editorValue != (int)expectedValue.managedReferenceValue) {
                expectedValue.managedReferenceValue = editorValue;
                property.FindPropertyRelative("serializedExpectedValue").stringValue = "i" + editorValue.ToString();
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
