using GOAP;
using UnityEditor;
using UnityEngine;

namespace GOAP {
    [CreateAssetMenu(fileName = "New At Location State", menuName = "GOAP/States/At Location State")]
    public class G_AtLocation : G_State   {
        [SerializeField]
        LocationType value;

        #region Basic Controls

        public override void Construct(string name, object value, bool isLocal) {
            this.name = name;
            this.isLocal = isLocal;
            SetValue(value);
        }

        public override void SetValue(object value) {
            this.value = (LocationType)value;
        }

        public override object GetValue() {
            return value;
        }

        public override G_State Clone() {
            return An.AtLocation(name).WithLocationType(value).IsLocal(isLocal);
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
        public override bool TestState(G_State state, G_StateComparison comparison, object expectedValue) {
            LocationType stateLocation = state.GetValue() as LocationType;
            LocationType expectedLocation = expectedValue as LocationType;
            bool success = false;

            if (StateSupportsComparison(comparison)) {

                success = CompareLocations(stateLocation, expectedLocation);
            }
            return success;
        }

        /// <summary>
        /// This function returns true if the two given conditions match their states, expected values, and comparisons
        /// </summary>
        /// <param name="preCondition"></param>
        /// <param name="effect"></param>
        /// <returns></returns>
        public override bool TestStateConditionMatch(G_Condition preCondition, G_Condition effect) {
            bool success = false;
            LocationType preLocation = preCondition.ExpectedReference as LocationType;
            LocationType effectLocation = effect.ExpectedReference as LocationType;

            if (StateSupportsComparison(preCondition.Comparison)
                && StateSupportsComparison(effect.Comparison)) {

                success = CompareLocations(preLocation, effectLocation);
            }

            return success;
        }
         
        /// <summary>
        /// Returns true if the state type has an implementation for the given comparison type
        /// </summary>
        /// <param name="comparison"></param>
        /// <returns></returns>
        public override bool StateSupportsComparison(G_StateComparison comparison) {
            return comparison == G_StateComparison.equal;
        }

        /// <summary>
        /// Tests if the given value is of the same type as the value stored in this state and returns true if it is
        /// </summary>
        /// <param name="testValue"></param>
        /// <returns></returns>
        public override bool TestValueMatch(object testValue) {
            return testValue is null
                || testValue is LocationType;
        }

        #endregion

        #region Conditions

        bool CompareLocations(LocationType locationOne, LocationType locationTwo) {
            return BothLocationsNull(locationOne, locationTwo)
                || BothLocationsMatch(locationOne, locationTwo);
        }

        bool BothLocationsNull(LocationType locationOne, LocationType locationTwo) {
            return locationOne == null && locationTwo == null;
        }
        
        bool BothLocationsMatch(LocationType locationOne, LocationType locationTwo) {
            return locationOne != null && locationTwo != null
                && locationOne == locationTwo;
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
            if (expectedValue.managedReferenceValue != null) {
                expectedValue.managedReferenceValue = null;
            }

            SerializedProperty expectedReference = property.FindPropertyRelative("expectedReference");
            SerializedProperty comparison = property.FindPropertyRelative("comparison");

            if (expectedReference.objectReferenceValue != null
                && !(expectedReference.objectReferenceValue is LocationType)) {

                expectedReference.objectReferenceValue = null;
                property.FindPropertyRelative("useExpectedReference").boolValue = true;
            }
            if (comparison.enumValueIndex != (int)G_StateComparison.equal) {
                comparison.enumValueIndex = (int)G_StateComparison.equal;
            }

            //Rect objectFieldRect = new Rect(position.x + position.width * 0.75f,
            //    position.y,
            //    position.width * 0.25f,
            //    position.height);

            expectedReference.objectReferenceValue = EditorGUI.ObjectField(position,
                new GUIContent("at"),
                (LocationType)expectedReference.objectReferenceValue,
                typeof(LocationType),
                false);

            //if (editorValue != (LocationType)expectedValue.managedReferenceValue) {
            //    expectedValue.managedReferenceValue = editorValue;
            //}

            propertyDrawer.IncrementHeight(out height, property, label);

            if (EditorGUI.EndChangeCheck()) {
                property.serializedObject.ApplyModifiedProperties();
            }
        }

        #endregion
#endif
    }
}