using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using TMPro;

public class InspectCanvasMenu
{
    [MenuItem("EmergencyVR/Inspect CanvasMenu Hierarchy")]
    public static void Inspect()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/Evaluacion.unity");
        GameObject canvasMenu = null;
        foreach (var root in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects())
        {
            if (root.name.Contains("Canvas") || root.name.Contains("Menu"))
            {
                canvasMenu = root;
                Debug.Log($"Found Root Canvas/Menu: {root.name}");
                PrintChildren(root.transform, "");
            }
        }
    }

    private static void PrintChildren(Transform t, string indent)
    {
        foreach (Transform child in t)
        {
            RectTransform rt = child.GetComponent<RectTransform>();
            TextMeshProUGUI tmp = child.GetComponent<TextMeshProUGUI>();
            UnityEngine.UI.Text txt = child.GetComponent<UnityEngine.UI.Text>();
            string extra = "";
            if (tmp != null) extra = $" [TMP: '{tmp.text}']";
            if (txt != null) extra = $" [Text: '{txt.text}']";
            if (rt != null) extra += $" AnchorMin:{rt.anchorMin}, AnchorMax:{rt.anchorMax}, Pos:{rt.anchoredPosition}";

            Debug.Log($"{indent}- {child.name}{extra}");
            PrintChildren(child, indent + "  ");
        }
    }
}
