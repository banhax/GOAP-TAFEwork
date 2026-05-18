using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using GOAP;

public class NodeTests {
    // constructors for normal nodes and for goal nodes
    // :}

    // process preconditions test - checking for fulfilled preconditions from the world state
    // :}

    // process node - get the node's planning result
    // :}

    // generate child nodes
    // :}

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

    [TestCase(G_NodeState.closed, TestName = "Closed")]
    [TestCase(G_NodeState.success, TestName = "Success")]
    [TestCase(G_NodeState.fail, TestName = "Failure")]
    public void ProcessNode(G_NodeState targetState) {
        GatherWoodTestData testData = new GatherWoodTestData();

        G_Node goalNode 
            = new G_Node(testData.npcWorldState.actionPool,
            testData.gatherWood.goalEffects,
            testData.npcWorldState);

        G_Node normalNode = null;
        G_AtLocation locationState = null;

        switch(targetState) {
            case G_NodeState.success:
                goalNode.preconditions[0].Meet();
                testData.npcInventoryComponent.AddToInventory(new ItemStack(testData.choppedWood, 10));
                locationState
                    = testData.npcWorldState.states.Find((state) => state.name == testData.atLocation.name) as G_AtLocation;

                locationState.SetValue(testData.woodstock);

                normalNode
                    = new G_Node(goalNode,
                    testData.deliverWood,
                    goalNode.HCost,
                    testData.npcWorldState.actionPool,
                    goalNode.preconditions,
                    testData.npcWorldState);
                break;

            case G_NodeState.closed:
                testData.npcInventoryComponent.AddToInventory(new ItemStack(testData.choppedWood, 10));
                locationState
                    = testData.npcWorldState.states.Find((state) => state.name == testData.atLocation.name) as G_AtLocation;

                locationState.SetValue(testData.woodstock);

                normalNode
                    = new G_Node(goalNode,
                    testData.deliverWood,
                    goalNode.HCost,
                    testData.npcWorldState.actionPool,
                    goalNode.preconditions,
                    testData.npcWorldState);
                break;

            case G_NodeState.fail:
                normalNode
                    = new G_Node(goalNode,
                    testData.deliverWood,
                    goalNode.HCost,
                    new List<G_Action>(),
                    goalNode.preconditions,
                    testData.npcWorldState);
                break;
        }

        normalNode.ProcessNode();

        Assert.AreEqual(targetState, normalNode.NodeState);
    }

    [TestCase(5, TestName = "Generates multiple nodes")]
    [TestCase(2, TestName = "Generates a node")]
    [TestCase(1, TestName = "Fails to generate any nodes")]
    public void GenerateNodes(int endNodeCount) {
        GatherWoodTestData testData = new GatherWoodTestData();

        List<G_Action> actionPool = testData.npcWorldState.actionPool;
        if (endNodeCount == 1) {
            actionPool.Clear();
        }

        G_Node goalNode 
            = new G_Node(actionPool,
                testData.gatherWood.goalEffects,
                testData.npcWorldState);

        List<G_Node> nodePool = new List<G_Node>();
        nodePool.Add(goalNode);

        List<G_Node> tempNodes = goalNode.GenerateChildNodes();

        nodePool.AddRange(tempNodes);

        if (endNodeCount == 5) {
            tempNodes = nodePool[1].GenerateChildNodes();
            nodePool.AddRange(tempNodes);
        }

        Assert.AreEqual(endNodeCount, nodePool.Count);
    }

    [TestCase(false, TestName = "Standard Plan")]
    [TestCase(true, TestName = "Null action in middle")]
    public void ReturnPlan(bool hasNullAction) {
        GatherWoodTestData testData = new GatherWoodTestData();

        G_Node goalNode
            = new G_Node(testData.npcWorldState.actionPool,
                testData.gatherWood.goalEffects,
                testData.npcWorldState);

        G_Node deliverWoodNode
            = new G_Node(goalNode,
                testData.deliverWood,
                goalNode.HCost,
                testData.npcWorldState.actionPool,
                goalNode.preconditions,
                testData.npcWorldState);

        G_Node goToWoodStockNode
            = new G_Node(deliverWoodNode,
                testData.goToWoodStock,
                deliverWoodNode.HCost,
                testData.npcWorldState.actionPool,
                deliverWoodNode.preconditions,
                testData.npcWorldState);

        G_Node chopTreeNode
            = new G_Node(goToWoodStockNode,
                testData.chopTree,
                goToWoodStockNode.HCost,
                testData.npcWorldState.actionPool,
                goToWoodStockNode.preconditions,
                testData.npcWorldState);

        G_Node goToTreeNode
            = new G_Node(chopTreeNode,
                hasNullAction ? null : testData.goToTree,
                chopTreeNode.HCost,
                testData.npcWorldState.actionPool,
                chopTreeNode.preconditions,
                testData.npcWorldState);

        G_Node takeAxeNode
            = new G_Node(goToTreeNode,
                testData.takeAxe,
                goToTreeNode.HCost,
                testData.npcWorldState.actionPool,
                goToTreeNode.preconditions,
                testData.npcWorldState);

        G_Node goToWorkshopNode
            = new G_Node(takeAxeNode,
                testData.goToWorkshop,
                takeAxeNode.HCost,
                testData.npcWorldState.actionPool,
                takeAxeNode.preconditions,
                testData.npcWorldState);

        List<G_Action> plan = goToWorkshopNode.ReturnPlan();

        /*
         * public G_Action deliverWood;
         * public G_Action goToWoodstock;
         * public G_Action chopTree;
         * public G_Action goToTree;
         * public G_Action takeAxe;
         * public G_Action goToWorkshop;
         */

        if (!hasNullAction) {
            Assert.AreEqual(true, plan != null);
            Assert.AreEqual(6, plan.Count);

            Assert.AreEqual("deliverWood", plan[5].name);
            Assert.AreEqual("goToWoodStock", plan[4].name);
            Assert.AreEqual("chopTree", plan[3].name);
            Assert.AreEqual("goToTree", plan[2].name);
            Assert.AreEqual("takeAxe", plan[1].name);
            Assert.AreEqual("goToWorkshop", plan[0].name);
        }
        else {
            Assert.AreEqual(true, plan == null);
        }
    }
}
