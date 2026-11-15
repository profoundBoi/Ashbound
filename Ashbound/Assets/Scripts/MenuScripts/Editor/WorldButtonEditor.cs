using UnityEditor;
using UnityEditor.UI;

[CustomEditor(typeof(WorldButton), true)]
[CanEditMultipleObjects]
public class WorldButtonEditor : SelectableEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.Space();
        serializedObject.Update();

        DrawPropertiesExcluding(serializedObject,
            "m_Script",
            "m_Navigation",
            "m_Transition",
            "m_Colors",
            "m_SpriteState",
            "m_AnimationTriggers",
            "m_Interactable",
            "m_TargetGraphic");

        serializedObject.ApplyModifiedProperties();
    }
}