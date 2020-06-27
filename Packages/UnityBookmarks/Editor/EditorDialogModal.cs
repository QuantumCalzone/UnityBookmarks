using System;
using UnityEditor;
using UnityEngine;

namespace QuantumCalzone
{
    public class DialogButton
    {
        public DialogButton(string label, Action action)
        {
            this.label = label;
            this.action = action;
        }

        public string label = string.Empty;
        public Action action = null;

        public override string ToString()
        {
            return string.Format("| {0} {1} |", label, action != null);
        }
    }

    public static class EditorDialogModal
    {
        public static void Display(string title, string message, DialogButton ok, string cancel = null)
        {
            var triggerAction = false;

            if (string.IsNullOrEmpty(cancel))
            {
                triggerAction = EditorUtility.DisplayDialog(title, message, ok.label);
            }
            else
            {
                triggerAction = EditorUtility.DisplayDialog(title, message, ok.label, cancel);
            }

            if (triggerAction)
            {
                ok.action();
            }
        }

        public static void Display(string title, string message, DialogButton ok, string cancel, DialogButton alt)
        {
            var selection = EditorUtility.DisplayDialogComplex(title, message, ok.label, cancel, alt.label);

            switch (selection)
            {
                // ok.
                case 0:
                    ok.action();
                    break;
                // Cancel.
                case 1:
                    break;
                // alt
                case 2:
                    alt.action();
                    break;
                default:
                    Debug.LogError(string.Format("No support for a selection of: {0}", selection));
                    break;
            }
        }
    }
}
