using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP {

    [CreateAssetMenu(fileName = "G_Goal", menuName = "GOAP/Goals/Base Goal")]
    public class G_Goal : ScriptableObject {
        public int priority = 0;

        public List<G_Condition> triggerConditions = new List<G_Condition>();
        public List<G_Condition> goalEffects = new List<G_Condition>();
        public void Construct(string name,
            List<G_Condition> triggerConditions,
            List<G_Condition> goalEffects,
            int priority = 0) {
            
            this.name = name;
            this.triggerConditions = triggerConditions;
            this.goalEffects = goalEffects;
            this.priority = priority;
        }

        public bool CanStartGoal() {
            return AllConditionsMet(triggerConditions);
        }

        public bool DidGoalSucceed() {
            return AllConditionsMet(goalEffects);
        }

        bool AllConditionsMet(List<G_Condition> conditions) {
            bool success = true;
            for (int i = 0; i < conditions.Count; i++) {
                if (!conditions[i].DoesStateMeetCondition()) {
                    success = false;
                }
            }
            return success;
        }

        public void TransferToLocalWorldStates(List<G_State> localStates) {
            for (int i = 0; i < triggerConditions.Count; i++) {
                triggerConditions[i].TrySwitchToLocalState(localStates);
            }
            for (int i = 0; i < goalEffects.Count; i++) {
                goalEffects[i].TrySwitchToLocalState(localStates);
            }
        }

        public virtual G_Goal Clone() {
            G_Goal clonedGoal = ScriptableObject.CreateInstance<G_Goal>();
            List<G_Condition> clonedTriggers = new List<G_Condition>();
            List<G_Condition> clonedEffects = new List<G_Condition>();

            for (int i = 0; i < this.triggerConditions.Count; i++) {
                clonedTriggers.Add(G_Condition.Clone(triggerConditions[i]));
            }
            for (int i = 0; i < this.goalEffects.Count; i++) {
                clonedEffects.Add(G_Condition.Clone(goalEffects[i]));
            }

            clonedGoal.Construct(this.name,
                clonedTriggers,
                clonedEffects,
                this.priority);
            return clonedGoal;
        }

#if UNITY_EDITOR
        #region Editor

        int triggerCount = 0;
        int effectCount = 0;

        private void OnValidate() { // only runs in the editor
            if (triggerConditions.Count != triggerCount) {
                G_Condition.ValidateReferenceConditions(triggerConditions, out triggerCount);
            }

            if (goalEffects.Count != effectCount) {
                G_Condition.ValidateReferenceConditions(goalEffects, out effectCount);
            }
        }

        #endregion
#endif
    }
}
