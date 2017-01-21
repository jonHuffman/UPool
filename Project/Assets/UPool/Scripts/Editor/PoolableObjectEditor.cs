using UnityEditor;
using UnityEngine;

namespace UPool
{
    [CustomEditor(typeof(PoolableObject))]
    public class PoolableObjectEditor : Editor
    {
        private SerializedProperty _disableObjectProperty;
        private SerializedProperty _disableCollidersProperty;
        private SerializedProperty _disableRenderersProperty;

        private void OnEnable()
        {
            _disableObjectProperty = serializedObject.FindProperty("_disableObject");
            _disableCollidersProperty = serializedObject.FindProperty("_disableColliders");
            _disableRenderersProperty = serializedObject.FindProperty("_disableRenderers");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            _disableObjectProperty.boolValue = EditorGUILayout.Toggle(new GUIContent("Disable Object", "Disables the GameObject when resting in the Pool"), _disableObjectProperty.boolValue);

            EditorGUI.BeginDisabledGroup(_disableObjectProperty.boolValue);
            {
                _disableCollidersProperty.boolValue = EditorGUILayout.Toggle(new GUIContent("Disable Colliders", "Disables the Colliders when resting in the Pool"), _disableCollidersProperty.boolValue);
                _disableRenderersProperty.boolValue = EditorGUILayout.Toggle(new GUIContent("Disable Renderers", "Disables the Renderers when resting in the Pool"), _disableRenderersProperty.boolValue);
            }
            EditorGUI.EndDisabledGroup();

            serializedObject.ApplyModifiedProperties();
        }
    }
}