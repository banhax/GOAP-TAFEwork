using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using GOAP;

public class InventoryStateTests
{
    [Test]
    public void InventoryClone() {
        GameObject go = new GameObject();
        Inventory inventory = go.AddComponent<Inventory>();
        Item testItem = An.Item("axe").isStackable(false);
        inventory.AddToInventory(new ItemStack(testItem, 1));
        G_Inventory testState = An.InventoryState("test").WithInventory(inventory);

        G_Inventory clone = testState.Clone() as G_Inventory;

        Assert.AreEqual(testState.name, clone.name);
        Assert.AreEqual(testState.GetValue() as Inventory, clone.GetValue() as Inventory);
    }

    [TestCase(true, true, true, true, 1, true, TestName = "All valid")]
    [TestCase(false, true, true, true, 1, false, TestName = "State Invalid")]
    [TestCase(true, false, true, true, 1, false, TestName = "Expected Value Invalid")]
    [TestCase(true, true, /* invalid item */ false, true, 1, false, TestName = "Item Value Invalid")]
    [TestCase(true, true, true, false, 0, true, TestName = "Item not in inventory and Expected Value 0")]
    public void InventoryTestState(bool stateIsValid, 
        bool expectedIsValid, 
        bool ItemIsValid,
        bool itemInInventory,
        int expectedQuantity,
        bool expectedResult) {

        GameObject go = new GameObject();
        Inventory inventory = go.AddComponent<Inventory>();
        Item testItem = null;
        G_Inventory testState = An.InventoryState("test");

        ItemStack expectedValue = null;

        if (stateIsValid) {
            testState = An.InventoryState("test").WithInventory(inventory);
        }
        if (ItemIsValid) {
            testItem = An.Item("axe").isStackable(false);
        }
        if (itemInInventory) {
            inventory.AddToInventory(new ItemStack(testItem, expectedQuantity));
        }
        if (expectedIsValid) {
            expectedValue = new ItemStack(testItem, expectedQuantity);
        }

        bool result = testState.TestState(testState, G_StateComparison.EqualTo, expectedValue);

        Assert.AreEqual(expectedResult, result);
    }
}
