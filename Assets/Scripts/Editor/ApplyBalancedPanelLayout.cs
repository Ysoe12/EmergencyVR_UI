using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class ApplyBalancedPanelLayout
{
    [MenuItem("EmergencyVR/Apply Balanced Panel Layout")]
    public static void Apply()
    {
        string scenePath = "Assets/Scenes/Evaluacion.unity";
        var scene = EditorSceneManager.OpenScene(scenePath);

        GameObject canvasMenu = GameObject.Find("CanvasMenu");
        if (canvasMenu == null)
        {
            Debug.LogError("CanvasMenu not found!");
            return;
        }

        Transform panelTrans = canvasMenu.transform.Find("Panel") ?? canvasMenu.transform.GetChild(0);
        if (panelTrans == null)
        {
            Debug.LogError("Panel not found!");
            return;
        }

        // Find existing TMP font
        TMP_FontAsset sharedFont = null;
        foreach (var tmp in canvasMenu.GetComponentsInChildren<TextMeshProUGUI>(true))
        {
            if (tmp.font != null)
            {
                sharedFont = tmp.font;
                break;
            }
        }
        if (sharedFont == null)
        {
            sharedFont = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>("Assets/TextMesh Pro/Resources/Fonts & Materials/LiberationSans SDF.asset");
        }

        // Ensure images are imported as Sprites
        string[] imagePaths = {
            "Assets/UI/ImagenCalle.jpeg",
            "Assets/UI/ImagenCasa.jpeg",
            "Assets/UI/ImagenPsicina.jpeg"
        };

        Sprite[] sprites = new Sprite[3];
        for (int i = 0; i < 3; i++)
        {
            string path = imagePaths[i];
            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
            if (importer != null)
            {
                importer.textureType = TextureImporterType.Sprite;
                importer.spriteImportMode = SpriteImportMode.Single;
                importer.SaveAndReimport();
            }
            sprites[i] = AssetDatabase.LoadAssetAtPath<Sprite>(path);
        }

        // Find original LIBRE button to duplicate for ATRAS
        GameObject originalButton = null;
        foreach (Transform child in panelTrans)
        {
            if (child.name.ToLower().Contains("libre") || child.name.ToLower().Contains("boton") || child.name.ToLower().Contains("button"))
            {
                originalButton = child.gameObject;
                break;
            }
        }

        // Keep Logo, delete everything else inside Panel except original button if found
        Transform logoTransform = null;
        for (int i = panelTrans.childCount - 1; i >= 0; i--)
        {
            Transform child = panelTrans.GetChild(i);
            if (child.name == "Logo")
            {
                logoTransform = child;
            }
            else if (child.gameObject != originalButton)
            {
                Object.DestroyImmediate(child.gameObject);
            }
        }

        // 1. Logo with balanced top-left padding
        if (logoTransform != null)
        {
            logoTransform.SetParent(panelTrans, false);
            RectTransform logoRt = logoTransform.GetComponent<RectTransform>();
            logoRt.anchorMin = new Vector2(0f, 1f);
            logoRt.anchorMax = new Vector2(0f, 1f);
            logoRt.pivot = new Vector2(0f, 1f);
            logoRt.anchoredPosition = new Vector2(50f, -40f); // Generous top-left padding
        }

        // 2. Central Vertical Group for Title + Circles to guarantee balanced vertical distribution
        GameObject contentGroup = new GameObject("BalancedContentGroup", typeof(RectTransform), typeof(VerticalLayoutGroup));
        contentGroup.transform.SetParent(panelTrans, false);
        RectTransform groupRect = contentGroup.GetComponent<RectTransform>();
        groupRect.anchorMin = new Vector2(0.5f, 0.5f);
        groupRect.anchorMax = new Vector2(0.5f, 0.5f);
        groupRect.pivot = new Vector2(0.5f, 0.5f);
        groupRect.anchoredPosition = new Vector2(0f, -10f); // Centered vertically in panel
        groupRect.sizeDelta = new Vector2(1000f, 500f);

        VerticalLayoutGroup mainVlg = contentGroup.GetComponent<VerticalLayoutGroup>();
        mainVlg.childAlignment = TextAnchor.MiddleCenter;
        mainVlg.spacing = 40f; // Spacing between title and circles
        mainVlg.childControlWidth = false;
        mainVlg.childControlHeight = false;
        mainVlg.childForceExpandWidth = false;
        mainVlg.childForceExpandHeight = false;

        // Title "MODO: EVALUACIÓN" inside central group
        GameObject titleObj = new GameObject("Title_ModoEvaluacion", typeof(RectTransform), typeof(TextMeshProUGUI));
        titleObj.transform.SetParent(contentGroup.transform, false);
        RectTransform titleRect = titleObj.GetComponent<RectTransform>();
        titleRect.sizeDelta = new Vector2(800f, 60f);
        
        TextMeshProUGUI titleText = titleObj.GetComponent<TextMeshProUGUI>();
        if (sharedFont != null) titleText.font = sharedFont;
        titleText.text = "MODO: EVALUACIÓN";
        titleText.fontSize = 38;
        titleText.fontStyle = FontStyles.Bold;
        titleText.color = Color.white;
        titleText.alignment = TextAlignmentOptions.Center;

        // Horizontal Layout Group for 3 circular scenario items inside central group
        GameObject hlgObj = new GameObject("ScenarioCirclesContainer", typeof(RectTransform), typeof(HorizontalLayoutGroup));
        hlgObj.transform.SetParent(contentGroup.transform, false);
        RectTransform hlgRect = hlgObj.GetComponent<RectTransform>();
        hlgRect.sizeDelta = new Vector2(1000f, 360f);

        HorizontalLayoutGroup hlg = hlgObj.GetComponent<HorizontalLayoutGroup>();
        hlg.childAlignment = TextAnchor.MiddleCenter;
        hlg.spacing = 60f;
        hlg.childControlWidth = false;
        hlg.childControlHeight = false;
        hlg.childForceExpandWidth = false;
        hlg.childForceExpandHeight = false;

        string[] names = { "CALLE", "CASA", "PISCINA" };
        bool[] highlights = { false, true, false };

        for (int i = 0; i < 3; i++)
        {
            float size = highlights[i] ? 230f : 190f;

            GameObject itemObj = new GameObject($"ScenarioItem_{names[i]}", typeof(RectTransform), typeof(VerticalLayoutGroup), typeof(Button), typeof(ScenarioItemEffect));
            itemObj.transform.SetParent(hlgObj.transform, false);
            RectTransform itemRect = itemObj.GetComponent<RectTransform>();
            itemRect.sizeDelta = new Vector2(260f, 340f);

            VerticalLayoutGroup vlg = itemObj.GetComponent<VerticalLayoutGroup>();
            vlg.childAlignment = TextAnchor.UpperCenter;
            vlg.spacing = 15f;
            vlg.childControlWidth = false;
            vlg.childControlHeight = false;
            vlg.childForceExpandWidth = false;
            vlg.childForceExpandHeight = false;

            // Circle Container
            GameObject circleContainer = new GameObject($"Circle_{names[i]}", typeof(RectTransform));
            circleContainer.transform.SetParent(itemObj.transform, false);
            RectTransform circleRect = circleContainer.GetComponent<RectTransform>();
            circleRect.sizeDelta = new Vector2(size, size);

            // Glow border around circle
            GameObject glowObj = new GameObject("GlowBorder", typeof(RectTransform), typeof(Image));
            glowObj.transform.SetParent(circleContainer.transform, false);
            RectTransform glowRect = glowObj.GetComponent<RectTransform>();
            glowRect.anchorMin = Vector2.zero; glowRect.anchorMax = Vector2.one;
            glowRect.sizeDelta = new Vector2(18f, 18f);
            glowRect.anchoredPosition = Vector2.zero;
            Image glowImg = glowObj.GetComponent<Image>();
            glowImg.color = new Color(0.4f, 0.4f, 0.4f, 0.4f);

            // Circular Mask Container
            GameObject maskObj = new GameObject("CircularMask", typeof(RectTransform), typeof(Image), typeof(Mask));
            maskObj.transform.SetParent(circleContainer.transform, false);
            RectTransform maskRect = maskObj.GetComponent<RectTransform>();
            maskRect.anchorMin = Vector2.zero; maskRect.anchorMax = Vector2.one;
            maskRect.sizeDelta = Vector2.zero;
            maskRect.anchoredPosition = Vector2.zero;
            Mask mask = maskObj.GetComponent<Mask>();
            mask.showMaskGraphic = false;

            Image maskImg = maskObj.GetComponent<Image>();
            maskImg.color = Color.white;

            // Image inside mask
            GameObject imgObj = new GameObject("ScenarioImage", typeof(RectTransform), typeof(Image));
            imgObj.transform.SetParent(maskObj.transform, false);
            RectTransform imgRect = imgObj.GetComponent<RectTransform>();
            imgRect.anchorMin = Vector2.zero; imgRect.anchorMax = Vector2.one;
            imgRect.sizeDelta = Vector2.zero;
            imgRect.anchoredPosition = Vector2.zero;
            Image scenarioImg = imgObj.GetComponent<Image>();
            if (sprites[i] != null)
            {
                scenarioImg.sprite = sprites[i];
            }
            scenarioImg.color = Color.white;
            scenarioImg.preserveAspect = false;

            ScenarioItemEffect effect = itemObj.GetComponent<ScenarioItemEffect>();
            effect.glowImage = glowImg;
            effect.normalColor = new Color(0.4f, 0.4f, 0.4f, 0.4f);
            effect.hoverColor = new Color(0.2f, 0.85f, 1f, 0.95f);

            // Text: Scenario Name
            GameObject nameObj = new GameObject("Text_Name", typeof(RectTransform), typeof(TextMeshProUGUI));
            nameObj.transform.SetParent(itemObj.transform, false);
            RectTransform nameRect = nameObj.GetComponent<RectTransform>();
            nameRect.sizeDelta = new Vector2(240f, 40f);
            
            TextMeshProUGUI nameT = nameObj.GetComponent<TextMeshProUGUI>();
            if (sharedFont != null) nameT.font = sharedFont;
            nameT.text = names[i];
            nameT.fontSize = 22;
            nameT.fontStyle = FontStyles.Bold;
            nameT.color = Color.white;
            nameT.alignment = TextAlignmentOptions.Center;
        }

        // 3. Back Button ("ATRÁS") in bottom-left corner with balanced padding
        if (originalButton != null)
        {
            GameObject backBtnObj = Object.Instantiate(originalButton, panelTrans);
            backBtnObj.name = "Button_Atras";
            RectTransform backRect = backBtnObj.GetComponent<RectTransform>();
            backRect.anchorMin = new Vector2(0f, 0f);
            backRect.anchorMax = new Vector2(0f, 0f);
            backRect.pivot = new Vector2(0f, 0f);
            backRect.anchoredPosition = new Vector2(50f, 40f); // Balanced bottom-left padding

            foreach (Transform child in backBtnObj.transform)
            {
                if (child.name.ToLower().Contains("icon") || child.name.ToLower().Contains("imagen") || child.name.ToLower().Contains("img"))
                {
                    Object.DestroyImmediate(child.gameObject);
                }
            }

            TextMeshProUGUI btnText = backBtnObj.GetComponentInChildren<TextMeshProUGUI>(true);
            if (btnText != null)
            {
                btnText.text = "◄ ATRÁS";
                btnText.alignment = TextAlignmentOptions.Center;
            }

            LoadSceneButton loadScript = backBtnObj.GetComponent<LoadSceneButton>() ?? backBtnObj.AddComponent<LoadSceneButton>();
            loadScript.sceneName = "Menu";

            Button btnComponent = backBtnObj.GetComponent<Button>();
            if (btnComponent != null)
            {
                btnComponent.onClick.RemoveAllListeners();
                #if UNITY_EDITOR
                UnityEditor.Events.UnityEventTools.AddPersistentListener(btnComponent.onClick, loadScript.LoadTargetScene);
                #endif
            }

            Object.DestroyImmediate(originalButton);
        }

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
        Debug.Log("Balanced panel layout applied successfully!");
    }
}
