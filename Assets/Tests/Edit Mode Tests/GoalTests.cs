using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using GOAP;

public class GoalTests {
    [Test]
    public void CloneTest() {
        ActionTests.SlicedBreadData breadData = new ActionTests.SlicedBreadData();

        G_Goal sliceBread = A.Goal("sliceBread")
            .WithTrigger(A.Condition().State(breadData.inventory).IsEqualTo(ItemStack.EmptyStack(breadData.slicedBread)))

            .WithEffect(A.Condition().State(breadData.inventory).IsGreaterThan(ItemStack.EmptyStack(breadData.slicedBread)))

            .WithPriority(0);

        G_Goal clone = sliceBread.Clone();

        Assert.AreEqual(true, clone != null);

        Assert.AreEqual(sliceBread.name, clone.name);

        Assert.AreEqual(true, sliceBread.triggerConditions.Count > 0);
        Assert.AreEqual(true, clone.triggerConditions.Count > 0);
        Assert.AreEqual(sliceBread.triggerConditions.Count, clone.triggerConditions.Count);
        for (int i = 0; i < sliceBread.triggerConditions.Count; i++) {
            Assert.AreEqual(sliceBread.triggerConditions[i].State, clone.triggerConditions[i].State);
            Assert.AreEqual(sliceBread.triggerConditions[i].Comparison, clone.triggerConditions[i].Comparison);
            Assert.AreEqual(sliceBread.triggerConditions[i].UseExpectedReference, clone.triggerConditions[i].UseExpectedReference);

            if (sliceBread.triggerConditions[i].UseExpectedReference) {
                Assert.AreEqual(sliceBread.triggerConditions[i].ExpectedReference, clone.triggerConditions[i].ExpectedReference);
            }
            else {
                Assert.AreEqual(sliceBread.triggerConditions[i].ExpectedValue, clone.triggerConditions[i].ExpectedValue);
            }
        }

        Assert.AreEqual(true, sliceBread.goalEffects.Count > 0);
        Assert.AreEqual(true, clone.goalEffects.Count > 0);
        Assert.AreEqual(sliceBread.goalEffects.Count, clone.goalEffects.Count);
        for (int i = 1; i < sliceBread.goalEffects.Count; i++) {
            Assert.AreEqual(sliceBread.goalEffects[i].State, clone.goalEffects[i].State);
            Assert.AreEqual(sliceBread.goalEffects[i].Comparison, clone.goalEffects[i].Comparison);
            Assert.AreEqual(sliceBread.goalEffects[i].UseExpectedReference, clone.goalEffects[i].UseExpectedReference);

            if (sliceBread.goalEffects[i].UseExpectedReference) {
                Assert.AreEqual(sliceBread.goalEffects[i].ExpectedReference, clone.goalEffects[i].ExpectedReference);
            }
            else {
                Assert.AreEqual(sliceBread.goalEffects[i].ExpectedValue, clone.goalEffects[i].ExpectedValue);
            }
        }

        Assert.AreEqual(sliceBread.priority, clone.priority);
    }
}
