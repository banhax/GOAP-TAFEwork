using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP {
    public class NPCGOAPHandler : MonoBehaviour
    {
        [Header("World State")]
        public G_WorldState worldStateReference;
        [SerializeField] G_WorldState localWorldState;

        [SerializeField] List<G_State> localStates = new List<G_State>();
        [SerializeField] List<G_Action> localActions = new List<G_Action>();
        [SerializeField] List<G_Goal> localGoals = new List<G_Goal>();

        [Header("References")]
        MapInjector mapInjector = new MapInjector();
        Map map;
        NPCPathing pathing;

        [Header("Action Running")]
        G_Action currentAction;
        List<G_Action> currentPlan = new List<G_Action>();
        bool readyForNextAction = true; // flag once an action has ended so that we can go to the next one

        [Header("Testing")]
        public bool isTest = false;
        public G_Action testAction;
        public List<G_Action> testPlan;
        public bool isPlanTest = false; // for running a test for a whole plan

        private void Awake() {
            map = mapInjector.FindAndInjectObject(transform.position, this);
        }

        void Start() {
            CreateLocalWorldState();
            NPCPathing pathing = GetComponent<NPCPathing>();
            if (pathing != null) {
                pathing.Init(this);
                this.pathing = pathing;
            }

            if (isTest) {
                StartTest();
            }
        }

        #region World State Setup

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

        #endregion

        #region Test Running

        void StartTest() {
            if (isPlanTest) {
                currentPlan = testPlan;
                StartAction(testPlan[0]);
            }
            else {
                StartAction(testAction);
            }
        }

        #endregion

        #region Action Running

        private void Update() {
            if (currentAction != null) {
                UpdateCurrentAction();
            }
            else if (readyForNextAction && currentPlan.Count > 0) {
                StartAction(currentPlan[0]);
            }
        }

        void StartAction(G_Action action) {
            currentAction = action;
            currentAction.ActionEnded += HandleEndOfAction;
            readyForNextAction = false;
            bool started = currentAction.StartAction(this);

            if (!started) {
                print($"Plan failed at action {currentAction.name}");
            }
        }

        void UpdateCurrentAction() {
            currentAction?.UpdateAction(this);
        }

        void HandleEndOfAction(bool success) {
            currentAction.ActionEnded -= HandleEndOfAction;
            if (currentPlan.Contains(currentAction)) {
                currentPlan.Remove(currentAction);
            }

            if (success) {
                readyForNextAction = true;
                print($"Action {currentAction.name} ended successfully");
            }
            else {
                print($"Action {currentAction.name} failed to succeed");
                // could attempt a replan, or just try a different goal
            }

            currentAction = null;
        }

        #endregion

        #region Retrieval Functions

        public Map GetLocalMap() {
            return map;
        }

        public NPCPathing GetPathing() {
            return pathing;
        }

        #endregion
    }
}
