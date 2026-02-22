using UnityEngine;

namespace GOAP {
    public static class An {
        public static G_IntStateBuilder IntState() {
            return new G_IntStateBuilder();
        }
        public static G_AtLocationBuilder AtLocation() {
            return new G_AtLocationBuilder();
        }
    }
}
