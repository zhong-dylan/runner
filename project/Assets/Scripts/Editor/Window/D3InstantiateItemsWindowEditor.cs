using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
public class D3InstantiateItemsWindowEditor : EditorWindow
{
    private static D3InstantiateItemsWindowEditor window;
    private D3PatternSystem pattern;
    SerializedObject patternSe;

    private float offset_X;
    private int indexPatternItem;

    public int[] sizeInt;
    public string[] stringSize;


    public static void Init()
    {
        // Get existing open window or if none, make a new one:
        window = (D3InstantiateItemsWindowEditor)EditorWindow.GetWindow(typeof(D3InstantiateItemsWindowEditor));
        window.Show();
    }

    void OnEnable()
    {
        if (FindObjectOfType<D3PatternSystem>())
        {
            pattern = FindObjectOfType<D3PatternSystem>();
            patternSe = new SerializedObject(pattern);
        }
    }

    void Update()
    {
        if (pattern == null)
        {
            GetTarget();
        }
        else
        {
            return;
        }
    }

    void OnSelectionChange()
    {
        GetTarget();
    }

    private void GetTarget()
    {
        if (Selection.gameObjects.Length > 0)
        {
            if (Selection.gameObjects[0].GetComponent<D3PatternSystem>() != null)
            {
                pattern = Selection.gameObjects[0].GetComponent<D3PatternSystem>();
                Debug.Log(pattern.name);
            }
        }
    }


    void OnGUI()
    {
        EditorGUI.BeginChangeCheck();

        const int width = 780;
        const int height = 990;

        var x = (Screen.currentResolution.width - width) / 2;
        var y = (Screen.currentResolution.height - height) / 2;


        offset_X = 0;
        pattern.SettingVariableFirst();

        GUI.Label(new Rect(10, 10, 720, 950), "Items for Instantiate", "box");


        if (GUI.Button(new Rect(125, 45, 120, 50), "Create \n Pattern Item"))
        {
            if (pattern != null)
            {
                pattern.ListItemsToInstantiate.Add(new D3PatternSystem.ItemToInstantiate());
                patternSe.Update();
                patternSe.ApplyModifiedProperties();
                indexPatternItem = pattern.ListItemsToInstantiate.Count - 1;
            }
        }
        EditorGUILayout.Space();


        if (GUI.Button(new Rect(310, 45, 120, 50), "Delete \n Last Pattern Item"))
        {
            if (pattern != null)
            {
                if (pattern.ListItemsToInstantiate.Count > 1)
                {
                    pattern.ListItemsToInstantiate.RemoveRange(pattern.ListItemsToInstantiate.Count - 1, 1);
                    indexPatternItem -= 1;
                }

                if (indexPatternItem <= 0)
                {
                    indexPatternItem = 0;
                }
            }
        }

        if (GUI.Button(new Rect(500, 45, 120, 50), "Modify \n Item Position"))
        {
            if (pattern != null)
            {
                WindowEditPositionItems window = (WindowEditPositionItems)EditorWindow.GetWindow(typeof(WindowEditPositionItems), false, " Position Items Editor");
                window.position = new Rect(x, y, width, height);
                window.pattern = pattern;
                window.selected = 0;
                window.Show();
            }
        }

        EditorGUILayout.Space();

        if (pattern != null)
        {
            if (pattern.ListItemsToInstantiate.Count > 0)
            {
                ButtonActiveIndex();

                SlotPatternItem();

                ShowDetial();
            }
        }

        if (EditorGUI.EndChangeCheck())
        {
            if (!Application.isPlaying)
            {
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            }
        }

    }

    private void ButtonActiveIndex()
    {
        if (pattern != null)
        {
            sizeInt = new int[pattern.ListItemsToInstantiate.Count];
            stringSize = new string[pattern.ListItemsToInstantiate.Count];
            for (int i = 0; i < sizeInt.Length; i++)
            {
                sizeInt[i] = i;
                stringSize[i] = (i).ToString();
            }
            EditorGUI.LabelField(new Rect(40, 100, 200, 50), "Total Pattern Item: " + pattern.ListItemsToInstantiate.Count);
            EditorGUI.LabelField(new Rect(300, 140, 200, 50), "Select Pattern Item To Edit: ");
            indexPatternItem = EditorGUI.IntPopup(new Rect(330, 190, 100, 20), "", indexPatternItem, stringSize, sizeInt);

            EditorGUILayout.Space();
        }
    }

    private void SlotPatternItem()
    {
        if (pattern != null)
        {
            Color normalColor = GUI.color;

            EditorGUI.LabelField(new Rect(160, 210, 200, 50), "Distance To Instantiate Objects: ");
            pattern.ListItemsToInstantiate[indexPatternItem].DistanceToInstantiateObject = EditorGUI.IntField(new Rect(370, 225, 200, 20), pattern.ListItemsToInstantiate[indexPatternItem].DistanceToInstantiateObject);

            for (int i = 0; i < pattern.ListItemsToInstantiate[indexPatternItem].patternItemToInstantiate.itemType_Left.Length; i++)
            {
                pattern.ListItemsToInstantiate[indexPatternItem].patternItemToInstantiate.itemType_Left[i].x = EditorGUI.IntField(new Rect(160 + offset_X, 270 + (i * 20), 20, 20), (int)pattern.ListItemsToInstantiate[indexPatternItem].patternItemToInstantiate.itemType_Left[i].x);
                if (pattern.ListItemsToInstantiate[indexPatternItem].patternItemToInstantiate.itemType_Left[i].x != 0 && (int)pattern.ListItemsToInstantiate[indexPatternItem].patternItemToInstantiate.itemType_Left[i].x <= pattern.ObjectToInstantiateObject.Count)
                {
                    float disZ = pattern.ObjectToInstantiateObject[(int)pattern.ListItemsToInstantiate[indexPatternItem].patternItemToInstantiate.itemType_Left[i].x - 1].GetComponent<D3Item>().distanceZ;
                    GUI.color = pattern.ObjectToInstantiateObject[(int)pattern.ListItemsToInstantiate[indexPatternItem].patternItemToInstantiate.itemType_Left[i].x - 1].GetComponent<D3Item>().colorPattern;
                    GUI.Box(new Rect(160 + offset_X, 270 + (i * 20), 20, 20), "");
                    if (disZ <= 1)
                        GUI.Box(new Rect(160 + offset_X, 270 + (i * 20), 20, 20 * (-(disZ - 1))), "");
                    else
                        GUI.Box(new Rect(160 + offset_X, 270 + (i * 20), 20, 20 * (-(disZ))), "");
                    GUI.color = normalColor;
                }
            }

            for (int i = 0; i < pattern.ListItemsToInstantiate[indexPatternItem].patternItemToInstantiate.itemType_Middle.Length; i++)
            {
                pattern.ListItemsToInstantiate[indexPatternItem].patternItemToInstantiate.itemType_Middle[i].x = EditorGUI.IntField(new Rect(210 + offset_X, 270 + (i * 20), 20, 20), (int)pattern.ListItemsToInstantiate[indexPatternItem].patternItemToInstantiate.itemType_Middle[i].x);
                if (pattern.ListItemsToInstantiate[indexPatternItem].patternItemToInstantiate.itemType_Middle[i].x != 0 && (int)pattern.ListItemsToInstantiate[indexPatternItem].patternItemToInstantiate.itemType_Middle[i].x <= pattern.ObjectToInstantiateObject.Count)
                {
                    float disZ = pattern.ObjectToInstantiateObject[(int)pattern.ListItemsToInstantiate[indexPatternItem].patternItemToInstantiate.itemType_Middle[i].x - 1].GetComponent<D3Item>().distanceZ;
                    GUI.color = pattern.ObjectToInstantiateObject[(int)pattern.ListItemsToInstantiate[indexPatternItem].patternItemToInstantiate.itemType_Middle[i].x - 1].GetComponent<D3Item>().colorPattern;
                    GUI.Box(new Rect(210 + offset_X, 270 + (i * 20), 20, 20), "");
                    if (disZ <= 1)
                        GUI.Box(new Rect(210 + offset_X, 270 + (i * 20), 20, 20 * (-(disZ - 1))), "");
                    else
                        GUI.Box(new Rect(210 + offset_X, 270 + (i * 20), 20, 20 * (-(disZ))), "");
                    GUI.color = normalColor;
                }
            }

            for (int i = 0; i < pattern.ListItemsToInstantiate[indexPatternItem].patternItemToInstantiate.itemType_Right.Length; i++)
            {
                pattern.ListItemsToInstantiate[indexPatternItem].patternItemToInstantiate.itemType_Right[i].x = EditorGUI.IntField(new Rect(260 + offset_X, 270 + (i * 20), 20, 20), (int)pattern.ListItemsToInstantiate[indexPatternItem].patternItemToInstantiate.itemType_Right[i].x);
                if (pattern.ListItemsToInstantiate[indexPatternItem].patternItemToInstantiate.itemType_Right[i].x != 0 && (int)pattern.ListItemsToInstantiate[indexPatternItem].patternItemToInstantiate.itemType_Right[i].x <= pattern.ObjectToInstantiateObject.Count)
                {
                    float disZ = pattern.ObjectToInstantiateObject[(int)pattern.ListItemsToInstantiate[indexPatternItem].patternItemToInstantiate.itemType_Right[i].x - 1].GetComponent<D3Item>().distanceZ;
                    GUI.color = pattern.ObjectToInstantiateObject[(int)pattern.ListItemsToInstantiate[indexPatternItem].patternItemToInstantiate.itemType_Right[i].x - 1].GetComponent<D3Item>().colorPattern;
                    GUI.Box(new Rect(260 + offset_X, 270 + (i * 20), 20, 20), "");
                    if (disZ <= 1)
                        GUI.Box(new Rect(260 + offset_X, 270 + (i * 20), 20, 20 * (-(disZ - 1))), "");
                    else
                        GUI.Box(new Rect(260 + offset_X, 270 + (i * 20), 20, 20 * (-(disZ))), "");
                    GUI.color = normalColor;
                }

            }
        }

    }

    private void ShowDetial()
    {
        if (pattern != null)
        {
            EditorGUI.LabelField(new Rect(400 + offset_X, 230, 200, 100), "[ 0 ] = Null");
            EditorGUI.LabelField(new Rect(170 + offset_X, 210, 200, 100), "---Pattern End---");
            EditorGUI.LabelField(new Rect(140 + offset_X, 850, 200, 100), "---Start (Generated First)---");

            for (int i = 0; i < pattern.defaultPosItem_Left.Count; i++)
            {
                EditorGUI.LabelField(new Rect(115, 230 + (i * 20), 220, 100), "[ " + (i) + " ]");
            }


            for (int i = 0; i < pattern.ObjectToInstantiateObject.Count; i++)
            {
                if (pattern.ObjectToInstantiateObject[i].GetComponent<D3Item>() != null)
                {
                    pattern.ObjectToInstantiateObject[i].GetComponent<D3Item>().colorPattern = EditorGUI.ColorField(new Rect(350 + offset_X, 300 + (i * 30), 40, 20), pattern.ObjectToInstantiateObject[i].GetComponent<D3Item>().colorPattern);
                    EditorGUI.LabelField(new Rect(400 + offset_X, 260 + (i * 30), 200, 100), "[ " + (i + 1) + " ] = " + pattern.ObjectToInstantiateObject[i].name + "");
                }
            }

        }
    }
}
