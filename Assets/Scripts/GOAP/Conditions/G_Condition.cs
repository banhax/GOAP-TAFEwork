using UnityEngine;

namespace GOAP {
    public class G_Condition
    {
        #region Variables
        // the state being tested by the condition
        G_State _state;
        public G_State State {
            get { return _state; }
        }

        // the actual comparison for the condition
        G_StateComparison _comparison;
        public G_StateComparison Comparison {
            get { return _comparison; }
        }

        // the value we will be comparing to the current value in the state
        object _expectedValue;
        public object ExpectedValue {
            get { return _expectedValue; }
        }

        // has the condition been meet during planning?
        bool _met = false;
        public bool Met {
            get { return _met; }
        }
        #endregion

        public G_Condition(G_State state,
            object expectedValue,
            G_StateComparison comparison = G_StateComparison.equal,
            bool met = false) {

            this._state = state;
            this._expectedValue = expectedValue;
            this._comparison = comparison;
            this._met = met;
        }

        #region Functions

        /// <summary>
        /// Returns true if this condition matches the effect condition parameter
        /// </summary>
        /// <param name="effect"></param>
        /// <returns></returns>
        public bool CompareConditionToEffect(G_Condition effect) {
            if (_state.TestValueMatch(effect._state.GetValue())) {
                return _state.TestStateConditionMatch(this, effect);
            }
            else {
                return false;
            }
        }

        /// <summary>
        /// Returns true if the condition defined in this condition instance is actually correct
        /// </summary>
        /// <returns></returns>
        public bool DoesStateMeetCondition() {
            return _state.TestState(_state, _comparison, _expectedValue);
        }

        /// <summary>
        /// Returns true if the condition defined in this condition instance is actually correct when applied to an external state
        /// </summary>
        /// <param name="stateToTest"></param>
        /// <returns></returns>
        public bool DoesStateMeetCondition(G_State stateToTest) {
            bool success = false;

            if (_state.TestValueMatch(stateToTest.GetValue())) {
                success = _state.TestState(stateToTest, _comparison, _expectedValue);
            }
            else {
                Debug.LogWarning("Value type of state to test did not match internal state");
            }

            return success;
        }

        /// <summary>
        /// Returns true if the state to compare has the same name as the state in this condition
        /// </summary>
        /// <param name="stateToCompare"></param>
        /// <returns></returns>
        public bool IsStateTheConditionState(G_State stateToCompare) {
            return stateToCompare.name == _state.name;
        }

        /// <summary>
        /// Sets variable 'met' to true. Used during planning
        /// </summary>
        public void Meet() {
            _met = true;
        }

        public static G_Condition Clone(G_Condition conditionToClone) {
            return new G_Condition(conditionToClone._state,
                conditionToClone._expectedValue,
                conditionToClone._comparison,
                conditionToClone._met);
        }

        #endregion
    }
}
