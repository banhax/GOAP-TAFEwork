using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP {
    [CreateAssetMenu(fileName = "New Trade Action", menuName = "GOAP/Actions/Trade Action")]
    public class G_Trade : G_Action{
        #region Data
        [Header("Trade")]
        [SerializeField] G_Inventory targetInventory;
        [SerializeField] ItemStack requestedItem = new ItemStack();
        [SerializeField] ItemStack offeredItem = new ItemStack();
        [SerializeField] bool requestFullQuantity = true;
        [SerializeField] float tradeTime = 0.3f;
        float tradeEndTime = 0f;
        Inventory targetInventoryRef;
        Inventory localInventory;

        [Header("Extract")]
        [SerializeField] bool isExtraction = false;
        // for implementing animations on extractions, like chopping down a tree
        // [SerializeField] string extractionAnimationName = "";

        public void Construct(string name,
            List<G_Condition> preconditions,
            List<G_Condition> effects,
            G_Inventory targetInventory,
            ItemStack requestedItem,
            ItemStack offeredItem,
            bool requestFullQuantity,
            float tradeTime,
            bool isExtraction,
            int cost = 10,
            int priority = 0) {

            Construct(name, preconditions, effects, cost, priority);
            this.targetInventory = targetInventory;
            this.requestedItem = requestedItem;
            this.offeredItem = offeredItem;
            this.requestFullQuantity = requestFullQuantity;
            this.tradeTime = tradeTime;
            this.isExtraction = isExtraction;
        }
        #endregion

        #region Behaviour

        internal override void StartActionContents(NPCGOAPHandler NPC) {
            targetInventoryRef = NPC.GetLocalWorldState().FindState(targetInventory).GetValue() as Inventory;
            localInventory = NPC.GetInventory();

            bool targetTradeValid = targetInventoryRef.IsTradeValid(requestedItem, offeredItem, requestFullQuantity);
            bool selfTradeValid = true;
            if (localInventory.IsStackValid(offeredItem)) {
                selfTradeValid = localInventory.CanTakeFromInventory(offeredItem, true);
            }

            Debug.Log($"targetTradeValid ({targetTradeValid}) and selfTradeValid ({selfTradeValid})");

            if (targetTradeValid && selfTradeValid) {
                Debug.Log($"Trade was valid");
                tradeEndTime = Time.time + tradeTime;
                // START RELEVANT ANIMATION HERE IF NEEDED
                //if (extractionAnimationName != "") {

                //}
            }
            else {
                Debug.Log($"Trade was NOT valid");
                EndAction(false);
            }
        }

        public override void UpdateAction(NPCGOAPHandler NPC) {
            if (Time.time >= tradeEndTime) {
                Debug.Log($"Start trade");
                ItemStack recievedStack;
                bool success = targetInventoryRef.Trade(requestedItem, offeredItem, requestFullQuantity, out recievedStack);
                
                if (recievedStack != null
                    && (localInventory.IsTrade(requestedItem, offeredItem)
                    || localInventory.IsTake(requestedItem, offeredItem))) {

                    localInventory.AddToInventory(recievedStack);
                }
                Debug.Log($"Was trade successful? {success}");

                //if(localInventory.IsTrade(requestedItem, offeredItem)) {
                //    if (recievedStack.item == requestedItem.item
                //        && (!requestFullQuantity
                //        || requestFullQuantity && recievedStack.quantity == requestedItem.quantity)) {

                //        succeeded = true;
                //    }
                //}
                //else if (localInventory.IsTake(requestedItem, offeredItem)) {
                //    if (recievedStack.item == requestedItem.item
                //        && (!requestFullQuantity
                //        || requestFullQuantity && recievedStack.quantity == requestedItem.quantity)) {

                //        succeeded = true;
                //    }
                //}
                //else if (localInventory.IsGive(requestedItem, offeredItem)) {
                //    succeeded = true;
                //}

                EndAction(success);
            }
        }

        internal override void EndAction(bool success) {
            // trigger end of animation if neccessary
            targetInventoryRef = null;
            localInventory = null;
            base.EndAction(success);
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
                this.targetInventory,
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
