using GOAP;
using NUnit.Framework;
using System.Collections;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.TestTools;

public class AtLocationTests
{
    [Test]
    public void AtLocationClone() {
        LocationType tree = A.LocationType("tree");
        G_AtLocation atLocation = An.AtLocation("atLocation").WithLocationType(tree);

        G_AtLocation clone = atLocation.Clone() as G_AtLocation;
        Assert.AreEqual(atLocation.name, clone.name);
        Assert.AreEqual(atLocation.GetValue() as LocationType, clone.GetValue() as LocationType);
    }

    [TestCase(true, true, true, TestName = "State Tree vs Expected Tree")]
    [TestCase(false, false, true, TestName = "State Null vs Expected Null")]
    [TestCase(true, false, false, TestName = "State Tree vs Expected Null")]
    [TestCase(false, true, false, TestName = "State Null vs Expected Tree")]
    public void AtLocationTestState(bool useLocationForState, bool useLocationForExpected, bool expectedResult) {
        LocationType tree = A.LocationType("tree");
        LocationType stateLocation = null;
        LocationType expectedLocation = null;

        if (useLocationForState) {
            stateLocation = tree;
        }
        if (useLocationForExpected) {
            expectedLocation = tree;
        }

        G_AtLocation atLocation = An.AtLocation("atLocation").WithLocationType(stateLocation);
        bool result = atLocation.TestState(atLocation, G_StateComparison.EqualTo, expectedLocation);
        Assert.AreEqual(expectedResult, result);
    }
}
