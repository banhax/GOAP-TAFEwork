using System.Collections.Generic;
using UnityEngine;
using GOAP;
using UtilityAI;

public class NPCStatManager : MonoBehaviour {
    [Header("Energy")]
    public G_FloatState refCurrentEnergy;
    G_FloatState currentEnergy;
    public float energyIncrementRate = 1f;

    [Header("Hunger")]
    public G_FloatState refCurrentHunger;
    G_FloatState currentHunger;
    public float hungerIncrementRate = 1f;
    public U_Value hunger;

    [Header("Energy Hunger Relationship")]
    public float regainThreshold = 0.85f;
    public float regainMultiplier = 1f;
    public float loseThreshold = 0.8f;
    public float lossMultiplier = -1f;
    float currentMultiplier = 0f;

    public void InjectLocalWorldState(G_UtilityWorldState worldState) {
        currentEnergy = worldState.FindState(refCurrentEnergy) as G_FloatState;
        currentHunger = worldState.FindState(refCurrentHunger) as G_FloatState;
        hunger = worldState.FindU_Value(hunger);
    }

    void Update() {
        DetermineEnergyMultiplier();
        currentEnergy.AddToValue(energyIncrementRate * currentMultiplier * Time.deltaTime);
        currentHunger.AddToValue(hungerIncrementRate * Time.deltaTime);
    }

    void DetermineEnergyMultiplier() {
        float currentHungerUtil = hunger.GetUtility();

        if (currentHungerUtil >= regainThreshold) {
            currentMultiplier = regainMultiplier;
        }
        else if (currentHungerUtil < regainThreshold && currentHungerUtil >= loseThreshold) {
            currentMultiplier = 0f;
        }
        else if (currentHungerUtil < loseThreshold) {
            currentMultiplier = lossMultiplier;
        }
    }
}
