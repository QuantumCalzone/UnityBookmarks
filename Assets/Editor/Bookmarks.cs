using UnityEditor;
using UnityEngine;

namespace QuantumCalzone
{
    [CreateAssetMenu(fileName = "Bookmarks", menuName = "Scriptable Objects/Bookmarks", order = int.MaxValue)]
    public class Bookmarks : ScriptableObject
    {
        [SerializeField]
        private string[] assetsPaths = new string[0];
        public string[] AssetPaths { get { return assetsPaths; } }

        [SerializeField]
        private string[] scenes = new string[0];
        public string[] Scenes { get { return scenes; } }

        public bool Contains(string bookmark, bool isScene)
        {
            return ArrayUtility.Contains(!isScene ? assetsPaths : scenes, bookmark);
        }

        public void Bookmark(string bookmark, bool isScene)
        {
            if (!isScene)
            {
                ArrayUtility.Add(ref assetsPaths, bookmark);
            }
            else
            {
                ArrayUtility.Add(ref scenes, bookmark);
            }

            EditorUtility.SetDirty(Utilities.Bookmarks);
        }

        public void Unbookmark(string bookmark, bool isScene)
        {
            if (!isScene)
            {
                ArrayUtility.Remove(ref assetsPaths, bookmark);

            }
            else
            {
                ArrayUtility.Remove(ref scenes, bookmark);
            }

            EditorUtility.SetDirty(Utilities.Bookmarks);
        }
    }
}