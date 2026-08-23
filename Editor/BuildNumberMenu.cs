using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace ADKOM
{
    /// <summary>
    /// Adds a Tools &gt; ADKOM Build Number menu with a read-only display of the
    /// current build number and manual increment/decrement commands. The display
    /// item's label embeds the live value, which <see cref="MenuItem"/> cannot do
    /// (its label is a compile-time constant), so that one item is registered
    /// through Unity's internal <c>Menu.AddMenuItem</c> API via reflection.
    /// </summary>
    [InitializeOnLoad]
    internal static class BuildNumberMenu
    {
        private const string MenuRoot = "Tools/ADKOM Build Number/";

        private static readonly MethodInfo AddMenuItemMethod = typeof(Menu).GetMethod(
            "AddMenuItem", BindingFlags.Static | BindingFlags.NonPublic);

        private static readonly MethodInfo RemoveMenuItemMethod = typeof(Menu).GetMethod(
            "RemoveMenuItem", BindingFlags.Static | BindingFlags.NonPublic);

        /// <summary>Menu path of the display item registered for this domain, if any.</summary>
        private static string _currentItemPath;

        static BuildNumberMenu()
        {
            // The menu system isn't ready to be modified while InitializeOnLoad
            // constructors run, so defer the first registration one editor tick.
            EditorApplication.delayCall += RefreshCurrentItem;
        }

        [MenuItem(MenuRoot + "Increment Build Number", false, 20)]
        private static void Increment() => Shift(+1);

        [MenuItem(MenuRoot + "Decrement Build Number", false, 21)]
        private static void Decrement() => Shift(-1);

        [MenuItem(MenuRoot + "Decrement Build Number", true)]
        private static bool ValidateDecrement() => BuildNumberAutoIncrement.ReadCurrent() > 0;

        private static void Shift(int delta)
        {
            int value = Mathf.Max(0, BuildNumberAutoIncrement.ReadCurrent() + delta);
            BuildNumberAutoIncrement.Write(value);
            Debug.Log($"[ADKOM] Build number set to {value}");
            RefreshCurrentItem();
        }

        /// <summary>
        /// (Re)registers the disabled "Current Build Number N" display item with
        /// the value currently on disk. Dynamic menu items don't survive a domain
        /// reload, so this runs once per reload and again after every manual
        /// change. If the internal API is ever removed, the display item is
        /// simply omitted and the two command items keep working.
        /// </summary>
        private static void RefreshCurrentItem()
        {
            if (AddMenuItemMethod == null || AddMenuItemMethod.GetParameters().Length != 6)
                return;

            string path = MenuRoot + "Current Build Number " + BuildNumberAutoIncrement.ReadCurrent();
            if (path == _currentItemPath)
                return;

            try
            {
                if (_currentItemPath != null)
                    RemoveMenuItemMethod?.Invoke(null, new object[] { _currentItemPath });

                // (name, shortcut, checked, priority, execute, validate) — validate
                // returning false keeps the item permanently disabled: display only.
                AddMenuItemMethod.Invoke(null, new object[]
                {
                    path, "", false, 0, (Action)(() => { }), (Func<bool>)(() => false)
                });
                _currentItemPath = path;
            }
            catch (Exception)
            {
                // Internal API changed shape — skip the display item.
            }
        }
    }
}
