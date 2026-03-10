using GOAP;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.TestTools;

public class PlannerTests {
    [TestCase(TestName = "Standard Expected Plan")]
    //[TestCase(TestName = "Shop Plan")]
    public void LoggerPlan() {
        GatherWoodTestData testData = new GatherWoodTestData();
        testData.AddDataForStandardTest();
        List<G_Action> plan = new List<G_Action>();

        bool success = G_Planner.GeneratePlan(testData.npcWorldState.goals[0],
            testData.npcWorldState,
            out plan);

        Assert.AreEqual(true, plan != null);
        Assert.AreEqual(true, success);
        Assert.AreEqual(6, plan.Count);

        Assert.AreEqual("deliverWood", plan[5].name);
        Assert.AreEqual("goToWoodStock", plan[4].name);
        Assert.AreEqual("chopTree", plan[3].name);
        Assert.AreEqual("goToTree", plan[2].name);
        Assert.AreEqual("takeAxe", plan[1].name);
        Assert.AreEqual("goToWorkshop", plan[0].name);
    }
}
