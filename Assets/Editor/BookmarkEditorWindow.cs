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
                    Utilities.DrawLabelCenteredBold("None");
                }
                else
                {
                    for(var i = 0; i < bookmarks.AssetPaths.Length; i++)
                    {
                        var assetPath = bookmarks.AssetPaths[i];
                        if(!string.IsNullOrEmpty(assetPath.Replace(" ", string.Empty)))
                        {
                            var selectAssetLabel = Path.GetFileNameWithoutExtension(assetPath);
                            selectAssetLabel = Utilities.AddSpacesToSentence(selectAssetLabel, false);
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

                                    return;
                                }

                                Selection.activeObject = asset;
                                EditorGUIUtility.PingObject(Selection.activeObject);
                            }
                        }
                    }
                }

                GUILayout.Label("Scenes");
                if(bookmarks.Scenes.Length == 0)
                {
                    Utilities.DrawLabelCenteredBold("None");
                }
                else
                {
                    for(var i = 0; i < bookmarks.Scenes.Length; i++)
                    {
                        var sceneName = bookmarks.Scenes[i];
                        if(!string.IsNullOrEmpty(sceneName.Replace(" ", string.Empty)))
                        {
                            var openSceneLabel = Utilities.AddSpacesToSentence(sceneName, false);
                            if(GUILayout.Button(openSceneLabel))
                            {
                                sceneName = string.Format("{0}{1}.unity", sceneDirectory, sceneName);
                                if(UnityEditor.SceneManagement.EditorSceneManager.GetSceneByName(sceneName) == null)
                                {
                                    EditorDialogModal.Display(
                                        "Invalid",
                                        string.Format("Could not find scene {0}\nWould you like to unbookmark it?", sceneName),
                                        new DialogButton("No", null),
                                        "Cancel",
                                        new DialogButton("Yes", () =>
                                        {
                                            Utilities.Bookmarks.Unbookmark(sceneName, true);
                                        })
                                    );

                                    return;
                                }
                                UnityEditor.SceneManagement.EditorSceneManager.OpenScene(
                                    sceneName, UnityEditor.SceneManagement.OpenSceneMode.Single);
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
