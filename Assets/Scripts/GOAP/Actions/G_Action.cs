using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP {

    [CreateAssetMenu(fileName = "G_Action", menuName = "GOAP/Actions/Base Action")]
    public class G_Action : ScriptableObject {
        #region Data
        [SerializeField]
        internal int cost = 10;
        [SerializeField]
        internal int priority = 0;
        public List<G_Condition> preconditions = new List<G_Condition>();
        public List<G_Condition> effects = new List<G_Condition>();

        internal event DelegateTypes.Void_Bool actionEnded;
        public event DelegateTypes.Void_Bool ActionEnded {
            add { actionEnded += value; }
            remove { actionEnded -= value; }
        }

        public void Construct(string name,
            List<G_Condition> preconditions,
            List<G_Condition> effects,
            int cost = 10,
            int priority = 0) {

            this.name = name;
            this.preconditions = preconditions;
            this.effects = effects;
            this.cost = cost;
            this.priority = priority;
        }

        #endregion

        #region Planning Functions

        /// <summary>
        /// Recieves a bunch of unmet preconditions and returns a list of any of them that are met
        /// by this Action's effects
        /// </summary>
        /// <param name="unmetPreconditions"></param>
        /// <returns></returns>
        public bool TestEffectsAgainstPreconditions(List<G_Condition> preconditions) {
            bool someConditionsMet = false;

            for (int i = 0; i < preconditions.Count; i++) {
                if (!preconditions[i].Met && DoesEffectMatch(preconditions[i])) {
                    someConditionsMet = true;
                    preconditions[i].Meet();
                }
            }

            return someConditionsMet;
        }

        /// <summary>
        /// Clones the given condition, sets it as met, adn then adds it to the metConditons list
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="metConditions"></param>
        void MeetCondition(G_Condition condition, List<G_Condition> metConditions) {
            G_Condition clone = G_Condition.Clone(condition);
            clone.Meet();
            metConditions.Add(clone);
        }

        /// <summary>
        /// Returns the first (if any) effect that has the same state as the unmetPrecondition
        /// and which succeeds at Condition Comparison test
        /// </summary>
        /// <param name="unmetPrecondition"></param>
        /// <returns></returns>
        public bool DoesEffectMatch(G_Condition unmetPrecondition) {
            G_Condition relevantEffect = effects.Find(
                (effect) => unmetPrecondition.CompareConditionToEffect(effect)
                );
            return relevantEffect != null;
        }

        public virtual int GetCost() {
            return cost;
        }

        public virtual int GetPriority() {
            return priority;
        }

        public void TransferToLocalWorldStates(List<G_State> localStates) {
            for (int i = 0; i < preconditions.Count; i++) {
                preconditions[i].TrySwitchToLocalState(localStates);
            }
            for (int i = 0; i < effects.Count; i++) {
                effects[i].TrySwitchToLocalState(localStates);
            }
        }

        /// <summary>
        /// Creates a duplicate of this action instance
        /// </summary>
        /// <returns></returns>
        public virtual G_Action Clone() {
            G_Action clonedAction = ScriptableObject.CreateInstance<G_Action>();
            List<G_Condition> clonedPreconditions = new List<G_Condition>();
            List<G_Condition> clonedEffects = new List<G_Condition>();

            for (int i = 0; i < this.preconditions.Count; i++) {
                clonedPreconditions.Add(G_Condition.Clone(preconditions[i]));
            }
            for (int i = 0; i < this.effects.Count; i++) {
                clonedEffects.Add(G_Condition.Clone(effects[i]));
            }

            clonedAction.Construct(this.name,
                clonedPreconditions,
                clonedEffects,
                this.cost,
                this.priority);
            return clonedAction;
        }

        #endregion

        #region Behaviour

        bool CheckPreconditionsAtRunTime() {
            bool ready = true;

            for (int i = 0; i < preconditions.Count; i++) {
                if (!preconditions[i].DoesStateMeetCondition()) {
                    ready = false;
                }
            }

            return ready;
        }


        /// <summary>
        /// Called when the action is first run by the NPC with the NPC's gameObject passed in as parameter
        /// </summary>
        /// <param name="NPC"></param>
        public bool StartAction(NPCGOAPHandler NPC) {
            if (CheckPreconditionsAtRunTime()) {
                StartActionContents(NPC);
                return true;
            }
            else {
                return false;
            }
        }

        internal virtual void StartActionContents(NPCGOAPHandler NPC) {

        }

        /// <summary>
        /// Called during Update every frame by the NPC object with GameObject of the NPC passed in as a parameter
        /// </summary>
        /// <param name="NPC"></param>
        public virtual void UpdateAction(NPCGOAPHandler NPC) {

        }

        /// <summary>
        /// Called internally in the behaviour when the action is finished and will send a callback to the NPC
        /// </summary>
        /// <param name="success"></param>
        internal virtual void EndAction(bool success) {
            actionEnded?.Invoke(success);
        }

        #endregion

#if UNITY_EDITOR
        #region Editor

        int preconCount = 0;
        int effectCount = 0;

        private void OnValidate() { // only runs in the editor
            if (preconditions.Count != preconCount) {
                G_Condition.ValidateReferenceConditions(preconditions, out preconCount);
            }

            if (effects.Count != effectCount) {
                G_Condition.ValidateReferenceConditions(effects, out effectCount);
            }
        }

        #endregion
#endif
    }
}
