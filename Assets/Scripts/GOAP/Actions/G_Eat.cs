using UnityEngine;
using GOAP;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Eat Action", menuName = "GOAP/Actions/Eat")]
public class G_Eat : G_Action {
    #region Data
    [Header("Eat")]
    public float consumptionTime;
    public ItemStack foodItem;
    public G_FloatState currentHunger;
    // public string animationTrigger = ""; // for triggering eat animation later

    Inventory localInventory;
    float endConsumptionTime;
    #endregion

    #region Behaviour

    internal override void StartActionContents(NPCGOAPHandler NPC) {
        localInventory = NPC.GetInventory();

        ItemStack foodStack = localInventory.FindInInventory(foodItem.item);
        if (foodStack != null) {
            endConsumptionTime = Time.time + consumptionTime;
        }
        else {
            EndAction(false);
        }
    }

    public override void UpdateAction(NPCGOAPHandler NPC) {
        if (Time.time > endConsumptionTime) {
            EndAction(true);
        }
    }

    internal override void EndAction(bool success) {
        localInventory.SubtractFromInventory(foodItem);
        currentHunger.SetValue(100);
        localInventory = null;
        base.EndAction(success);
    }

    #endregion

    #region Construction
    public void Construct(string name,
            List<G_Condition> preconditions,
            List<G_Condition> effects,
            int cost,
            int priority,
            float consumptionTime,
            ItemStack foodItem,
            G_FloatState currentHunger) {

        Construct(name, preconditions, effects, cost, priority);
        this.consumptionTime = consumptionTime;
        this.foodItem = foodItem;
        this.currentHunger = currentHunger;
    }

    public override void TransferToLocalWorldStates(List<G_State> localStates) {
        base.TransferToLocalWorldStates(localStates);
        G_State foundHungerState = localStates.Find((state) => state != null && state.isLocal && state.name == currentHunger.name);

        if (foundHungerState != null) {
            currentHunger = foundHungerState as G_FloatState;
        }
    }

    public override G_Action Clone() {
        G_Eat clonedAction = ScriptableObject.CreateInstance<G_Eat>();
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
            this.priority,
            this.consumptionTime,
            this.foodItem,
            this.currentHunger);
        return clonedAction;
    }
    #endregion
}
