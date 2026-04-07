using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;

public class Inventory : MonoBehaviour
{
    //public int capacity = 10;
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


    #region World State
    void AssignWorldState() {
        if (refInventoryState.isLocal) {
            inventoryState = refInventoryState.Clone() as G_Inventory;
        }

        inventoryState.SetValue(this);
    }

    public G_Inventory GetWorldState() {
        return inventoryState;
    }

    #endregion

    #region Inventory Functions

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

    #endregion

    #region Trade

    public ItemStack Trade(ItemStack requestedItem, ItemStack offeredItem, bool requestFullQuantity) {

        ItemStack recievedItem = null;

        if (IsTrade(requestedItem, offeredItem)) { // trade
            recievedItem = TradeItem(requestedItem, offeredItem, requestFullQuantity);
        }
        else if (IsTake(requestedItem, offeredItem)) { // take
            recievedItem = TakeItem(requestedItem, requestFullQuantity);
        }
        else if (IsGive(requestedItem, offeredItem)) { // give
            GiveItem(offeredItem);
        }

        return recievedItem;
    }

    ItemStack TradeItem(ItemStack requestedItem, ItemStack offeredItem, bool requestFullQuantity) {
        ItemStack recievedItem = null;
        recievedItem = TakeItem(requestedItem, requestFullQuantity);
        GiveItem(offeredItem);
        return recievedItem;
    }

    ItemStack TakeItem(ItemStack requestedItem, bool requestFullQuantity) {
        ItemStack foundItem = FindInInventory(requestedItem.item);

        if (CanTakeFromInventory(requestedItem, foundItem, requestFullQuantity)) {
            inventory.Remove(foundItem);
        }
        return foundItem;
    }

    void GiveItem(ItemStack offeredItem) {
        // commented code left for reference to possible expansion of the system
        //if (inventory.Count < capacity
        //        || inventory.Count == capacity && inventory.Exists((stack) => stack.item == offeredItem.item)) {

        //}
        AddToInventory(offeredItem);
    }

    #endregion

    #region Conditions

    bool IsTrade(ItemStack requestedItem, ItemStack offeredItem) {
        return requestedItem != null && offeredItem != null;
    }

    bool IsTake(ItemStack requestedItem, ItemStack offeredItem) {
        return requestedItem != null && offeredItem == null;
    }

    bool IsGive(ItemStack requestedItem, ItemStack offeredItem) {
        return requestedItem == null && offeredItem != null;
    }

    bool CanTakeFromInventory(ItemStack requestedItem, ItemStack foundItem, bool requestFullQuantity) {
        return foundItem != null
                && (!requestFullQuantity
                || InventoryHasQuantity(foundItem, requestedItem, requestFullQuantity));
    }

    bool InventoryHasQuantity(ItemStack foundItem, ItemStack requestedItem, bool requestFullQuantity) {
        return requestFullQuantity && foundItem.quantity >= requestedItem.quantity;
    }

    #endregion
}
