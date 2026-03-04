using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP {
    public class G_ActionBuilder {
        #region Basic Values
        // any values to be transferred into the build object
        string name = "";
        List<G_Condition> preconditions = new List<G_Condition>();
        List<G_Condition> effects = new List<G_Condition>();
        int cost = 10;

        public G_ActionBuilder(string name) {
            this.name = name;
        }
        #endregion

        #region With Functions
        public G_ActionBuilder WithCost(int cost) {
            this.cost = cost;
            return this;
        }

        public G_ActionBuilder WithPrecondition(G_Condition precondition) {
            
            if (preconditions == null) {
                preconditions = new List<G_Condition>();
            }

            preconditions.Add(precondition);
            return this;
        }

        public G_ActionBuilder WithEffect(G_Condition precondition) {
            
            if (effects == null) {
                effects = new List<G_Condition>();
            }

            effects.Add(precondition);
            return this;
        }
        #endregion

        #region Object Creation

        public G_Action Build() {
            G_Action action = ScriptableObject.CreateInstance<G_Action>();
            action.Construct(name, preconditions, effects, cost);
            return action;
        }

        public static implicit operator G_Action(G_ActionBuilder builder) {
            return builder.Build();
        }

        #endregion
    }
}
