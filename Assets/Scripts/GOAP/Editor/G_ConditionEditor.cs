using UnityEngine;
using UnityEditor;
using GOAP;

[CustomPropertyDrawer(typeof(G_Condition))]
public class G_ConditionEditor : PropertyDrawer { // only one instance of a custom property drawer running
    bool active = false;
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        return base.GetPropertyHeight(property, label);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        SerializedProperty active = property.FindPropertyRelative("editorActive");

        active.boolValue =
            EditorGUI.Foldout(new Rect(position.x, position.y, position.width, base.GetPropertyHeight(property, label)),
            active.boolValue,
            "Condition");
    }
}
