using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    List<ItemStack> inventory = new List<ItemStack>();
    MapInjector mapInjector = new MapInjector();

    [SerializeField] G_Inventory refInventoryState;
    [SerializeField] G_Inventory inventoryState;

    private void Awake() { // when object is loaded
        mapInjector.FindAndInjectObject(transform.position, this);

        if (refInventoryState != null) {
            AssignWorldState();
        }   
    }

    void AssignWorldState() {
        if (refInventoryState.isLocal) {
            inventoryState = refInventoryState.Clone() as G_Inventory;
        }

        inventoryState.SetValue(this);
    }

    public G_Inventory GetWorldState() {
        return inventoryState;
    }

    /// <summary>
    /// For adding items to the inventory. If it finds the item type in the inventory, it will add to the stack of that item.
    /// If it doesn't, it will start a new stack
    /// </summary>
    /// <param name="stack"></param>
    public void AddToInventory(ItemStack stack) {
        if (stack.item == null) {
            return;
        }
        
        if (stack.item.stackable) {
            StackItem(stack);
        }
        else {
            inventory.Add(new ItemStack(stack));
        }
    }
    void StackItem(ItemStack stack) {
        ItemStack existingStack = FindInInventory(stack.item);

        if (existingStack != null) { // stack item
            existingStack.quantity += stack.quantity;
        }
        else { // add new stack
            inventory.Add(new ItemStack(stack));
        }
    }

    /// <summary>
    /// Returns a stack of the given item in the inventory if it is there, otherwise returns null
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public ItemStack FindInInventory(Item item) {
        ItemStack foundStack = null;

        if (item != null) {
            foundStack = inventory.Find((stack) => stack.item == item);
        }

        return foundStack;
    }
}
