using System.Collections.Generic;
using UnityEngine;

namespace UtilityAI {
    [System.Serializable]
    public class U_Scorer {
        #region Data

        float currentScore = 0f;
        public float CurrentScore { get { return currentScore; } }
        public List<U_Value> values = new List<U_Value>();

        #endregion

        public float CalculateScore() {
            float total = 0f;
            float valuesUsed = 0f;
            float score = 0f;

            for (int i = 0; i < values.Count; i++) {
                total += values[i].GetUtility();
                valuesUsed += 1;
            }

            if (total == 0f) {
                score = 0f;
            }
            else {
                score = total / valuesUsed;
            }

            currentScore = score;

            return score;
        }

        #region Construction

        public U_Scorer() { }
        public U_Scorer(List<U_Value> values, float currentScore) {
            this.values = values;
            this.currentScore = currentScore;
        }

        public U_Scorer Clone() {
            return new U_Scorer(new List<U_Value>(values), currentScore);
        }

        #endregion

        #region Local Value Handling
        public void AssignLocalValues(List<U_Value> localValues) {
            for (int i = 0; i < values.Count; i++) {
                if (values[i] != null) {
                    TryAssignLocalValues(localValues, values[i]);
                }
            }
        }

        void TryAssignLocalValues(List<U_Value> localValues, U_Value valueToReassign) {
            U_Value localValue = localValues.Find((value) => value != null && value.name == valueToReassign.name);

            if (localValue != null) {
                valueToReassign = localValue;
            }
        }

        #endregion
    }
}
