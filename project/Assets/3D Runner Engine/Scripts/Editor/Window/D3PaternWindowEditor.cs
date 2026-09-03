using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class PaternWindowEditor : EditorWindow {

	private static PaternWindowEditor window;
	private D3PatternSystem pattern;

    public string stringToEdit = "Default Pattern System: It is the default generation of the scene.\nRocket Pattern System: It is the generation of Items when activating the rokect.\nSpecial Pattern System: It is the generation of items and buildings when activating the Special Item.\nThe system is responsible for selecting them randomly. They are not consecutive. This configuration is only valid for this scene.\n";

    private int indexPatternItem;
	private int indexPatternBuilding;
    private int indexPatternBuildingSpecial;
    private int indexPatternItemRocket;
    private int indexPatternItemSpecial;

    private float offset_X;
	private float offset_X_Building;

    private float offset_XBSpecial;
    private float offset_X_BuildingSpecial;

    public int[] sizeInt;
	public string[] stringSize;
	
	public int[] sizeInt_Building;
	public string[] stringSize_Building;

    public int[] sizeInt_BuildingSpecial;
    public string[] stringSize_BuildingSpecial;

    public int[] sizeInt_ItemRocket;
    public string[] stringSize_ItemRocket;

    public int[] sizeIntSpecial;
    public string[] stringSizeSpecial;

    int selected = 0;
    string[] options = new string[]
    {
    "Default Patter System", "Rocket Pattern System", "Special Pattern System",
    };

    public static void Init()
	{
		// Get existing open window or if none, make a new one:
		window = (PaternWindowEditor)EditorWindow.GetWindow(typeof(PaternWindowEditor));
        window.Show();
    }
	void OnEnable()
	{
		if (FindObjectOfType<D3PatternSystem>())
		{
			pattern = FindObjectOfType<D3PatternSystem>();
		}
    }

    void OnGUI(){
        EditorGUI.BeginChangeCheck();
        const int width = 780;
        const int height = 970;

        var x = (Screen.currentResolution.width - width) / 2;
        var y = (Screen.currentResolution.height - height) / 2;


        offset_X = 0;
		offset_X_Building = 100;

        GUI.Label(new Rect(10, 0, 760, 120), "Select the Pattern System to Edit", "box");
        
        GUI.TextArea(new Rect(20, 20, 730, 60), stringToEdit, 500);

        selected = EditorGUI.Popup(new Rect(30, 90, 600, 20), "Pattern System to Edit:", selected,options);
        
        
        pattern.SettingVariableFirst();

        switch (selected)
        {
            case 0:
                GUI.Label(new Rect(10, 130, 760, 830), "Items for Default Pattern System", "box");
               

                if (GUI.Button(new Rect(15, 180, 120, 50), "Create \n Pattern Item"))
                {
                    if (pattern != null)
                    {
                        pattern.patternItem.Add(new D3PatternSystem.SetItem());
                        indexPatternItem = pattern.patternItem.Count - 1;
                    }
                }
                EditorGUILayout.Space();


                if (GUI.Button(new Rect(145, 180, 120, 50), "Delete \n Pattern Item"))
                {
                    if (pattern != null)
                    {
                        if (indexPatternItem != 0)
                        {
                            pattern.patternItem.RemoveRange(pattern.patternItem.Count - 1, 1);
                        }
                        indexPatternItem -= 1;
                        if (indexPatternItem <= 0)
                        {
                            indexPatternItem = 0;
                        }
                    }
                }


                if (GUI.Button(new Rect(275, 180, 120, 50), "Modify \n Item Position"))
                {
                    if (pattern != null)
                    {
                        WindowEditPositionItems window = (WindowEditPositionItems)EditorWindow.GetWindow(typeof(WindowEditPositionItems), false, " Position Items Editor");
                        window.position = new Rect(x, y, width, height);
                        window.pattern = pattern;
                        window.selected = selected;
                        window.Show();
                    }
                }



                if (GUI.Button(new Rect(450, 180, 120, 50), "Create \n Pattern Building"))
                {
                    if (pattern != null)
                    {
                        pattern.patternBuilding.Add(new D3PatternSystem.SetBuilding());
                        indexPatternBuilding = pattern.patternBuilding.Count - 1;
                    }
                }

                if (GUI.Button(new Rect(585, 180, 120, 50), "Delete \n Pattern Building"))
                {
                    if (pattern != null)
                    {
                        if (indexPatternBuilding != 0)
                        {
                            pattern.patternBuilding.RemoveRange(pattern.patternBuilding.Count - 1, 1);
                        }
                        indexPatternBuilding -= 1;
                        if (indexPatternBuilding <= 0)
                        {
                            indexPatternBuilding = 0;
                        }
                    }
                }
                EditorGUILayout.Space();

                ButtonActiveIndex();

                SlotPatternItem();

                SlotPatternBuilding();

                ShowDetial();

                break;

            case 1:
                GUI.Label(new Rect(10, 130, 760, 830), "Rocket Pattern System", "box");

                if (GUI.Button(new Rect(125, 180, 120, 50), "Create \n Pattern Item"))
                {
                    if (pattern != null)
                    {
                        pattern.patternItemRocket.Add(new D3PatternSystem.SetItem());
                        indexPatternItemRocket = pattern.patternItemRocket.Count - 1;
                    }
                }

                if (GUI.Button(new Rect(310, 180, 120, 50), "Delete \n Pattern Item"))
                {
                    if (pattern != null)
                    {
                        if (indexPatternItemRocket != 0)
                        {
                            pattern.patternItemRocket.RemoveRange(pattern.patternItemRocket.Count - 1, 1);
                        }
                        indexPatternItemRocket -= 1;
                        if (indexPatternItemRocket <= 0)
                        {
                            indexPatternItemRocket = 0;
                        }
                    }
                }

                if (GUI.Button(new Rect(500, 180, 120, 50), "Modify \n Item Position"))
                {
                    if (pattern != null)
                    {
                        WindowEditPositionItems window = (WindowEditPositionItems)EditorWindow.GetWindow(typeof(WindowEditPositionItems), false, " Position Items Editor");
                        window.position = new Rect(x, y, width, height);
                        window.pattern = pattern;
                        window.selected = selected;
                        window.Show();
                    }
                }

                EditorGUILayout.Space();

                ButtonActiveIndex();

                SlotPatternItemRockect();

                ShowDetial();

                break;

            case 2:
                GUI.Label(new Rect(10, 130, 760, 830), "Items for Special Pattern System", "box");

                if (GUI.Button(new Rect(15, 180, 120, 50), "Create \n Pattern Item"))
                {
                    if (pattern != null)
                    {
                        pattern.patternItemSpecial.Add(new D3PatternSystem.SetItem());
                        indexPatternItemSpecial = pattern.patternItemSpecial.Count - 1;
                    }
                }
                EditorGUILayout.Space();


                if (GUI.Button(new Rect(145, 180, 120, 50), "Delete \n Pattern Item"))
                {
                    if (pattern != null)
                    {
                        if (indexPatternItemSpecial != 0)
                        {
                            pattern.patternItemSpecial.RemoveRange(pattern.patternItemSpecial.Count - 1, 1);
                        }
                        indexPatternItemSpecial -= 1;
                        if (indexPatternItemSpecial <= 0)
                        {
                            indexPatternItemSpecial = 0;
                        }
                    }
                }

                if (GUI.Button(new Rect(275, 180, 120, 50), "Modify \n Item Position"))
                {
                    if (pattern != null)
                    {
                        WindowEditPositionItems window = (WindowEditPositionItems)EditorWindow.GetWindow(typeof(WindowEditPositionItems), false, " Position Items Editor");
                        window.position = new Rect(x, y, width, height);
                        window.pattern = pattern;
                        window.selected = selected;
                        window.Show();
                    }
                }

                if (GUI.Button(new Rect(450, 180, 120, 50), "Create \n Pattern Building"))
                {
                    if (pattern != null)
                    {
                        pattern.patternBuildingSpecial.Add(new D3PatternSystem.SetBuilding());
                        indexPatternBuildingSpecial = pattern.patternBuildingSpecial.Count - 1;
                    }
                }

                if (GUI.Button(new Rect(585, 180, 120, 50), "Delete \n Pattern Building"))
                {
                    if (pattern != null)
                    {
                        if (indexPatternBuildingSpecial != 0)
                        {
                            pattern.patternBuildingSpecial.RemoveRange(pattern.patternBuildingSpecial.Count - 1, 1);
                        }
                        indexPatternBuildingSpecial -= 1;
                        if (indexPatternBuildingSpecial <= 0)
                        {
                            indexPatternBuildingSpecial = 0;
                        }
                    }
                }
                EditorGUILayout.Space();

                ButtonActiveIndex();

                SlotPatternItemSpecial();

                SlotPatternBuildingSpecial();


                ShowDetial();




                break;
        }


        if (EditorGUI.EndChangeCheck())
        {
            if (!Application.isPlaying)
            {
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            }
        }

    }
	
	void Update(){
		if(pattern == null){
			GetTarget();	
		}else{
			return;	
		}
	}
	
	void OnSelectionChange(){
		GetTarget();
	}
	
	private void GetTarget(){
		if(Selection.gameObjects.Length > 0){
			if(Selection.gameObjects[0].GetComponent<D3PatternSystem>() != null){
				pattern = Selection.gameObjects[0].GetComponent<D3PatternSystem>();
				Debug.Log(pattern.name);
			}
		}
	}
	
	private void ButtonActiveIndex(){
        Texture2D alert = (Texture2D)Resources.Load("Img/Alert", typeof(Texture2D));
        if (pattern != null){
            switch (selected)
            {
                case 0: //Default

                    sizeInt = new int[pattern.patternItem.Count];
                    
                    stringSize = new string[pattern.patternItem.Count];
                    for (int i = 0; i < sizeInt.Length; i++)
                    {
                        sizeInt[i] = i;
                        stringSize[i] = (i).ToString();
                    }
                    EditorGUI.LabelField(new Rect(60, 220, 200, 50), "Active Index Pattern Item");
                    indexPatternItem = EditorGUI.IntPopup(new Rect(60, 270, 100, 20), "", indexPatternItem, stringSize, sizeInt);

                    int TotalItens = 0;
                    int TotalItensInit = 0;
                    int TotalItensEnd = 0;
                    string Text1, Text2, Text3;
                   
                    for (int i = 0; i < pattern.patternItem[indexPatternItem].itemType_Left.Length; i++)
                    {
                        if (pattern.patternItem[indexPatternItem].itemType_Left[i].x > 0)
                        {
                            TotalItens++;
                        }
                    }

                    for (int i = 0; i < pattern.patternItem[indexPatternItem].itemType_Right.Length; i++)
                    {
                        if (pattern.patternItem[indexPatternItem].itemType_Right[i].x > 0)
                        {
                            TotalItens++;
                        }
                    }

                    for (int i = 0; i < pattern.patternItem[indexPatternItem].itemType_Middle.Length; i++)
                    {
                        if (pattern.patternItem[indexPatternItem].itemType_Middle[i].x > 0)
                        {
                            TotalItens++;
                        }
                    }

                    if (TotalItens <= 2)
                    {
                        GUI.Box(new Rect(400, 710, 100, 65), alert);
                        Text1 = "1) Warning:\r\nPattern system with empty or only 2 object is not recommended,\r\nit may cause overlapping";
                    }
                    else {
                        Text1 = "";
                    }

                    for (int i = 0; i < 3; i++)
                    {
                        if (pattern.patternItem[indexPatternItem].itemType_Left[i].x > 0)
                        {
                            TotalItensInit++;
                        }
                        if (pattern.patternItem[indexPatternItem].itemType_Right[i].x > 0)
                        {
                            TotalItensInit++;
                        }
                        if (pattern.patternItem[indexPatternItem].itemType_Middle[i].x > 0)
                        {
                            TotalItensInit++;
                        }
                    }

                    if (TotalItensInit <= 0)
                    {
                        GUI.Box(new Rect(500, 710, 100, 65), alert);
                        Text2 = "2) Warning:\r\nTo avoid overlaps do not leave the positions\r\n[0] [1] [2] empty, place at least 1 item in any of those";
                    }
                    else {
                        Text2 = "";
                    }

                    if (pattern.patternItem[indexPatternItem].itemType_Left[28].x > 0)
                    {
                        TotalItensEnd++;
                    }
                    if (pattern.patternItem[indexPatternItem].itemType_Right[28].x > 0)
                    {
                        TotalItensEnd++;
                    }
                    if (pattern.patternItem[indexPatternItem].itemType_Middle[28].x > 0)
                    {
                        TotalItensEnd++;
                    }
                    if (pattern.patternItem[indexPatternItem].itemType_Left[29].x > 0)
                    {
                        TotalItensEnd++;
                    }
                    if (pattern.patternItem[indexPatternItem].itemType_Right[29].x > 0)
                    {
                        TotalItensEnd++;
                    }
                    if (pattern.patternItem[indexPatternItem].itemType_Middle[29].x > 0)
                    {
                        TotalItensEnd++;
                    }
                    if (pattern.patternItem[indexPatternItem].itemType_Left[30].x > 0)
                    {
                        TotalItensEnd++;
                    }
                    if (pattern.patternItem[indexPatternItem].itemType_Right[30].x > 0)
                    {
                        TotalItensEnd++;
                    }
                    if (pattern.patternItem[indexPatternItem].itemType_Middle[30].x > 0)
                    {
                        TotalItensEnd++;
                    }

                    if (TotalItensEnd <= 0)
                    {
                        GUI.Box(new Rect(600, 710, 100, 65), alert);
                        Text3 = "3) Warning:\r\nTo avoid overlaps do not leave the positions\r\n[28] [29] [30] empty, place at least 1 item in any of those";
                    }
                    else {
                        Text3 = "";
                    }

                    string TextEnd = Text1 + "\r\n" + Text2 + "\r\n" + Text3;

                    EditorGUI.LabelField(new Rect(400, 775, 400, 190), TextEnd);

                    sizeInt_Building = new int[pattern.patternBuilding.Count];
                    stringSize_Building = new string[pattern.patternBuilding.Count];
                    for (int i = 0; i < sizeInt_Building.Length; i++)
                    {
                        sizeInt_Building[i] = i;
                        stringSize_Building[i] = (i).ToString();
                    }
                    EditorGUI.LabelField(new Rect(455, 230, 200, 50), "Active Index Pattern Building");
                    indexPatternBuilding = EditorGUI.IntPopup(new Rect(500, 280, 100, 20), "", indexPatternBuilding, stringSize_Building, sizeInt_Building);



                    break;




                case 1: //Rockect

                    sizeInt_ItemRocket = new int[pattern.patternItemRocket.Count];
                    stringSize_ItemRocket = new string[pattern.patternItemRocket.Count];
                    for (int i = 0; i < sizeInt_ItemRocket.Length; i++)
                    {
                        sizeInt_ItemRocket[i] = i;
                        stringSize_ItemRocket[i] = (i).ToString();
                    }
                    EditorGUI.LabelField(new Rect(300, 220, 200, 50), "Active Index Pattern Rocket");
                    indexPatternItemRocket = EditorGUI.IntPopup(new Rect(330, 270, 100, 20), "", indexPatternItemRocket, stringSize_ItemRocket, sizeInt_ItemRocket);






                    break;

                case 2: //Special


                    sizeIntSpecial = new int[pattern.patternItemSpecial.Count];
                    stringSizeSpecial = new string[pattern.patternItemSpecial.Count];
                    for (int i = 0; i < sizeIntSpecial.Length; i++)
                    {
                        sizeIntSpecial[i] = i;
                        stringSizeSpecial[i] = (i).ToString();
                    }
                    EditorGUI.LabelField(new Rect(60, 220, 200, 50), "Active Index Pattern Item");
                    indexPatternItemSpecial = EditorGUI.IntPopup(new Rect(60, 270, 100, 20), "", indexPatternItemSpecial, stringSizeSpecial, sizeIntSpecial);


                    sizeInt_BuildingSpecial = new int[pattern.patternBuildingSpecial.Count];
                    stringSize_BuildingSpecial = new string[pattern.patternBuildingSpecial.Count];
                    for (int i = 0; i < sizeInt_BuildingSpecial.Length; i++)
                    {
                        sizeInt_BuildingSpecial[i] = i;
                        stringSize_BuildingSpecial[i] = (i).ToString();
                    }
                    EditorGUI.LabelField(new Rect(465, 230, 200, 50), "Active Index Pattern Building");
                    indexPatternBuildingSpecial = EditorGUI.IntPopup(new Rect(500, 280, 100, 20), "", indexPatternBuildingSpecial, stringSize_BuildingSpecial, sizeInt_BuildingSpecial);





                    break;
            }


            EditorGUILayout.Space();
			EditorGUILayout.Space();
		}
	}


	private void SlotPatternItem(){
        if (pattern != null){

            Color normalColor = GUI.color;
            EditorGUI.LabelField(new Rect(20,160,200,15), "Item Pattern");
            GUI.color = normalColor;
            for (int i = 0; i < pattern.patternItem[indexPatternItem].itemType_Left.Length; i++){
				pattern.patternItem[indexPatternItem].itemType_Left[i].x = EditorGUI.IntField(new Rect(60+offset_X,310+(i*20),20,20), (int)pattern.patternItem[indexPatternItem].itemType_Left[i].x);
                GUI.color = normalColor;
                if (pattern.patternItem[indexPatternItem].itemType_Left[i].x != 0 && (int)pattern.patternItem[indexPatternItem].itemType_Left[i].x <= pattern.item_Pref.Count){
					float disZ = pattern.item_Pref[(int)pattern.patternItem[indexPatternItem].itemType_Left[i].x-1].GetComponent<D3Item>().distanceZ;
					GUI.color = pattern.item_Pref[(int)pattern.patternItem[indexPatternItem].itemType_Left[i].x-1].GetComponent<D3Item>().colorPattern;
					GUI.Box(new Rect(60+offset_X, 310 + (i*20),20,20), "");
					if(disZ<=1)

						GUI.Box(new Rect(60+offset_X, 310 + (i*20),20,20*(-(disZ-1))), "");
					else
						GUI.Box(new Rect(60+offset_X, 310 + (i*20),20,20*(-(disZ))), "");
                    GUI.color = normalColor;

                }
			}
			
			for(int i = 0; i < pattern.patternItem[indexPatternItem].itemType_Middle.Length; i++){
				pattern.patternItem[indexPatternItem].itemType_Middle[i].x = EditorGUI.IntField(new Rect(110+offset_X, 310 + (i*20),20,20), (int)pattern.patternItem[indexPatternItem].itemType_Middle[i].x);
                GUI.color = normalColor;
                if (pattern.patternItem[indexPatternItem].itemType_Middle[i].x != 0 && (int)pattern.patternItem[indexPatternItem].itemType_Middle[i].x <= pattern.item_Pref.Count){
					float disZ = pattern.item_Pref[(int)pattern.patternItem[indexPatternItem].itemType_Middle[i].x-1].GetComponent<D3Item>().distanceZ;
                    GUI.color = pattern.item_Pref[(int)pattern.patternItem[indexPatternItem].itemType_Middle[i].x - 1].GetComponent<D3Item>().colorPattern;
                    GUI.Box(new Rect(110 + offset_X, 310 + (i * 20), 20, 20), "");
                    if (disZ <= 1)

                        GUI.Box(new Rect(110 + offset_X, 310 + (i * 20), 20, 20 * (-(disZ - 1))), "");
                    else
                        GUI.Box(new Rect(110 + offset_X, 310 + (i * 20), 20, 20 * (-(disZ))), "");
                    GUI.color = normalColor;
                }
			}
			
			for(int i = 0; i < pattern.patternItem[indexPatternItem].itemType_Right.Length; i++){
				pattern.patternItem[indexPatternItem].itemType_Right[i].x = EditorGUI.IntField(new Rect(160+offset_X, 310 + (i*20),20,20), (int)pattern.patternItem[indexPatternItem].itemType_Right[i].x);
                GUI.color = normalColor;
                if (pattern.patternItem[indexPatternItem].itemType_Right[i].x != 0 && (int)pattern.patternItem[indexPatternItem].itemType_Right[i].x <= pattern.item_Pref.Count){
					float disZ = pattern.item_Pref[(int)pattern.patternItem[indexPatternItem].itemType_Right[i].x-1].GetComponent<D3Item>().distanceZ;
					GUI.color = pattern.item_Pref[(int)pattern.patternItem[indexPatternItem].itemType_Right[i].x-1].GetComponent<D3Item>().colorPattern;
					GUI.Box(new Rect(160+offset_X, 310 + (i*20),20,20), "");
					if(disZ<=1)
						GUI.Box(new Rect(160+offset_X, 310 + (i*20),20,20*(-(disZ-1))), "");
					else
						GUI.Box(new Rect(160+offset_X, 310 + (i*20),20,20*(-(disZ))), "");
                    GUI.color = normalColor;
                }
			
			}
		}

	}
	
	
	private void SlotPatternBuilding(){
		if(pattern != null){

            EditorGUI.LabelField(new Rect(330+offset_X_Building,160,200,15), "Building Pattern");
			
			for(int i = 0; i < pattern.patternBuilding[indexPatternBuilding].stateBuilding_Left.Length; i++){
				pattern.patternBuilding[indexPatternBuilding].stateBuilding_Left[i] = EditorGUI.IntField(new Rect(400 + offset_X_Building,350+(i*30),20,20), pattern.patternBuilding[indexPatternBuilding].stateBuilding_Left[i]);
			}
			
			for(int i = 0; i < pattern.patternBuilding[indexPatternBuilding].stateBuilding_Right.Length; i++){
				pattern.patternBuilding[indexPatternBuilding].stateBuilding_Right[i] = EditorGUI.IntField(new Rect(460+offset_X_Building,350+(i*30),20,20), pattern.patternBuilding[indexPatternBuilding].stateBuilding_Right[i]);
			}	
		}
	}


    private void SlotPatternBuildingSpecial()
    {
        if (pattern != null)
        {

            EditorGUI.LabelField(new Rect(450 + offset_X_BuildingSpecial, 160, 200, 15), "Building Pattern");

            for (int i = 0; i < pattern.patternBuildingSpecial[indexPatternBuildingSpecial].stateBuilding_Left.Length; i++)
            {
                pattern.patternBuildingSpecial[indexPatternBuildingSpecial].stateBuilding_Left[i] = EditorGUI.IntField(new Rect(450 + offset_X_BuildingSpecial, 350 + (i * 30), 20, 20), pattern.patternBuildingSpecial[indexPatternBuildingSpecial].stateBuilding_Left[i]);
            }

            for (int i = 0; i < pattern.patternBuildingSpecial[indexPatternBuildingSpecial].stateBuilding_Right.Length; i++)
            {
                pattern.patternBuildingSpecial[indexPatternBuildingSpecial].stateBuilding_Right[i] = EditorGUI.IntField(new Rect(500 + offset_X_BuildingSpecial, 350 + (i * 30), 20, 20), pattern.patternBuildingSpecial[indexPatternBuildingSpecial].stateBuilding_Right[i]);
            }
        }
    }


    private void SlotPatternItemRockect()
    {
        if (pattern != null)
        {
            EditorGUI.LabelField(new Rect(20, 160, 200, 15), "Item Pattern");
            Color normalColor = GUI.color;
            for (int i = 0; i < pattern.patternItem[indexPatternItemRocket].itemType_Left.Length; i++)
            {
                pattern.patternItemRocket[indexPatternItemRocket].itemType_Left[i].x = EditorGUI.IntField(new Rect(140 + offset_X, 310 + (i * 20), 20, 20), (int)pattern.patternItemRocket[indexPatternItemRocket].itemType_Left[i].x);
                
                if (pattern.patternItemRocket[indexPatternItemRocket].itemType_Left[i].x != 0 && (int)pattern.patternItemRocket[indexPatternItemRocket].itemType_Left[i].x <= pattern.item_Pref.Count)
                {
                    float disZ = pattern.item_Pref[(int)pattern.patternItemRocket[indexPatternItemRocket].itemType_Left[i].x - 1].GetComponent<D3Item>().distanceZ;
                    GUI.color =pattern.item_Pref[(int)pattern.patternItemRocket[indexPatternItemRocket].itemType_Left[i].x - 1].GetComponent<D3Item>().colorPattern;
                    GUI.Box(new Rect(140 + offset_X, 310 + (i * 20), 20, 20), "");
                    if (disZ <= 1)
                        GUI.Box(new Rect(140 + offset_X, 310 + (i * 20), 20, 20 * (-(disZ - 1))), "");
                    else
                        GUI.Box(new Rect(140 + offset_X, 310 + (i * 20), 20, 20 * (-(disZ))), "");
                    GUI.color = normalColor;
                }
            }

            for (int i = 0; i < pattern.patternItemRocket[indexPatternItemRocket].itemType_Middle.Length; i++)
            {

                pattern.patternItemRocket[indexPatternItemRocket].itemType_Middle[i].x = EditorGUI.IntField(new Rect(210 + offset_X, 310 + (i * 20), 20, 20), (int)pattern.patternItemRocket[indexPatternItemRocket].itemType_Middle[i].x);
                if (pattern.patternItemRocket[indexPatternItemRocket].itemType_Middle[i].x != 0 && (int)pattern.patternItemRocket[indexPatternItemRocket].itemType_Middle[i].x <= pattern.item_Pref.Count)
                {
                    float disZ = pattern.item_Pref[(int)pattern.patternItemRocket[indexPatternItemRocket].itemType_Middle[i].x - 1].GetComponent<D3Item>().distanceZ;
                    GUI.color = pattern.item_Pref[(int)pattern.patternItemRocket[indexPatternItemRocket].itemType_Middle[i].x - 1].GetComponent<D3Item>().colorPattern;
                    GUI.Box(new Rect(210 + offset_X, 310 + (i * 20), 20, 20), "");
                    if (disZ <= 1)
                        GUI.Box(new Rect(210 + offset_X, 310 + (i * 20), 20, 20 * (-(disZ - 1))), "");
                    else
                        GUI.Box(new Rect(210 + offset_X, 310 + (i * 20), 20, 20 * (-(disZ))), "");
                    GUI.color = normalColor;
                }
            }

            for (int i = 0; i < pattern.patternItemRocket[indexPatternItemRocket].itemType_Right.Length; i++)
            {
                pattern.patternItemRocket[indexPatternItemRocket].itemType_Right[i].x = EditorGUI.IntField(new Rect(280 + offset_X, 310 + (i * 20), 20, 20), (int)pattern.patternItemRocket[indexPatternItemRocket].itemType_Right[i].x);
                if (pattern.patternItemRocket[indexPatternItemRocket].itemType_Right[i].x != 0 && (int)pattern.patternItemRocket[indexPatternItemRocket].itemType_Right[i].x <= pattern.item_Pref.Count)
                {
                    float disZ = pattern.item_Pref[(int)pattern.patternItemRocket[indexPatternItemRocket].itemType_Right[i].x - 1].GetComponent<D3Item>().distanceZ;
                    GUI.color = pattern.item_Pref[(int)pattern.patternItemRocket[indexPatternItemRocket].itemType_Right[i].x - 1].GetComponent<D3Item>().colorPattern;
                    GUI.Box(new Rect(280 + offset_X, 310 + (i * 20), 20, 20), "");
                    if (disZ <= 1)
                        GUI.Box(new Rect(280 + offset_X, 310 + (i * 20), 20, 20 * (-(disZ - 1))), "");
                    else
                        GUI.Box(new Rect(280 + offset_X, 310 + (i * 20), 20, 20 * (-(disZ))), "");
                    GUI.color = normalColor;
                }

            }
        }

    }

    private void SlotPatternItemSpecial()
    {
        if (pattern != null)
        {

            EditorGUI.LabelField(new Rect(20, 160, 200, 15), "Item Pattern");
            Color normalColor = GUI.color;
            for (int i = 0; i < pattern.patternItemSpecial[indexPatternItemSpecial].itemType_Left.Length; i++)
            {
                pattern.patternItemSpecial[indexPatternItemSpecial].itemType_Left[i].x = EditorGUI.IntField(new Rect(60 + offset_X, 310 + (i * 20), 20, 20), (int)pattern.patternItemSpecial[indexPatternItemSpecial].itemType_Left[i].x);
                if (pattern.patternItemSpecial[indexPatternItemSpecial].itemType_Left[i].x != 0 && (int)pattern.patternItemSpecial[indexPatternItemSpecial].itemType_Left[i].x <= pattern.item_Pref.Count)
                {
                    float disZ = pattern.item_Pref[(int)pattern.patternItemSpecial[indexPatternItemSpecial].itemType_Left[i].x - 1].GetComponent<D3Item>().distanceZ;
                    GUI.color = pattern.item_Pref[(int)pattern.patternItemSpecial[indexPatternItemSpecial].itemType_Left[i].x - 1].GetComponent<D3Item>().colorPattern;
                    GUI.Box(new Rect(60 + offset_X, 310 + (i * 20), 20, 20), "");
                    if (disZ <= 1)
                        GUI.Box(new Rect(60 + offset_X, 310 + (i * 20), 20, 20 * (-(disZ - 1))), "");
                    else
                        GUI.Box(new Rect(60 + offset_X, 310 + (i * 20), 20, 20 * (-(disZ))), "");
                    GUI.color = normalColor;
                }
            }

            for (int i = 0; i < pattern.patternItemSpecial[indexPatternItemSpecial].itemType_Middle.Length; i++)
            {
                pattern.patternItemSpecial[indexPatternItemSpecial].itemType_Middle[i].x = EditorGUI.IntField(new Rect(110 + offset_X, 310 + (i * 20), 20, 20), (int)pattern.patternItemSpecial[indexPatternItemSpecial].itemType_Middle[i].x);
                if (pattern.patternItemSpecial[indexPatternItemSpecial].itemType_Middle[i].x != 0 && (int)pattern.patternItemSpecial[indexPatternItemSpecial].itemType_Middle[i].x <= pattern.item_Pref.Count)
                {
                    float disZ = pattern.item_Pref[(int)pattern.patternItemSpecial[indexPatternItemSpecial].itemType_Middle[i].x - 1].GetComponent<D3Item>().distanceZ;
                    GUI.color = pattern.item_Pref[(int)pattern.patternItemSpecial[indexPatternItemSpecial].itemType_Middle[i].x - 1].GetComponent<D3Item>().colorPattern;
                    GUI.Box(new Rect(110 + offset_X, 310 + (i * 20), 20, 20), "");
                    if (disZ <= 1)
                        GUI.Box(new Rect(110 + offset_X, 310 + (i * 20), 20, 20 * (-(disZ - 1))), "");
                    else
                        GUI.Box(new Rect(110 + offset_X, 310 + (i * 20), 20, 20 * (-(disZ))), "");
                    GUI.color = normalColor;
                }
            }

            for (int i = 0; i < pattern.patternItemSpecial[indexPatternItemSpecial].itemType_Right.Length; i++)
            {
                pattern.patternItemSpecial[indexPatternItemSpecial].itemType_Right[i].x = EditorGUI.IntField(new Rect(160 + offset_X, 310 + (i * 20), 20, 20), (int)pattern.patternItemSpecial[indexPatternItemSpecial].itemType_Right[i].x);
                if (pattern.patternItemSpecial[indexPatternItemSpecial].itemType_Right[i].x != 0 && (int)pattern.patternItemSpecial[indexPatternItemSpecial].itemType_Right[i].x <= pattern.item_Pref.Count)
                {
                    float disZ = pattern.item_Pref[(int)pattern.patternItemSpecial[indexPatternItemSpecial].itemType_Right[i].x - 1].GetComponent<D3Item>().distanceZ;
                    GUI.color = pattern.item_Pref[(int)pattern.patternItemSpecial[indexPatternItemSpecial].itemType_Right[i].x - 1].GetComponent<D3Item>().colorPattern;
                    GUI.Box(new Rect(160 + offset_X, 310 + (i * 20), 20, 20), "");
                    if (disZ <= 1)
                        GUI.Box(new Rect(160 + offset_X, 310 + (i * 20), 20, 20 * (-(disZ - 1))), "");
                    else
                        GUI.Box(new Rect(160 + offset_X, 310 + (i * 20), 20, 20 * (-(disZ))), "");
                    GUI.color = normalColor;
                }

            }
        }

    }

    private void ShowDetial(){
		if(pattern != null)
        {
            switch (selected)
            {
                case 0: //Default

                    EditorGUI.LabelField(new Rect(250 + offset_X, 285, 200, 100), "[ 0 ] = Null");
                    EditorGUI.LabelField(new Rect(70 + offset_X, 250, 200, 100), "---Pattern End---");
                    EditorGUI.LabelField(new Rect(40 + offset_X, 890, 200, 100), "---Start (Generated First)---");

                    for (int i = 0; i < pattern.defaultPosItem_Left.Count; i++)
                    {
                        EditorGUI.LabelField(new Rect(25, 267 + (i * 20), 220, 100), "[ " + (i) + " ]" );
                    }


                    for (int i = 0; i < pattern.item_Pref.Count; i++)
                    {
                        if (pattern.item_Pref[i].GetComponent<D3Item>() != null)
                        {
                            pattern.item_Pref[i].GetComponent<D3Item>().colorPattern = EditorGUI.ColorField(new Rect(200 + offset_X, 350 + (i * 20), 30, 20), pattern.item_Pref[i].GetComponent<D3Item>().colorPattern);
                            EditorGUI.LabelField(new Rect(250 + offset_X, 305 + (i * 20), 200, 100), "[ " + (i + 1) + " ] = " + pattern.item_Pref[i].name + "");
                        }
                    }
                    EditorGUI.LabelField(new Rect(510 + offset_X_Building, 300, 220, 100), "[ 0 ] = Null");
                    for (int i = 0; i < pattern.building_Pref.Count; i++)
                    {
                        EditorGUI.LabelField(new Rect(510 + offset_X_Building, 330 + (i * 20), 220, 100), "[ " + (i + 1) + " ] = " + pattern.building_Pref[i].name + "");
                    }


                    break;





                case 1: //Rockect

                    EditorGUI.LabelField(new Rect(480 + offset_X, 285, 200, 100), "[ 0 ] = Null");
                    EditorGUI.LabelField(new Rect(170 + offset_X, 250, 200, 100), "---Pattern End---");
                    EditorGUI.LabelField(new Rect(140 + offset_X, 890, 200, 100), "---Start (Generated First)---");
                    for (int i = 0; i < pattern.defaultPosItem_LeftRocket.Count; i++)
                    {
                        EditorGUI.LabelField(new Rect(85, 267 + (i * 20), 220, 100), "[ " + (i) + " ]");
                    }
                    for (int i = 0; i < pattern.item_Pref.Count; i++)
                    {
                        if (pattern.item_Pref[i].GetComponent<D3Item>() != null)
                        {
                            pattern.item_Pref[i].GetComponent<D3Item>().colorPattern = EditorGUI.ColorField(new Rect(430 + offset_X, 350 + (i * 20), 30, 20), pattern.item_Pref[i].GetComponent<D3Item>().colorPattern);
                            EditorGUI.LabelField(new Rect(480 + offset_X, 305 + (i * 20), 200, 100), "[ " + (i + 1) + " ] = " + pattern.item_Pref[i].name + "");

                        }
                    }



                    break;




                case 2: //Special

                    EditorGUI.LabelField(new Rect(250 + offset_XBSpecial, 285, 200, 100), "[ 0 ] = Null");
                    EditorGUI.LabelField(new Rect(70 + offset_XBSpecial, 250, 200, 100), "---Pattern End---");
                    EditorGUI.LabelField(new Rect(40 + offset_XBSpecial, 890, 200, 100), "---Start (Generated First)---");
                    for (int i = 0; i < pattern.defaultPosItem_LeftSpecial.Count; i++)
                    {
                        EditorGUI.LabelField(new Rect(25, 267 + (i * 20), 220, 100), "[ " + (i) + " ]");
                    }
                    for (int i = 0; i < pattern.item_Pref.Count; i++)
                    {
                        if (pattern.item_Pref[i].GetComponent<D3Item>() != null)
                        {
                            pattern.item_Pref[i].GetComponent<D3Item>().colorPattern = EditorGUI.ColorField(new Rect(200 + offset_XBSpecial, 350 + (i * 20), 30, 20), pattern.item_Pref[i].GetComponent<D3Item>().colorPattern);
                            EditorGUI.LabelField(new Rect(250 + offset_XBSpecial, 305 + (i * 20), 200, 100), "[ " + (i + 1) + " ] = " + pattern.item_Pref[i].name + "");
                        }
                    }
                    EditorGUI.LabelField(new Rect(540 + offset_X_BuildingSpecial, 300, 220, 100), "[ 0 ] = Null");
                    for (int i = 0; i < pattern.building_Pref.Count; i++)
                    {
                        EditorGUI.LabelField(new Rect(540 + offset_X_BuildingSpecial, 330 + (i * 20), 220, 100), "[ " + (i + 1) + " ] = " + pattern.building_Pref[i].name + "");
                    }


                    break;
            }


           


           

        }
	}
}

[System.Serializable]
class WindowEditPositionItems : EditorWindow
{
    public int selected = 0;
    public D3PatternSystem pattern;
    SerializedObject patternSerialized;
    Vector2 scrollPos;

    private void OnEnable()
    {
        pattern = FindAnyObjectByType<D3PatternSystem>();
        patternSerialized = new SerializedObject(pattern);
    }

    void OnGUI()
    {
        EditorGUI.BeginChangeCheck();
        GUILayout.BeginVertical("GroupBox");
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        GUILayout.Space(5f);

        if (pattern != null)
        {
            patternSerialized.Update();
            GUILayout.BeginVertical("GroupBox");

            switch (selected)
            {
                case 0: //Default

                    GUILayout.Space(10f);

                    GUILayout.Label("Default Patter System", EditorStyles.boldLabel);

                    GUILayout.Space(10f);

                    Text();

                    GUILayout.Space(10f);
                    if (GUILayout.Button("Load Default Changes"))
                    {

                        pattern.PositionYItem = 0;
                        Vector3 pos = new Vector3(-1.8f, 0, 15);
                        for (int i = 0; i < 31; i++)
                        {
                            pattern.defaultPosItem_Left[i] = new Vector3(pos.x, pos.y, pos.z - i);
                        }
                        Vector3 pos2 = new Vector3(0, 0, 15);
                        for (int i = 0; i < 31; i++)
                        {
                            pattern.defaultPosItem_Middle[i] = new Vector3(pos2.x, pos2.y, pos2.z - i);
                        }

                        Vector3 pos3 = new Vector3(1.8f, 0, 15);
                        for (int i = 0; i < 31; i++)
                        {
                            pattern.defaultPosItem_Right[i] = new Vector3(pos3.x, pos3.y, pos3.z - i);
                        }

                    }
                    GUILayout.Space(20f);
                    EditorGUI.BeginChangeCheck();

                    pattern.PositionYItem = EditorGUILayout.FloatField("Position Y for All Item", pattern.PositionYItem);

                    if (EditorGUI.EndChangeCheck())
                    {
                        Vector3 pos = new Vector3(-1.8f, 0, 15);
                        for (int i = 0; i < 31; i++)
                        {
                            pattern.defaultPosItem_Left[i] = new Vector3(pos.x, pos.y + pattern.PositionYItem, pos.z - i);
                        }
                        Vector3 pos2 = new Vector3(0, 0, 15);
                        for (int i = 0; i < 31; i++)
                        {
                            pattern.defaultPosItem_Middle[i] = new Vector3(pos2.x, pos2.y + pattern.PositionYItem, pos2.z - i);
                        }

                        Vector3 pos3 = new Vector3(1.8f, 0, 15);
                        for (int i = 0; i < 31; i++)
                        {
                            pattern.defaultPosItem_Right[i] = new Vector3(pos3.x, pos3.y + pattern.PositionYItem, pos3.z - i);
                        }
                        patternSerialized.Update();
                    }
                    GUILayout.Space(10f);

                    SerializedProperty stringsProperty = patternSerialized.FindProperty("defaultPosItem_Left");
                    EditorGUILayout.PropertyField(stringsProperty, true);

                    GUILayout.Space(10f);

                    SerializedProperty stringsProperty2 = patternSerialized.FindProperty("defaultPosItem_Middle");
                    EditorGUILayout.PropertyField(stringsProperty2, true);

                    GUILayout.Space(10f);

                    SerializedProperty stringsProperty3 = patternSerialized.FindProperty("defaultPosItem_Right");
                    EditorGUILayout.PropertyField(stringsProperty3, true);


                    break;

                case 1: //Rockect
                    GUILayout.Space(10f);
                    
                    GUILayout.Label("Rocket Pattern System", EditorStyles.boldLabel);

                    GUILayout.Space(10f);

                    Text();

                    GUILayout.Space(10f);
                    if (GUILayout.Button("Load Default Changes")) {

                        pattern.PositionYItemRocket = 6;
                        Vector3 pos = new Vector3(-1.8f, 0, 15);
                        for (int i = 0; i < 31; i++)
                        {
                            pattern.defaultPosItem_LeftRocket[i] = new Vector3(pos.x, pattern.PositionYItemRocket, pos.z - i);
                        }
                        Vector3 pos2 = new Vector3(0, 0, 15);
                        for (int i = 0; i < 31; i++)
                        {
                            pattern.defaultPosItem_MiddleRocket[i] = new Vector3(pos2.x, pattern.PositionYItemRocket, pos2.z - i);
                        }

                        Vector3 pos3 = new Vector3(1.8f, 0, 15);
                        for (int i = 0; i < 31; i++)
                        {
                            pattern.defaultPosItem_RightRocket[i] = new Vector3(pos3.x, pattern.PositionYItemRocket, pos3.z - i);
                        }

                    }
                    GUILayout.Space(20f);

                    EditorGUI.BeginChangeCheck();

                    pattern.PositionYItemRocket = EditorGUILayout.FloatField("Position Y for All Item", pattern.PositionYItemRocket);

                    if (EditorGUI.EndChangeCheck())
                    {
                        Vector3 pos = new Vector3(-1.8f, 0, 15);
                        for (int i = 0; i < 31; i++)
                        {
                            pattern.defaultPosItem_LeftRocket[i] = new Vector3(pos.x, pos.y + pattern.PositionYItemRocket, pos.z - i);
                        }
                        Vector3 pos2 = new Vector3(0, 0, 15);
                        for (int i = 0; i < 31; i++)
                        {
                            pattern.defaultPosItem_MiddleRocket[i] = new Vector3(pos2.x, pos2.y + pattern.PositionYItemRocket, pos2.z - i);
                        }

                        Vector3 pos3 = new Vector3(1.8f, 0, 15);
                        for (int i = 0; i < 31; i++)
                        {
                            pattern.defaultPosItem_RightRocket[i] = new Vector3(pos3.x, pos3.y + pattern.PositionYItemRocket, pos3.z - i);
                        }
                        patternSerialized.Update();
                    }


                    GUILayout.Space(10f);
                    SerializedProperty stringsProperty4 = patternSerialized.FindProperty("defaultPosItem_LeftRocket");
                    EditorGUILayout.PropertyField(stringsProperty4, true);
                    GUILayout.Space(10f);

                    SerializedProperty stringsProperty5 = patternSerialized.FindProperty("defaultPosItem_MiddleRocket");
                    EditorGUILayout.PropertyField(stringsProperty5, true);
                    GUILayout.Space(10f);

                    SerializedProperty stringsProperty6 = patternSerialized.FindProperty("defaultPosItem_RightRocket");
                    EditorGUILayout.PropertyField(stringsProperty6, true);

                    break;

                case 2: //Special
                    GUILayout.Space(10f);

                    GUILayout.Label("Special Patter System", EditorStyles.boldLabel);

                    GUILayout.Space(10f);

                    Text();

                    GUILayout.Space(10f);
                    if (GUILayout.Button("Load Default Changes"))
                    {
                        pattern.PositionYItemSpecial = 0;
                        Vector3 pos = new Vector3(-1.8f, 0, 15);
                        for (int i = 0; i < 31; i++)
                        {
                            pattern.defaultPosItem_LeftSpecial[i] = new Vector3(pos.x, pattern.PositionYItemSpecial, pos.z - i);
                        }
                        Vector3 pos2 = new Vector3(0, 0, 15);
                        for (int i = 0; i < 31; i++)
                        {
                            pattern.defaultPosItem_MiddleSpecial[i] = new Vector3(pos2.x, pattern.PositionYItemSpecial, pos2.z - i);
                        }

                        Vector3 pos3 = new Vector3(1.8f, 0, 15);
                        for (int i = 0; i < 31; i++)
                        {
                            pattern.defaultPosItem_RightSpecial[i] = new Vector3(pos3.x, pattern.PositionYItemSpecial, pos3.z - i);
                        }

                    }
                    GUILayout.Space(20f);
                    EditorGUI.BeginChangeCheck();

                    pattern.PositionYItemSpecial = EditorGUILayout.FloatField("Position Y for All Item", pattern.PositionYItemSpecial);

                    if (EditorGUI.EndChangeCheck())
                    {
                        Vector3 pos = new Vector3(-1.8f, 0, 15);
                        for (int i = 0; i < 31; i++)
                        {
                            pattern.defaultPosItem_LeftSpecial[i] = new Vector3(pos.x, pattern.PositionYItemSpecial, pos.z - i);
                        }
                        Vector3 pos2 = new Vector3(0, 0, 15);
                        for (int i = 0; i < 31; i++)
                        {
                            pattern.defaultPosItem_MiddleSpecial[i] = new Vector3(pos2.x, pattern.PositionYItemSpecial, pos2.z - i);
                        }

                        Vector3 pos3 = new Vector3(1.8f, 0, 15);
                        for (int i = 0; i < 31; i++)
                        {
                            pattern.defaultPosItem_RightSpecial[i] = new Vector3(pos3.x, pattern.PositionYItemSpecial, pos3.z - i);
                        }
                        patternSerialized.Update();
                    }
                    GUILayout.Space(10f);

                    SerializedProperty stringsProperty7 = patternSerialized.FindProperty("defaultPosItem_LeftSpecial");
                    EditorGUILayout.PropertyField(stringsProperty7, true);

                    GUILayout.Space(10f);

                    SerializedProperty stringsProperty8 = patternSerialized.FindProperty("defaultPosItem_MiddleSpecial");
                    EditorGUILayout.PropertyField(stringsProperty8, true);

                    GUILayout.Space(10f);

                    SerializedProperty stringsProperty9 = patternSerialized.FindProperty("defaultPosItem_RightSpecial");
                    EditorGUILayout.PropertyField(stringsProperty9, true);


                    break;
            }
            
            GUILayout.EndVertical();
        }

        EditorGUILayout.EndScrollView();
        GUILayout.EndVertical();
        if (EditorGUI.EndChangeCheck())
        {

            


            patternSerialized.ApplyModifiedProperties();

            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());

        }
    }


    void Text()
    {
        GUILayout.Space(5f);
        EditorGUILayout.TextArea("Attention, if you do not know how Unity handles the positions of X, Y, Z of each element, do not modify them, you could destroy the game.", GUI.skin.GetStyle("HelpBox"));

        GUILayout.Space(5f);
        EditorGUILayout.TextArea("Do not modify the Z position of the elements.", GUI.skin.GetStyle("HelpBox"));

        GUILayout.Space(5f);
        EditorGUILayout.TextArea("It is only advisable to move the Y position positively, and the X position if the element is larger or smaller than normal.", GUI.skin.GetStyle("HelpBox"));

        GUILayout.Space(5f);
        EditorGUILayout.TextArea("If it doesn't work correctly, revert the changes..", GUI.skin.GetStyle("HelpBox"));

        GUILayout.Space(5f);
        EditorGUILayout.TextArea("Do not add more elements, only 31 are allowed, that is enough, do not insist.", GUI.skin.GetStyle("HelpBox"));

    }

}
