using System.Collections.Generic;
using UnityEngine;
using GOAP;

public class InventoryDisplayer : MonoBehaviour {

    public List<Item> observedItems = new List<Item>();
    G_Inventory observedState;

    void Start() {
        Inventory inventory = GetComponent<Inventory>();
        if (inventory != null) {
            G_Inventory tempState = inventory.GetWorldState();

            if (tempState != null) {
                observedState = tempState;
                observedState.ValueChanged += RecieveUpdate;
            }
        }
    }

    private void OnDestroy() {
        if (observedState != null) {
            observedState.ValueChanged -= RecieveUpdate;
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
    }
}
