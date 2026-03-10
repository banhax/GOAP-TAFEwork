using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP {
    public class G_Node {
        #region Data

        G_NodeState nodeState = G_NodeState.open;
        public G_NodeState NodeState { get { return nodeState; } }

        G_Action nodeAction;
        public G_Action NodeAction { get { return nodeAction; } }

        G_Node parentNode;
        public G_Node ParentNode { get { return parentNode; } }

        int hCost = 0; 
        public int HCost { get { return hCost; } }

        int unmetPreconditions = 0;
        public int UnmetPreconditions { get { return unmetPreconditions; } }

        List<G_Action> nodeActionPool = new List<G_Action>();
        public List<G_Condition> preconditions = new List<G_Condition>();

        G_WorldState worldStateRef;
        public G_WorldState WorldStateRef { get { return worldStateRef; } }

        bool isGoalNode = false;
        public bool IsGoalNode { get { return isGoalNode; } }
        #endregion

        #region Constructors

        /// <summary>
        /// Constructor for Actions
        /// </summary>
        /// <param name="parentNode"></param>
        /// <param name="nodeAction"></param>
        /// <param name="hCost"></param>
        /// <param name="actionPool"></param>
        /// <param name="preconditions"></param>
        /// <param name="worldStateRef"></param>
        /// <param name="processUnmetPreconditions"></param>
        public G_Node(G_Node parentNode,
            G_Action nodeAction,
            int hCost,
            List<G_Action> nodeActionPool,
            List<G_Condition> preconditions,
            G_WorldState worldStateRef,
            bool processUnmetPreconditions = true) {

            // parent node
            this.parentNode = parentNode;

            // action
            this.nodeAction = nodeAction;

            

            // action pool
            this.nodeActionPool = new List<G_Action>(nodeActionPool);
            this.nodeActionPool.Remove(this.nodeAction);

            // preconditions
            this.preconditions = new List<G_Condition>(preconditions);
            // world state reference
            this.worldStateRef = worldStateRef;

            nodeState = G_NodeState.open;

            if (nodeAction != null) {
                // hCost
                this.hCost = hCost + nodeAction.GetCost();
                for (int i = 0; i < nodeAction.preconditions.Count; i++) {
                    this.preconditions.Add(G_Condition.Clone(nodeAction.preconditions[i]));
                }
            }

            if (processUnmetPreconditions) {
                // determine unmet preconditions
                this.unmetPreconditions = ProcessPreconditions(this.preconditions, this.worldStateRef);
            }

        }

        /// <summary>
        /// Constructor for Goals
        /// </summary>
        /// <param name="nodeActionPool"></param>
        /// <param name="preconditions"></param>
        /// <param name="worldStateRef"></param>
        public G_Node(List<G_Action> nodeActionPool,
            List<G_Condition> preconditions,
            G_WorldState worldStateRef) {

                // actionPool
                this.nodeActionPool = new List<G_Action>(nodeActionPool);

                // preconditions
                this.preconditions = new List<G_Condition>(preconditions);

                // world state reference
                this.worldStateRef = worldStateRef;

                nodeState = G_NodeState.open;

                // determine unmet preconditions
                this.unmetPreconditions = ProcessPreconditions(this.preconditions, this.worldStateRef);

                isGoalNode = true;
        }

        #endregion

        #region Functions

        /// <summary>
        /// Take the list of preconditions and test them against the current world state to see if they are met or not.
        /// If a precondition is met by the world state, we will set it to Met and add 1 to the unmetCount
        /// </summary>
        /// <param name="preconditions"></param>
        /// <param name="worldStateRef"></param>
        /// <returns></returns>
        public int ProcessPreconditions(List<G_Condition> preconditions, G_WorldState worldStateRef) {
            int unmetCount = 0;

            for (int i = 0; i < preconditions.Count; i++) {
                if (!preconditions[i].Met) {
                    ProcessPrecondition(preconditions[i], ref unmetCount);
                }
            }

            return unmetCount;
        }

        void ProcessPrecondition(G_Condition precondition, ref int unmetCount) {
            G_State stateRef = worldStateRef.states.Find((state) => precondition.IsStateTheConditionState(state));
            if(stateRef != null && precondition.DoesStateMeetCondition(stateRef)) {
                precondition.Meet();
            }
            if (!precondition.Met) {
                unmetCount += 1;
            }
        }


        /// <summary>
        /// Sets the node state to closed, succes, or fail based on current conditions
        /// </summary>
        /// <param name="nodePool"></param>
        public void ProcessNode() {
            if (unmetPreconditions > 0 && nodeActionPool.Count > 0) {
                nodeState = G_NodeState.closed;
            }
            else if (unmetPreconditions == 0) {
                nodeState = G_NodeState.success;
            }
            else if (unmetPreconditions > 0 && nodeActionPool.Count <= 0) {
                nodeState = G_NodeState.fail;
            }
        }

        public List<G_Node> GenerateChildNodes() {
            List<G_Node> newNodes = new List<G_Node>();

            for (int i = 0; i < nodeActionPool.Count; i++) {
                G_Node newNode = TestActionForNewNode(nodeActionPool[i]);

                if (newNode != null) {
                    newNodes.Add(newNode);
                }
            }

            return newNodes;
        }

        G_Node TestActionForNewNode(G_Action action) {
            G_Node newNode = null;
            List<G_Condition> clonedPreconditions = new List<G_Condition>();

            for (int i = 0; i < preconditions.Count; i++) {
                clonedPreconditions.Add(G_Condition.Clone(preconditions[i]));
            }

            bool someConditionsMet = action.TestEffectsAgainstPreconditions(clonedPreconditions);

            if (someConditionsMet) { // build new node
                newNode = new G_Node(this,
                    action,
                    hCost,
                    nodeActionPool,
                    clonedPreconditions,
                    worldStateRef);
            }

            return newNode;
        }

        public List<G_Action> ReturnPlan() {
            List<G_Action> plan = new List<G_Action>();

            plan = AddToPlan(plan);
            if (plan != null) {
                plan.Reverse();
            }

            return plan;
        }

        List<G_Action> AddToPlan(List<G_Action> plan) {
            plan.Add(nodeAction);
            if (!parentNode.isGoalNode && parentNode != null && nodeAction != null) {
                plan = parentNode.AddToPlan(plan);
            }
            else if (!isGoalNode && nodeAction == null) {
                plan = null;
                Debug.LogWarning($"Node action was null, returning null plan");
            }
                return plan;
        }

        #endregion
    }
}
