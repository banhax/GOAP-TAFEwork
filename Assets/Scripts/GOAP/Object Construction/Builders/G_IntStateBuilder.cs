using UnityEngine;

namespace GOAP {
    public class G_IntStateBuilder {
        #region Basic Values
        // any values to be transferred into the build object
        string name = "";
        int value;
        bool isLocal = false;

        public G_IntStateBuilder(string name) {
            this.name = name;
        }
        #endregion

        #region With Functions
        // public G_IntStateBuilder WithName(string name) {
        //     this.name = name;
        //     return this;
        // }
        public G_IntStateBuilder WithValue(int value) {
            this.value = value;
            return this;
        }
        public G_IntStateBuilder IsLocal(bool isLocal) {
            this.isLocal = isLocal;
            return this;
        }
        #endregion

        #region Object Creation

        public G_IntState Build() {
            G_IntState state = ScriptableObject.CreateInstance<G_IntState>();
            state.Construct(name, value, isLocal);
            return state;
        }

        public static implicit operator G_IntState(G_IntStateBuilder builder) {
            return builder.Build();
        }

        #endregion
    }   
}
