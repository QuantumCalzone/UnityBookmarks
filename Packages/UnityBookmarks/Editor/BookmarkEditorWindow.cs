#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

namespace QuantumCalzone
{
    public class BookmarkEditorWindow : EditorWindow
    {
        private const string menuItemItemName = "Window/Bookmarks";
        private const int menuItemPriority = 9999;
        private const string sceneDirectory = "Assets/Scenes/";
        private Bookmarks bookmarks = null;
        private Vector2 scrollPosition = Vector2.zero;

        [MenuItem(itemName: menuItemItemName, isValidateFunction: false, priority: menuItemPriority)]
        public static void Init()
        {
            var window = EditorWindow.GetWindow(typeof(BookmarkEditorWindow));
            window.titleContent = new GUIContent("Bookmarks");
            window.Show();
        }

        private void OnGUI()
        {
            if(!bookmarks)
            {
                bookmarks = Utilities.Bookmarks;
            }

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            if(bookmarks.AssetPaths.Length == 0 && bookmarks.Scenes.Length == 0)
            {
                EditorGUILayout.HelpBox(new GUIContent("Add/Remove Bookmarks asset's context menu"));
            }
            else
            {
                GUILayout.Label("Assets");

                if(bookmarks.AssetPaths.Length == 0)
                {
                    EditorUtilities.DrawLabelCenteredBold("None");
                }
                else
                {
                    for(var i = 0; i < bookmarks.AssetPaths.Length; i++)
                    {
                        var assetPath = bookmarks.AssetPaths[i];

                        if(!string.IsNullOrEmpty(assetPath.Replace(" ", string.Empty)))
                        {
                            var selectAssetLabel = Path.GetFileNameWithoutExtension(assetPath);
                            selectAssetLabel = StringUtilities.AddSpacesToSentence(selectAssetLabel, false);

                            if(GUILayout.Button(selectAssetLabel))
                            {
                                var asset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(assetPath);

                                if(!asset)
                                {
                                    EditorDialogModal.Display(
                                        "Invalid",
                                        string.Format("Could not find {0} at path {1}\nWould you like to unbookmark it?", selectAssetLabel, assetPath),
                                        new DialogButton("No", null),
                                        "Cancel",
                                        new DialogButton("Yes", () =>
                                        {
                                            Utilities.Bookmarks.Unbookmark(assetPath, false);
                                        })
                                    );
                                }
                                else
                                {
                                    Selection.activeObject = asset;
                                    EditorGUIUtility.PingObject(Selection.activeObject);
                                }
                            }
                        }
                    }
                }

                GUILayout.Label("Scenes");
                if(bookmarks.Scenes.Length == 0)
                {
                    EditorUtilities.DrawLabelCenteredBold("None");
                }
                else
                {
                    for(var i = 0; i < bookmarks.Scenes.Length; i++)
                    {
                        var scenePath = bookmarks.Scenes[i];
                        var sceneName = Path.GetFileName(scenePath);

                        if (!string.IsNullOrEmpty(sceneName.Replace(" ", string.Empty)))
                        {
                            var openSceneLabel = StringUtilities.AddSpacesToSentence(sceneName, false);
                            openSceneLabel = openSceneLabel.Replace(".unity", string.Empty);

                            if (GUILayout.Button(openSceneLabel))
                            {
                                var asset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(scenePath);

                                if (!asset)
                                {
                                    EditorDialogModal.Display(
                                        "Invalid",
                                        string.Format("Could not find scene {0}\nWould you like to unbookmark it?", scenePath),
                                        new DialogButton("No", null),
                                        "Cancel",
                                        new DialogButton("Yes", () =>
                                        {
                                            Utilities.Bookmarks.Unbookmark(scenePath, true);
                                        })
                                    );
                                }
                                else
                                {
                                    UnityEditor.SceneManagement.EditorSceneManager.OpenScene(
                                        scenePath, UnityEditor.SceneManagement.OpenSceneMode.Single);
                                }
                            }
                        }
                    }
                }
            }

            EditorGUILayout.EndScrollView();

            Repaint();
        }
    }
}
#endif
