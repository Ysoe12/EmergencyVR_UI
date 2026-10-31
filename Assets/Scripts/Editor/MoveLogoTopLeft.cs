using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using TMPro;

public class MoveLogoTopLeft
{
    [MenuItem("EmergencyVR/Move Logo Top Left")]
    public static void Move()
    {
        string scenePath = "Assets/Scenes/Evaluacion.unity";
        var scene = EditorSceneManager.OpenScene(scenePath);

        // Find CanvasMenu
        GameObject canvasMenu = null;
        foreach (var root in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects())
        {
            if (root.name == "CanvasMenu" || root.GetComponentInChildren<Canvas>() != null)
            {
                canvasMenu = root;
                break;
            }
        }

        if (canvasMenu == null)
        {
            Debug.LogError("CanvasMenu not found!");
            return;
        }

        // Find text or object containing "EMERGENCYVR" or similar in CanvasMenu
        TextMeshProUGUI[] texts = canvasMenu.GetComponentsInChildren<TextMeshProUGUI>(true);
        TextMeshProUGUI targetText = null;
        foreach (var t in texts)
        {
            if (t.text.ToUpper().Contains("EMERGENCY") || t.gameObject.name.ToLower().Contains("logo") || t.gameObject.name.ToLower().Contains("title"))
            {
                targetText = t;
                break;
            }
        }

        if (targetText != null)
        {
            RectTransform rt = targetText.GetComponent<RectTransform>();
            // Set anchors to top-left
            rt.anchorMin = new Vector2(0f, 1f);
            rt.anchorMax = new Vector2(0f, 1f);
            rt.pivot = new Vector2(0f, 1f);
            rt.anchoredPosition = new Vector2(40f, -30f); // Top-left padding inside panel
            
            // Optionally make it slightly smaller / compact if it was huge/centered
            rt.sizeDelta = new Vector2(250f, 50f);
            targetText.alignment = TextAlignmentOptions.Left;

            Debug.Log($"Moved logo/text '{targetText.text}' to top-left corner.");
        }
        else
        {
            Debug.LogWarning("Could not find EMERGENCYVR text object automatically.");
        }

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
        Debug.Log("Moved EMERGENCYVR logo to top left in Evaluacion scene.");
    }
}
