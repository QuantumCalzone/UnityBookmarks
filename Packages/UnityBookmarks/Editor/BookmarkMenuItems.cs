#if UNITY_EDITOR
using UnityEditor;

namespace UnityBookmarks
{
    public static class BookmarkMenuItems
    {
        private const string bookmarkMenuItemName = "Assets/Bookmark";
        private const int bookmarkMenuItemPriority = 9999;
        private const string unbookmarkMenuItemName = "Assets/Unbookmark";
        private const int unbookmarkMenuItemPriority = 9999;

        private static string SelectionID
        {
            get
            {
                var selection = string.Empty;

                if (Selection.activeObject)
                {
                    if (AssetDatabase.IsMainAsset(Selection.activeObject))
                    {
                        selection = AssetDatabase.GetAssetPath(Selection.activeObject);
                    }
                }

                return selection;
            }
        }

        [MenuItem(itemName: bookmarkMenuItemName, priority = bookmarkMenuItemPriority)]
        private static void Bookmark()
        {
            Utilities.Bookmarks.Bookmark(SelectionID, IsScene(Selection.activeObject));
        }

        [MenuItem(itemName: bookmarkMenuItemName, priority = bookmarkMenuItemPriority, validate = true)]
        private static bool CanBookmark()
        {
            var doesNotContains = true;
            if (Utilities.BookmarksExists)
            {
                doesNotContains = !Utilities.Bookmarks.Contains(SelectionID, IsScene(Selection.activeObject));
            }

            return Selection.activeObject &&
                AssetDatabase.IsMainAsset(Selection.activeObject) &&
                doesNotContains;
        }

        [MenuItem(itemName: unbookmarkMenuItemName, priority = unbookmarkMenuItemPriority)]
        private static void Unbookmark()
        {
            Utilities.Bookmarks.Unbookmark(SelectionID, IsScene(Selection.activeObject));
        }

        [MenuItem(itemName: unbookmarkMenuItemName, priority = unbookmarkMenuItemPriority, validate = true)]
        private static bool CanUnbookmark()
        {
            var contains = false;
            if (Utilities.BookmarksExists)
            {
                contains = Utilities.Bookmarks.Contains(SelectionID, IsScene(Selection.activeObject));
            }

            return Selection.activeObject &&
                AssetDatabase.IsMainAsset(Selection.activeObject) &&
                contains;
        }

        private static bool IsScene(UnityEngine.Object target)
        {
            var targetPath = AssetDatabase.GetAssetPath(target);
            if (string.IsNullOrEmpty(targetPath))
            {
                return false;
            }

            return AssetDatabase.GetMainAssetTypeAtPath(targetPath) == typeof(SceneAsset);
        }
    }
}
#endif
