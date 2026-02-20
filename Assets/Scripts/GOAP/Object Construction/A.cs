using UnityEngine;

namespace GOAP {
    public static class A {
        public static G_BoolStateBuilder BoolState() {
            return new G_BoolStateBuilder();
        }
        
        public static G_FloatStateBuilder FloatState() {
            return new G_FloatStateBuilder();
        }
    }
}
