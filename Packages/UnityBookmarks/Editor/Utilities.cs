using System.Text;
using UnityEditor;
using UnityEngine;

namespace QuantumCalzone
{
    public static class Utilities
    {
        public static Bookmarks Bookmarks {
            get {
                var bookmarks = GetAllInstances<Bookmarks>();
                if (bookmarks.Length == 0)
                {
                    var newBookmarks = ScriptableObject.CreateInstance(typeof(Bookmarks)) as Bookmarks;
                    AssetDatabase.CreateAsset(newBookmarks, string.Format("Assets/{0}.asset", typeof(Bookmarks).Name));
                    AssetDatabase.SaveAssets();
                    return newBookmarks;
                } else if (bookmarks.Length > 1)
                {
                    Debug.LogError(string.Format("Sorry! This only supports 1 Bookmark asset. Please remove {0} of the ones logged below so that only 1 remains.", bookmarks.Length - 1));
                    for (var i = 0; i < bookmarks.Length; i++)
                    {
                        Debug.Log(bookmarks[i].name, bookmarks[i]);
                    }
                }

                return bookmarks[0];
            }
        }

        public static T[] GetAllInstances<T>() where T : ScriptableObject
        {
            var guids = AssetDatabase.FindAssets("t:" + typeof(T).Name);  //FindAssets uses tags check documentation for more info
            T[] a = new T[guids.Length];
            for (var i = 0; i < guids.Length; i++)
            {
                var path = AssetDatabase.GUIDToAssetPath(guids[i]);
                a[i] = AssetDatabase.LoadAssetAtPath<T>(path);
            }

            return a;
        }

        public static void DrawLabelCenteredBold(string s)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            EditorGUILayout.LabelField(s);
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }

        public static string AddSpacesToSentence(string text, bool preserveAcronyms)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return string.Empty;
            }

            var newText = new StringBuilder(text.Length * 2);
            newText.Append(text[0]);
            for (var i = 1; i < text.Length; i++)
            {
                if (char.IsUpper(text[i]))
                {
                    if ((text[i - 1] != ' ' && !char.IsUpper(text[i - 1])) ||
                        (preserveAcronyms && char.IsUpper(text[i - 1]) &&
                         i < text.Length - 1 && !char.IsUpper(text[i + 1])))
                    {
                        newText.Append(' ');
                    }
                }

                newText.Append(text[i]);
            }

            return newText.ToString();
        }
    }
}
