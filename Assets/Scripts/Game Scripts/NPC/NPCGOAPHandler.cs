using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP {
    public class NPCGOAPHandler : MonoBehaviour
    {
        public G_WorldState worldStateReference;
        [SerializeField] G_WorldState localWorldState;

        [SerializeField] List<G_State> localStates = new List<G_State>();
        [SerializeField] List<G_Action> localActions = new List<G_Action>();
        [SerializeField] List<G_Goal> localGoals = new List<G_Goal>();

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Awake()
        {
            CreateLocalWorldState();
        }

        public G_WorldState GetLocalWorldState() {
            return localWorldState;
        }

        public void CreateLocalWorldState() {
            if (worldStateReference != null) {
                for (int i = 0; i < worldStateReference.states.Count; i++) {
                    TryTransferState(localStates, worldStateReference.states[i]);
                }
                for (int i = 0; i < worldStateReference.actionPool.Count; i++) {
                    TryTransferAction(localActions, localStates, worldStateReference.actionPool[i]);
                }
                for (int i = 0; i < worldStateReference.goals.Count; i++) {
                    TryTransferGoal(localGoals, localStates, worldStateReference.goals[i]);
                }

                localWorldState = ScriptableObject.CreateInstance<G_WorldState>();
                localWorldState.Construct(localStates, localActions, localGoals);
            }
        }

        void TryTransferState(List<G_State> localStatesPool, G_State stateToAdd) {
            if (stateToAdd != null) {
                if (stateToAdd.isLocal) {
                    localStatesPool.Add(stateToAdd.Clone());
                }
                else {
                    localStatesPool.Add(stateToAdd);
                }
            }
        }

        void TryTransferAction(List<G_Action> localActionPool, List<G_State> localStates, G_Action actionToAdd) {
            if (actionToAdd != null) {
                G_Action clonedAction = actionToAdd.Clone();
                clonedAction.TransferToLocalWorldStates(localStates);
                localActionPool.Add(clonedAction);
            }
        }

        void TryTransferGoal(List<G_Goal> localGoalPool, List<G_State> localStates, G_Goal goalToAdd) {
            if (goalToAdd != null) {
                G_Goal clonedGoal = goalToAdd.Clone();
                clonedGoal.TransferToLocalWorldStates(localStates);
                localGoalPool.Add(clonedGoal);
            }
        }
    }
}
