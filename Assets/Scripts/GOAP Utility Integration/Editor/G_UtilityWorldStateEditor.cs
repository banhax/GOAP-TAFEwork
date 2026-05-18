#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using GOAP;
using UtilityAI;

[CustomEditor(typeof(G_UtilityWorldState))]
public class G_UtilityWorldStateEditor : Editor {
    public override void OnInspectorGUI() {

        G_UtilityWorldState worldRef = target as G_UtilityWorldState;
        List<string> missingStateNames = new List<string>();
        List<string> missingU_ValueNames = new List<string>();

        for (int a = 0; a < worldRef.actionPool.Count; a++) {
            if (worldRef.actionPool[a] != null) {
                ActionTest(worldRef, worldRef.actionPool[a], missingStateNames);
            }
        }
        for (int g = 0; g < worldRef.goals.Count; g++) {
            if (worldRef.goals[g] != null) {
                GoalTest(worldRef, worldRef.goals[g], missingStateNames, missingU_ValueNames);
            }
        }
        for (int u = 0; u < worldRef.utilityValues.Count; u++) {
            if (worldRef.utilityValues[u] != null) {
                U_ValueTest(worldRef, worldRef.utilityValues[u], missingStateNames, missingU_ValueNames);
            }
        }

        if (missingStateNames.Count > 0) {
            DisplayMissingNames("States", missingStateNames);
        }
        if (missingU_ValueNames.Count > 0) {
            DisplayMissingNames("U Values", missingU_ValueNames);
        }

        EditorGUILayout.PropertyField(serializedObject.FindProperty("goals"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("actionPool"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("states"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("utilityValues"));
        serializedObject.ApplyModifiedProperties();
    }

    #region Object Type Tests
    void ActionTest(G_UtilityWorldState worldRef,
        G_Action action,
        List<string> missingStateNames) {

        TestForStatesInConditions(worldRef, action.preconditions, missingStateNames);
        TestForStatesInConditions(worldRef, action.effects, missingStateNames);
    }

    void GoalTest(G_UtilityWorldState worldRef,
        G_Goal goal,
        List<string> missingStateNames,
        List<string> missingU_ValueNames) {

        TestForStatesInConditions(worldRef, goal.triggerConditions, missingStateNames);
        TestForStatesInConditions(worldRef, goal.goalEffects, missingStateNames);

        if (goal is G_UtilityGoal utilityGoal) {
            TestForUtilitesInWorldState(worldRef, utilityGoal.UtilityScorer.values, missingU_ValueNames);
        }
    }

    void U_ValueTest(G_UtilityWorldState worldRef,
        U_Value value,
        List<string> missingStateNames,
        List<string> missingU_ValueNames) {

        switch (value.VarSource) {
            case U_ValueSource.IntState:
                if (value.IntStateVar != null) {
                    TestForStateInWorldState(worldRef, value.IntStateVar, missingStateNames);
                }
                break;
            case U_ValueSource.FloatState:
                if (value.FloatStateVar != null) {
                    TestForStateInWorldState(worldRef, value.FloatStateVar, missingStateNames);
                }
                break;
            case U_ValueSource.Utility:
                if (value.UtilityVar != null) {
                    IsUtilityInWorldState(worldRef, value.UtilityVar, missingU_ValueNames);
                }
                break;
        }

        switch (value.MaxSource) {
            case U_ValueSource.IntState:
                if (value.IntStateMax != null) {
                    TestForStateInWorldState(worldRef, value.IntStateMax, missingStateNames);
                }
                break;
            case U_ValueSource.FloatState:
                if (value.FloatStateMax != null) {
                    TestForStateInWorldState(worldRef, value.FloatStateMax, missingStateNames);
                }
                break;
            case U_ValueSource.Utility:
                if (value.UtilityMax != null) {
                    IsUtilityInWorldState(worldRef, value.UtilityMax, missingU_ValueNames);
                }
                break;
        }
    }
    #endregion

    #region Missing Utility Tests
    void TestForUtilitesInWorldState(G_UtilityWorldState worldRef,
        List<U_Value> values,
        List<string> missingU_ValueNames) {

        for (int v = 0; v < values.Count; v++) {
            if (values[v] != null) {
                IsUtilityInWorldState(worldRef, values[v], missingU_ValueNames);
            }
        }
    }

    void IsUtilityInWorldState(G_UtilityWorldState worldRef,
        U_Value value,
        List<string> missingU_ValueNames) {

        U_Value foundValue = worldRef.FindU_Value(value);
        if (foundValue == null) {
            AddToMissingNameList(missingU_ValueNames, value.name);
        }
    }
    #endregion

    #region Missing State Tests
    void TestForStatesInConditions(G_UtilityWorldState worldRef,
        List<G_Condition> conditions,
        List<string> missingStateNames) {

        for (int c = 0; c < conditions.Count; c++) {
            if (conditions[c] != null && conditions[c].State != null) {
                TestForStateInWorldState(worldRef, conditions[c].State, missingStateNames);
            }
        }
    }

    void TestForStateInWorldState(G_UtilityWorldState worldRef,
        G_State state,
        List<string> missingStateNames) {

        G_State foundState = worldRef.FindState(state);
        if (foundState == null) {
            AddToMissingNameList(missingStateNames, state.name);
        }
    }
    #endregion

    #region Missing Name Lists
    void AddToMissingNameList(List<string> missingNames, string name) {
        if (!missingNames.Contains(name)) {
            missingNames.Add(name);
        }
    }

    void DisplayMissingNames(string objectTitle, List<string> missingNames) {
        EditorGUILayout.LabelField($"Missing {objectTitle}", EditorStyles.boldLabel);
        EditorGUILayout.LabelField($"{missingNames.Count} {objectTitle} are missing from this World State:");
        for (int m = 0; m < missingNames.Count; m++) {
            EditorGUILayout.LabelField($"{m + 1}. {missingNames[m]}");
        }
        EditorGUILayout.Space();
    }
    #endregion
}
#endif