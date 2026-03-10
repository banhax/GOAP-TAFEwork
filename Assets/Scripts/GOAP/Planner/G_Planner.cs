using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP {
    public static class G_Planner {
        public static bool GeneratePlan(G_Goal goal, G_WorldState worldState, out List<G_Action> plan) {
            bool success = false;
            plan = new List<G_Action>();

            List<G_Node> nodePool = new List<G_Node>();

            G_Node rootNode = new G_Node(worldState.actionPool, goal.goalEffects, worldState);

            nodePool.Add(rootNode);

            G_Node currentNode = null;

            while (true) {
                currentNode = nodePool[0];
                currentNode.ProcessNode();

                if (currentNode.NodeState == G_NodeState.success) {
                    success = true;

                    plan = currentNode.ReturnPlan();
                    if (plan == null) {
                        success = false;
                    }
                    break;
                }
                else if (currentNode.NodeState == G_NodeState.fail) {
                    success = false;
                    break;
                }
                else if (currentNode.NodeState == G_NodeState.closed) {
                    nodePool.AddRange(currentNode.GenerateChildNodes());
                    nodePool = SortPool(nodePool);

                    if (nodePool[0].NodeState != G_NodeState.open) {
                        success = false;
                        break;
                    }
                }
            }

            return success;
        }

        public static List<G_Node> SortPool(List<G_Node> pool) {
            return pool.OrderBy((node) => node.NodeState)
                .ThenBy((node) => node.HCost)
                .ToList();
        }
    }
}
