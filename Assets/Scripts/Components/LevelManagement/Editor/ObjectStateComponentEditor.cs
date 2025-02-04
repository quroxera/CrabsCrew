using System;
using UnityEditor;
using UnityEngine;
using Scripts.Component.LevelManagement;

[CustomEditor(typeof(ObjectStateComponent)), CanEditMultipleObjects]
public class ObjectStateComponentEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var selectedComponents = targets;
        
        DrawDefaultInspector();
        
        if (GUILayout.Button("Generate ID"))
        {
            foreach (var targetObject in selectedComponents)
            {
                ObjectStateComponent component = (ObjectStateComponent)targetObject;
                GenerateUniqueId(component);
            }
        }
    }

    private void GenerateUniqueId(ObjectStateComponent component)
    {
        string newId;

        do
        {
            newId = Guid.NewGuid().ToString();
        } while (IsDuplicateIdExists(newId, component));
        
        Undo.RecordObject(component, "Generate Unique ID");
        component.GetType()
            .GetField("_id", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.SetValue(component, newId);
        EditorUtility.SetDirty(component);
    }

    private bool IsDuplicateIdExists(string id, ObjectStateComponent currentComponent)
    {
        ObjectStateComponent[] allComponents = FindObjectsOfType<ObjectStateComponent>();

        foreach (var component in allComponents)
        {
            if (component != currentComponent && component.Id == id)
            {
                return true;
            }
        }

        return false;
    }
}