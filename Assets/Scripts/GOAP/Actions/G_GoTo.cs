using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;

namespace GOAP {
    [CreateAssetMenu(fileName = "New Go To Action", menuName = "GOAP/Actions/Go to Action")]
    public class G_GoTo : G_Action {
        #region Data
        [Header("Variables")]
        public LocationType targetLocationType;
        public bool canEndAtDifferentTarget = false;

        [Header("Instance References")]
        //Map localMap;
        LocationInstance target;

        public void Construct(string name,
            List<G_Condition> preconditions,
            List<G_Condition> effects,
            LocationType targetLocationType,
            bool canEndAtDifferentTarget,
            int cost = 10,
            int priority = 0) {

            Construct(name, preconditions, effects, cost, priority);
            this.targetLocationType = targetLocationType;
            this.canEndAtDifferentTarget = canEndAtDifferentTarget;
        }

        public override int GetPriority() {
            return 1;
        }

        #endregion

        #region Action Running

        internal override void StartActionContents(NPCGOAPHandler NPC) {
            //localMap = NPC.GetLocalMap();
            target = NPC.GetLocalMap().FindNearestObjectOfType(NPC.transform.position, targetLocationType);

            if (target == null) { // could not find an instance of the target location
                EndAction(false);
            }
            else {
                NPC.GetPathing().StartPath(target);
            }
        }

        public override void UpdateAction(NPCGOAPHandler NPC) {
            if (AtTarget(NPC.GetPathing())) {
                EndAction(true);
            }
        }

        internal override void EndAction(bool success) {
            target = null;
            base.EndAction(success);
        }

        #endregion

        #region Conditions

        bool AtTarget(NPCPathing pathing) {
            return pathing.IsAtLocation(target)
                || canEndAtDifferentTarget
                && pathing.IsAtLocationOfType(targetLocationType);
        }

        #endregion

        #region Other

        public override G_Action Clone() {
            G_GoTo clonedAction = ScriptableObject.CreateInstance<G_GoTo>();
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
                this.targetLocationType,
                this.canEndAtDifferentTarget,
                this.cost,
                this.priority);

            return clonedAction;
        }

        #endregion
    }

}
