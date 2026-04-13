using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP {
    [CreateAssetMenu(fileName = "New World State", menuName = "GOAP/World States/Base World State")]
    public class G_WorldState : ScriptableObject {
        public List<G_State> states = new List<G_State>();
        public List<G_Action> actionPool = new List<G_Action>();
        public List<G_Goal> goals = new List<G_Goal>();

        public void Construct(List<G_State> states, List<G_Action> actionPool, List<G_Goal> goals) {
            this.states = states;
            this.actionPool = actionPool;
            this.goals = goals;
        }
        public G_State FindState(G_State referenceState) {
            return states.Find((state) => state != null && state.name == referenceState.name);
        }

        public G_Action FindAction(G_Action referenceAction) {
            return actionPool.Find((action) => action != null && action.name == referenceAction.name);
        }

        public G_Goal FindGoal(G_Goal referenceGoal) {
            return goals.Find((goal) => goal != null && goal.name == referenceGoal.name);
        }
    }
}
