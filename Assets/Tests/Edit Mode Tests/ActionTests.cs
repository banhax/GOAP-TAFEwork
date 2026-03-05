using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using GOAP;

public class ActionTests
{
    [Test]
    public void ActionClone() {
        SlicedBreadData breadData = new SlicedBreadData();
        G_Action clone = breadData.sliceBread.Clone();

        Assert.AreEqual(true, clone != null);
        Assert.AreEqual(breadData.sliceBread.name, clone.name);
        Assert.AreEqual(breadData.sliceBread.preconditions.Count, clone.preconditions.Count);

        for (int i = 0; i < breadData.sliceBread.preconditions.Count; i++) {
            Assert.AreEqual(breadData.sliceBread.preconditions[i].State, clone.preconditions[i].State);
            Assert.AreEqual(breadData.sliceBread.preconditions[i].Comparison, clone.preconditions[i].Comparison);
            Assert.AreEqual(breadData.sliceBread.preconditions[i].ExpectedValue, clone.preconditions[i].ExpectedValue);
        }

        Assert.AreEqual(breadData.sliceBread.effects.Count, clone.effects.Count);
        for (int i = 0; i < breadData.sliceBread.effects.Count; i++) {
            Assert.AreEqual(breadData.sliceBread.effects[i].State, clone.effects[i].State);
            Assert.AreEqual(breadData.sliceBread.effects[i].Comparison, clone.effects[i].Comparison);
            Assert.AreEqual(breadData.sliceBread.effects[i].ExpectedValue, clone.effects[i].ExpectedValue);
        }

        Assert.AreEqual(breadData.sliceBread.GetCost(), clone.GetCost());
    }

    [Test]
    public void TestEffectsAgainstPreconditions() {
        SlicedBreadData breadData = new SlicedBreadData();

        bool preconditionsMet = breadData.goToKitchen.TestEffectsAgainstPreconditions(breadData.sliceBread.preconditions);

        Assert.AreEqual(true, preconditionsMet);
    }

    public class SlicedBreadData {
        public GameObject go;
        public Inventory inventoryComponent;
        // states
        public G_AtLocation atLocation;
        public G_Inventory inventory;
        public G_BoolState isAble;

        // Actions
        // precondition action
        public G_Action goToKitchen;
        // effect action
        public G_Action sliceBread;

        // location
        public LocationType kitchen = A.LocationType("kitchen");

        // items
        public Item bread = An.Item("bread").isStackable(true);
        public Item breadKnife = An.Item("breadKnife").isStackable(false);
        public Item slicedBread = An.Item("slicedBread").isStackable(true);


        public SlicedBreadData() {
            // inventory data
            go = new GameObject();
            inventoryComponent = go.AddComponent<Inventory>();

            inventoryComponent.AddToInventory(new ItemStack(bread, 1));
            inventoryComponent.AddToInventory(new ItemStack(breadKnife, 1));

            // states setup
            isAble = A.BoolState().WithName("isAble").WithValue(true);
            inventory = An.InventoryState("inventory").WithInventory(inventoryComponent);
            atLocation = An.AtLocation().WithName("atLocation");

            goToKitchen = An.Action("goToKitchen")
               .WithEffect(A.Condition().WithState(atLocation).WithExpectedValue(kitchen));

            sliceBread = An.Action("sliceBread")
                .WithPrecondition(A.Condition()
                .WithState(inventory).WithComparison(G_StateComparison.greater).WithExpectedValue(ItemStack.EmptyStack(breadKnife)))
                .WithPrecondition(A.Condition()
                .WithState(inventory).WithComparison(G_StateComparison.greater).WithExpectedValue(ItemStack.EmptyStack(bread)))
                .WithPrecondition(A.Condition()
                .WithState(atLocation).WithExpectedValue(kitchen))
                .WithPrecondition(A.Condition()
                .WithState(isAble).WithExpectedValue(true))

                .WithEffect(A.Condition()
                .WithState(inventory).WithComparison(G_StateComparison.greater).WithExpectedValue(ItemStack.EmptyStack(slicedBread)));
        }
    }
}
