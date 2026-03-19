using UnityEngine;

namespace GOAP {
    public static class An {
        public static G_IntStateBuilder IntState(string name) {
            return new G_IntStateBuilder(name);
        }
        public static G_AtLocationBuilder AtLocation(string name) {
            return new G_AtLocationBuilder(name);
        }
        public static ItemBuilder Item(string name) {
            return new ItemBuilder(name);
        }
        public static G_InventoryBuilder InventoryState(string name) {
            return new G_InventoryBuilder(name);
        }
        public static G_ActionBuilder Action(string name) {
            return new G_ActionBuilder(name);
        }
    }
}
