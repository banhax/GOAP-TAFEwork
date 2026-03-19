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
            return comparison != G_StateComparison.not_equal;
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
    }
}
