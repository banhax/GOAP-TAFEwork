using UnityEngine;

namespace GOAP {
    public class G_FloatStateBuilder {
        #region Basic Values
        // any values to be transferred into the build object
        string name = "";
        float value;

        public G_FloatStateBuilder() {

        }
        #endregion

        #region With Functions
        public G_FloatStateBuilder WithName(string name) {
            this.name = name;
            return this;
        }
        public G_FloatStateBuilder WithValue(float value) {
            this.value = value;
            return this;
        }
        #endregion

        #region Object Creation

        public G_FloatState Build() {
            G_FloatState state = ScriptableObject.CreateInstance<G_FloatState>();
            state.Construct(name, value);
            return state;
        }

        public static implicit operator G_FloatState(G_FloatStateBuilder builder) {
            return builder.Build();
        }

        #endregion
    }   
}
