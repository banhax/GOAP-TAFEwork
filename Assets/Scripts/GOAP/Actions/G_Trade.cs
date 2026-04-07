using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP {
    [CreateAssetMenu(fileName = "New Trade Action", menuName = "GOAP/Actions/Trade Action")]
    public class G_Trade : G_Action{
        #region Data
        [Header("Trade")]
        [SerializeField] ItemStack requestedItem = new ItemStack();
        [SerializeField] ItemStack offeredItem = new ItemStack();
        [SerializeField] bool requestFullQuantity = true;
        [SerializeField] float tradeTime = 0.3f;

        [Header("Extract")]
        [SerializeField] bool isExtraction = false;
        // for implementing animations on extractions, like chopping down a tree
        // [SerializeField] string extractionAnimationName = "";

        public void Construct(string name,
            List<G_Condition> preconditions,
            List<G_Condition> effects,
            ItemStack requestedItem,
            ItemStack offeredItem,
            bool requestFullQuantity,
            float tradeTime,
            bool isExtraction,
            int cost = 10,
            int priority = 0) {

            Construct(name, preconditions, effects, cost, priority);
            this.requestedItem = requestedItem;
            this.offeredItem = offeredItem;
            this.requestFullQuantity = requestFullQuantity;
            this.tradeTime = tradeTime;
            this.isExtraction = isExtraction;
        }
        #endregion

        public override G_Action Clone() {
            G_Trade clonedAction = ScriptableObject.CreateInstance<G_Trade>();
            List<G_Condition> clonedPreconditions = new List<G_Condition>();
            List<G_Condition> clonedEffects = new List<G_Condition>();

            for (int i = 0; i < this.preconditions.Count; i++) {
                clonedPreconditions.Add(G_Condition.Clone(preconditions[i]));
            }
            for (int i = 0; i < this.effects.Count; i++) {
                clonedEffects.Add(G_Condition.Clone(effects[i]));
            }

            ItemStack clonedRequestedItem = new ItemStack(requestedItem);
            ItemStack clonedOfferedItem = new ItemStack(offeredItem);

            clonedAction.Construct(this.name,
                clonedPreconditions,
                clonedEffects,
                clonedRequestedItem,
                clonedOfferedItem,
                this.requestFullQuantity,
                this.tradeTime,
                this.isExtraction,
                this.cost,
                this.priority);

            return clonedAction;
        }
    }
}
