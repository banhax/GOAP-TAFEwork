using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP {

    [CreateAssetMenu(fileName = "G_Action", menuName = "Scriptable Objects/G_Action")]
    public class G_Action : ScriptableObject {
        #region Values

        internal int cost = 10;
        public List<G_Condition> preconditions = new List<G_Condition>();
        public List<G_Condition> effects = new List<G_Condition>();
        public void Construct(string name,
            List<G_Condition> preconditions,
            List<G_Condition> effects,
            int cost = 10) {

            this.name = name;
            this.preconditions = preconditions;
            this.effects = effects;
            this.cost = cost;
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
            G_Condition releveantEffect = effects.Find(
                (effect) => unmetPrecondition.CompareConditionToEffect(effect)
                );
            return releveantEffect != null;
        }

        public virtual int GetCost() {
            return cost;
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
                this.cost);
            return clonedAction;
        }

        #endregion

        #region Behaviour

        /// <summary>
        /// Called when the action is first run by the NPC with the NPC's gameObject passed in as parameter
        /// </summary>
        /// <param name="npcObject"></param>
        public virtual void StartAction(GameObject npcObject) {

        }

        /// <summary>
        /// Called during Update every frame by the NPC object with GameObject of the NPC passed in as a parameter
        /// </summary>
        /// <param name="npcObject"></param>
        public virtual void UpdateAction(GameObject npcObject) {

        }

        /// <summary>
        /// Called internally in the behaviour when the action is finished and will send a callback to the NPC
        /// </summary>
        /// <param name="success"></param>
        internal virtual void EndAction(bool success) {

        }

        #endregion
    }
}
