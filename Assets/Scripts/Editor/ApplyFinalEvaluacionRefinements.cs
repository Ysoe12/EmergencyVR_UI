using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class ApplyFinalEvaluacionRefinements
{
    [MenuItem("EmergencyVR/Apply Final Evaluacion Refinements")]
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

        // Find existing TMP font to maintain consistency
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

        // Keep Logo, delete everything else inside Panel (except we want to inspect or rebuild cleanly)
        Transform logoTransform = null;
        for (int i = panelTrans.childCount - 1; i >= 0; i--)
        {
            Transform child = panelTrans.GetChild(i);
            if (child.name == "Logo")
            {
                logoTransform = child;
            }
            else
            {
                Object.DestroyImmediate(child.gameObject);
            }
        }

        // 1. Position Logo cleanly in top-left corner
        if (logoTransform != null)
        {
            logoTransform.SetParent(panelTrans, false);
            RectTransform logoRt = logoTransform.GetComponent<RectTransform>();
            logoRt.anchorMin = new Vector2(0f, 1f);
            logoRt.anchorMax = new Vector2(0f, 1f);
            logoRt.pivot = new Vector2(0f, 1f);
            logoRt.anchoredPosition = new Vector2(35f, -25f);
        }

        // 2. Title "MODO: EVALUACIÓN" (white, original style/size, centered at top)
        GameObject titleObj = new GameObject("Title_ModoEvaluacion", typeof(RectTransform), typeof(TextMeshProUGUI));
        titleObj.transform.SetParent(panelTrans, false);
        RectTransform titleRect = titleObj.GetComponent<RectTransform>();
        titleRect.anchorMin = new Vector2(0.5f, 1f);
        titleRect.anchorMax = new Vector2(0.5f, 1f);
        titleRect.pivot = new Vector2(0.5f, 1f);
        titleRect.anchoredPosition = new Vector2(0f, -40f);
        titleRect.sizeDelta = new Vector2(500f, 60f);
        
        TextMeshProUGUI titleText = titleObj.GetComponent<TextMeshProUGUI>();
        if (sharedFont != null) titleText.font = sharedFont;
        titleText.text = "MODO: EVALUACIÓN";
        titleText.fontSize = 28;
        titleText.fontStyle = FontStyles.Bold;
        titleText.color = Color.white;
        titleText.alignment = TextAlignmentOptions.Center;

        // 3. Horizontal Layout Group for 3 circular scenario items - larger size to fill panel
        GameObject hlgObj = new GameObject("ScenarioCirclesContainer", typeof(RectTransform), typeof(HorizontalLayoutGroup));
        hlgObj.transform.SetParent(panelTrans, false);
        RectTransform hlgRect = hlgObj.GetComponent<RectTransform>();
        hlgRect.anchorMin = new Vector2(0.5f, 0.5f);
        hlgRect.anchorMax = new Vector2(0.5f, 0.5f);
        hlgRect.pivot = new Vector2(0.5f, 0.5f);
        hlgRect.anchoredPosition = new Vector2(0f, -10f);
        hlgRect.sizeDelta = new Vector2(1000f, 380f);

        HorizontalLayoutGroup hlg = hlgObj.GetComponent<HorizontalLayoutGroup>();
        hlg.childAlignment = TextAnchor.MiddleCenter;
        hlg.spacing = 60f;
        hlg.childControlWidth = false;
        hlg.childControlHeight = false;
        hlg.childForceExpandWidth = false;
        hlg.childForceExpandHeight = false;

        string[] names = { "CALLE", "CASA", "PISCINA" };
        bool[] highlights = { false, true, false }; // Center one is larger

        for (int i = 0; i < 3; i++)
        {
            float size = highlights[i] ? 230f : 190f; // Much larger circles to fill panel

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

            // Image inside mask (fully filling the circle)
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
            scenarioImg.preserveAspect = false; // Stretch to fill circular mask perfectly

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

        // 4. Back Button ("ATRAS") in bottom-left corner of panel, reusing button style and LoadSceneButton
        GameObject backBtnObj = new GameObject("Button_Atras", typeof(RectTransform), typeof(Image), typeof(Button), typeof(LoadSceneButton));
        backBtnObj.transform.SetParent(panelTrans, false);
        RectTransform backRect = backBtnObj.GetComponent<RectTransform>();
        backRect.anchorMin = new Vector2(0f, 0f);
        backRect.anchorMax = new Vector2(0f, 0f);
        backRect.pivot = new Vector2(0f, 0f);
        backRect.anchoredPosition = new Vector2(40f, 30f);
        backRect.sizeDelta = new Vector2(180f, 55f);

        Image backBg = backBtnObj.GetComponent<Image>();
        backBg.color = new Color(0.15f, 0.15f, 0.2f, 0.9f); // Dark semi-transparent pill style

        Button backButton = backBtnObj.GetComponent<Button>();
        LoadSceneButton loadScript = backBtnObj.GetComponent<LoadSceneButton>();
        loadScript.sceneName = "Menu";
        
        // Add onClick event binding
        // UnityAction action = new UnityAction(loadScript.LoadTargetScene);
        // UnityEventTools.AddPersistentListener(backButton.onClick, loadScript.LoadTargetScene);
        // Alternatively, add runtime listener or set up in editor. For persistent:
        #if UNITY_EDITOR
        UnityEditor.Events.UnityEventTools.AddPersistentListener(backButton.onClick, loadScript.LoadTargetScene);
        #endif

        // Text inside back button ("ATRÁS")
        GameObject backTextObj = new GameObject("Text_Atras", typeof(RectTransform), typeof(TextMeshProUGUI));
        backTextObj.transform.SetParent(backBtnObj.transform, false);
        RectTransform backTextRect = backTextObj.GetComponent<RectTransform>();
        backTextRect.anchorMin = Vector2.zero; backTextRect.anchorMax = Vector2.one;
        backTextRect.sizeDelta = Vector2.zero;
        backTextRect.anchoredPosition = Vector2.zero;

        TextMeshProUGUI backText = backTextObj.GetComponent<TextMeshProUGUI>();
        if (sharedFont != null) backText.font = sharedFont;
        backText.text = "◄ ATRÁS";
        backText.fontSize = 18;
        backText.fontStyle = FontStyles.Bold;
        backText.color = Color.white;
        backText.alignment = TextAlignmentOptions.Center;

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
        Debug.Log("Evaluacion panel final refinements applied successfully!");
    }
}
