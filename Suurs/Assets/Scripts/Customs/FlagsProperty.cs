using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;



public class EnumFlagAttribute : PropertyAttribute
{
    public string Name;
	public EnumFlagAttribute() {}

}

[CustomPropertyDrawer(typeof(EnumFlagAttribute))]
/*public class EnumFlagAttributeDrawer : PropertyDrawer
{
    const float mininumWidth = 60.0f;
 
    int enumLength;
    float enumWidth;
 
    int numBtns;
    int numRows;
 
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        SetDimensions(property);
        return numRows * EditorGUIUtility.singleLineHeight + (numRows - 1) * EditorGUIUtility.standardVerticalSpacing;
    }
 
    void SetDimensions(SerializedProperty property) {
        enumLength = property.enumNames.Length;
        enumWidth = (EditorGUIUtility.currentViewWidth - EditorGUIUtility.labelWidth - 20 );
 
        numBtns = Mathf.FloorToInt(enumWidth / mininumWidth);
        numRows = Mathf.CeilToInt((float)enumLength / (float)numBtns);
    }
 
    public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label)
    {
        SetDimensions(_property);
 
        int buttonsIntValue = 0;
        bool[] buttonPressed = new bool[enumLength];
        float buttonWidth = enumWidth / Mathf.Min(numBtns, enumLength);
 
        EditorGUI.LabelField(new Rect(_position.x, _position.y, EditorGUIUtility.labelWidth, _position.height), _label);
 
        EditorGUI.BeginChangeCheck ();
 
        for (int row = 0; row < numRows; row++) {
            for (int btn = 0; btn < numBtns; btn++) {
                int i = btn + row * numBtns;
 
                if (i >= enumLength) {
                    break;
                }
 
                // Check if the button is/was pressed
                if ((_property.intValue & (1 << i)) == 1 << i) {
                    buttonPressed[i] = true;
                }
 
                Rect buttonPos = new Rect(
                    _position.x + EditorGUIUtility.labelWidth + buttonWidth * btn, 
                    _position.y + row * (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing), 
                    buttonWidth, EditorGUIUtility.singleLineHeight);

                buttonPressed[i] = GUI.Toggle(buttonPos, buttonPressed[i], _property.enumNames[i], EditorStyles.toolbarButton); 
                if (buttonPressed[i])
                    buttonsIntValue += 1 << i;
            }
        }
 
        if (EditorGUI.EndChangeCheck()) {
            _property.intValue = buttonsIntValue;
        }
    }
}
 */
/*public class EnumFlagAttributeDrawer : PropertyDrawer
{
    const float mininumWidth = 80.0f;
 
    public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label)
    {
        int buttonsIntValue = 0;
        int enumLength = _property.enumNames.Length;
 
        float enumWidth = (EditorGUIUtility.currentViewWidth - EditorGUIUtility.labelWidth - 30);
 
        int buttonsPerRow = Mathf.FloorToInt(enumWidth / mininumWidth);
        int numRows = Mathf.CeilToInt((float)enumLength / (float)buttonsPerRow);
 
       
        bool[] buttonPressed = new bool[enumLength];
 
        float buttonWidth = enumWidth / Mathf.Min(buttonsPerRow, enumLength);
 
        EditorGUI.LabelField(new Rect(_position.x, _position.y, EditorGUIUtility.labelWidth, _position.height), _label);
 
        EditorGUI.BeginChangeCheck();
 
       
        for (int row = 0; row < numRows; row++)
        {
            for (int button = 0; button < buttonsPerRow; button++)
            {
                int i = button + row * buttonsPerRow;
 
                if (i >= enumLength) { break; }
 
                // Check if the button is/was pressed
                if ((_property.intValue & (1 << i)) == 1 << i)
                {
                    buttonPressed[i] = true;
                }
 
                Rect buttonPos = new Rect(_position.x + EditorGUIUtility.labelWidth + buttonWidth * button, _position.y + row * (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing), buttonWidth, EditorGUIUtility.singleLineHeight);
 
                buttonPressed[i] = GUI.Toggle(buttonPos, buttonPressed[i], _property.enumNames[i], EditorStyles.toolbarButton);
 
                if (buttonPressed[i])
                    buttonsIntValue += 1 << i;
            }
        }
 
        if (EditorGUI.EndChangeCheck())
        {
            _property.intValue = buttonsIntValue;
        }
    }
   
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        int enumLength = property.enumNames.Length;
 
        float enumWidth = (EditorGUIUtility.currentViewWidth - EditorGUIUtility.labelWidth - 30);
 
        int buttonsPerRow = Mathf.FloorToInt(enumWidth / mininumWidth);
        int numRows = Mathf.CeilToInt((float)enumLength / (float)buttonsPerRow);
 
 
        return numRows * (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing);
    }
}
/*
public class EnumFlagAttributeDrawer : PropertyDrawer
{
 
    public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label)
    {
        int buttonsIntValue = 0;
        int enumLength = _property.enumNames.Length;
        bool[] buttonPressed = new bool[enumLength];
        float buttonWidth = (_position.width - EditorGUIUtility.labelWidth) / enumLength;
 
        EditorGUI.LabelField(new Rect(_position.x, _position.y, EditorGUIUtility.labelWidth, _position.height), _label);
 
        EditorGUI.BeginChangeCheck ();
 
        for (int i = 0; i < enumLength; i++) {
 
            // Check if the button is/was pressed 
            if ( ( _property.intValue & (1 << i) ) == 1 << i ) {
                buttonPressed[i] = true;
            }
 
            Rect buttonPos = new Rect (_position.x + EditorGUIUtility.labelWidth + buttonWidth * i, _position.y, buttonWidth, _position.height);
 
            buttonPressed[i] = GUI.Toggle(buttonPos, buttonPressed[i], _property.enumNames[i],  "Button");
 
            if (buttonPressed[i])
                buttonsIntValue += 1 << i;
        }
 
        if (EditorGUI.EndChangeCheck()) {
            _property.intValue = buttonsIntValue;
        }
    }
}*/

public class EnumFlagDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EnumFlagAttribute flagSettings = (EnumFlagAttribute)attribute;
        Enum targetEnum = (Enum)fieldInfo.GetValue(property.serializedObject.targetObject);

        string propName = flagSettings.Name;
        if (string.IsNullOrEmpty(propName))
            propName = ObjectNames.NicifyVariableName(property.name);

        EditorGUI.BeginProperty(position, label, property);
        Enum enumNew = EditorGUI.EnumFlagsField(position, propName, targetEnum);
        property.intValue = (int)Convert.ChangeType(enumNew, targetEnum.GetType());
        EditorGUI.EndProperty();
    }
}

/*
public class EnumFlagsAttributeDrawer : PropertyDrawer
{
    private string[] enumNames;
    private readonly Dictionary<string, int> enumNameToValue = new Dictionary<string, int>();
    private readonly Dictionary<string, string> enumNameToDisplayName = new Dictionary<string, string>();
    private readonly Dictionary<string, string> enumNameToTooltip = new Dictionary<string, string>();
    private readonly List<string> activeEnumNames = new List<string>();
    private SerializedProperty serializedProperty;
    private ReorderableList reorderableList;
    private bool firstTime = true;

    private Type EnumType
    {
        get { return fieldInfo.FieldType; }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        serializedProperty = property;
        SetupIfFirstTime();
        return reorderableList.GetHeight();
    }

    private void SetupIfFirstTime()
    {
        if (!firstTime)
        {
            return;
        }
        enumNames = serializedProperty.enumNames;
        CacheEnumMetadata();
        ParseActiveEnumNames();
        reorderableList = GenerateReorderableList();
        firstTime = false;
    }

    private void CacheEnumMetadata()
    {
        for (var index = 0; index < enumNames.Length; index++)
        {
            enumNameToDisplayName[enumNames[index]] = serializedProperty.enumDisplayNames[index];
        }
        foreach (string enumName in enumNames)
        {
            enumNameToTooltip[enumName] = EnumType.Name + "." + enumName;
        }
        foreach (string name in enumNames)
        {
            enumNameToValue.Add(name, (int)Enum.Parse(EnumType, name));
        }
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginDisabledGroup(serializedProperty.hasMultipleDifferentValues);
        reorderableList.DoList(position);
        EditorGUI.EndDisabledGroup();
    }

    private ReorderableList GenerateReorderableList()
    {
        return new ReorderableList(activeEnumNames, typeof(string), false, true, true, true)
        {
            drawHeaderCallback = rect =>
            {
                EditorGUI.LabelField(rect, new GUIContent(serializedProperty.displayName, "EnumType: " + EnumType.Name));
            },
            drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                rect.y += 2;
                EditorGUI.LabelField(
                    new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight),
                    new GUIContent(enumNameToDisplayName[activeEnumNames[index]], enumNameToTooltip[activeEnumNames[index]]),
                    EditorStyles.label);
            },
            onAddDropdownCallback = (Rect buttonRect, ReorderableList l) =>
            {
                var menu = new GenericMenu();
                foreach (string enumName in enumNames)
                {
                    if (activeEnumNames.Contains(enumName) == false)
                    {
                        menu.AddItem(new GUIContent(enumNameToDisplayName[enumName]),
                            false, data =>
                            {
                                if (enumNameToValue[(string)data] == 0)
                                {
                                    activeEnumNames.Clear();
                                }
                                activeEnumNames.Add((string)data);
                                SaveActiveValues();
                                ParseActiveEnumNames();
                            },
                            enumName);
                    }
                }
                menu.ShowAsContext();
            },
            onRemoveCallback = l =>
            {
                ReorderableList.defaultBehaviours.DoRemoveButton(l);
                SaveActiveValues();
                ParseActiveEnumNames();
            }
        };
    }

    private void ParseActiveEnumNames()
    {
        activeEnumNames.Clear();
        foreach (string enumValue in enumNames)
        {
            if (IsFlagSet(enumValue))
            {
                activeEnumNames.Add(enumValue);
            }
        }
    }

    private bool IsFlagSet(string enumValue)
    {
        if (enumNameToValue[enumValue] == 0)
        {
            return serializedProperty.intValue == 0;
        }
        return (serializedProperty.intValue & enumNameToValue[enumValue]) == enumNameToValue[enumValue];
    }
    
    private void SaveActiveValues()
    {
        serializedProperty.intValue = ConvertActiveNamesToInt();
        serializedProperty.serializedObject.ApplyModifiedProperties();
    }
    private int ConvertActiveNamesToInt()
    {
        return activeEnumNames.Aggregate(0, (current, activeEnumName) => current | enumNameToValue[activeEnumName]);
    }
}*/