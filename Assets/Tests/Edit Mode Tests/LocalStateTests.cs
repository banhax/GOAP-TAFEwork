using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using GOAP;

public class LocalStateTests
{
    // A Test behaves as an ordinary method
    [Test]
    public void LocalCloneTransfer()
    {
        GameObject go = new GameObject();

        G_BoolState boolState = A.BoolState("boolState").WithValue(true).IsLocal(true);
        G_IntState intState = An.IntState("intState").WithValue(5).IsLocal(true);
        G_FloatState floatState = A.FloatState("floatState").WithValue(7f).IsLocal(true);

        LocationType testLocation = A.LocationType("testLocation");
        G_AtLocation atLocationState = An.AtLocation("atLocationState").WithLocationType(testLocation).IsLocal(true);

        Inventory inventory = go.AddComponent<Inventory>();
        G_Inventory inventoryState = An.InventoryState("inventoryState").WithInventory(inventory).IsLocal(true);

        G_WorldState refWorldState = A.WorldState("worldState")
            .WithState(boolState)
            .WithState(intState)
            .WithState(floatState)
            .WithState(atLocationState)
            .WithState(inventoryState);

        NPCGOAPHandler component = go.AddComponent<NPCGOAPHandler>();

        component.worldStateReference = refWorldState;
        component.CreateLocalWorldState();
        G_WorldState localWorldState = component.GetLocalWorldState();

        Assert.AreEqual(5, localWorldState.states.Count);

        for (int i = 0; i < localWorldState.states.Count; i++) {

            Assert.IsNotNull(localWorldState.states[i]);

            if (localWorldState.states[i].name == boolState.name) {
                //Assert.AreEqual(boolState.name, localWorldState.states[i].name);
                Assert.AreEqual((bool)boolState.GetValue(), (bool)localWorldState.states[i].GetValue());
                Assert.AreEqual(boolState.isLocal, localWorldState.states[i].isLocal);
            }
            else if (localWorldState.states[i].name == intState.name) {
                Assert.AreEqual((int)intState.GetValue(), (int)localWorldState.states[i].GetValue());
                Assert.AreEqual(intState.isLocal, localWorldState.states[i].isLocal);
            }
            else if (localWorldState.states[i].name == floatState.name) {
                Assert.AreEqual((float)floatState.GetValue(), (float)localWorldState.states[i].GetValue());
                Assert.AreEqual(floatState.isLocal, localWorldState.states[i].isLocal);
            }
            else if (localWorldState.states[i].name == atLocationState.name) {
                Assert.AreEqual((LocationType)atLocationState.GetValue(), (LocationType)localWorldState.states[i].GetValue());
                Assert.AreEqual(atLocationState.isLocal, localWorldState.states[i].isLocal);
            }
            else if (localWorldState.states[i].name == inventoryState.name) {
                Assert.AreEqual((Inventory)inventoryState.GetValue(), (Inventory)localWorldState.states[i].GetValue());
                Assert.AreEqual(inventoryState.isLocal, localWorldState.states[i].isLocal);
            }
            else {
                Debug.Log("State name failed to transfer");
            }
        }
    }
}
