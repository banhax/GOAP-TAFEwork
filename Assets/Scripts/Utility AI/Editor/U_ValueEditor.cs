#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UtilityAI;

[CustomEditor(typeof(U_Value))]
// [CanEditMultipleObjects()]
public class U_ValueEditor : Editor {
    public override void OnInspectorGUI() {
        // base.OnInspectorGUI();
        SerializedProperty varSource = serializedObject.FindProperty("varSource");
        SerializedProperty maxSource = serializedObject.FindProperty("maxSource");
        SerializedProperty responseCurve = serializedObject.FindProperty("responseCurve");

        EditorGUILayout.LabelField("Controls", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(responseCurve);

        CreateSourceGUI("Variable", varSource, true);
        CreateSourceGUI("Maximum", maxSource, false);
        serializedObject.ApplyModifiedProperties();
    }

    void CreateSourceGUI(string headerName, SerializedProperty source, bool isVar) {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField(headerName, EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(source);
        string suffix = isVar ? "Var" : "Max";

        switch ((U_ValueSource)source.enumValueIndex) {
            case U_ValueSource.Int:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("int" + suffix));
                break;
            case U_ValueSource.Float:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("float" + suffix));
                break;
            case U_ValueSource.IntState:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("intState" + suffix));
                break;
            case U_ValueSource.FloatState:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("floatState" + suffix));
                break;
            case U_ValueSource.Utility:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("utility" + suffix));
                break;
        }
    }
}

#endif