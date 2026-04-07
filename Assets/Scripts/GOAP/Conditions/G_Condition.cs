using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP {
    [System.Serializable]
    public class G_Condition : ISerializationCallbackReceiver
    {
        #region Variables
        // the state being tested by the condition
        [SerializeField]
        G_State state;
        public G_State State {
            get { return state; }
        }

        // the actual comparison for the condition
        [SerializeField]
        G_StateComparison comparison;
        public G_StateComparison Comparison {
            get { return comparison; }
        }

        // the value we will be comparing to the current value in the state
        [SerializeReference]
        object expectedValue;
        public object ExpectedValue {
            get { return expectedValue; }
        }

        // the value we will be comparing to the current value in the state
        [SerializeReference]
        Object expectedReference;
        public Object ExpectedReference {
            get { return expectedReference; }
        }

        [SerializeField]
        bool useExpectedReference = false;
        public bool UseExpectedReference {
            get { return useExpectedReference; }
        }

        // has the condition been meet during planning?
        bool met = false;
        public bool Met {
            get { return met; }
        }

        [SerializeField]
        [HideInInspector]
        string serializedExpectedValue = "";

        #endregion

        public G_Condition(G_State state,
            object expectedValue,
            Object expectedReference,
            bool useExpectedReference,
            G_StateComparison comparison = G_StateComparison.EqualTo,
            bool met = false) {

            this.state = state;
            this.expectedValue = expectedValue;
            this.expectedReference = expectedReference;
            this.useExpectedReference = useExpectedReference;
            this.comparison = comparison;
            this.met = met;
        }

        #region Functions

        /// <summary>
        /// Returns true if this condition matches the effect condition parameter
        /// </summary>
        /// <param name="effect"></param>
        /// <returns></returns>
        public bool CompareConditionToEffect(G_Condition effect) {
            if (IsStateTheConditionState(effect.state)
                && state.TestValueMatch(effect.state.GetValue())) {
                return state.TestStateConditionMatch(this, effect);
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
            if (useExpectedReference) {
                return state.TestState(state, comparison, expectedReference);
            }
            else {
                return state.TestState(state, comparison, expectedValue);
            } 
        }

        /// <summary>
        /// Returns true if the condition defined in this condition instance is actually correct when applied to an external state
        /// </summary>
        /// <param name="stateToTest"></param>
        /// <returns></returns>
        public bool DoesStateMeetCondition(G_State stateToTest) {
            bool success = false;

            if (state.TestValueMatch(stateToTest.GetValue())) {
                if (useExpectedReference) {
                    success = state.TestState(stateToTest, comparison, expectedReference);
                }
                else {
                    success = state.TestState(stateToTest, comparison, expectedValue);
                }
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
            return stateToCompare.name == state.name;
        }

        /// <summary>
        /// Sets variable 'met' to true. Used during planning
        /// </summary>
        public void Meet() {
            met = true;
        }

        public void UnMeet() {
            met = false;
        }

        public void SetState(G_State state) {
            this.state = state;
        }

        public void TrySwitchToLocalState(List<G_State> localStates) {
            if (CanSwitchToLocalState()) {
                G_State stateHolder = localStates.Find((localState) => this.IsStateTheConditionState(localState));

                if (stateHolder != null) {
                    this.SetState(stateHolder);
                }
            }
        }

        public static G_Condition Clone(G_Condition conditionToClone) {
            return A.Condition().State(conditionToClone.state)
            .WithComparison(conditionToClone.comparison)
            .WithExpectedValue(conditionToClone.expectedValue)
            .WithExpectedReference(conditionToClone.expectedReference, conditionToClone.useExpectedReference)
            .IsMet(conditionToClone.met);
        }

        #endregion

        #region Conditions

        bool CanSwitchToLocalState() {
            return state != null && state.isLocal;
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize(){}
        void ISerializationCallbackReceiver.OnAfterDeserialize() {
            if (serializedExpectedValue.Length > 0 && state != null) {
                expectedValue = state.ConvertSerializedStringToValue(serializedExpectedValue);
            }
        }

        #endregion

#if UNITY_EDITOR
        #region Editor
        
        public void ClearExpectedValue() {
            expectedValue = null;
        }

        [SerializeField] bool editorActive = false;

        public static void ValidateReferenceConditions(List<G_Condition> conditions, out int trackerCount) {
            List<object> compareValues = new List<object>();

            for (int i = 0; i < conditions.Count; i++) {
                ValidatePrecondition(conditions[i], compareValues);
            }
            trackerCount = conditions.Count;
        }

        static void ValidatePrecondition(G_Condition condition, List<object> compareValues) {
            if (condition.State != null && condition.State.NeedsEditorValidation()) {
                if(compareValues.Contains(condition.ExpectedValue)) {
                    condition.ClearExpectedValue();
                }
                else {
                    compareValues.Add(condition.ExpectedValue);
                }
            }
        }

        #endregion
#endif
    }
}
