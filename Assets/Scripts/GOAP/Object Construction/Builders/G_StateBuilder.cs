using UnityEngine;

namespace GOAP {
    public class G_StateBuilder {
        #region Basic Values
        // any values to be transferred into the build object
        string name = "";
        bool isLocal = false;
        object value = null;

        public G_StateBuilder(string name) {
            this.name = name;
        }
        #endregion

        #region With Functions
        public G_StateBuilder WithValue(object value) {
            this.value = value;
            return this;
        }
        #endregion

        #region Object Creation

        public G_State Build() {
            G_State state = ScriptableObject.CreateInstance<G_State>();
            state.Construct(this.name, this.value, this.isLocal);
            return state;
        }

        public static implicit operator G_State(G_StateBuilder builder) {
            return builder.Build();
        }

        #endregion
    }   
}
