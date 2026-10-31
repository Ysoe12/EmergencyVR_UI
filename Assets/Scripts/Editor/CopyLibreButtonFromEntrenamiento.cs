using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class CopyLibreButtonFromEntrenamiento
{
    [MenuItem("EmergencyVR/Copy Libre Button from Entrenamiento")]
    public static void CopyButton()
    {
        // 1. Open Entrenamiento scene
        EditorSceneManager.OpenScene("Assets/Scenes/Entrenamiento.unity");
        GameObject libreButton = null;
        foreach (var root in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects())
        {
            Transform found = FindDeep(root.transform, "Libre");
            if (found != null)
            {
                libreButton = found.gameObject;
                break;
            }
        }

        if (libreButton == null)
        {
            Debug.LogError("Could not find 'Libre' button in Entrenamiento scene!");
            return;
        }

        // Save as temporary prefab
        System.IO.Directory.CreateDirectory("Assets/Temp");
        string prefabPath = "Assets/Temp/TempLibreButton.prefab";
        
        // Remove existing temp prefab if any
        AssetDatabase.DeleteAsset(prefabPath);

        GameObject prefab = PrefabUtility.SaveAsPrefabAsset(libreButton, prefabPath);
        if (prefab == null)
        {
            Debug.LogError("Failed to save temporary prefab of Libre button!");
            return;
        }

        // 2. Open Evaluacion scene
        EditorSceneManager.OpenScene("Assets/Scenes/Evaluacion.unity");
        GameObject canvasMenu = GameObject.Find("CanvasMenu");
        if (canvasMenu == null)
        {
            Debug.LogError("Evaluacion CanvasMenu not found!");
            return;
        }

        Transform panelTrans = canvasMenu.transform.Find("Panel") ?? canvasMenu.transform.GetChild(0);
        if (panelTrans == null)
        {
            Debug.LogError("Evaluacion Panel not found!");
            return;
        }

        // Remove old Button_Atras or duplicate Libre if any
        Transform oldAtras = panelTrans.Find("Button_Atras") ?? panelTrans.Find("Libre");
        if (oldAtras != null)
        {
            Object.DestroyImmediate(oldAtras.gameObject);
        }

        // Instantiate prefab into Evaluacion panel
        GameObject instantiatedBtn = (GameObject)PrefabUtility.InstantiatePrefab(prefab, panelTrans);
        instantiatedBtn.name = "Libre";

        RectTransform rect = instantiatedBtn.GetComponent<RectTransform>();
        if (rect != null)
        {
            rect.anchorMin = new Vector2(0f, 0f);
            rect.anchorMax = new Vector2(0f, 0f);
            rect.pivot = new Vector2(0f, 0f);
            rect.anchoredPosition = new Vector2(50f, 40f);
        }

        // Clean up temp prefab asset
        AssetDatabase.DeleteAsset(prefabPath);

        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
        Debug.Log("Successfully copied 'Libre' button from Entrenamiento to Evaluacion panel via prefab!");
    }

    private static Transform FindDeep(Transform parent, string name)
    {
        foreach (Transform child in parent)
        {
            if (child.name == name || child.name.ToLower().Contains(name.ToLower())) return child;
            Transform found = FindDeep(child, name);
            if (found != null) return found;
        }
        return null;
    }
}
