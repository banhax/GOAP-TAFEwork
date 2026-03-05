using UnityEngine;

namespace GOAP {
    public class G_InventoryBuilder {
        #region Basic Values
        // any values to be transferred into the build object
        string name = "";
        Inventory inventory;

        public G_InventoryBuilder(string name) {
            this.name = name;
        }
        #endregion

        #region With Functions
        public G_InventoryBuilder WithName(string name) {
            this.name = name;
            return this;
        }
        public G_InventoryBuilder WithInventory(Inventory inventory) {
            this.inventory = inventory;
            return this;
        }
        #endregion

        #region Object Creation

        public G_Inventory Build() {
            G_Inventory inventoryState = ScriptableObject.CreateInstance<G_Inventory>();
            inventoryState.Construct(name, inventory);
            return inventoryState;
        }

        public static implicit operator G_Inventory(G_InventoryBuilder builder) {
            return builder.Build();
        }

        #endregion
    }
}
