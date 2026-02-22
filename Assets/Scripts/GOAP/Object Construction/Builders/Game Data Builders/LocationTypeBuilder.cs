using UnityEngine;

namespace GOAP {
    public class LocationTypeBuilder {
        #region Basic Values
        // any values to be transferred into the build object
        string name = "";

        public LocationTypeBuilder(string name) {
            this.name = name;
        }
        #endregion

        #region With Functions
        //public LocationTypeBuilder WithName(string name) {
        //    this.name = name;
        //    return this;
        //}
        #endregion

        #region Object Creation

        public LocationType Build() {
            LocationType location = ScriptableObject.CreateInstance<LocationType>();
            location.name = name;
            return location;
        }

        public static implicit operator LocationType(LocationTypeBuilder builder) {
            return builder.Build();
        }

        #endregion
    }
}
