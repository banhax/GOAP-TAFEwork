using UnityEngine;

namespace GOAP {
    public class G_AtLocationBuilder {
        #region Basic Values
        // any values to be transferred into the build object
        string name = "";
        bool isLocal = false;
        LocationType value;

        public G_AtLocationBuilder(string name) {
            this.name = name;
        }
        #endregion

        #region With Functions
        // public G_AtLocationBuilder WithName(string name) {
        //     this.name = name;
        //     return this;
        // }
        public G_AtLocationBuilder WithLocationType(LocationType value) {
            this.value = value;
            return this;
        }
        public G_AtLocationBuilder IsLocal(bool isLocal) {
            this.isLocal = isLocal;
            return this;
        }
        #endregion

        #region Object Creation

        public G_AtLocation Build() {
            G_AtLocation state = ScriptableObject.CreateInstance<G_AtLocation>();
            state.Construct(name, value, isLocal);
            return state;
        }

        public static implicit operator G_AtLocation(G_AtLocationBuilder builder) {
            return builder.Build();
        }

        #endregion
    }
}
