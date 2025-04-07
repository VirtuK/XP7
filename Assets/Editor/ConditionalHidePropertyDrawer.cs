using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(ConditionalHideAttribute))]
[System.Serializable]
public class ConditionalHidePropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ConditionalHideAttribute condHAtt = (ConditionalHideAttribute)attribute;
        bool enabled = GetConditionalValue(property, condHAtt.ConditionalSourceField);

        if (enabled)
        {
            EditorGUI.PropertyField(position, property, label, true);
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        ConditionalHideAttribute condHAtt = (ConditionalHideAttribute)attribute;
        bool enabled = GetConditionalValue(property, condHAtt.ConditionalSourceField);

        return enabled ? EditorGUI.GetPropertyHeight(property, label, true) : 0;
    }

    private bool GetConditionalValue(SerializedProperty property, string sourceFieldName)
    {
        SerializedProperty sourceProperty = property.serializedObject.FindProperty(sourceFieldName);
        return sourceProperty != null && sourceProperty.boolValue;
    }
}