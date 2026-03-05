using UnityEngine;

namespace GOAP {
    public static class A {
        public static G_BoolStateBuilder BoolState() {
            return new G_BoolStateBuilder();
        }
        
        public static G_FloatStateBuilder FloatState() {
            return new G_FloatStateBuilder();
        }

        public static G_ConditionBuilder Condition() {
            return new G_ConditionBuilder();
        }
        public static G_GoalBuilder Goal(string name) {
            return new G_GoalBuilder(name);
        }
        public static LocationTypeBuilder LocationType(string name) {
            return new LocationTypeBuilder(name);
        }
    }
}
