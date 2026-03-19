using UnityEngine;

namespace GOAP {
    public static class A {
        public static G_BoolStateBuilder BoolState(string name) {
            return new G_BoolStateBuilder(name);
        }
        
        public static G_FloatStateBuilder FloatState(string name) {
            return new G_FloatStateBuilder(name);
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
        public static G_StateBuilder State(string name) {
            return new G_StateBuilder(name);
        }
    }
}
