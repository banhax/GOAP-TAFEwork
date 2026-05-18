using GOAP;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.TestTools;

public class PlannerTests {
    [TestCase(false, TestName = "Standard Expected Plan")]
    [TestCase(true, TestName = "Shop Plan")]
    public void LoggerPlan(bool useShopPlan) {
        GatherWoodTestData testData = new GatherWoodTestData();

        testData.AddDataForTest(useShopPlan);

        List<G_Action> plan = new List<G_Action>();

        bool success = G_Planner.GeneratePlan(testData.npcWorldState.goals[0],
            testData.npcWorldState,
            out plan);

        if (useShopPlan) {
            Assert.AreEqual(true, plan != null);
            Assert.AreEqual(true, success);
            Assert.AreEqual(4, plan.Count);

            Assert.AreEqual("deliverWood", plan[3].name);
            Assert.AreEqual("goToWoodStock", plan[2].name);
            Assert.AreEqual("buyWood", plan[1].name);
            Assert.AreEqual("goToShop", plan[0].name);
        }
        else {
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
}
