using System.Collections.Generic;
using UnityEngine;
using GOAP;
using UtilityAI;

public class NPCStatManager : MonoBehaviour {
    [Header("Energy")]
    public G_FloatState refCurrentEnergy;
    public G_FloatState maxCurrentEnergy;
    [SerializeField] G_FloatState currentEnergy;
    public float energyIncrementRate = 1f;

    [Header("Fullness")]
    public G_FloatState refCurrentFullness;
    public G_FloatState maxCurrentFullness;
    [SerializeField] G_FloatState currentFullness;
    public float hungerIncrementRate = -1f;
    public U_Value hunger;

    [Header("Energy Hunger Relationship")]
    public float regainThreshold = 0.15f;
    public float regainMultiplier = 1;
    public float loseThreshold = 0.2f;
    public float lossMultiplier = -1;
    float currentMultiplier = 0f;

    public void InjectLocalWorldState(G_UtilityWorldState worldState) {
        currentEnergy = worldState.FindState(refCurrentEnergy) as G_FloatState;
        currentFullness = worldState.FindState(refCurrentFullness) as G_FloatState;
        hunger = worldState.FindU_Value(hunger);
    }

    void Update() {
        DetermineEnergyMultiplier();

        

        currentEnergy.AddToValue(energyIncrementRate * currentMultiplier * Time.deltaTime);
        currentFullness.AddToValue(hungerIncrementRate * Time.deltaTime);
        ClampValues(currentEnergy, maxCurrentEnergy);
        ClampValues(currentFullness, maxCurrentFullness);
    }

    void ClampValues(G_FloatState currentValue, G_FloatState maxValue) {
        float current = (float)currentValue.GetValue();
        float max = (float)maxValue.GetValue();

        if (current > max) {
            currentValue.SetValue((float)maxValue.GetValue());
        }
        else if (current < 0) {
            currentValue.SetValue(0);
        }
    }

    void DetermineEnergyMultiplier() {
        float currentHungerUtil = hunger.GetUtility();

        if (currentHungerUtil <= regainThreshold) {
            currentMultiplier = regainMultiplier;
        }
        else if (currentHungerUtil > regainThreshold && currentHungerUtil <= loseThreshold) {
            currentMultiplier = 0f;
        }
        else if (currentHungerUtil > loseThreshold) {
            currentMultiplier = lossMultiplier;
        }
    }
}
