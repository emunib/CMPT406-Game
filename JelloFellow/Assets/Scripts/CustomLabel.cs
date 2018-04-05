using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

/// <summary>
/// Custom label for serialized fields.
/// Code URL: https://answers.unity.com/questions/1005277/can-i-change-variable-name-on-inspector.html
/// </summary>
public class CustomLabel : PropertyAttribute {
  private readonly string label;

  public CustomLabel(string _label) {
    label = _label;
  }
  
#if UNITY_EDITOR
  [CustomPropertyDrawer(typeof(CustomLabel))]
  public class ThisPropertyDrawer : PropertyDrawer {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {      
      CustomLabel propertyAttribute = attribute as CustomLabel;
      if (propertyAttribute != null) label.text = propertyAttribute.label;
      EditorGUI.PropertyField(position, property, label);  
    }
  }
  #endif
}

/// <summary>
/// Custom label for serialized range fields, also allows us to have
/// range with Integers, not just floats.
/// Code URL: https://docs.unity3d.com/ScriptReference/PropertyDrawer.html
/// </summary>
public class CustomRangeLabel : PropertyAttribute {
  private readonly string label;
  private readonly float min;
  private readonly float max;
  
  public CustomRangeLabel(string _label, float _min, float _max) {
    min = _min;
    max = _max;
    label = _label;
  }

#if UNITY_EDITOR
  [CustomPropertyDrawer(typeof(CustomRangeLabel))]
  public class ThisPropertyDrawer : PropertyDrawer {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
      CustomRangeLabel propertyAttribute = attribute as CustomRangeLabel;
      if (propertyAttribute != null) {
        label.text = propertyAttribute.label;

        if (property.propertyType == SerializedPropertyType.Float) {
          EditorGUI.Slider(position, property, propertyAttribute.min, propertyAttribute.max, label);
        } else if (property.propertyType == SerializedPropertyType.Integer) {
          EditorGUI.IntSlider(position, property, Convert.ToInt32(propertyAttribute.min),
            Convert.ToInt32(propertyAttribute.max), label);
        } else {
          EditorGUI.LabelField(position, label.text, propertyAttribute.label);
        }
      }
    }
  }
  #endif
}

public class ReadOnlyAttribute : PropertyAttribute {}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer {
  public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
    return EditorGUI.GetPropertyHeight(property, label, true);
  }
 
  public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
    GUI.enabled = false;
    EditorGUI.PropertyField(position, property, label, true);
    GUI.enabled = true;
  }
}
#endif