using UnityEngine;

[System.Serializable]
public class ItemStack
{
    public Item item;
    public int quantity = 0;

    public ItemStack() {

    }

    public ItemStack(Item item, int quantity) {
        this.item = item;
        this.quantity = quantity;
    }

    /// <summary>
    /// Use this overload to duplicate a stack
    /// </summary>
    /// <param name="stack"></param>
    public ItemStack(ItemStack stack) {
        this.item = stack.item;
        this.quantity = stack.quantity;
    }

    public static ItemStack EmptyStack(Item item) {
        return new ItemStack(item, 0);
    }
}
