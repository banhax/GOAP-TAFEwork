using System.Collections.Generic;
using UnityEngine;
using GOAP;

public class InventoryDisplayer : MonoBehaviour {

    public List<Item> observedItems = new List<Item>();
    public ValueTracker trackerReference;
    Inventory inventory;

    void Start() {
        inventory = GetComponent<Inventory>();
        if (inventory != null) {
            inventory.InventoryUpdated += RecieveUpdate;
            RecieveUpdate(inventory);
        }
    }

    private void OnDestroy() {
        if (inventory != null) {
            inventory.InventoryUpdated -= RecieveUpdate;
        }
    }

    void RecieveUpdate(object value) {
        Inventory inventory = value as Inventory;
        List<string> displayStrings = new List<string>();

        for (int i = 0; i < observedItems.Count; i++) {
            string displayText = observedItems[i].name;
            ItemStack stack = inventory.FindInInventory(observedItems[i]);

            if (stack != null) {
                displayText += $" {stack.quantity}";
            }
            else {
                displayText += $" {0}";
            }

            displayStrings.Add(displayText);
        }

        if (trackerReference != null) {
            trackerReference.Track(displayStrings);
        }
    }
}
