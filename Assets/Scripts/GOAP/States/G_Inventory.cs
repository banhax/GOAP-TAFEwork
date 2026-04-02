using GOAP;
using System;
using UnityEditor;
using UnityEngine;

namespace GOAP {
    [CreateAssetMenu(fileName = "New Inventory State", menuName = "GOAP/States/Inventory State")]
    public class G_Inventory : G_State   {
        // the value we are storing
        Inventory value;

        #region Basic Controls

        public override void Construct(string name, object value, bool isLocal) {
            this.name = name;
            this.isLocal = isLocal;
            SetValue(value);
        }

        public override void SetValue(object value) {
            this.value = value as Inventory;
        }

        public override object GetValue() {
            return value;
        }

        public override G_State Clone() {
            return An.InventoryState(name).WithInventory(value).IsLocal(isLocal);
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
            bool success = false;
            object stateValue = state.GetValue();

            if (CanTestState(stateValue, expectedValue)) {
                success = TestInventoryState(stateValue, comparison, expectedValue);
            }

            return success;
        }

        bool TestInventoryState(object stateValue, G_StateComparison comparison, object expectedValue) {
            bool success = false;
            ItemStack expectedStack = expectedValue as ItemStack;
            Inventory testInventory = stateValue as Inventory;

            ItemStack inventoryStack = testInventory.FindInInventory(expectedStack.item);

            if (inventoryStack != null) {
                success = G_NumberConditionComparer.TestValues(inventoryStack.quantity, comparison, expectedStack.quantity);
            }
            else if (NullStackIsEqualToZero(inventoryStack, comparison, expectedStack)) {
                success = true;
            }

            return success;
        }


        public override bool TestStateConditionMatch(G_Condition preCondition, G_Condition effect) {
            bool success = false;
            ItemStack preExpectedStack = preCondition.ExpectedValue as ItemStack;
            ItemStack effectExpectedStack = effect.ExpectedValue as ItemStack;

            if (CanCompareConditions(preCondition, effect, preExpectedStack, effectExpectedStack)) {

                success = G_NumberConditionComparer.CompareNumberCondition(preExpectedStack.quantity,
                    preCondition.Comparison,
                    effectExpectedStack.quantity,
                    effect.Comparison);
            }
            return success;
        }
         

        public override bool StateSupportsComparison(G_StateComparison comparison) {
            return comparison == G_StateComparison.equal
                || comparison == G_StateComparison.greater
                || comparison == G_StateComparison.lesser
                || comparison == G_StateComparison.greater_or_equal
                || comparison == G_StateComparison.lesser_or_equal;
        }


        public override bool TestValueMatch(object testValue) {
            return testValue != null
                && (testValue is ItemStack || testValue is Inventory);
        }

        #endregion

        #region Conditions

        bool CanTestState(object stateValue, object expectedValue) {
            return TestValueMatch(stateValue) && TestValueMatch(expectedValue)
                && (expectedValue as ItemStack).item != null;
        }

        bool NullStackIsEqualToZero(ItemStack inventoryStack, G_StateComparison comparison, ItemStack expectedStack) {
            return inventoryStack == null
                && comparison == G_StateComparison.equal
                && expectedStack.quantity == 0;
        }

        bool CanCompareConditions(G_Condition preCondition, G_Condition effect, ItemStack preExpectedStack, ItemStack effectExpectedStack) {
            return preCondition.IsStateTheConditionState(effect.State)
                && TestValueMatch(preCondition.State.GetValue())
                && TestValueMatch(effect.State.GetValue())
                && TestValueMatch(preCondition.ExpectedValue)
                && TestValueMatch(effect.ExpectedValue)
                && preExpectedStack.item != null
                && effectExpectedStack.item != null
                && preExpectedStack.item == effectExpectedStack.item;
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
                || !(expectedValue.managedReferenceValue is ItemStack)) {

                expectedValue.managedReferenceValue = null;
                expectedValue.managedReferenceValue = new ItemStack();
                property.FindPropertyRelative("serializedExpectedValue").stringValue = "";
                property.FindPropertyRelative("expectedReference").objectReferenceValue = null;
                property.FindPropertyRelative("useExpectedReference").boolValue = true;
            }

            SerializedProperty item = expectedValue.FindPropertyRelative("item");
            SerializedProperty quantity = expectedValue.FindPropertyRelative("quantity");

            Rect labelRect = new Rect(position.x,
                position.y,
                position.width * 0.15f,
                position.height);

            Rect comparisonRect = new Rect(position.x + position.width * 0.15f,
                position.y,
                position.width * 0.35f,
                position.height);

            Rect intFieldRect = new Rect(position.x + position.width * 0.475f,
                position.y,
                position.width * 0.15f,
                position.height);

            Rect objectFieldRect = new Rect(position.x + position.width * 0.6f,
                position.y,
                position.width * 0.4f,
                position.height);

            EditorGUI.LabelField(labelRect, new GUIContent("has"));

            comparison.enumValueIndex = (int)(G_StateComparison)EditorGUI.EnumPopup(comparisonRect,
                new GUIContent(""),
                (G_StateComparison)comparison.enumValueIndex,
                (option) => StateSupportsComparison((G_StateComparison)option));

            quantity.intValue = EditorGUI.IntField(intFieldRect, quantity.intValue);

            item.objectReferenceValue = EditorGUI.ObjectField(objectFieldRect,
                (Item)item.objectReferenceValue,
                typeof(Item),
                false);

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