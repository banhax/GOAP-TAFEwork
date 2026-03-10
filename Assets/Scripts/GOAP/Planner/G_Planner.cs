using System.Collections.Generic;
using UnityEngine;

namespace GOAP {
    public static class G_Planner {
        public static bool GeneratePlan(G_Goal goal, G_WorldState worldState, out List<G_Action> plan) {
            bool success = false;
            plan = new List<G_Action>();

            // initialise the node pool
            // create a node for the goal
            // add node to node pool

            //while plan not found
            //  find cheapest node
            //  process node
            //  if node is successful
            //      return plan
            //      break from loop
            //  else if plan failed
            //      return empty plan
            //      break from loop
            //  else
            //      generate child nodes
            //      continue loop

            return success;
        }
    }
}
