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

            }
        }
        for (int g = 0; g < worldRef.goals.Count; g++) {
            if (worldRef.goals[g] != null) {

            }
        }
        for (int u = 0; u < worldRef.utilityValues.Count; u++) {
            if (worldRef.utilityValues[u] != null) {

            }
        }

        EditorGUILayout.PropertyField(serializedObject.FindProperty("goals"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("actionPool"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("states"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("utilityValues"));
    }

    void ActionTest(G_UtilityWorldState worldRef,
        G_Action action,
        List<string> missingStateNames) {

        TestForStatesInConditions(worldRef, action.preconditions, missingStateNames);
        TestForStatesInConditions(worldRef, action.effects, missingStateNames);
    }

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

    void AddToMissingNameList(List<string> missingNames, string name) {
        if (!missingNames.Contains(name)) {
            missingNames.Add(name);
        }
    }
}
#endif