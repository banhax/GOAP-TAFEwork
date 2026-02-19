using UnityEngine;

namespace GOAP {
    public class BuilderTemplate {
        #region Basic Values
        // any values to be transferred into the build object
        string name = "";

        public BuilderTemplate() {

        }
        #endregion

        #region With Functions
        public BuilderTemplate WithName(string name) {
            this.name = name;
            return this;
        }
        #endregion

        #region Object Creation

        public object Build() {
            object returnedObject = name;
            return returnedObject;
        }

        // public static implicit operator object(BuilderTemplate builder) {
        //     return builder.Build();
        // }

        #endregion
    }   
}
