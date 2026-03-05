using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP {
    public class G_GoalBuilder {
        #region Basic Values
        // any values to be transferred into the build object
        string name = "";
        int priority;
        List<G_Condition> triggers = new List<G_Condition>();
        List<G_Condition> effects = new List<G_Condition>();

        public G_GoalBuilder(string name) {
            this.name = name;
        }
        #endregion

        #region With Functions
        public G_GoalBuilder WithPriority(int priority) {
            this.priority = priority;
            return this;
        }
        public G_GoalBuilder WithTrigger(G_Condition trigger) {
            if (triggers == null) {
                triggers = new List<G_Condition>();
            }

            triggers.Add(trigger);
            return this;
        }
        public G_GoalBuilder WithEffect(G_Condition effect) {
            if (effects == null) {
                effects = new List<G_Condition>();
            }
            
            effects.Add(effect);
            return this;
        }
        #endregion

        #region Object Creation

        public G_Goal Build() {
            G_Goal goal = ScriptableObject.CreateInstance<G_Goal>();
            goal.Construct(name, triggers, effects, priority);
            return goal;
        }

        public static implicit operator G_Goal(G_GoalBuilder builder) {
            return builder.Build();
        }

        #endregion
    }   
}
