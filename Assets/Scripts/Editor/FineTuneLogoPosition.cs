using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class FineTuneLogoPosition
{
    [MenuItem("EmergencyVR/Fine Tune Logo Top Left Corner")]
    public static void Tune()
    {
        string scenePath = "Assets/Scenes/Evaluacion.unity";
        var scene = EditorSceneManager.OpenScene(scenePath);

        GameObject canvasMenu = GameObject.Find("CanvasMenu");
        if (canvasMenu == null) return;

        Transform logoTrans = canvasMenu.transform.Find("Panel/Logo") ?? canvasMenu.transform.Find("Logo") ?? FindDeep(canvasMenu.transform, "Logo");
        if (logoTrans != null)
        {
            RectTransform rt = logoTrans.GetComponent<RectTransform>();
            if (rt != null)
            {
                rt.anchorMin = new Vector2(0f, 1f);
                rt.anchorMax = new Vector2(0f, 1f);
                rt.pivot = new Vector2(0f, 1f);
                rt.anchoredPosition = new Vector2(15f, -15f); // Even closer to the top-left corner with minimal padding
                Debug.Log("Fine-tuned 'Logo' position to top-left corner.");
            }
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
