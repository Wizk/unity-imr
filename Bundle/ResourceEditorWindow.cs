#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine.Localization;

public class StaticResourcesWindow : EditorWindow
{
    private ScriptableObject sharedData;
    private Dictionary<string, string> fieldTypes = new Dictionary<string, string>();
    private string[] tabs = { "Audio", "Colors", "GameObject", "Sprites", "Animations", "String References" };
    private int selectedTab = 0;
    private string newFieldName = "";
    private Vector2 scrollPosition;

    private const string scriptDirectory = "Assets/Scripts/Generated";
    private const string scriptableName = "SharedAssetsData.cs";
    private const string scriptName = "SharedAssets.cs";

    private string ScriptPath => Path.Combine(scriptDirectory, scriptName);
    private string ScriptablePath => Path.Combine(scriptDirectory, scriptableName);
    private bool ScriptExist => File.Exists(ScriptPath);

    [MenuItem("Tools/Static Resources")]
    public static void ShowWindow()
    {
        GetWindow<StaticResourcesWindow>("Static Resources");
    }

    private void OnEnable()
    {
        LoadResourceData();
    }

    private void LoadResourceData()
    {
        if (!ScriptExist)
            return;

        sharedData = AssetDatabase.LoadAssetAtPath<ScriptableObject>("Assets/Resources/SharedAssets.asset");

        if (sharedData == null)
        {
            sharedData = CreateInstance<ScriptableObject>();

            if (!AssetDatabase.IsValidFolder("Assets/Resources"))
                AssetDatabase.CreateFolder("Assets", "Resources");

            AssetDatabase.CreateAsset(sharedData, "Assets/Resources/SharedAssets.asset");
            AssetDatabase.SaveAssets();
        }

        fieldTypes.Clear();
        SerializedObject serializedObject = new SerializedObject(sharedData);
        SerializedProperty property = serializedObject.GetIterator();

        while (property.NextVisible(true))
        {
            if (property.name != "m_Script")
            {
                string unityTypeName = property.type;
                Type fieldType = GetTypeFromUnityTypeName(unityTypeName);

                if (fieldType != null)
                    fieldTypes[property.name] = fieldType.Name;
            }
        }
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        for (int i = 0; i < tabs.Length; i++)
        {
            if (GUILayout.Toggle(selectedTab == i, tabs[i], "Button"))
                selectedTab = i;
        }
        EditorGUILayout.EndHorizontal();

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Height(position.height - 100));
        DrawFieldEditor();
        EditorGUILayout.EndScrollView();
        GUILayout.FlexibleSpace();
        DrawAddFieldSection();

        if (GUILayout.Button("Save", GUILayout.Height(30)))
        {
            Save();
        }
    }

    private void DrawFieldEditor()
    {
        List<string> keysToRemove = new List<string>();

        foreach (var field in fieldTypes)
        {
            string fieldTab = GetTabForType(field.Value);
            if (fieldTab != tabs[selectedTab]) continue;

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(field.Key + " :", GUILayout.Width(150));

            if (ScriptExist)
            {
                object value = GetValueFromResourceData(field.Key);
                object newValue = DrawFieldByType(field.Value, value);

                if (!Equals(value, newValue))
                    SetValueToResourceData(field.Key, newValue);
            }
            else
                DrawFieldByType(field.Value, null);

            if (GUILayout.Button("X", GUILayout.Width(30)))
                keysToRemove.Add(field.Key);

            EditorGUILayout.EndHorizontal();
        }

        foreach (string key in keysToRemove)
            fieldTypes.Remove(key);
    }

    private object DrawFieldByType(string typeName, object value)
    {
        Type type = GetTypeFromName(typeName);
        if (type == typeof(AudioClip))
            return (AudioClip)EditorGUILayout.ObjectField((AudioClip)value, typeof(AudioClip), false);
        if (type == typeof(Color))
            return EditorGUILayout.ColorField(value != null ? (Color)value : Color.white);
        if (type == typeof(GameObject))
            return (GameObject)EditorGUILayout.ObjectField((GameObject)value, typeof(GameObject), false);
        if (type == typeof(Sprite))
            return (Sprite)EditorGUILayout.ObjectField((Sprite)value, typeof(Sprite), false);
        if (type == typeof(AnimationClip))
            return (AnimationClip)EditorGUILayout.ObjectField((AnimationClip)value, typeof(AnimationClip), false);
        return null;
    }

    private void DrawAddFieldSection()
    {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Add a field :", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        newFieldName = EditorGUILayout.TextField("Name :", newFieldName);

        if (GUILayout.Button("Add"))
            AddNewField();

        EditorGUILayout.EndHorizontal();
    }

    private void AddNewField()
    {
        if (string.IsNullOrEmpty(newFieldName))
        {
            Debug.LogWarning("You need to enter a field name.");
            return;
        }

        string fieldType = GetFieldTypeFromTab();
        if (fieldTypes.ContainsKey(newFieldName))
        {
            Debug.LogWarning("This field already exist !");
            return;
        }

        fieldTypes[newFieldName] = fieldType;
        newFieldName = "";
        scrollPosition = Vector2.zero;
    }

    private void Save()
    {
        if (!Directory.Exists(scriptDirectory))
            Directory.CreateDirectory(scriptDirectory);

        string fieldsCode = "";
        string propertiesCode = "";

        foreach (var field in fieldTypes)
        {
            fieldsCode += $"    public {field.Value} {field.Key};\n";
            propertiesCode += $"    public static {field.Value} {field.Key} => Instance.data.{field.Key};\n";
        }

        string scriptableContent = templateScriptable.Replace("{0}", fieldsCode);
        string scriptContent = templateScript.Replace("{0}", propertiesCode);

        File.WriteAllText(ScriptablePath, scriptableContent);
        File.WriteAllText(ScriptPath, scriptContent);
        AssetDatabase.Refresh();
    }

    private object GetValueFromResourceData(string fieldName)
    {
        SerializedObject serializedObject = new SerializedObject(sharedData);
        SerializedProperty property = serializedObject.FindProperty(fieldName);

        if (property == null)
            return null;

        if (property.propertyType == SerializedPropertyType.String)
        {
            return property.stringValue;
        }
        else if (property.propertyType == SerializedPropertyType.Integer)
        {
            return property.intValue;
        }
        else if (property.propertyType == SerializedPropertyType.Float)
        {
            return property.floatValue;
        }
        else if (property.propertyType == SerializedPropertyType.Boolean)
        {
            return property.boolValue;
        }
        else if (property.propertyType == SerializedPropertyType.Color)
        {
            return property.colorValue;
        }
        else if (property.propertyType == SerializedPropertyType.ObjectReference)
        {
            return property.objectReferenceValue;
        }
        else if (property.type == "LocalizedString")
        {
            SerializedProperty tableReference = property.FindPropertyRelative("m_TableReference");
            SerializedProperty entryReference = property.FindPropertyRelative("m_TableEntryReference");
            
            return new LocalizedString
            {
                TableReference = tableReference.stringValue,
                TableEntryReference = entryReference.intValue
            };
        }
        else
        {
            Debug.LogWarning($"Type {property.type} not supported in GetValueFromResourceData.");
            return null;
        }
    }

    private void SetValueToResourceData(string fieldName, object value)
    {
        SerializedObject serializedObject = new SerializedObject(sharedData);
        SerializedProperty property = serializedObject.FindProperty(fieldName);

        if (property == null)
            return;

        if (value is UnityEngine.Object unityObject)
            property.objectReferenceValue = (UnityEngine.Object)value;

        if (value is int intVal)
            property.intValue = intVal;
        if (value is Color colorVal)
            property.colorValue = colorVal;

        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(sharedData);
    }

    private Type GetTypeFromUnityTypeName(string unityTypeName)
    {
        if (unityTypeName.Contains("PPtr<$")) // Gestion des objets Unity (Sprite, AudioClip, etc.)
        {
            unityTypeName = unityTypeName.Replace("PPtr<$", "").Replace(">", "").Trim();
        }

        return unityTypeName switch
        {
            "AudioClip" => typeof(AudioClip),
            "Color" => typeof(Color),
            "GameObject" => typeof(GameObject),
            "Sprite" => typeof(Sprite),
            "AnimationClip" => typeof(AnimationClip),
            "LocalizedString" => typeof(LocalizedString),
            "string" => typeof(string),
            "int" => typeof(int),
            "float" => typeof(float),
            "bool" => typeof(bool),
            _ => null // Si le type n'est pas reconnu
        };
    }

    private Type GetTypeFromName(string typeName)
    {
        return typeName switch
        {
            "AudioClip" => typeof(AudioClip),
            "Color" => typeof(Color),
            "GameObject" => typeof(GameObject),
            "Sprite" => typeof(Sprite),
            "AnimationClip" => typeof(AnimationClip),
            "LocalizedString" => typeof(LocalizedString),
            _ => null
        };
    }

    private string GetTabForType(string type)
    {
        return type switch
        {
            "AudioClip" => "Audio",
            "Color" => "Colors",
            "GameObject" => "GameObject",
            "Sprite" => "Sprites",
            "AnimationClip" => "Animations",
            "LocalizedString" => "String Références",
            _ => null
        };
    }

    private string GetFieldTypeFromTab()
    {
        return tabs[selectedTab] switch
        {
            "Audio" => "AudioClip",
            "Colors" => "Color",
            "GameObject" => "GameObject",
            "Sprites" => "Sprite",
            "Animations" => "AnimationClip",
            "String Références" => "LocalizedString",
            _ => null
        };
    }

    private const string templateScriptable = @"using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu] // TODO Remove
public class SharedAssetsData : ScriptableObject
{
{0}
}";
    private const string templateScript = @"using UnityEngine;
using UnityEngine.Localization;

public class SharedAssets : SingleBehaviour<SharedAssets>
{
    protected override void InternalAwake() { }
    protected override void InternalOnDestroy() { }

    public SharedAssetsData data;
{0}
}";
}
#endif