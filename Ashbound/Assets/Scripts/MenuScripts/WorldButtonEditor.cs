using UnityEditor;
using UnityEditor.UI;

[CustomEditor(typeof(WorldButton), true)]
public class WorldButtonEditor : SelectableEditor
{
    static readonly string[] fields =
        { "visual", "selectedScale", "pressedScale", "lerpSpeed", "pressDuration", "onSelected", "onClick" };

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.Space();
        serializedObject.Update();
        foreach (var f in fields)
            EditorGUILayout.PropertyField(serializedObject.FindProperty(f));
        serializedObject.ApplyModifiedProperties();
    }
}