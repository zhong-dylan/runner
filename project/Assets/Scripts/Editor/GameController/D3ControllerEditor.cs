
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using Unity.VisualScripting;

[ExecuteInEditMode]
[CustomEditor(typeof(D3Controller))]
public class D3ControllerEditor : Editor
{
    public Object instance;
    D3Controller itemTarget;
    public Object targetPref;

    SerializedObject TargetS;


    private void OnEnable()
    {
        itemTarget = target as D3Controller;
        if (itemTarget)
        {
            TargetS = new SerializedObject(itemTarget);

            if (itemTarget.GetComponent<D3AnimationManager>())
            {
                if (itemTarget.GetComponent<D3AnimationManager>().m_Animator == null)
                {
                    if (itemTarget.GetComponent<Animator>())
                    {
                        itemTarget.GetComponent<D3AnimationManager>().m_Animator = itemTarget.GetComponent<Animator>();
                    }
                    else
                    {

                        itemTarget.AddComponent<Animator>();
                        itemTarget.GetComponent<D3AnimationManager>().m_Animator = itemTarget.GetComponent<Animator>();
                    }
                }
            }
        }
    }

    public override void OnInspectorGUI()
    {
            EditorGUI.BeginChangeCheck();
            var style = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };
            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true), GUILayout.Height(170f));
            Texture2D m_Logo = (Texture2D)Resources.Load("Img/D3Icon", typeof(Texture2D));
            GUILayout.Label(m_Logo, style, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            GUILayout.EndVertical();
            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
            GUILayout.Label("Character Editor", style);
            GUILayout.EndVertical();
            if (itemTarget)
            {
                GUILayout.Space(10f);
                GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
                GUILayout.Space(10f);
                GUILayout.Label("Basic", style);
                EditorGUILayout.TextArea("Initial speed, gravity and jump distance", GUI.skin.GetStyle("HelpBox"));

                GUILayout.Space(10f);
                itemTarget.speedMove = EditorGUILayout.FloatField("Speed Move: ", itemTarget.speedMove);


                GUILayout.Space(10f);
                itemTarget.gravity = EditorGUILayout.FloatField("Gravity: ", itemTarget.gravity);

                GUILayout.Space(10f);
                itemTarget.jumpValue = EditorGUILayout.FloatField("JumpValue: ", itemTarget.jumpValue);

                GUILayout.Space(10f);
                GUILayout.EndVertical();

                GUILayout.Space(10f);
                GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
                GUILayout.Space(10f);
                GUILayout.Label("Statistics", style);
                EditorGUILayout.TextArea("Add Time to items", GUI.skin.GetStyle("HelpBox"));


                GUILayout.Space(10f);
                itemTarget.InitialPriceSprintTime = EditorGUILayout.IntField("Initial Price Sprint Time: ", itemTarget.InitialPriceSprintTime);
                GUILayout.Space(10f);
                itemTarget.AddSprintTime = EditorGUILayout.FloatField("Initial Add Sprint Time: ", itemTarget.AddSprintTime);
                GUILayout.Space(10f);
                itemTarget.AddSprintTimeMax = EditorGUILayout.FloatField("Max Add Sprint Time: ", itemTarget.AddSprintTimeMax);


                GUILayout.Space(10f);
                itemTarget.InitialPriceSpecialTime = EditorGUILayout.IntField("Initial Price Special Time: ", itemTarget.InitialPriceSpecialTime);
                GUILayout.Space(10f);
                itemTarget.AddSpecialTime = EditorGUILayout.FloatField("Initial Add Special Time: ", itemTarget.AddSpecialTime);
                GUILayout.Space(10f);
                itemTarget.AddSpecialTimeMax = EditorGUILayout.FloatField("Max Add Special Time: ", itemTarget.AddSpecialTimeMax);

                GUILayout.Space(10f);
                itemTarget.InitialPriceMultiplyTime = EditorGUILayout.IntField("Initial Price Multiply Time: ", itemTarget.InitialPriceMultiplyTime);
                GUILayout.Space(10f);
                itemTarget.AddMultiplyTime = EditorGUILayout.FloatField("Initial Add Multiply Time: ", itemTarget.AddMultiplyTime);
                GUILayout.Space(10f);
                itemTarget.AddMultiplyTimeMax = EditorGUILayout.FloatField("Max Add Multiply Time: ", itemTarget.AddMultiplyTimeMax);


                GUILayout.Space(10f);
                itemTarget.InitialPriceMagnetTime = EditorGUILayout.IntField("Initial Price Magnet Time: ", itemTarget.InitialPriceMagnetTime);
                GUILayout.Space(10f);
                itemTarget.AddMagnetTime = EditorGUILayout.FloatField("Initial Add Magnet Time: ", itemTarget.AddMagnetTime);
                GUILayout.Space(10f);
                itemTarget.AddMagnetTimeMax = EditorGUILayout.FloatField("Max Add Magnet Time: ", itemTarget.AddMagnetTimeMax);


                GUILayout.Space(10f);
                itemTarget.InitialPriceShieldTime = EditorGUILayout.IntField("Initial Price Shield Time: ", itemTarget.InitialPriceShieldTime);
                GUILayout.Space(10f);
                itemTarget.AddShieldTime = EditorGUILayout.FloatField("Initial Add Shield Time: ", itemTarget.AddShieldTime);
                GUILayout.Space(10f);
                itemTarget.AddShieldTimeMax = EditorGUILayout.FloatField("Max Add Shield Time: ", itemTarget.AddShieldTimeMax);


                GUILayout.Space(10f);
                GUILayout.EndVertical();



                GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
                GUILayout.Space(10f);
                GUILayout.Label("(Optional)Immortality Effect (Shield)", style);
                EditorGUILayout.TextArea("adds a shield to protect the player when starting the game or resetting", GUI.skin.GetStyle("HelpBox"));

                GUILayout.Space(10f);
                itemTarget.useImmortalityOnInit = GUILayout.Toggle(itemTarget.useImmortalityOnInit, " Use Shield");

                if (itemTarget.useImmortalityOnInit)
                {
                    GUILayout.Space(10f);
                    itemTarget.ImmortalityTimeWhenRevived = EditorGUILayout.FloatField("Time Duration: ", itemTarget.ImmortalityTimeWhenRevived);
                    GUILayout.Space(10f);
                    EditorGUILayout.TextArea("It is recommended to add the shield effect to the character's spine, review the example characters", GUI.skin.GetStyle("HelpBox"));

                    itemTarget.ImmortalityEffect = EditorGUILayout.ObjectField("Effect: ", itemTarget.ImmortalityEffect, typeof(GameObject), true) as GameObject;

                    GUILayout.Space(10f);

                }
                else
                {
                    itemTarget.ImmortalityTimeWhenRevived = 0;
                }
                GUILayout.Space(10f);
                GUILayout.EndVertical();

                GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
                GUILayout.Space(10f);
                GUILayout.Label("Magnet Detector Collider", style);
                EditorGUILayout.TextArea("Used to detect coins (Required)", GUI.skin.GetStyle("HelpBox"));

                GUILayout.Space(10f);

                if (itemTarget.magnetCollider != null)
                {
                    itemTarget.magnetCollider = EditorGUILayout.ObjectField("Magnet Detect: ", itemTarget.magnetCollider, typeof(GameObject), true) as GameObject;

                    GUILayout.Space(10f);
                    EditorGUILayout.TextArea("It is recommended to add the Magnet item to the character's right hand, please review the example characters for more information.", GUI.skin.GetStyle("HelpBox"));

                    itemTarget.magnetCollider.GetComponent<D3Magnet>().ObjectMagnet = EditorGUILayout.ObjectField("Visual Object (Magnet): ", itemTarget.magnetCollider.GetComponent<D3Magnet>().ObjectMagnet, typeof(GameObject), true) as GameObject;

                }
                if (itemTarget.magnetCollider == null)
                {
                    GUILayout.Space(10f);
                    if (GUILayout.Button("Magnet Detect not found, Click to add"))
                    {
                        //instantiate ui canvas
                        GameObject ASS = Instantiate(Resources.Load("MagnetCollision")) as GameObject;
                        //rename it
                        ASS.name = "MagnetCollision";
                        ASS.transform.position = new Vector3(0f, 0f, 0f);
                        ASS.transform.SetParent(itemTarget.gameObject.transform);

                        itemTarget.magnetCollider = ASS;

                        Debug.Log("Magnet Detect Created!");
                    }


                }

                GUILayout.Space(10f);
                GUILayout.EndVertical();

                GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
                GUILayout.Space(10f);
                GUILayout.Label("Coin Detector Collider", style);
                EditorGUILayout.TextArea("Used to detect coins (Required)", GUI.skin.GetStyle("HelpBox"));

                GUILayout.Space(10f);

                if (itemTarget.coinRotate != null)
                {
                    itemTarget.coinRotate = EditorGUILayout.ObjectField("Coin Detect: ", itemTarget.coinRotate, typeof(D3CoinRotation), true) as D3CoinRotation;
                }
                if (itemTarget.coinRotate == null)
                {
                    GUILayout.Space(10f);
                    if (GUILayout.Button("Coin Detect not found, Click to add"))
                    {
                        //instantiate ui canvas
                        GameObject ASS = Instantiate(Resources.Load("Get_Item_For_Delete")) as GameObject;
                        //rename it
                        ASS.name = "Get_Item_For_Delete";
                        ASS.transform.position = new Vector3(0f, 0f, 0f);
                        ASS.transform.SetParent(itemTarget.gameObject.transform);

                        itemTarget.coinRotate = ASS.GetComponent<D3CoinRotation>();

                        Debug.Log("Coin Detect Created!");
                    }

                }

                GUILayout.Space(10f);
                GUILayout.EndVertical();


                GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
                GUILayout.Space(10f);
                GUILayout.Label("HoverBike", style);
                EditorGUILayout.TextArea("Object Visual to activate/deactivate", GUI.skin.GetStyle("HelpBox"));

                GUILayout.Space(10f);
                itemTarget.HoverBike = EditorGUILayout.ObjectField("HoverBike: ", itemTarget.HoverBike, typeof(GameObject), true) as GameObject;

                GUILayout.Space(10f);
                itemTarget.EffectHover = EditorGUILayout.ObjectField("Effect Hover: ", itemTarget.EffectHover, typeof(ParticleSystem), true) as ParticleSystem;

                GUILayout.Space(10f);
                GUILayout.EndVertical();

                GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
                GUILayout.Space(10f);
                GUILayout.Label("Special Item", style);
                EditorGUILayout.TextArea("Object Visual to activate/deactivate", GUI.skin.GetStyle("HelpBox"));

                GUILayout.Space(10f);
                itemTarget.SpecialItem = EditorGUILayout.ObjectField("SpecialItem: ", itemTarget.SpecialItem, typeof(GameObject), true) as GameObject;

                GUILayout.Space(10f);
                GUILayout.EndVertical();

                GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
                GUILayout.Space(10f);
                GUILayout.Label("HoverBoard Position List", style);
                EditorGUILayout.TextArea("Object Visual to activate/deactivate", GUI.skin.GetStyle("HelpBox"));

                GUILayout.Space(10f);
                itemTarget.HoverBoardPositionList = EditorGUILayout.ObjectField("HoverBoardPositionList: ", itemTarget.HoverBoardPositionList, typeof(GameObject), true) as GameObject;


                GUILayout.Space(10f);

            TargetS.Update();

            Rect myRect2 = GUILayoutUtility.GetRect(0.0f, 50.0f, GUILayout.ExpandWidth(true));
            GUI.Box(myRect2, "Drag and drop HoverBoard Prefab \n into this box!");

            if (myRect2.Contains(Event.current.mousePosition))
            {
                if (Event.current.type == EventType.DragUpdated)
                {
                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                    Event.current.Use();
                }
                else if (Event.current.type == EventType.DragPerform)
                {
                    for (int i = 0; i < DragAndDrop.objectReferences.Length; i++)
                    {

                        GameObject AddGO = DragAndDrop.objectReferences[i] as GameObject;

                        if (AddGO)
                        {
                            itemTarget.HoverBoardList.Add(AddGO);
                            TargetS.ApplyModifiedProperties();
                            if (!Application.isPlaying)
                            {
                                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
                                EditorUtility.SetDirty(itemTarget);
                                PrefabUtility.RecordPrefabInstancePropertyModifications(itemTarget);
                                AssetDatabase.SaveAssets();
                                AssetDatabase.Refresh();
                            }
                        }
                        else
                        {
                            Debug.Log("Invalid HoverBoard Prefab");
                        }

                    }
                    Event.current.Use();
                }
            }

            if (itemTarget.HoverBoardList.Count > 0)
            {
                GUILayout.Space(10);
                if (GUILayout.Button("Remove Last HoverBoard Prefab"))
                {
                    itemTarget.HoverBoardList.RemoveAt(itemTarget.HoverBoardList.Count - 1);
                    EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
                }
                GUILayout.Space(10);
                GUILayout.Label("Total items added: " + itemTarget.HoverBoardList.Count);
                for (int i = 0; i < itemTarget.HoverBoardList.Count; i++)
                {
                    if (itemTarget.HoverBoardList[i] != null)
                    {
                        GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
                        GUILayout.Space(5f);
                        GUILayout.Label("ID For Shop Manager:  " + i, EditorStyles.boldLabel);
                        EditorGUILayout.TextArea("This is the ID Code that you must use in the shop administrator tab in the main scene.", GUI.skin.GetStyle("HelpBox"));
                        GUILayout.Space(5f);
                        itemTarget.HoverBoardList[i] = EditorGUILayout.ObjectField("HoverBoard Prefab: ", itemTarget.HoverBoardList[i], typeof(GameObject), true) as GameObject;
                        GUILayout.EndVertical();
                    }
                }

                if (GUILayout.Button("Remove Last HoverBoard Prefab"))
                {
                    itemTarget.HoverBoardList.RemoveAt(itemTarget.HoverBoardList.Count - 1);
                    EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
                }
            }

                TargetS.ApplyModifiedProperties();

                GUILayout.Space(10f);
                GUILayout.EndVertical();


            }
            if (EditorGUI.EndChangeCheck())
            {
                if (!Application.isPlaying)
                {
                    EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
                    EditorUtility.SetDirty(itemTarget);
                    PrefabUtility.RecordPrefabInstancePropertyModifications(itemTarget);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }

            }
    }
}

#endif

