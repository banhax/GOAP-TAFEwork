using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using GOAP;

public class NodeTests {
    // constructors for normal nodes and for goal nodes
    // :}

    // process preconditions test - checking for fulfilled preconditions from the world state

    // process node - get the node's planning result

    // generate child nodes

    // return plan - return the whole plan as a list

    [TestCase(true, 0, 1, 1, TestName = "Goal  Node")]
    [TestCase(false, 10, 3, 3, TestName = "Normal Node")]
    public void Constructor(bool testGoalNode,
            int hCost,
            int unmetCount,
            int preconCount) {
        GatherWoodTestData testData = new GatherWoodTestData();

        G_Node goalNode 
            = new G_Node(testData.npcWorldState.actionPool,
                testData.gatherWood.goalEffects,
                testData.npcWorldState);

        G_Node normalNode
            = new G_Node(goalNode,
                testData.deliverWood,
                goalNode.HCost,
                testData.npcWorldState.actionPool,
                goalNode.preconditions,
                testData.npcWorldState);

        G_Node testNode = testGoalNode ? goalNode : normalNode; 

        if (testGoalNode) {
            testNode = goalNode;
        }
        else {
            testNode = normalNode;
        }

        Assert.NotNull(testNode);
        Assert.AreEqual(G_NodeState.open, testNode.NodeState);
        Assert.AreEqual(testGoalNode, testNode.ParentNode == null);            
        Assert.AreEqual(testGoalNode, testNode.NodeAction == null);
        Assert.AreEqual(hCost, testNode.HCost);
        Assert.AreEqual(unmetCount, testNode.UnmetPreconditions);
        Assert.AreEqual(preconCount, testNode.preconditions.Count);

        Assert.NotNull(testNode.preconditions);
        Assert.AreEqual(testGoalNode, testNode.IsGoalNode);
    }
    
    [TestCase(1, TestName = "0 Preconditions met by worldState")]
    [TestCase(2, TestName = "Some Preconditions met by worldState")]
    [TestCase(3, TestName = "All Preconditions met by worldState")]
    public void ProcessPreconditons(int preconsMet) {
        GatherWoodTestData testData = new GatherWoodTestData();

        G_Node goalNode 
            = new G_Node(testData.npcWorldState.actionPool,
                testData.gatherWood.goalEffects,
                testData.npcWorldState);

        goalNode.preconditions[0].Meet(); // forcing it to be met to simulate correct planning

        G_Node normalNode
            = new G_Node(goalNode,
                testData.deliverWood,
                goalNode.HCost,
                testData.npcWorldState.actionPool,
                goalNode.preconditions,
                testData.npcWorldState,
                false);

        if (preconsMet >= 2) {
            testData.npcInventoryComponent.AddToInventory(new ItemStack(testData.choppedWood, 10));
        }
        if (preconsMet == 3) {
            G_AtLocation locationState
                = testData.npcWorldState.states.Find((state) => state.name == testData.atLocation.name) as G_AtLocation;

            locationState.SetValue(testData.woodstock);
        }

        int unmetPreconCount = normalNode.ProcessPreconditions(normalNode.preconditions, normalNode.WorldStateRef);
        int assertedRemainingPrecons = 3 - preconsMet;
        Assert.AreEqual(assertedRemainingPrecons, unmetPreconCount);
    }

    [TestCase(TestName = "Closed")]
    [TestCase(TestName = "Success")]
    [TestCase(TestName = "Failure")]
    public void ProcessNode() {

    }

    [TestCase(TestName = "Generates several nodes")]
    [TestCase(TestName = "Fails to generate any nodes")]
    public void GenerateNodes() {

    }

    [TestCase(TestName = "Standard Plan")]
    [TestCase(TestName = "Null action in middle")]
    public void ReturnPlan() {

    }
}
