using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP {
    public class G_WorldStateBuilder {
        #region Basic Values
        // any values to be transferred into the build object
        string name = "";
        List<G_State> states = new List<G_State>();
        List<G_Action> actions = new List<G_Action>();
        List<G_Goal> goals = new List<G_Goal>();

        public G_WorldStateBuilder(string name) {
            this.name = name;
        }
        #endregion

        #region With Functions
        public G_WorldStateBuilder WithState(G_State state) {
            if (states == null) {
                states = new List<G_State>();
            }

            states.Add(state);
            return this;
        }

        public G_WorldStateBuilder WithAction(G_Action action) {
            if (actions == null) {
                actions = new List<G_Action>();
            }

            actions.Add(action);
            return this;
        }

        public G_WorldStateBuilder WithGoal(G_Goal goal) {
            if (goals == null) {
                goals = new List<G_Goal>();
            }

            goals.Add(goal);
            return this;
        }
        #endregion

        #region Object Creation

        public G_WorldState Build() {
            G_WorldState worldState = ScriptableObject.CreateInstance<G_WorldState>();
            worldState.Construct(states, actions, goals);
            return worldState;
        }

        public static implicit operator G_WorldState(G_WorldStateBuilder builder) {
            return builder.Build();
        }

        #endregion
    }   
}
