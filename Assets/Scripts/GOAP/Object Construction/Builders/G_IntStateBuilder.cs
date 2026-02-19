using UnityEngine;

namespace GOAP {
    public class G_IntStateBuilder {
        #region Basic Values
        // any values to be transferred into the build object
        string name = "";
        int value;

        public G_IntStateBuilder() {

        }
        #endregion

        #region With Functions
        public G_IntStateBuilder WithName(string name) {
            this.name = name;
            return this;
        }
        public G_IntStateBuilder WithValue(int value) {
            this.value = value;
            return this;
        }
        #endregion

        #region Object Creation

        public G_IntState Build() {
            G_IntState state = ScriptableObject.CreateInstance<G_IntState>();
            state.Construct(name, value);
            return state;
        }

        public static implicit operator G_IntState(G_IntStateBuilder builder) {
            return builder.Build();
        }

        #endregion
    }   
}
