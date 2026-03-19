using UnityEngine;

namespace GOAP {
    public class BuilderTemplate {
        #region Basic Values
        // any values to be transferred into the build object
        string name = "";
        //bool isLocal = false;
        object value = null;

        public BuilderTemplate(string name) {
            this.name = name;
        }
        #endregion

        #region With Functions
        public BuilderTemplate WithValue(object value) {
            this.value = value;
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
