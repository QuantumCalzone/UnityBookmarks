#if UNITY_EDITOR
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace QuantumCalzone
{
    public static class Utilities
    {
        public static Bookmarks Bookmarks {
            get {
                var bookmarks = EditorUtilities.GetAllInstances<Bookmarks>();
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
    }
}
#endif
