using System.Collections.Generic;
using UnityEngine;
using GOAP;
using UtilityAI;

[CreateAssetMenu(fileName = "New Utility Goal", menuName = "GOAP/Goals/Utility Goal")]
public class G_UtilityGoal : G_Goal {
    [Header("Utility")]
    public bool canInterrupt = false;
    [SerializeField] U_Scorer utilityScorer = new U_Scorer();
    public U_Scorer UtilityScorer { get { return utilityScorer; } }

    public override float GetPriority() {
        priority = utilityScorer.CalculateScore();
        return priority;
    }

    #region Construction and References
    public void Construct(string name,
        List<G_Condition> triggerConditions,
        List<G_Condition> goalEffects,
        float priority,
        bool canInterrupt,
        U_Scorer utilityScorer) {

        Construct(name, triggerConditions, goalEffects, priority);
        this.utilityScorer = utilityScorer;
        this.canInterrupt = canInterrupt;
    }
    
    public void AssignLocalValues(List<U_Value> localValues) {
        utilityScorer.AssignLocalValues(localValues);
    }

    public override G_Goal Clone() {
        G_UtilityGoal clonedGoal = ScriptableObject.CreateInstance<G_UtilityGoal>();
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
            this.priority,
            this.canInterrupt,
            this.utilityScorer.Clone());
        return clonedGoal;
    }
    #endregion
}
