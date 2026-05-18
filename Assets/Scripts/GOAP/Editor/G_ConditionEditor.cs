#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using GOAP;

[CustomPropertyDrawer(typeof(G_Condition))]
public class G_ConditionEditor : PropertyDrawer { // only one instance of a custom property drawer running

    float height = 0f;
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        int heightMultiplier = 1;

        SerializedProperty stateProperty = property.FindPropertyRelative("state");
        SerializedProperty active = property.FindPropertyRelative("editorActive");

        if (stateProperty.objectReferenceValue != null && active.boolValue) {
            heightMultiplier = (stateProperty.objectReferenceValue as G_State).GetEditorHeight();
        }
        else if (stateProperty.objectReferenceValue == null && active.boolValue) {
            heightMultiplier = 2;
        }

            return (base.GetPropertyHeight(property, label) 
                + EditorGUIUtility.standardVerticalSpacing) * heightMultiplier;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        height = 0f;
        SerializedProperty active = property.FindPropertyRelative("editorActive");

        active.boolValue = // being assigned based on return value of Foldout
            EditorGUI.Foldout(GetFormattedRect(position, property, label),
            active.boolValue, // the one that is drawn
            "Condition");

        IncrementHeight(out height, property, label);

        int originalIndent = EditorGUI.indentLevel;
        EditorGUI.indentLevel += 1;

        if (active.boolValue) {
            BuildEditor(position, property, label);
        }

        EditorGUI.indentLevel = originalIndent;
    }

    void BuildEditor(Rect position, SerializedProperty property, GUIContent label) {
        SerializedProperty stateProperty = property.FindPropertyRelative("state");
        //Object preStateObject = stateProperty.objectReferenceValue;

        EditorGUI.ObjectField(GetFormattedRect(position, property, label), stateProperty);

        IncrementHeight(out height, property, label);

        if (stateProperty.objectReferenceValue != null) {
            ((G_State)stateProperty.objectReferenceValue).Editor(this, ref height, position, property, label);
        }
    }

    public Rect GetFormattedRect(Rect position, SerializedProperty property, GUIContent label) {
        return new Rect(position.x,
            position.y + height,
            position.width,
            base.GetPropertyHeight(property, label));
    }

    /// <summary>
    /// Increases local height variable by one standard property height + one standard vertical spacing gap
    /// every time it is called so that we can increment the height variable to continue drawing elements
    /// without them being drawn on top of each other
    /// </summary>
    /// <param name="progressiveHeight"></param>
    /// <param name="property"></param>
    /// <param name="label"></param>
    public void IncrementHeight(out float progressiveHeight, SerializedProperty property, GUIContent label) {
        progressiveHeight = height;

        progressiveHeight += base.GetPropertyHeight(property, label);
        progressiveHeight += EditorGUIUtility.standardVerticalSpacing;
    }
}
#endif