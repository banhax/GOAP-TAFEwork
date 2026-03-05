using UnityEngine;

namespace GOAP {
    public class G_BoolStateBuilder {
        #region Basic Values
        // any values to be transferred into the build object
        string name = "";
        bool value = false;

        public G_BoolStateBuilder() {

        }
        #endregion

        #region With Functions
        public G_BoolStateBuilder WithName(string name) {
            this.name = name;
            return this;
        }

        public G_BoolStateBuilder WithValue(bool value) {
            this.value = value;
            return this;
        }
        #endregion

        #region Object Creation

        public G_BoolState Build() {
            G_BoolState state = ScriptableObject.CreateInstance<G_BoolState>();
            state.Construct(this.name, this.value);
            return state;
        }

        public static implicit operator G_BoolState(G_BoolStateBuilder builder) {
            return builder.Build();
        }

        #endregion
    }
}