using UnityEngine;

namespace GOAP {
    public static class A {
        public static G_BoolStateBuilder G_BoolState() {
            return new G_BoolStateBuilder();
        }
        
        public static G_FloatStateBuilder G_FloatState() {
            return new G_FloatStateBuilder();
        }
    }
}
