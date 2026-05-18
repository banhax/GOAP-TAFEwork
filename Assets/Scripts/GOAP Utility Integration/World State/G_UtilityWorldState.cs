using System.Collections.Generic;
using UnityEngine;
using GOAP;
using UtilityAI;

[CreateAssetMenu(fileName = "New Utility World State", menuName = "GOAP/World States/Utility World State")]
public class G_UtilityWorldState : G_WorldState {
    public List<U_Value> utilityValues = new List<U_Value>();

    public void Construct(List<G_State> states, List<G_Action> actionPool, List<G_Goal> goals, List<U_Value> utilityValues) {
        Construct(states, actionPool, goals);
        this.utilityValues = utilityValues;
    }

    public G_Goal CheckForInterrupts(G_Goal currentGoal) {
        for (int i = 0; i < goals.Count; i++) {
            if (goals[i] != null && goals[i] == currentGoal) {
                currentGoal.GetPriority();
            }
            if (goals[i] != null && goals[i] is G_UtilityGoal utilityGoal && utilityGoal.canInterrupt) {
                utilityGoal.GetPriority();
            }
        }

        float highestPriority = currentGoal.priority;
        G_Goal highestGoal = currentGoal;
        for (int i = 0; i < goals.Count; i++) {
            if (goals[i] != null && goals[i] != currentGoal && goals[i].priority > highestPriority) {
                highestGoal = goals[i];
            }
        }

        return highestGoal;
    }

    public U_Value FindU_Value(U_Value referenceU_Value) {
        return utilityValues.Find((value) => value != null && value.name == referenceU_Value.name);
    }
}
