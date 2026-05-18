using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UtilityAI;

namespace GOAP {
    public class NPCGOAPHandler : MonoBehaviour
    {
        [Header("World State")]
        public G_WorldState worldStateReference;
        [SerializeField] G_WorldState localWorldState;

        [SerializeField] List<G_State> localStates = new List<G_State>();
        [SerializeField] List<G_Action> localActions = new List<G_Action>();
        [SerializeField] List<G_Goal> localGoals = new List<G_Goal>();
        [SerializeField] List<U_Value> localU_Values = new List<U_Value>();

        [Header("References")]
        MapInjector mapInjector = new MapInjector();
        Map map;
        NPCPathing pathing;
        Inventory inventory;

        [Header("Action Running")]
        [SerializeField]
        G_Action currentAction;
        [SerializeField]
        List<G_Action> currentPlan = new List<G_Action>();
        bool readyForNextAction = true; // flag once an action has ended so that we can go to the next one

        [Header("Goal Selection")]
        [SerializeField] G_Goal currentGoal;
        [SerializeField] bool includeInterrupts = false;

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

            Inventory inventory = GetComponent<Inventory>();
            if (inventory != null) {
                this.inventory = inventory;
            }

            for (int i = 0; i < localWorldState.states.Count; i++) {
                if (localWorldState.states[i] != null
                    && localWorldState.states[i] is G_Inventory inventoryState) {

                    AssignInventoryState(inventoryState);
                }
            }

            NPCStatManager stats = GetComponent<NPCStatManager>();
            if (stats != null && localWorldState is G_UtilityWorldState utilityWorldState) {
                stats.InjectLocalWorldState(utilityWorldState);
            }

            if (isTest) {
                StartTest();
            }
        }
        private void Update() {
            UpdateUtilities();
            if (currentGoal == null || currentGoal != null && currentPlan.Count == 0) {
                SelectGoal(true);
            }
            if (currentAction != null) {
                UpdateCurrentAction();
            }
            else if (readyForNextAction && currentPlan.Count > 0 && currentPlan[0] != null) {
                StartAction(currentPlan[0]);
            }
        }

        void AssignInventoryState(G_Inventory inventoryState) {
            if (inventoryState.name == inventory.GetWorldState().name) {
                inventoryState.SetValue(inventory);
            }
            else {
                Inventory foundInventory = map.FindNearestObjectOfType(transform.position, inventoryState);
                if (foundInventory != null) {
                    inventoryState.SetValue(foundInventory);
                }
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
                if (worldStateReference is G_UtilityWorldState utilityWorldState) {
                    TryTransferU_Values(utilityWorldState);
                    ProcessGoals();
                    localWorldState = ScriptableObject.CreateInstance<G_UtilityWorldState>();
                    ((G_UtilityWorldState)localWorldState).Construct(localStates, localActions, localGoals, localU_Values);
                }
                else {
                    ProcessGoals();
                    localWorldState = ScriptableObject.CreateInstance<G_WorldState>();
                    localWorldState.Construct(localStates, localActions, localGoals);
                }
            }
        }

        void ProcessGoals() {
            for (int i = 0; i < worldStateReference.goals.Count; i++) {
                if (worldStateReference.goals[i] is G_UtilityGoal utilityGoal) {
                    TryTransferGoal(localGoals, localStates, localU_Values, utilityGoal);
                }
                else {
                    TryTransferGoal(localGoals, localStates, worldStateReference.goals[i]);
                }
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

        void TryTransferGoal(List<G_Goal> localGoalPool, List<G_State> localStates, List<U_Value> localU_Values, G_UtilityGoal goalToAdd) {
            if (goalToAdd != null) {
                G_UtilityGoal clonedGoal = goalToAdd.Clone() as G_UtilityGoal;
                clonedGoal.TransferToLocalWorldStates(localStates);
                clonedGoal.AssignLocalValues(localU_Values);
                localGoalPool.Add(clonedGoal);
            }
        }

        void TryTransferU_Values(G_UtilityWorldState worldRef) {
            for (int i = 0; i < worldRef.utilityValues.Count; i++) {
                localU_Values.Add(worldRef.utilityValues[i].Clone());
            }
            for (int i = 0; i < worldRef.utilityValues.Count; i++) {
                if (localU_Values[i] != null) {
                    localU_Values[i].AssignLocalValues(localU_Values, localStates);
                }
            }
        }

        #endregion

        #region Test Running

        void StartTest() {
            if (isPlanTest) {
                for (int i = 0; i < testPlan.Count; i++) {
                    currentPlan.Add(AddTestActionToPool(testPlan[i]));
                }
                StartAction(testPlan[0]);
            }
            else {
                StartAction(AddTestActionToPool(testAction));
            }
        }

        G_Action AddTestActionToPool(G_Action testAction) {
            G_Action testActionDupe = localWorldState.FindAction(testAction);

            if (testActionDupe == null) {
                TryTransferAction(localActions, localStates, testAction);
            }

            return localWorldState.FindAction(testAction);
        }

        #endregion

        #region Action Running

        void SelectGoal(bool recalculatePriority) {
            Debug.Log($"plan goal");
            List<G_Action> tempPlan = new List<G_Action>();

            if (recalculatePriority) {
                for (int i = 0; i < localWorldState.goals.Count; i++) {
                    localWorldState.goals[i].GetPriority();
                }
            }
            localWorldState.OrderGoalsByPriority();

            for (int i = 0; i < localWorldState.goals.Count; i++) {
                if (G_Planner.GeneratePlan(localWorldState.goals[i], localWorldState, out tempPlan)) {
                    Debug.Log($"planned goal {localWorldState.goals[i]} successfully");
                    currentGoal = localWorldState.goals[i];
                    currentPlan = tempPlan;
                    break;
                }
                else {
                    Debug.Log($"Failed to plan for {localWorldState.goals[i]}");
                }
            }
        }

        void StartAction(G_Action action) {
            currentAction = action;
            currentAction.ActionEnded += HandleEndOfAction;
            readyForNextAction = false;
            bool started = currentAction.StartAction(this);

            if (!started) {
                print($"Plan failed at action {currentAction.name}");
                AttemptReplan();
            }
        }

        void AttemptReplan() {
            ClearCurrentAction();
            readyForNextAction = true;
            List<G_Action> tempPlan = new List<G_Action>();
            if (G_Planner.GeneratePlan(currentGoal, localWorldState, out tempPlan)) {
                Debug.Log("replanned");
                currentPlan = tempPlan;
            }
            else {
                currentGoal = null;
            }
        }

        void UpdateCurrentAction() {
            currentAction?.UpdateAction(this);
        }

        void HandleEndOfAction(bool success) {
            
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

            if (currentPlan.Count == 0 && currentGoal != null) {
                print("plan finished");
                bool goalAchieved = currentGoal.DidGoalSucceed();
                print($"Did goal succeed? {goalAchieved}");
                currentGoal = null;
            }
            ClearCurrentAction();
        }

        void ClearCurrentAction() {
            currentAction.ActionEnded -= HandleEndOfAction;
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

        public Inventory GetInventory() {
            return inventory;
        }

        #endregion

        #region Utilities

        void UpdateUtilities() {
            for (int i = 0; i < localU_Values.Count; i++) {
                if (localU_Values[i] != null) {
                    localU_Values[i].GetUtility();
                }
            }
            if (includeInterrupts) {
                G_Goal possibleInterrupt = (localWorldState as G_UtilityWorldState).CheckForInterrupts(currentGoal);

                if (possibleInterrupt != currentGoal) {
                    SelectGoal(false);
                }
            }
        }

        #endregion
    }
}
