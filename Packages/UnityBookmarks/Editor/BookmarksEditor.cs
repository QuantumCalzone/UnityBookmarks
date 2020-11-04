#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace QuantumCalzone
{
    [CustomEditor(typeof(Bookmarks))]
    public class BookmarksEditor : Editor
    {
        private SerializedProperty assetsPaths = null;
        private SerializedProperty scenes = null;
        private ReorderableList assetsPathsReorderableList = null;
        private ReorderableList scenesReorderableList = null;

        private void OnEnable()
        {
            assetsPaths = serializedObject.FindProperty("assetsPaths");
            scenes = serializedObject.FindProperty("scenes");
            assetsPathsReorderableList = new ReorderableList(serializedObject, assetsPaths);
            scenesReorderableList = new ReorderableList(serializedObject, scenes);
            assetsPathsReorderableList.drawHeaderCallback += DrawHeader;
            scenesReorderableList.drawHeaderCallback += DrawHeader;
            assetsPathsReorderableList.drawElementCallback += DrawAssetElement;
            scenesReorderableList.drawElementCallback += DrawSceneElement;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            if (GUILayout.Button("Open Window"))
            {
                BookmarkEditorWindow.Init();
            }

            //EditorGUILayout.PropertyField(assetsPaths, true);
            //EditorGUILayout.PropertyField(scenes, true);

            EditorGUILayout.LabelField("Assets");
            assetsPathsReorderableList.DoLayoutList();
            EditorGUILayout.LabelField("Scenes");
            scenesReorderableList.DoLayoutList();

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawHeader(Rect rect)
        {
        }

        private void DrawAssetElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            var property = assetsPathsReorderableList.serializedProperty;
            DrawElement(property, rect, index, isActive, isFocused);
        }

        private void DrawSceneElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            var property = scenesReorderableList.serializedProperty;
            DrawElement(property, rect, index, isActive, isFocused);
        }

        private void DrawElement(SerializedProperty property, Rect rect, int index, bool isActive, bool isFocused)
        {
            property = property.GetArrayElementAtIndex(index);
            EditorGUI.PropertyField(rect, property, new GUIContent(""), true);
        }
    }
}
#endif
