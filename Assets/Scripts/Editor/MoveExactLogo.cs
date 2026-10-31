using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class MoveExactLogo
{
    [MenuItem("EmergencyVR/Move Exact Logo Top Left")]
    public static void Move()
    {
        string scenePath = "Assets/Scenes/Evaluacion.unity";
        var scene = EditorSceneManager.OpenScene(scenePath);

        GameObject canvasMenu = GameObject.Find("CanvasMenu");
        if (canvasMenu == null)
        {
            Debug.LogError("CanvasMenu not found!");
            return;
        }

        Transform logoTrans = canvasMenu.transform.Find("Panel/Logo") ?? canvasMenu.transform.Find("Logo") ?? FindDeep(canvasMenu.transform, "Logo");
        if (logoTrans != null)
        {
            RectTransform rt = logoTrans.GetComponent<RectTransform>();
            if (rt != null)
            {
                rt.anchorMin = new Vector2(0f, 1f);
                rt.anchorMax = new Vector2(0f, 1f);
                rt.pivot = new Vector2(0f, 1f);
                rt.anchoredPosition = new Vector2(40f, -40f); // Top left with padding
                Debug.Log("Successfully moved 'Logo' object to top-left of panel.");
            }
        }
        else
        {
            Debug.LogError("Could not find object named 'Logo'!");
        }

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
    }

    private static Transform FindDeep(Transform parent, string name)
    {
        foreach (Transform child in parent)
        {
            if (child.name == name) return child;
            Transform found = FindDeep(child, name);
            if (found != null) return found;
        }
        return null;
    }
}
