using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class D3PatternSystem : MonoBehaviour {

    public static D3PatternSystem instance;

    #region Global Variables Items
    //Global Variables Items
    [System.Serializable]
    public class SetItem
    {
        public Vector2[] itemType_Left = new Vector2[31];
        public Vector2[] itemType_Middle = new Vector2[31];
        public Vector2[] itemType_Right = new Vector2[31];
    }
    [System.Serializable]
    public class Item_Type
    {
        public List<D3Item> itemList = new List<D3Item>();
    }
    [System.Serializable]
    public class FloorItemSlot
    {
        public bool[] floor_Slot_Left, floor_Slot_Middle, floor_Slot_Right;
    }
    [System.Serializable]
    public class SetFloatItemType
    {
        public List<int> item = new List<int>();
    }
    public List<GameObject> item_Pref = new List<GameObject>();

    public bool ChangueSkybox;
    public Material SkyBoxForDefaultPatternSystem;
    bool DefaultInUse;
    public Material SkyBoxForSpecialtPatternSystem;
    bool SpecialtInUse;

    #endregion

    #region Global Variables Building
    //Global Variables Building
    public enum StateBuilding
    {
        Build_1, Build_2, Build_3, Build_4, Null
    }
    [System.Serializable]
    public class SetBuilding
    {

        public int[] stateBuilding_Left = new int[4];
        public int[] stateBuilding_Right = new int[4];
    }

    [System.Serializable]
    public class SetBuildingAmount
    {
        public int[] stateBuilding_Left = new int[4];
        public int[] stateBuilding_Right = new int[4];
    }

    public List<GameObject> building_Pref = new List<GameObject>();

    #endregion

    #region PooTransform

    [HideInInspector] public GameObject ItemsPool;
    [HideInInspector] public GameObject BuildingPool;
    [HideInInspector] public GameObject GeneratedPatternPool;
    [HideInInspector] public GameObject ScrappedPool;
    public bool FirstFloorClean = true;

    #endregion

    # region Building

    public List<Vector3> defaultPosBuilding_Left = new List<Vector3>();
    public List<Vector3> defaultPosBuilding_Right = new List<Vector3>();
    [HideInInspector] public List<SetBuilding> patternBuilding = new List<SetBuilding>();

    private List<D3Building> building_Script = new List<D3Building>();
    private int[] maxAmountBuilding;
    private int[] amountBuildingSpawn;
    private List<GameObject> building_Obj = new List<GameObject>();
    private List<SetFloatItemType> _buildingType = new List<SetFloatItemType>();
    private SetFloatItemType buildTypeMax;
    private int randomPattern;

    #endregion

    #region Item
    public float PositionYItem = 0f;
    [HideInInspector] public List<Vector3> defaultPosItem_Left = new List<Vector3>(31);
    [HideInInspector] public List<Vector3> defaultPosItem_Middle = new List<Vector3>(31);
    [HideInInspector] public List<Vector3> defaultPosItem_Right = new List<Vector3>(31);
    public List<SetItem> patternItem = new List<SetItem>();
    private List<Item_Type> item_Type_Script = new List<Item_Type>();
    private List<int> amount_Item_Pattern_Left = new List<int>();
    private List<int> amount_Item_Pattern_Middle = new List<int>();
    private List<int> amount_Item_Pattern_Right = new List<int>();
    private int[] amountItemSpawn;
    private List<SetFloatItemType> _itemType = new List<SetFloatItemType>();
    public  List<GameObject> item_Obj = new List<GameObject>();
    private SetFloatItemType itemTypeMax;
    private int randomItem;

    #endregion

    #region Item Rocket

    public float PositionYItemRocket = 5.8f;
    public float TimeForTheTransitionToRocket = 1f;
    [HideInInspector] public List<Vector3> defaultPosItem_LeftRocket = new List<Vector3>();
    [HideInInspector] public List<Vector3> defaultPosItem_MiddleRocket = new List<Vector3>();
    [HideInInspector] public List<Vector3> defaultPosItem_RightRocket = new List<Vector3>();
    private List<Item_Type> item_Type_ScriptRocket = new List<Item_Type>();
    private List<int> amount_Item_Pattern_LeftRocket = new List<int>();
    private List<int> amount_Item_Pattern_MiddleRocket = new List<int>();
    private List<int> amount_Item_Pattern_RightRocket = new List<int>();
    private int[] amountItemSpawnRocket;
    [HideInInspector] public List<SetItem> patternItemRocket = new List<SetItem>();
    private List<SetFloatItemType> _itemTypeRocket = new List<SetFloatItemType>();
    private List<GameObject> item_ObjRocket = new List<GameObject>();
    private SetFloatItemType itemTypeMaxRocket;
    private int randomItemRocket;

    #endregion

    #region Item Special
    public float PositionYItemSpecial = 0f;
    public float TimeForTheTransitionToSpecial = 1f;
    public float PositionAnimationYItemSpecial = 4f;
    [HideInInspector] public List<Vector3> defaultPosItem_LeftSpecial = new List<Vector3>(31);
    [HideInInspector] public List<Vector3> defaultPosItem_MiddleSpecial = new List<Vector3>(31);
    [HideInInspector] public List<Vector3> defaultPosItem_RightSpecial = new List<Vector3>(31);
    [HideInInspector] public List<SetItem> patternItemSpecial = new List<SetItem>();
    private List<Item_Type> item_Type_ScriptSpecial = new List<Item_Type>();
    private List<int> amount_Item_Pattern_LeftSpecial = new List<int>();
    private List<int> amount_Item_Pattern_MiddleSpecial = new List<int>();
    private List<int> amount_Item_Pattern_RightSpecial = new List<int>();
    private int[] amountItemSpawnSpecial;
    private List<SetFloatItemType> _itemTypeSpecial = new List<SetFloatItemType>();
    private List<GameObject> item_ObjSpecial = new List<GameObject>();
    private SetFloatItemType itemTypeMaxSpecial;
    private int randomItemSpecial;

    #endregion

    # region Building Special

    public List<Vector3> defaultPosBuildingSpecial_Left = new List<Vector3>();
    public List<Vector3> defaultPosBuildingSpecial_Right = new List<Vector3>();
    public List<SetBuilding> patternBuildingSpecial = new List<SetBuilding>();

    private List<D3Building> buildingSpecial_Script = new List<D3Building>();
    private int[] maxAmountBuildingSpecial;
    private int[] amountBuildingSpecialSpawn;
    private List<GameObject> buildingSpecial_Obj = new List<GameObject>();
    private List<SetFloatItemType> _buildingSpecialType = new List<SetFloatItemType>();
    private SetFloatItemType buildSpecialTypeMax;
    private int randomPatternbuildingSpecial;

    #endregion

    #region Floor

    [System.Serializable]
	public class Floor{
		public bool[] floor_Slot_Left, floor_Slot_Right;	
	}

    [System.Serializable]
	public class QueueFloor{
		public Floor floorClass;
        public Floor floorClassBuildingSpecial;
        public FloorItemSlot floorItemSlotClass;
        public FloorItemSlot floorItemSlotClassRocket;
        public FloorItemSlot floorItemSlotClassSpecial;
        public D3Floor floorObj;
        public List<D3Building> getBuilding = new List<D3Building>();
        public List<D3Building> getBuildingSpecial = new List<D3Building>();
        public List<D3Item> getItem = new List<D3Item>();
        public List<D3Item> getItemRocket = new List<D3Item>();
        public List<D3Item> getItemSpecial = new List<D3Item>();
    }
	
    public GameObject spawnObj_Pref;
    public D3Floor floor_Pref;

    private int amountFloorSpawn = 8;
    private float nextPosFloor = 32;

    private List<Floor> floor_Slot = new List<Floor>(4);
    private List<Floor> floorbuildingSpecial_Slot = new List<Floor>(4);
    private List<FloorItemSlot> floor_item_Slot = new List<FloorItemSlot>(31);
    private List<FloorItemSlot> floor_item_SlotRocket = new List<FloorItemSlot>(31);
    private List<FloorItemSlot> floor_item_SlotSpecial = new List<FloorItemSlot>(31);
    public List<QueueFloor> queneFloor = new List<QueueFloor>();
    private List<D3Floor> floor_Obj = new List<D3Floor>();

    #endregion

    #region Variable value in game

    [HideInInspector] public bool loadingComplete;
    [HideInInspector] public float loadingPercent;
    private GameObject spawnObj_Obj;
    private D3ColliderSpawnCheck colSpawnCheck;
	private Vector3 posFloorLast;
	private bool FirstFloor = true;
    //Defalut
    private Vector3 posStart = new Vector3(-100, -100, -100);
    private Vector3 angleLeft = new Vector3(0, 180, 0);
    private Vector3 angleRight = new Vector3(0, 0, 0);
    public int FloorsCleanwhenFinishingRocketItem = 3;
    public bool CleanwhenFinishingRocketItem = true;

    #endregion

    #region InstantiateObjectInGame

    [System.Serializable]
    [HideInInspector]
    public class ItemToInstantiate
    {
        public int DistanceToInstantiateObject = 0;
        public SetItem patternItemToInstantiate;
    }

    [HideInInspector]public List<GameObject> ObjectToInstantiateObject = new List<GameObject>();
    public List<GameObject> ObjectInstantiate = new List<GameObject>();
    public QueueFloor FloorObjectInstantiate;
    [HideInInspector] public List<ItemToInstantiate> ListItemsToInstantiate = new List<ItemToInstantiate>();
    int CantItems = 0;
    int ItemsGenerated = 0;
    bool EnabledDistanceToInstantiateObject = true;
    public float Distance = 0;

    #endregion



    public D3PatternSystem(){
		SettingVariableFirst ();
	}

	public void SettingVariableFirst(){
		//Building ---
		if (defaultPosBuilding_Left.Count <= 0) {
			Vector3 pos = new Vector3(-3,0,16);
			for(int i = 0; i < 4; i++){
				defaultPosBuilding_Left.Add(new Vector3(pos.x,pos.y,pos.z-(i*8)));
			}
		}

		if(defaultPosBuilding_Right.Count <= 0){
			Vector3 pos = new Vector3(3,0,16);
			for(int i = 0; i < 4; i++){
				defaultPosBuilding_Right.Add(new Vector3(pos.x,pos.y,pos.z-(i*8)));
			}
		}
        //-------

        //Building Special---
        if (defaultPosBuildingSpecial_Left.Count <= 0)
        {
            Vector3 pos = new Vector3(-3, 0, 16);
            for (int i = 0; i < 4; i++)
            {
                defaultPosBuildingSpecial_Left.Add(new Vector3(pos.x, pos.y, pos.z - (i * 8)));
            }
        }

        if (defaultPosBuildingSpecial_Right.Count <= 0)
        {
            Vector3 pos = new Vector3(3, 0, 16);
            for (int i = 0; i < 4; i++)
            {
                defaultPosBuildingSpecial_Right.Add(new Vector3(pos.x, pos.y, pos.z - (i * 8)));
            }
        }
        //-------

        //Items ------
        if (defaultPosItem_Left.Count <= 0){
			Vector3 pos = new Vector3(-1.8f,0,15);
			for(int i = 0; i < 31; i++){
				defaultPosItem_Left.Add(new Vector3(pos.x,pos.y,pos.z-i));
			}
		}


		if(defaultPosItem_Middle.Count <= 0){
			Vector3 pos = new Vector3(0,0,15);
			for(int i = 0; i < 31; i++){
				defaultPosItem_Middle.Add(new Vector3(pos.x, pos.y, pos.z-i));
			}
		}

		if(defaultPosItem_Right.Count <= 0){
			Vector3 pos = new Vector3(1.8f,0,15);
			for(int i = 0; i < 31; i++){
				defaultPosItem_Right.Add(new Vector3(pos.x, pos.y, pos.z-i));
			}
		}
        //-----------


        //Items Rocket------
        if (defaultPosItem_LeftRocket.Count <= 0)
        {
            Vector3 pos = new Vector3(-1.8f, 0, 15);
            for (int i = 0; i < 31; i++)
            {
                defaultPosItem_LeftRocket.Add(new Vector3(pos.x, pos.y+PositionYItemRocket, pos.z - i));
            }
        }


        if (defaultPosItem_MiddleRocket.Count <= 0)
        {
            Vector3 pos = new Vector3(0, 0, 15);
            for (int i = 0; i < 31; i++)
            {
                defaultPosItem_MiddleRocket.Add(new Vector3(pos.x, pos.y+PositionYItemRocket, pos.z - i));
            }
        }

        if (defaultPosItem_RightRocket.Count <= 0)
        {
            Vector3 pos = new Vector3(1.8f, 0, 15);
            for (int i = 0; i < 31; i++)
            {
                defaultPosItem_RightRocket.Add(new Vector3(pos.x, pos.y + PositionYItemRocket, pos.z - i));
            }
        }
        //-----------

        //Items Rocket------
        if (defaultPosItem_LeftSpecial.Count <= 0)
        {
            Vector3 pos = new Vector3(-1.8f, 0, 15);
            for (int i = 0; i < 31; i++)
            {
                defaultPosItem_LeftSpecial.Add(new Vector3(pos.x, pos.y, pos.z - i));
            }
        }


        if (defaultPosItem_MiddleSpecial.Count <= 0)
        {
            Vector3 pos = new Vector3(0, 0, 15);
            for (int i = 0; i < 31; i++)
            {
                defaultPosItem_MiddleSpecial.Add(new Vector3(pos.x, pos.y, pos.z - i));
            }
        }

        if (defaultPosItem_RightSpecial.Count <= 0)
        {
            Vector3 pos = new Vector3(1.8f, 0, 15);
            for (int i = 0; i < 31; i++)
            {
                defaultPosItem_RightSpecial.Add(new Vector3(pos.x, pos.y, pos.z - i));
            }
        }
        //-----------



        if (patternBuilding.Count <= 0) {
			patternBuilding.Add(new SetBuilding());
		}


        if (patternBuildingSpecial.Count <= 0)
        {
            patternBuildingSpecial.Add(new SetBuilding());
        }

        if (patternItem.Count <= 0){
			patternItem.Add(new SetItem());
		}

        if (patternItemRocket.Count <= 0)
        {
            patternItemRocket.Add(new SetItem());
        }

        if (patternItemSpecial.Count <= 0)
        {
            patternItemSpecial.Add(new SetItem());
        }
       
    }

	void Start(){
		
		SettingVariableFirst();
        instance = this;
		GenerateObjectPool();
		if (ItemsPool != null && BuildingPool != null && GeneratedPatternPool != null)
		{
			StartCoroutine(CalAmountItem());
		}
		else {
            GenerateObjectPool();
        }

        EnabledInstantiateObject();

    }

    void EnabledInstantiateObject()
    {
        CantItems = 0;

        for (int i = 0; i < ListItemsToInstantiate.Count; i++)
        {
            if (ListItemsToInstantiate[i].DistanceToInstantiateObject > 0)
            {
                CantItems++;
            }
        }
        if (CantItems > 0)
        {
            EnabledDistanceToInstantiateObject = true;
        }
        else
        {
            EnabledDistanceToInstantiateObject = false;
        }
    }

    public void HidenObjectInPatterSystem() {

        for (int x = 0; x < FloorsCleanwhenFinishingRocketItem; x++)
        {
            int i = 0;

            int itemCount = queneFloor[x].getItem.Count;
            while (i < itemCount)
            {
                if (queneFloor[x].getItem[0] != null)
                {
                    queneFloor[x].getItem[0].itemActive = false;
                    queneFloor[x].getItem[0].transform.parent = ScrappedPool.transform;
                    queneFloor[x].getItem[0].transform.position = posStart;
                    ReturnItemWithType(queneFloor[x].getItem[0]);
                    queneFloor[x].getItem.RemoveRange(0, 1);
                }
                i++;
            }

            i = 0;
            int itemCountSpecial = queneFloor[0].getItemSpecial.Count;
            while (i < itemCountSpecial)
            {
                if (queneFloor[x].getItemSpecial[0] != null)
                {
                    queneFloor[x].getItemSpecial[0].itemActive = false;
                    queneFloor[x].getItemSpecial[0].transform.parent = ScrappedPool.transform;
                    queneFloor[x].getItemSpecial[0].transform.position = posStart;
                    ReturnItemWithTypeSpecial(queneFloor[x].getItemSpecial[0]);
                    queneFloor[x].getItemSpecial.RemoveRange(0, 1);
                }

                i++;
            }

            i = 0;
            while (i < queneFloor[x].floorItemSlotClass.floor_Slot_Left.Length)
            {
                queneFloor[x].floorItemSlotClass.floor_Slot_Left[i] = false;
                queneFloor[x].floorItemSlotClass.floor_Slot_Middle[i] = false;
                queneFloor[x].floorItemSlotClass.floor_Slot_Right[i] = false;
                i++;
            }


            i = 0;
            while (i < queneFloor[x].floorItemSlotClassSpecial.floor_Slot_Left.Length)
            {
                queneFloor[x].floorItemSlotClassSpecial.floor_Slot_Left[i] = false;
                queneFloor[x].floorItemSlotClassSpecial.floor_Slot_Middle[i] = false;
                queneFloor[x].floorItemSlotClassSpecial.floor_Slot_Right[i] = false;
                i++;
            }
        }

        for (int x = 0; x < FloorsCleanwhenFinishingRocketItem; x++)
        {
            RemoveInstantiateObject(queneFloor[x]);
        }


    }


    public void HidenObjectForInstatiateNew(int NumFloor)
    {
        FloorObjectInstantiate = queneFloor[NumFloor];

        int i = 0;

        int itemCount = queneFloor[NumFloor].getItem.Count;

        while (i < itemCount)
        {
            if (queneFloor[NumFloor].getItem[0] != null)
            {
                queneFloor[NumFloor].getItem[0].itemActive = false;
                queneFloor[NumFloor].getItem[0].transform.parent = ScrappedPool.transform;
                queneFloor[NumFloor].getItem[0].transform.position = posStart;
                ReturnItemWithType(queneFloor[NumFloor].getItem[0]);
                queneFloor[NumFloor].getItem.RemoveRange(0, 1);
            }
            i++;
        }

        i = 0;
        while (i < queneFloor[NumFloor].floorItemSlotClass.floor_Slot_Left.Length)
        {
            queneFloor[NumFloor].floorItemSlotClass.floor_Slot_Left[i] = false;
            queneFloor[NumFloor].floorItemSlotClass.floor_Slot_Middle[i] = false;
            queneFloor[NumFloor].floorItemSlotClass.floor_Slot_Right[i] = false;
            i++;
        }

        RemoveInstantiateObject(FloorObjectInstantiate);

        InstantiateItemsInGame(NumFloor);
    }


    void InstantiateItemsInGame(int NumFloor)
    {
        GameObject ListItemsInstantiate = new GameObject("ListItemsInstantiate");
        ListItemsInstantiate.gameObject.transform.parent = queneFloor[NumFloor].floorObj.transform;
        ListItemsInstantiate.transform.localPosition = Vector3.zero;
        
        for (int i = 0; i < ListItemsToInstantiate[ItemsGenerated].patternItemToInstantiate.itemType_Left.Length; i++)
        {
            if (ListItemsToInstantiate[ItemsGenerated].patternItemToInstantiate.itemType_Left[i].x > 0 )
            {
                int Item = (int)ListItemsToInstantiate[ItemsGenerated].patternItemToInstantiate.itemType_Left[i].x -1;

                if (ObjectToInstantiateObject[Item] != null)
                {
                    GameObject go = Instantiate(ObjectToInstantiateObject[Item], defaultPosItem_Left[i], Quaternion.identity);
                    ObjectInstantiate.Add(go);
                    if (go.GetComponent<D3Item>())
                    {
                        go.GetComponent<D3Item>().itemActive = true;
                    }
                    go.transform.parent = ListItemsInstantiate.transform;
                    go.transform.localPosition = defaultPosItem_Left[i];
                }
                
            }
        }

        for (int i = 0; i < ListItemsToInstantiate[ItemsGenerated].patternItemToInstantiate.itemType_Middle.Length; i++)
        {
            if (ListItemsToInstantiate[ItemsGenerated].patternItemToInstantiate.itemType_Middle[i].x > 0)
            {
                int Item = (int)ListItemsToInstantiate[ItemsGenerated].patternItemToInstantiate.itemType_Middle[i].x - 1;
                if (ObjectToInstantiateObject.Count > Item)
                {
                    if (ObjectToInstantiateObject[Item] != null)
                    {
                        GameObject go = Instantiate(ObjectToInstantiateObject[Item], defaultPosItem_Middle[i], Quaternion.identity);
                        ObjectInstantiate.Add(go);
                        if (go.GetComponent<D3Item>())
                        {
                            go.GetComponent<D3Item>().itemActive = true;
                        }
                        go.transform.parent = ListItemsInstantiate.transform;
                        go.transform.localPosition = defaultPosItem_Middle[i];
                    }
                }

            }
        }

        for (int i = 0; i < ListItemsToInstantiate[ItemsGenerated].patternItemToInstantiate.itemType_Right.Length; i++)
        {
            if (ListItemsToInstantiate[ItemsGenerated].patternItemToInstantiate.itemType_Right[i].x > 0)
            {
                int Item = (int)ListItemsToInstantiate[ItemsGenerated].patternItemToInstantiate.itemType_Right[i].x - 1;

                if (ObjectToInstantiateObject[Item] != null)
                {
                    GameObject go = Instantiate(ObjectToInstantiateObject[Item], defaultPosItem_Right[i], Quaternion.identity);
                    ObjectInstantiate.Add(go);
                    if (go.GetComponent<D3Item>())
                    {
                        go.GetComponent<D3Item>().itemActive = true;
                    }
                    go.transform.parent = ListItemsInstantiate.transform;
                    go.transform.localPosition = defaultPosItem_Right[i];
                }

            }
        }

        ItemsGenerated++;

        if (ItemsGenerated < CantItems)
        {
            Debug.Log("Generate Items By Distance: Patterns To Generate: " + CantItems + " Patterns Generated: " + ItemsGenerated + " Next Distance: " + ListItemsToInstantiate[ItemsGenerated].DistanceToInstantiateObject);
        }
    }


    private void Update()
    {
        if (EnabledDistanceToInstantiateObject)
        {
            if (ItemsGenerated < CantItems)
            {
                if (D3GameAttribute.gameAttribute != null)
                {
                    if (D3Controller.instace && !D3GameAttribute.gameAttribute.pause)
                    {
                        if (!D3Controller.instace.isSprint && !D3Controller.instace.IsSpecial)
                        {
                            Distance += D3GameAttribute.gameAttribute.speed * Time.deltaTime;
                        }
                        
                        if ((int)Distance == (ListItemsToInstantiate[ItemsGenerated].DistanceToInstantiateObject - 60))
                        {
                            if (ItemsGenerated < CantItems)
                            {
                                HidenObjectForInstatiateNew(3);
                            }
                        }

                    }
                }
            }
            else
            {
                EnabledDistanceToInstantiateObject = false;
            }   
        }

        if (D3Controller.instace)
        {
            if (D3Controller.instace.isSprint)
            {
                for (int a = 0; queneFloor.Count > a; a++)
                {
                    for (int x = 0; instance.queneFloor[a].getItemRocket.Count > x; x++)
                    {
                        queneFloor[a].getItemRocket[x].gameObject.SetActive(true);
                    }
                }
            }
            else {
                for (int a = 0; queneFloor.Count > a; a++)
                {
                    for (int x = 0; instance.queneFloor[a].getItemRocket.Count > x; x++)
                    {
                        queneFloor[a].getItemRocket[x].gameObject.SetActive(false);
                    }
                }

            }

            if (D3Controller.instace.IsSpecial)
            {
                if (!SpecialtInUse && ChangueSkybox && SkyBoxForSpecialtPatternSystem != null)
                {
                    
                    SpecialtInUse = true;
                    DefaultInUse = false;
                    D3SoundManager.instance.StopCoroutine(D3SoundManager.instance.StartBGM());
                    D3SoundManager.instance.StartCoroutine(D3SoundManager.instance.StartBGMSpecial());
                    RenderSettings.skybox = SkyBoxForSpecialtPatternSystem;
                }

                for (int a = 0; queneFloor.Count > a; a++)
                {
                    instance.queneFloor[a].floorObj.UseFloorSpecial();

                    for (int x = 0; instance.queneFloor[a].getItem.Count > x; x++)
                    {
                        queneFloor[a].getItem[x].gameObject.SetActive(false);
                    }
                    for (int x = 0; instance.queneFloor[a].getBuilding.Count > x; x++)
                    {
                        queneFloor[a].getBuilding[x].gameObject.SetActive(false);
                    }
                    for (int x = 0; instance.queneFloor[a].getItemSpecial.Count > x; x++)
                    {
                        queneFloor[a].getItemSpecial[x].gameObject.SetActive(true);
                    }
                    for (int x = 0; instance.queneFloor[a].getBuildingSpecial.Count > x; x++)
                    {
                        queneFloor[a].getBuildingSpecial[x].gameObject.SetActive(true);
                    }
                }
            }
            else
            {
                if (!DefaultInUse && ChangueSkybox && SkyBoxForDefaultPatternSystem != null)
                {
                    
                    SpecialtInUse = false;
                    DefaultInUse = true;
                    D3SoundManager.instance.StopCoroutine(D3SoundManager.instance.StartBGMSpecial());
                    D3SoundManager.instance.StartCoroutine(D3SoundManager.instance.StartBGM());
                    RenderSettings.skybox = SkyBoxForDefaultPatternSystem;
                }
                for (int a = 0; queneFloor.Count > a; a++)
                {
                    instance.queneFloor[a].floorObj.UseFloorDefault() ;

                    for (int x = 0; instance.queneFloor[a].getItem.Count > x; x++)
                    {
                        queneFloor[a].getItem[x].gameObject.SetActive(true);
                    }
                    for (int x = 0; instance.queneFloor[a].getItemSpecial.Count > x; x++)
                    {
                        queneFloor[a].getItemSpecial[x].gameObject.SetActive(false);
                    }
                    for (int x = 0; instance.queneFloor[a].getBuilding.Count > x; x++)
                    {
                        queneFloor[a].getBuilding[x].gameObject.SetActive(true);
                    }
                    for (int x = 0; instance.queneFloor[a].getBuildingSpecial.Count > x; x++)
                    {
                        queneFloor[a].getBuildingSpecial[x].gameObject.SetActive(false);
                    }
                }

            }

        }
    }
    public void GenerateObjectPool()
    {
        GameObject Pool1 = new GameObject("Pool1");
        GameObject Pool2 = new GameObject("Pool2");
        GameObject Pool3 = new GameObject("Pool3");
        GameObject Pool4 = new GameObject("Pool4");
        ItemsPool = Pool1;
        ItemsPool.transform.parent = transform;
        ItemsPool.name = "ItemsPool";
        BuildingPool = Pool2;
        BuildingPool.transform.parent = transform;
        BuildingPool.name = "BuildingPool";
        GeneratedPatternPool = Pool3;
        GeneratedPatternPool.transform.parent = transform;
        GeneratedPatternPool.name = "GeneratedPatternPool";
        ScrappedPool = Pool4;
        ScrappedPool.transform.parent = transform;
        ScrappedPool.name = "ScrappedPool";
    }
    IEnumerator CalAmountItem(){
		//25%
		ConvertPatternToItemTpyeSet();
		itemTypeMax = new SetFloatItemType();
		int i = 0;
		while(i < item_Pref.Count){
			itemTypeMax.item.Add(0);
			i++;
		}
		i = 0;
		loadingPercent = 1;
		while(i < _itemType.Count){
			int j = 0;
			while(j < _itemType[i].item.Count){
				if(_itemType[i].item[j] > itemTypeMax.item[j]){
					itemTypeMax.item[j] = _itemType[i].item[j];	
				}
				j++;
			}
			i++;
		}
		i = 0;
		loadingPercent = 3;
		amountItemSpawn = new int[itemTypeMax.item.Count];
		while(i < amountItemSpawn.Length){
			amountItemSpawn[i] = itemTypeMax.item[i] * amountFloorSpawn;
			amountItemSpawn[i]++;
			i++;
		}
		yield return 0;
		loadingPercent = 5;
		StartCoroutine(CalAmountItemRocket());
	}
    private void ConvertPatternToItemTpyeSet()
    {
        int i = 0;
        while (i < patternItem.Count)
        {
            _itemType.Add(new SetFloatItemType());
            int j = 0;
            while (j < item_Pref.Count)
            {
                _itemType[i].item.Add(0);
                j++;
            }
            i++;
        }
        i = 0;
        while (i < patternItem.Count)
        {
            int j = 0;
            //Left
            while (j < patternItem[i].itemType_Left.Length)
            {
                int k = 0;
                while (k < item_Pref.Count)
                {
                    if (patternItem[i].itemType_Left[j].x == k + 1)
                    {
                        _itemType[i].item[k] += 1;
                    }

                    k++;
                }
                j++;
            }
            j = 0;
            //Middle
            while (j < patternItem[i].itemType_Middle.Length)
            {
                int k = 0;
                while (k < item_Pref.Count)
                {
                    if (patternItem[i].itemType_Middle[j].x == k + 1)
                    {
                        _itemType[i].item[k] += 1;
                    }

                    k++;
                }
                j++;
            }
            j = 0;
            //Right
            while (j < patternItem[i].itemType_Right.Length)
            {
                int k = 0;
                while (k < item_Pref.Count)
                {
                    if (patternItem[i].itemType_Right[j].x == k + 1)
                    {
                        _itemType[i].item[k] += 1;
                    }

                    k++;
                }
                j++;
            }
            i++;
        }
    }
    IEnumerator CalAmountItemRocket()
    {
        //35%
        ConvertPatternToItemTpyeSetRocket();
        itemTypeMaxRocket = new SetFloatItemType();
        int i = 0;
        while (i < item_Pref.Count)
        {
            itemTypeMaxRocket.item.Add(0);
            i++;
        }
        i = 0;
        loadingPercent = 7;
        while (i < _itemTypeRocket.Count)
        {
            int j = 0;
            while (j < _itemTypeRocket[i].item.Count)
            {
                if (_itemTypeRocket[i].item[j] > itemTypeMaxRocket.item[j])
                {
                    itemTypeMaxRocket.item[j] = _itemTypeRocket[i].item[j];
                }
                j++;
            }
            i++;
        }
        i = 0;
        loadingPercent = 8;
        amountItemSpawnRocket = new int[itemTypeMaxRocket.item.Count];
        while (i < amountItemSpawnRocket.Length)
        {
            amountItemSpawnRocket[i] = itemTypeMaxRocket.item[i] * amountFloorSpawn;
            amountItemSpawnRocket[i]++;
            i++;
        }
        yield return 0;
        loadingPercent = 10;
        StartCoroutine(CalAmountItemSpecial());
    }
    private void ConvertPatternToItemTpyeSetRocket()
    {
        int i = 0;
        while (i < patternItemRocket.Count)
        {
            _itemTypeRocket.Add(new SetFloatItemType());
            int j = 0;
            while (j < item_Pref.Count)
            {
                _itemTypeRocket[i].item.Add(0);
                j++;
            }
            i++;
        }
        i = 0;
        while (i < patternItemRocket.Count)
        {
            int j = 0;
            //Left
            while (j < patternItemRocket[i].itemType_Left.Length)
            {
                int k = 0;
                while (k < item_Pref.Count)
                {
                    if (patternItemRocket[i].itemType_Left[j].x == k + 1)
                    {
                        _itemTypeRocket[i].item[k] += 1;
                    }

                    k++;
                }
                j++;
            }
            j = 0;
            //Middle
            while (j < patternItemRocket[i].itemType_Middle.Length)
            {
                int k = 0;
                while (k < item_Pref.Count)
                {
                    if (patternItemRocket[i].itemType_Middle[j].x == k + 1)
                    {
                        _itemTypeRocket[i].item[k] += 1;
                    }

                    k++;
                }
                j++;
            }
            j = 0;
            //Right
            while (j < patternItemRocket[i].itemType_Right.Length)
            {
                int k = 0;
                while (k < item_Pref.Count)
                {
                    if (patternItemRocket[i].itemType_Right[j].x == k + 1)
                    {
                        _itemTypeRocket[i].item[k] += 1;
                    }

                    k++;
                }
                j++;
            }
            i++;
        }
    }
    IEnumerator CalAmountItemSpecial()
    {
        //35%
        ConvertPatternToItemTpyeSetSpecial();
        itemTypeMaxSpecial = new SetFloatItemType();
        int i = 0;
        while (i < item_Pref.Count)
        {
            itemTypeMaxSpecial.item.Add(0);
            i++;
        }
        i = 0;
        loadingPercent = 12;
        while (i < _itemTypeSpecial.Count)
        {
            int j = 0;
            while (j < _itemTypeSpecial[i].item.Count)
            {
                if (_itemTypeSpecial[i].item[j] > itemTypeMaxSpecial.item[j])
                {
                    itemTypeMaxSpecial.item[j] = _itemTypeSpecial[i].item[j];
                }
                j++;
            }
            i++;
        }
        i = 0;
        loadingPercent = 15;
        amountItemSpawnSpecial = new int[itemTypeMaxSpecial.item.Count];
        while (i < amountItemSpawnSpecial.Length)
        {
            amountItemSpawnSpecial[i] = itemTypeMaxSpecial.item[i] * amountFloorSpawn;
            amountItemSpawnSpecial[i]++;
            i++;
        }
        yield return 0;
        loadingPercent = 17;
        StartCoroutine(CalAmountBuilding());
    }
    private void ConvertPatternToItemTpyeSetSpecial()
    {
        int i = 0;
        while (i < patternItemSpecial.Count)
        {
            _itemTypeSpecial.Add(new SetFloatItemType());
            int j = 0;
            while (j < item_Pref.Count)
            {
                _itemTypeSpecial[i].item.Add(0);
                j++;
            }
            i++;
        }
        i = 0;
        while (i < patternItemSpecial.Count)
        {
            int j = 0;
            //Left
            while (j < patternItemSpecial[i].itemType_Left.Length)
            {
                int k = 0;
                while (k < item_Pref.Count)
                {
                    if (patternItemSpecial[i].itemType_Left[j].x == k + 1)
                    {
                        _itemTypeSpecial[i].item[k] += 1;
                    }

                    k++;
                }
                j++;
            }
            j = 0;
            //Middle
            while (j < patternItemSpecial[i].itemType_Middle.Length)
            {
                int k = 0;
                while (k < item_Pref.Count)
                {
                    if (patternItemSpecial[i].itemType_Middle[j].x == k + 1)
                    {
                        _itemTypeSpecial[i].item[k] += 1;
                    }

                    k++;
                }
                j++;
            }
            j = 0;
            //Right
            while (j < patternItemSpecial[i].itemType_Right.Length)
            {
                int k = 0;
                while (k < item_Pref.Count)
                {
                    if (patternItemSpecial[i].itemType_Right[j].x == k + 1)
                    {
                        _itemTypeSpecial[i].item[k] += 1;
                    }

                    k++;
                }
                j++;
            }
            i++;
        }
    }
    IEnumerator CalAmountBuilding(){
		//50%
		ConvertPatternToBuildingTpyeSet();
		buildTypeMax = new SetFloatItemType();
		int i = 0;
		while(i < building_Pref.Count){
			buildTypeMax.item.Add(0);
			i++;
		}
		i = 0;
		loadingPercent = 18;
		while(i < _buildingType.Count){
			int j = 0;
			while(j < _buildingType[i].item.Count){
				if(_buildingType[i].item[j] > buildTypeMax.item[j]){
					buildTypeMax.item[j] = _buildingType[i].item[j];	
				}
				j++;
			}
			i++;
		}
		i = 0;
		loadingPercent = 20;
		amountBuildingSpawn = new int[buildTypeMax.item.Count];
		while(i < amountBuildingSpawn.Length){
			amountBuildingSpawn[i] = buildTypeMax.item[i] * amountFloorSpawn;
			amountBuildingSpawn[i]++;
			i++;
		}
		yield return 0;
		loadingPercent = 22;
		StartCoroutine(CalAmountBuildingSpecial());
	}
	
	private void ConvertPatternToBuildingTpyeSet(){
		int i = 0;
		while(i < patternBuilding.Count){
			_buildingType.Add(new SetFloatItemType());
			int j = 0;
			while(j < building_Pref.Count){
				_buildingType[i].item.Add(0);
				j++;
			}
			i++;	
		}
		i = 0;
		while(i < patternBuilding.Count){
			int j = 0;
			//Left
			while(j < patternBuilding[i].stateBuilding_Left.Length){
				int k = 0;
				while(k < building_Pref.Count){
					if(patternBuilding[i].stateBuilding_Left[j] == k+1){
						_buildingType[i].item[k] += 1;
					}
					
					k++;
				}
				j++;
			}
			j = 0;
			//Right
			while(j < patternBuilding[i].stateBuilding_Right.Length){
				int k = 0;
				while(k < building_Pref.Count){
					if(patternBuilding[i].stateBuilding_Right[j] == k+1){
						_buildingType[i].item[k] += 1;
					}
					
					k++;
				}
				j++;
			}
			i++;
		}
	}

    IEnumerator CalAmountBuildingSpecial()
    {
        //50%
        ConvertPatternToBuildingSpecialTpyeSet();
        buildSpecialTypeMax = new SetFloatItemType();
        int i = 0;
        while (i < building_Pref.Count)
        {
            buildSpecialTypeMax.item.Add(0);
            i++;
        }
        i = 0;
        loadingPercent = 24;
        while (i < _buildingSpecialType.Count)
        {
            int j = 0;
            while (j < _buildingSpecialType[i].item.Count)
            {
                if (_buildingSpecialType[i].item[j] > buildSpecialTypeMax.item[j])
                {
                    buildSpecialTypeMax.item[j] = _buildingSpecialType[i].item[j];
                }
                j++;
            }
            i++;
        }
        i = 0;
        loadingPercent = 26;
        amountBuildingSpecialSpawn = new int[buildSpecialTypeMax.item.Count];
        while (i < amountBuildingSpecialSpawn.Length)
        {
            amountBuildingSpecialSpawn[i] = buildSpecialTypeMax.item[i] * amountFloorSpawn;
            amountBuildingSpecialSpawn[i]++;
            i++;
        }
        yield return 0;
        loadingPercent = 28;
        StartCoroutine(InitBuilding());
    }

    private void ConvertPatternToBuildingSpecialTpyeSet()
    {
        int i = 0;
        while (i < patternBuildingSpecial.Count)
        {
            _buildingSpecialType.Add(new SetFloatItemType());
            int j = 0;
            while (j < building_Pref.Count)
            {
                _buildingSpecialType[i].item.Add(0);
                j++;
            }
            i++;
        }
        i = 0;
        while (i < patternBuildingSpecial.Count)
        {
            int j = 0;
            //Left
            while (j < patternBuildingSpecial[i].stateBuilding_Left.Length)
            {
                int k = 0;
                while (k < building_Pref.Count)
                {
                    if (patternBuildingSpecial[i].stateBuilding_Left[j] == k + 1)
                    {
                        _buildingSpecialType[i].item[k] += 1;
                    }

                    k++;
                }
                j++;
            }
            j = 0;
            //Right
            while (j < patternBuildingSpecial[i].stateBuilding_Right.Length)
            {
                int k = 0;
                while (k < building_Pref.Count)
                {
                    if (patternBuildingSpecial[i].stateBuilding_Right[j] == k + 1)
                    {
                        _buildingSpecialType[i].item[k] += 1;
                    }

                    k++;
                }
                j++;
            }
            i++;
        }
    }

    IEnumerator InitBuilding(){
		//75%
		int i = 0;
		while(i < building_Pref.Count){
			int j = 0;
			while(j < amountBuildingSpawn[i]){
				GameObject go = (GameObject)Instantiate(building_Pref[i], posStart, Quaternion.identity);
				go.transform.parent = BuildingPool.transform;
				go.name = "Building["+i+"]["+j+"]";
				building_Obj.Add(go);
				D3Building building = go.GetComponent<D3Building>();
				building.buildIndex = i;
				building_Script.Add(building);
				j++;
			}
			i++;
			yield return 0;
		}

        i = 0;
        while (i < building_Pref.Count)
        {
            int j = 0;
            while (j < amountBuildingSpecialSpawn[i])
            {
                GameObject go = (GameObject)Instantiate(building_Pref[i], posStart, Quaternion.identity);
                go.transform.parent = BuildingPool.transform;
                go.name = "BuildingSpecial[" + i + "][" + j + "]";
                buildingSpecial_Obj.Add(go);
                D3Building buildingSpecial = go.GetComponent<D3Building>();
                buildingSpecial.buildIndex = i;
                buildingSpecial_Script.Add(buildingSpecial);
                j++;
            }
            i++;
            yield return 0;
        }

        loadingPercent = 30;
		i = 0;
		while(i < item_Pref.Count){
			int j = 0;
			item_Type_Script.Add(new Item_Type());
			amount_Item_Pattern_Left.Add(0);
			amount_Item_Pattern_Middle.Add(0);
			amount_Item_Pattern_Right.Add(0);
			while(j < amountItemSpawn[i]){
				GameObject go = (GameObject)Instantiate(item_Pref[i], posStart, Quaternion.identity);
				go.transform.parent = ItemsPool.transform;
				go.name = "Item["+i+"]["+j+"]";
                item_Obj.Add(go);
				item_Type_Script[i].itemList.Add(go.GetComponent<D3Item>());
				item_Type_Script[i].itemList[j].itemID = i+1;
				j++;
			}
			i++;
			yield return 0;
		}
        i = 0;
        while (i < item_Pref.Count)
        {
            int j = 0;
            item_Type_ScriptRocket.Add(new Item_Type());
            amount_Item_Pattern_LeftRocket.Add(0);
            amount_Item_Pattern_MiddleRocket.Add(0);
            amount_Item_Pattern_RightRocket.Add(0);
            while (j < amountItemSpawnRocket[i])
            {
                GameObject goRocket = (GameObject)Instantiate(item_Pref[i], posStart, Quaternion.identity);
                goRocket.transform.parent = ItemsPool.transform;
                goRocket.name = "ItemRocket[" + i + "][" + j + "]";
                item_ObjRocket.Add(goRocket);
                item_Type_ScriptRocket[i].itemList.Add(goRocket.GetComponent<D3Item>());
                item_Type_ScriptRocket[i].itemList[j].itemID = i + 1;
                j++;
            }
            i++;
            yield return 0;
        }
        i = 0;
        while (i < item_Pref.Count)
        {
            int j = 0;
            item_Type_ScriptSpecial.Add(new Item_Type());
            amount_Item_Pattern_LeftSpecial.Add(0);
            amount_Item_Pattern_MiddleSpecial.Add(0);
            amount_Item_Pattern_RightSpecial.Add(0);
            while (j < amountItemSpawnSpecial[i])
            {
                GameObject goSpecial = (GameObject)Instantiate(item_Pref[i], posStart, Quaternion.identity);
                goSpecial.transform.parent = ItemsPool.transform;
                goSpecial.name = "ItemSpecial[" + i + "][" + j + "]";
                item_ObjSpecial.Add(goSpecial);
                item_Type_ScriptSpecial[i].itemList.Add(goSpecial.GetComponent<D3Item>());
                item_Type_ScriptSpecial[i].itemList[j].itemID = i + 1;
                j++;
            }
            i++;
            yield return 0;
        }
        i = 0;
		loadingPercent = 70;
		while(i < amountFloorSpawn){
			D3Floor go = (D3Floor)Instantiate(floor_Pref, posStart, Quaternion.identity);
			go.gameObject.transform.parent = GeneratedPatternPool.transform;
			go.name = "Floor["+i+"]";

            floor_Obj.Add(go);
			floor_Slot.Add(new Floor());
			floor_Slot[i].floor_Slot_Left = new bool[defaultPosBuilding_Left.Count];
			floor_Slot[i].floor_Slot_Right = new bool[defaultPosBuilding_Right.Count];
            floorbuildingSpecial_Slot.Add(new Floor());
            floorbuildingSpecial_Slot[i].floor_Slot_Left = new bool[defaultPosBuildingSpecial_Left.Count];
            floorbuildingSpecial_Slot[i].floor_Slot_Right = new bool[defaultPosBuildingSpecial_Right.Count];
            floor_item_Slot.Add(new FloorItemSlot());
            floor_item_Slot[i].floor_Slot_Left = new bool[defaultPosItem_Left.Count];
            floor_item_Slot[i].floor_Slot_Middle = new bool[defaultPosItem_Middle.Count];
            floor_item_Slot[i].floor_Slot_Right = new bool[defaultPosItem_Right.Count];
            floor_item_SlotRocket.Add(new FloorItemSlot());
            floor_item_SlotRocket[i].floor_Slot_Left = new bool[defaultPosItem_LeftRocket.Count];
            floor_item_SlotRocket[i].floor_Slot_Middle = new bool[defaultPosItem_MiddleRocket.Count];
            floor_item_SlotRocket[i].floor_Slot_Right = new bool[defaultPosItem_RightRocket.Count];
            floor_item_SlotSpecial.Add(new FloorItemSlot());
            floor_item_SlotSpecial[i].floor_Slot_Left = new bool[defaultPosItem_LeftSpecial.Count];
            floor_item_SlotSpecial[i].floor_Slot_Middle = new bool[defaultPosItem_MiddleSpecial.Count];
            floor_item_SlotSpecial[i].floor_Slot_Right = new bool[defaultPosItem_RightSpecial.Count];
            QueueFloor qFloor = new QueueFloor();
            qFloor.floorObj = floor_Obj[i];
            qFloor.floorClass = floor_Slot[i];
            qFloor.floorClassBuildingSpecial = floorbuildingSpecial_Slot[i];
            qFloor.floorItemSlotClass = floor_item_Slot[i];
            qFloor.floorItemSlotClassRocket = floor_item_SlotRocket[i];
            qFloor.floorItemSlotClassSpecial = floor_item_SlotSpecial[i];
            queneFloor.Add(qFloor);
			i++;
			yield return 0;
		}
        loadingPercent = 100;
		spawnObj_Obj = (GameObject)Instantiate(spawnObj_Pref, posStart, Quaternion.identity);
		spawnObj_Obj.transform.parent = transform;
        colSpawnCheck = spawnObj_Obj.GetComponentInChildren<D3ColliderSpawnCheck>();
		colSpawnCheck.headParent = spawnObj_Obj;
		StartCoroutine(SetPosStarter());
	}
	
	IEnumerator SetPosStarter(){
        //100%
        Vector3 pos = Vector3.zero;
		pos.z = nextPosFloor;
		int i = 0;
        while (i < floor_Obj.Count)
        {
            AddBuildingToFloor(queneFloor[i]);
            queneFloor[i].floorObj.transform.position = pos;
            pos.z += nextPosFloor;
            i++;
        }
        if (FirstFloor && FirstFloorClean)
        {
            for (int x = 0; x < 2; x++)
            {
                i = 0;
                int itemCount = queneFloor[x].getItem.Count;
                while (i < itemCount)
                {
                    if (queneFloor[x].getItem[0] != null)
                    {
                        queneFloor[x].getItem[0].itemActive = false;
                        queneFloor[x].getItem[0].transform.parent = ScrappedPool.transform;
                        queneFloor[x].getItem[0].transform.position = posStart;
                        ReturnItemWithType(queneFloor[x].getItem[0]);
                        queneFloor[x].getItem.RemoveRange(0, 1);
                    }
                    i++;
                }

                i = 0;
                int itemCountSpecial = queneFloor[0].getItemSpecial.Count;
                while (i < itemCountSpecial)
                {
                    if (queneFloor[x].getItemSpecial[0] != null)
                    {
                        queneFloor[x].getItemSpecial[0].itemActive = false;
                        queneFloor[x].getItemSpecial[0].transform.parent = ScrappedPool.transform;
                        queneFloor[x].getItemSpecial[0].transform.position = posStart;
                        ReturnItemWithTypeSpecial(queneFloor[x].getItemSpecial[0]);
                        queneFloor[x].getItemSpecial.RemoveRange(0, 1);
                    }
                    
                    i++;
                }


                i = 0;
                while (i < queneFloor[x].floorItemSlotClass.floor_Slot_Left.Length)
                {
                    queneFloor[x].floorItemSlotClass.floor_Slot_Left[i] = false;
                    queneFloor[x].floorItemSlotClass.floor_Slot_Middle[i] = false;
                    queneFloor[x].floorItemSlotClass.floor_Slot_Right[i] = false;
                    i++;
                }


                i = 0;
                while (i < queneFloor[x].floorItemSlotClassSpecial.floor_Slot_Left.Length)
                {
                    queneFloor[x].floorItemSlotClassSpecial.floor_Slot_Left[i] = false;
                    queneFloor[x].floorItemSlotClassSpecial.floor_Slot_Middle[i] = false;
                    queneFloor[x].floorItemSlotClassSpecial.floor_Slot_Right[i] = false;
                    i++;
                }
            }
            //////

            FirstFloor = false;
        }
        posFloorLast = pos;
		pos = Vector3.zero;
		pos.z += nextPosFloor*2;
        colSpawnCheck.headParent.transform.position = pos;
		loadingComplete = true;
        if (D3Controller.instace)
        {
            D3Controller.instace.PauseOff();
            D3GUIManager.instance.Fade_Out();
        }
        StartCoroutine(WaitCheckFloor());
		yield return 0;
        

    }
	
	IEnumerator WaitCheckFloor(){
		while(colSpawnCheck.isCollision == false){
			yield return 0;
		}
		colSpawnCheck.isCollision = false;
        if (queneFloor[0] == FloorObjectInstantiate)
        {
            RemoveInstantiateObject(queneFloor[0]);
        }
        StartCoroutine(SetPosFloor());

	}
	
	IEnumerator SetPosFloor(){
        
        Vector3 pos = Vector3.zero;
		pos.z = colSpawnCheck.headParent.transform.position.z;
		pos.z += nextPosFloor;
		colSpawnCheck.headParent.transform.position = pos;
		colSpawnCheck.nextPos = colSpawnCheck.headParent.transform.position.z;
        
        int i = 1;
		while(i < queneFloor[0].floorClass.floor_Slot_Left.Length){
			queneFloor[0].floorClass.floor_Slot_Left[i] = false;
			queneFloor[0].floorClass.floor_Slot_Right[i] = false;
			i++;
		}
        i = 1;
        while (i < queneFloor[0].floorClassBuildingSpecial.floor_Slot_Left.Length)
        {
            queneFloor[0].floorClassBuildingSpecial.floor_Slot_Left[i] = false;
            queneFloor[0].floorClassBuildingSpecial.floor_Slot_Right[i] = false;
            i++;
        }
        i = 1;
		while(i < queneFloor[0].floorItemSlotClass.floor_Slot_Left.Length){
			queneFloor[0].floorItemSlotClass.floor_Slot_Left[i] = false;
			queneFloor[0].floorItemSlotClass.floor_Slot_Middle[i] = false;
			queneFloor[0].floorItemSlotClass.floor_Slot_Right[i] = false;
			i++;
		}
        i = 1;
        while (i < queneFloor[0].floorItemSlotClassRocket.floor_Slot_Left.Length)
        {
            queneFloor[0].floorItemSlotClassRocket.floor_Slot_Left[i] = false;
            queneFloor[0].floorItemSlotClassRocket.floor_Slot_Middle[i] = false;
            queneFloor[0].floorItemSlotClassRocket.floor_Slot_Right[i] = false;
            i++;
        }

        i = 1;
        while (i < queneFloor[0].floorItemSlotClassSpecial.floor_Slot_Left.Length)
        {
            queneFloor[0].floorItemSlotClassSpecial.floor_Slot_Left[i] = false;
            queneFloor[0].floorItemSlotClassSpecial.floor_Slot_Middle[i] = false;
            queneFloor[0].floorItemSlotClassSpecial.floor_Slot_Right[i] = false;
            i++;
        }
        i = 1;
		int itemCount = queneFloor[0].getItem.Count;
		while(i < itemCount){
			queneFloor[0].getItem[0].itemActive = false;
			queneFloor[0].getItem[0].transform.parent = ScrappedPool.transform;
			queneFloor[0].getItem[0].transform.position = posStart;
			ReturnItemWithType(queneFloor[0].getItem[0]);
			queneFloor[0].getItem.RemoveRange(0,1);
			i++;
		}

        i = 1;
        int itemCountRocket = queneFloor[0].getItemRocket.Count;
        while (i < itemCountRocket)
        {
            queneFloor[0].getItemRocket[0].itemActive = false;
            queneFloor[0].getItemRocket[0].transform.parent = ScrappedPool.transform;
            queneFloor[0].getItemRocket[0].transform.position = posStart;
            ReturnItemWithTypeRocket(queneFloor[0].getItemRocket[0]);
            queneFloor[0].getItemRocket.RemoveRange(0, 1);
            i++;
        }

        i = 1;
        int itemCountSpecial = queneFloor[0].getItemSpecial.Count;
        while (i < itemCountSpecial)
        {
            queneFloor[0].getItemSpecial[0].itemActive = false;
            queneFloor[0].getItemSpecial[0].transform.parent = ScrappedPool.transform;
            queneFloor[0].getItemSpecial[0].transform.position = posStart;
            ReturnItemWithTypeSpecial(queneFloor[0].getItemSpecial[0]);
            queneFloor[0].getItemSpecial.RemoveRange(0, 1);
            i++;
        }

        i = 1;
		int buildingCount = queneFloor[0].getBuilding.Count;
		while(i < buildingCount){
			queneFloor[0].getBuilding[0].transform.parent = ScrappedPool.transform;
			queneFloor[0].getBuilding[0].transform.position = posStart;
			queneFloor[0].getBuilding[0].buildingActive = false;
			queneFloor[0].getBuilding.RemoveRange(0,1);
			i++;
		}

        i = 1;
        int buildingSpecialCount = queneFloor[0].getBuildingSpecial.Count;
        while (i < buildingSpecialCount)
        {
            queneFloor[0].getBuildingSpecial[0].transform.parent = ScrappedPool.transform;
            queneFloor[0].getBuildingSpecial[0].transform.position = posStart;
            queneFloor[0].getBuildingSpecial[0].buildingActive = false;
            queneFloor[0].getBuildingSpecial.RemoveRange(0, 1);
            i++;
        }
        
        StartCoroutine(AddBuilding());
		yield return 0;
	}

    IEnumerator AddBuilding() {
        QueueFloor qFloor = queneFloor[0];
        queneFloor.RemoveRange(0, 1);
        int i = 0;
        randomPattern = Random.Range(0, patternBuilding.Count);
        randomPatternbuildingSpecial = Random.Range(0, patternBuildingSpecial.Count);
        randomItem = Random.Range(0, patternItem.Count);
        while (randomItem <= 1)
        {
            randomItem = Random.Range(0, patternItem.Count);
        }
		randomItemRocket = Random.Range(0, patternItemRocket.Count);
        while (i < building_Script.Count){
			int j = 0;
			while(j < patternBuilding[randomPattern].stateBuilding_Left.Length){
				CheckAddBuilding_Left(i,j,qFloor);
				j++;
			}
			j = 0;
			while(j < patternBuilding[randomPattern].stateBuilding_Right.Length){
				CheckAddBuilding_Right(i,j,qFloor);
				j++;
			}
			i++;	
		}
        i = 0;
        while (i < buildingSpecial_Script.Count)
        {
            int j = 0;
            while (j < patternBuildingSpecial[randomPatternbuildingSpecial].stateBuilding_Left.Length)
            {
                CheckAddBuildingSpecial_Left(i, j, qFloor);
                j++;
            }
            j = 0;
            while (j < patternBuildingSpecial[randomPatternbuildingSpecial].stateBuilding_Right.Length)
            {
                CheckAddBuildingSpecial_Right(i, j, qFloor);
                j++;
            }
            i++;
        }
        i = 0;
		CheckTypeItemFormAdd(qFloor, i);
		qFloor.floorObj.transform.position = posFloorLast;
		posFloorLast.z += nextPosFloor;
		queneFloor.Add(qFloor);
        StartCoroutine(WaitCheckFloor());
		yield return 0;
	}

    void RemoveInstantiateObject(QueueFloor Floor)
    {
        for (int j = 0; j < ObjectInstantiate.Count; j++)
        {
            if (ObjectInstantiate != null)
            {
                Destroy(ObjectInstantiate[j]);
            }
            ObjectInstantiate.RemoveAt(j);
        }

        if (Floor.floorObj.gameObject.transform.Find("ListItemsInstantiate"))
        {
            GameObject Delete = Floor.floorObj.gameObject.transform.Find("ListItemsInstantiate").gameObject;

            if (Delete != null)
            {
                for (int j = 0; j < Delete.transform.childCount; j++)
                {
                    if (Delete.transform.GetChild(j).gameObject != null)
                    {
                        Destroy(Delete.transform.GetChild(j).gameObject);
                    }

                }
                Destroy(Delete);
            }
        }
    }
	public void Reseted(){
            FirstFloor = true;
			Vector3 pos = Vector3.zero;
			nextPosFloor = 32;
			pos.z += nextPosFloor;
			colSpawnCheck.headParent.transform.position = pos;
			colSpawnCheck.nextPos = colSpawnCheck.headParent.transform.position.z;

        EnabledInstantiateObject();

        int y = 0;
			while(y < queneFloor.Count){
            RemoveInstantiateObject(queneFloor[y]);
            int i = 0;
                 while (i < queneFloor[y].floorClass.floor_Slot_Left.Length)
                {
					queneFloor[y].floorClass.floor_Slot_Left[i] = false;
					queneFloor[y].floorClass.floor_Slot_Right[i] = false;
                i++;
				}

                i = 0;
                while (i < queneFloor[y].floorClassBuildingSpecial.floor_Slot_Left.Length)
                {
                    queneFloor[y].floorClassBuildingSpecial.floor_Slot_Left[i] = false;
                    queneFloor[y].floorClassBuildingSpecial.floor_Slot_Right[i] = false;
                    i++;
                }
                i = 0;
                while (i < queneFloor[y].floorItemSlotClass.floor_Slot_Left.Length)
                {
                    queneFloor[y].floorItemSlotClass.floor_Slot_Left[i] = false;
                    queneFloor[y].floorItemSlotClass.floor_Slot_Middle[i] = false;
                    queneFloor[y].floorItemSlotClass.floor_Slot_Right[i] = false;
                    i++;
                }
                i = 0;
                while (i < queneFloor[y].floorItemSlotClassRocket.floor_Slot_Left.Length)
                {
                    queneFloor[y].floorItemSlotClassRocket.floor_Slot_Left[i] = false;
                    queneFloor[y].floorItemSlotClassRocket.floor_Slot_Middle[i] = false;
                    queneFloor[y].floorItemSlotClassRocket.floor_Slot_Right[i] = false;
                    i++;
                }
                i = 0;
                while (i < queneFloor[y].floorItemSlotClassSpecial.floor_Slot_Left.Length)
                {
                    queneFloor[y].floorItemSlotClassSpecial.floor_Slot_Left[i] = false;
                    queneFloor[y].floorItemSlotClassSpecial.floor_Slot_Middle[i] = false;
                    queneFloor[y].floorItemSlotClassSpecial.floor_Slot_Right[i] = false;
                    i++;
                }
                i = 0;
				int itemCount = queneFloor[y].getItem.Count;
				while(i < itemCount){
					queneFloor[y].getItem[0].itemActive = false;
					queneFloor[y].getItem[0].transform.parent = ScrappedPool.transform;
					queneFloor[y].getItem[0].transform.position = posStart;
					ReturnItemWithType(queneFloor[y].getItem[0]);
					queneFloor[y].getItem.RemoveRange(0,1);
					i++;
				}
				i = 0;
				int itemCountRocket = queneFloor[y].getItemRocket.Count;
				while (i < itemCountRocket)
				{
					queneFloor[y].getItemRocket[0].itemActive = false;
					queneFloor[y].getItemRocket[0].transform.parent = ScrappedPool.transform;
					queneFloor[y].getItemRocket[0].transform.position = posStart;
					ReturnItemWithTypeRocket(queneFloor[y].getItemRocket[0]);
					queneFloor[y].getItemRocket.RemoveRange(0, 1);
					i++;
				}
                i = 0;
                int itemCountSpecial = queneFloor[y].getItemSpecial.Count;
                while (i < itemCountSpecial)
                {
                    queneFloor[y].getItemSpecial[0].itemActive = false;
                    queneFloor[y].getItemSpecial[0].transform.parent = ScrappedPool.transform;
                    queneFloor[y].getItemSpecial[0].transform.position = posStart;
                    ReturnItemWithTypeSpecial(queneFloor[y].getItemSpecial[0]);
                    queneFloor[y].getItemSpecial.RemoveRange(0, 1);
                    i++;
                }

                i = 0;
				int buildingCount = queneFloor[y].getBuilding.Count;
				while(i < buildingCount){
					queneFloor[y].getBuilding[0].transform.parent = ScrappedPool.transform;
					queneFloor[y].getBuilding[0].transform.position = posStart;
					queneFloor[y].getBuilding[0].buildingActive = false;
					queneFloor[y].getBuilding.RemoveRange(0,1);
					i++;
				}
                i = 0;
                int buildingSpecialCount = queneFloor[y].getBuildingSpecial.Count;
                while (i < buildingSpecialCount)
                {
                    queneFloor[y].getBuildingSpecial[0].transform.parent = ScrappedPool.transform;
                    queneFloor[y].getBuildingSpecial[0].transform.position = posStart;
                    queneFloor[y].getBuildingSpecial[0].buildingActive = false;
                    queneFloor[y].getBuildingSpecial.RemoveRange(0, 1);
                    i++;
                }
                i = 0;
            y++;
			}
			posFloorLast.z = 32;
		StopAllCoroutines();
		StartCoroutine(SetPosStarter());
	}

    // Function Call
    #region Function Call
    void AddBuildingToFloor(QueueFloor floor){
		
		randomPattern = Random.Range(0, patternBuilding.Count);
        randomPatternbuildingSpecial = Random.Range(0, patternBuildingSpecial.Count);
        randomItem = Random.Range(0, patternItem.Count);
        randomItemRocket = Random.Range(0, patternItemRocket.Count);
        int i = 0;
        while (i < building_Script.Count){
			int j = 0;
			while(j < patternBuilding[randomPattern].stateBuilding_Left.Length){
				CheckAddBuilding_Left(i,j,floor);
				j++;
			}
			j = 0;
			while(j < patternBuilding[randomPattern].stateBuilding_Right.Length){
				CheckAddBuilding_Right(i,j,floor);
				j++;
			
			}
            i++;	
		}
		i = 0;
        while (i < buildingSpecial_Script.Count)
        {
            int j = 0;
            while (j < patternBuildingSpecial[randomPatternbuildingSpecial].stateBuilding_Left.Length)
            {
                CheckAddBuildingSpecial_Left(i, j, floor);
                j++;
            }
            j = 0;
            while (j < patternBuildingSpecial[randomPatternbuildingSpecial].stateBuilding_Right.Length)
            {
                CheckAddBuildingSpecial_Right(i, j, floor);
                j++;

            }
            i++;
        }
        i = 0;
        CheckTypeItemFormAdd(floor, i);
	}
	
	void ReturnItemWithType(D3Item _item){
		int i = 0;
		while(i < amountItemSpawn.Length){
			ReturnItem(_item, i+1);
			i++;
		}
		i = 0;
		while(i < amount_Item_Pattern_Right.Count){
			amount_Item_Pattern_Left[i] = 0;
			amount_Item_Pattern_Middle[i] = 0;
			amount_Item_Pattern_Right[i] = 0;
			i++;
		}
	}
	
	void ReturnItem(D3Item _item, int itemID){
		if(_item.itemID == itemID){
			item_Type_Script[itemID-1].itemList.Add(_item);	
		}
	}

    void ReturnItemWithTypeRocket(D3Item _item)
    {
        int i = 0;
        while (i < amountItemSpawnRocket.Length)
        {
            ReturnItemRocket(_item, i + 1);
            i++;
        }
        i = 0;
        while (i < amount_Item_Pattern_RightRocket.Count)
        {
            amount_Item_Pattern_LeftRocket[i] = 0;
            amount_Item_Pattern_MiddleRocket[i] = 0;
            amount_Item_Pattern_RightRocket[i] = 0;
            i++;
        }
    }

    void ReturnItemRocket(D3Item _item, int itemID)
    {
        if (_item.itemID == itemID)
        {
            item_Type_ScriptRocket[itemID - 1].itemList.Add(_item);
        }
    }

    void ReturnItemWithTypeSpecial(D3Item _item)
    {
        int i = 0;
        while (i < amountItemSpawnSpecial.Length)
        {
            ReturnItemSpecial(_item, i + 1);
            i++;
        }
        i = 0;
        while (i < amount_Item_Pattern_RightSpecial.Count)
        {
            amount_Item_Pattern_LeftSpecial[i] = 0;
            amount_Item_Pattern_MiddleSpecial[i] = 0;
            amount_Item_Pattern_RightSpecial[i] = 0;
            i++;
        }
    }

    void ReturnItemSpecial(D3Item _item, int itemID)
    {
        if (_item.itemID == itemID)
        {
            item_Type_ScriptSpecial[itemID - 1].itemList.Add(_item);
        }
    }

    void CheckTypeItemFormAdd(QueueFloor floor, int i) {
		while (i < patternItem[randomItem].itemType_Left.Length) {
			int j = 0;
			while (j < amount_Item_Pattern_Left.Count) {
				if (patternItem[randomItem].itemType_Left[i].x == j + 1) {
					amount_Item_Pattern_Left[j] += 1;
				}
				j++;
			}
			i++;
		}

        i = 0;
        while (i < patternItemRocket[randomItemRocket].itemType_Left.Length)
		{
			int j = 0;
			while (j < amount_Item_Pattern_LeftRocket.Count)
			{
				if (patternItemRocket[randomItemRocket].itemType_Left[i].x == j + 1)
				{
					amount_Item_Pattern_LeftRocket[j] += 1;
				}
				j++;
			}
			i++;
		}
        i = 0;
        while (i < patternItemSpecial[randomItemSpecial].itemType_Left.Length)
        {
            int j = 0;
            while (j < amount_Item_Pattern_LeftSpecial.Count)
            {
                if (patternItemSpecial[randomItemSpecial].itemType_Left[i].x == j + 1)
                {
                    amount_Item_Pattern_LeftSpecial[j] += 1;
                }
                j++;
            }
            i++;
        }

        i = 0;
		while (i < patternItem[randomItem].itemType_Middle.Length) {
			int j = 0;
			while (j < amount_Item_Pattern_Middle.Count) {
				if (patternItem[randomItem].itemType_Middle[i].x == j + 1) {
					amount_Item_Pattern_Middle[j] += 1;
				}
				j++;
			}
			i++;
		}
		i = 0;
		while (i < patternItemRocket[randomItemRocket].itemType_Middle.Length)
		{
			int j = 0;
			while (j < amount_Item_Pattern_MiddleRocket.Count)
			{
				if (patternItemRocket[randomItemRocket].itemType_Middle[i].x == j + 1)
				{
					amount_Item_Pattern_MiddleRocket[j] += 1;
				}
				j++;
			}
			i++;
		}
        i = 0;
        while (i < patternItemSpecial[randomItemSpecial].itemType_Middle.Length)
        {
            int j = 0;
            while (j < amount_Item_Pattern_MiddleSpecial.Count)
            {
                if (patternItemSpecial[randomItemSpecial].itemType_Middle[i].x == j + 1)
                {
                    amount_Item_Pattern_MiddleSpecial[j] += 1;
                }
                j++;
            }
            i++;
        }

        i = 0;

		while (i < patternItem[randomItem].itemType_Right.Length) {
			int j = 0;
			while (j < amount_Item_Pattern_Right.Count) {
				if (patternItem[randomItem].itemType_Right[i].x == j + 1) {
					amount_Item_Pattern_Right[j] += 1;
				}
				j++;
			}
			i++;
		}

		i = 0;

		while (i < patternItemRocket[randomItemRocket].itemType_Right.Length)
		{
			int j = 0;
			while (j < amount_Item_Pattern_RightRocket.Count)
			{
				if (patternItemRocket[randomItemRocket].itemType_Right[i].x == j + 1)
				{
					amount_Item_Pattern_RightRocket[j] += 1;
				}
				j++;
			}
			i++;
		}

        i = 0;

        while (i < patternItemSpecial[randomItemSpecial].itemType_Right.Length)
        {
            int j = 0;
            while (j < amount_Item_Pattern_RightSpecial.Count)
            {
                if (patternItemSpecial[randomItemSpecial].itemType_Right[i].x == j + 1)
                {
                    amount_Item_Pattern_RightSpecial[j] += 1;
                }
                j++;
            }
            i++;
        }

        //Add Item To Floor Left
        i = 0;	
		while(i < patternItem[randomItem].itemType_Left.Length){
			int s = 0;
			while(s < amountItemSpawn.Length){
				AddItemWihtType_Left(floor, i, s+1);
				s++;
			}
			i++;
		}

        i = 0;
        while (i < patternItemRocket[randomItemRocket].itemType_Left.Length)
        {
            int s = 0;
            while (s < amountItemSpawnRocket.Length)
            {
                AddItemWihtType_LeftRocket(floor, i, s + 1);
                s++;
            }
            i++;
        }

        i = 0;
        while (i < patternItemSpecial[randomItemSpecial].itemType_Left.Length)
        {
            int s = 0;
            while (s < amountItemSpawnSpecial.Length)
            {
                AddItemWihtType_LeftSpecial(floor, i, s + 1);
                s++;
            }
            i++;
        }

        //Add Item To Floor Middle
        i = 0;
		while(i < patternItem[randomItem].itemType_Middle.Length){
			int s = 0;
			while(s < amountItemSpawn.Length){
				AddItemWihtType_Middle(floor, i, s+1);
				s++;
			}
			i++;
		}

        i = 0;
        while (i < patternItemRocket[randomItemRocket].itemType_Middle.Length)
        {
            int s = 0;
            while (s < amountItemSpawnRocket.Length)
            {
                AddItemWihtType_MiddleRocket(floor, i, s + 1);
                s++;
            }
            i++;
        }

        i = 0;
        while (i < patternItemSpecial[randomItemSpecial].itemType_Middle.Length)
        {
            int s = 0;
            while (s < amountItemSpawnSpecial.Length)
            {
                AddItemWihtType_MiddleSpecial(floor, i, s + 1);
                s++;
            }
            i++;
        }


        //Add Item To Floor Right
        i = 0;
		
		while(i < patternItem[randomItem].itemType_Right.Length){
			int s = 0;
			while(s < amountItemSpawn.Length){
				AddItemWihtType_Right(floor, i, s+1);
				s++;
			}
			i++;
		}
        i = 0;

        while (i < patternItemRocket[randomItemRocket].itemType_Right.Length)
        {
            int s = 0;
            while (s < amountItemSpawnRocket.Length)
            {
                AddItemWihtType_RightRocket(floor, i, s + 1);
                s++;
            }
            i++;
        }
        i = 0;

        while (i < patternItemSpecial[randomItemSpecial].itemType_Right.Length)
        {
            int s = 0;
            while (s < amountItemSpawnSpecial.Length)
            {
                AddItemWihtType_RightSpecial(floor, i, s + 1);
                s++;
            }
            i++;
        }
        i = 0;
        
    }
	
	void AddItemWihtType_Left(QueueFloor floor, int slotIndex,int type){
		if(patternItem[randomItem].itemType_Left[slotIndex].x == type && patternItem[randomItem].itemType_Left[slotIndex] !=null)
        {
			int j = 0;
			while(j < amount_Item_Pattern_Left[type-1]){
				if(j < item_Type_Script[type-1].itemList.Count){
					if(item_Type_Script[type-1].itemList[j].itemActive == false && floor.floorItemSlotClass.floor_Slot_Left[slotIndex] == false ){
						SetPosItem_Left_For_Type(slotIndex,type-1,j,floor, patternItem[randomItem].itemType_Left[slotIndex].y);
						j = 0;
					}
				}
				
				j++;
			}
		}	
	}
	
	void AddItemWihtType_Middle(QueueFloor floor, int slotIndex,int type){
		if(patternItem[randomItem].itemType_Middle[slotIndex].x == type && patternItem[randomItem].itemType_Middle[slotIndex] !=null)
        {
			int j = 0;
			while(j < amount_Item_Pattern_Middle[type-1]){
				if(j < item_Type_Script[type-1].itemList.Count){
					if(item_Type_Script[type-1].itemList[j].itemActive == false && floor.floorItemSlotClass.floor_Slot_Middle[slotIndex] == false){
						SetPosItem_Middle_For_Type(slotIndex,type-1,j,floor, patternItem[randomItem].itemType_Middle[slotIndex].y);
						j = 0;
					}
				}
				
				j++;
			}
		}	
	}
	
	void AddItemWihtType_Right(QueueFloor floor, int slotIndex,int type){
		if(patternItem[randomItem].itemType_Right[slotIndex].x == type && patternItem[randomItem].itemType_Right[slotIndex] != null){
			int j = 0;
			while(j < amount_Item_Pattern_Right[type-1]){
				if(j < item_Type_Script[type-1].itemList.Count){
					if(item_Type_Script[type-1].itemList[j].itemActive == false && floor.floorItemSlotClass.floor_Slot_Right[slotIndex] == false){
						SetPosItem_Right_For_Type(slotIndex,type-1,j,floor, patternItem[randomItem].itemType_Right[slotIndex].y);
						j = 0;
					}
				}
				j++;
			}
		}	
	}
	
	void SetPosItem_Left_For_Type(int i, int j, int countItem, QueueFloor floor, float height){
		item_Type_Script[j].itemList[countItem].transform.parent = floor.floorObj.transform;
		item_Type_Script[j].itemList[countItem].transform.localPosition = new Vector3(defaultPosItem_Left[i].x, defaultPosItem_Left[i].y + height, defaultPosItem_Left[i].z);
		item_Type_Script[j].itemList[countItem].itemActive = true;
		floor.floorItemSlotClass.floor_Slot_Left[i] = true;
		floor.getItem.Add(item_Type_Script[j].itemList[countItem]);
		item_Type_Script[j].itemList.RemoveRange(countItem,1);
	}

	void SetPosItem_Middle_For_Type(int i, int j, int countItem, QueueFloor floor, float height){
		item_Type_Script[j].itemList[countItem].transform.parent = floor.floorObj.transform;
		item_Type_Script[j].itemList[countItem].transform.localPosition = new Vector3(defaultPosItem_Middle[i].x, defaultPosItem_Middle[i].y + height, defaultPosItem_Middle[i].z);
		item_Type_Script[j].itemList[countItem].itemActive = true;
		floor.floorItemSlotClass.floor_Slot_Middle[i] = true;
		floor.getItem.Add(item_Type_Script[j].itemList[countItem]);
		
		item_Type_Script[j].itemList.RemoveRange(countItem,1);
	}
	
	void SetPosItem_Right_For_Type(int i, int j, int countItem, QueueFloor floor, float height){
		item_Type_Script[j].itemList[countItem].transform.parent = floor.floorObj.transform;
		item_Type_Script[j].itemList[countItem].transform.localPosition = new Vector3( defaultPosItem_Right[i].x, defaultPosItem_Right[i].y + height, defaultPosItem_Right[i].z);
		item_Type_Script[j].itemList[countItem].itemActive = true;
		floor.floorItemSlotClass.floor_Slot_Right[i] = true;
		floor.getItem.Add(item_Type_Script[j].itemList[countItem]);
		
		item_Type_Script[j].itemList.RemoveRange(countItem,1);
	}

    void AddItemWihtType_LeftRocket(QueueFloor floor, int slotIndex, int type)
    {
        if (patternItemRocket[randomItemRocket].itemType_Left[slotIndex].x == type && patternItemRocket[randomItemRocket].itemType_Left[slotIndex] != null)
        {
            int j = 0;
            while (j < amount_Item_Pattern_LeftRocket[type - 1])
            {
                if (j < item_Type_ScriptRocket[type - 1].itemList.Count)
                {
                    if (item_Type_ScriptRocket[type - 1].itemList[j].itemActive == false
                       && floor.floorItemSlotClassRocket.floor_Slot_Left[slotIndex] == false)
                    {
                        SetPosItem_Left_For_TypeRocket(slotIndex, type - 1, j, floor, patternItemRocket[randomItemRocket].itemType_Left[slotIndex].y);
                        j = 0;
                    }
                }

                j++;
            }
        }
    }
    
    void SetPosItem_Left_For_TypeRocket(int i, int j, int countItem, QueueFloor floor, float height)
    {
        item_Type_ScriptRocket[j].itemList[countItem].transform.parent = floor.floorObj.transform;
        item_Type_ScriptRocket[j].itemList[countItem].transform.localPosition = new Vector3(defaultPosItem_LeftRocket[i].x, defaultPosItem_LeftRocket[i].y + height, defaultPosItem_LeftRocket[i].z);
        item_Type_ScriptRocket[j].itemList[countItem].itemActive = true;
        floor.floorItemSlotClassRocket.floor_Slot_Left[i] = true;
        floor.getItemRocket.Add(item_Type_ScriptRocket[j].itemList[countItem]);
        item_Type_ScriptRocket[j].itemList.RemoveRange(countItem, 1);
    }

    void AddItemWihtType_MiddleRocket(QueueFloor floor, int slotIndex, int type)
    {
        if (patternItemRocket[randomItemRocket].itemType_Middle[slotIndex].x == type && patternItemRocket[randomItemRocket].itemType_Middle[slotIndex] != null)
        {
            int j = 0;
            while (j < amount_Item_Pattern_MiddleRocket[type - 1])
            {
                if (j < item_Type_ScriptRocket[type - 1].itemList.Count)
                {
                    if (item_Type_ScriptRocket[type - 1].itemList[j].itemActive == false
                       && floor.floorItemSlotClassRocket.floor_Slot_Middle[slotIndex] == false)
                    {
                        SetPosItem_Middle_For_TypeRocket(slotIndex, type - 1, j, floor, patternItemRocket[randomItemRocket].itemType_Middle[slotIndex].y);
                        j = 0;
                    }
                }

                j++;
            }
        }
    }

    void SetPosItem_Middle_For_TypeRocket(int i, int j, int countItem, QueueFloor floor, float height)
    {
        item_Type_ScriptRocket[j].itemList[countItem].transform.parent = floor.floorObj.transform;
        item_Type_ScriptRocket[j].itemList[countItem].transform.localPosition = new Vector3(defaultPosItem_MiddleRocket[i].x, defaultPosItem_MiddleRocket[i].y + height, defaultPosItem_MiddleRocket[i].z);
        item_Type_ScriptRocket[j].itemList[countItem].itemActive = true;
        floor.floorItemSlotClassRocket.floor_Slot_Middle[i] = true;
        floor.getItemRocket.Add(item_Type_ScriptRocket[j].itemList[countItem]);

        item_Type_ScriptRocket[j].itemList.RemoveRange(countItem, 1);
    }
  
	void AddItemWihtType_RightRocket(QueueFloor floor, int slotIndex, int type)
    {
        if (patternItemRocket[randomItemRocket].itemType_Right[slotIndex].x == type && patternItemRocket[randomItemRocket].itemType_Right[slotIndex] != null)
        {
            int j = 0;
            while (j < amount_Item_Pattern_RightRocket[type - 1])
            {
                if (j < item_Type_ScriptRocket[type - 1].itemList.Count)
                {
                    if (item_Type_ScriptRocket[type - 1].itemList[j].itemActive == false
                       && floor.floorItemSlotClassRocket.floor_Slot_Right[slotIndex] == false)
                    {
                        SetPosItem_Right_For_TypeRocket(slotIndex, type - 1, j, floor, patternItemRocket[randomItemRocket].itemType_Right[slotIndex].y);
                        j = 0;
                    }
                }
                j++;
            }
        }
    }

    void SetPosItem_Right_For_TypeRocket(int i, int j, int countItem, QueueFloor floor, float height)
    {
        item_Type_ScriptRocket[j].itemList[countItem].transform.parent = floor.floorObj.transform;
        item_Type_ScriptRocket[j].itemList[countItem].transform.localPosition = new Vector3(defaultPosItem_RightRocket[i].x, defaultPosItem_RightRocket[i].y + height, defaultPosItem_RightRocket[i].z);
        item_Type_ScriptRocket[j].itemList[countItem].itemActive = true;
        floor.floorItemSlotClassRocket.floor_Slot_Right[i] = true;
        floor.getItemRocket.Add(item_Type_ScriptRocket[j].itemList[countItem]);

        item_Type_ScriptRocket[j].itemList.RemoveRange(countItem, 1);
    }

    void AddItemWihtType_LeftSpecial(QueueFloor floor, int slotIndex, int type)
    {
        if (patternItemSpecial[randomItemSpecial].itemType_Left[slotIndex].x == type && patternItemSpecial[randomItemSpecial].itemType_Left[slotIndex] != null)
        {
            int j = 0;
            while (j < amount_Item_Pattern_LeftSpecial[type - 1])
            {
                if (j < item_Type_ScriptSpecial[type - 1].itemList.Count)
                {
                    if (item_Type_ScriptSpecial[type - 1].itemList[j].itemActive == false
                       && floor.floorItemSlotClassSpecial.floor_Slot_Left[slotIndex] == false)
                    {
                        SetPosItem_Left_For_TypeSpecial(slotIndex, type - 1, j, floor, patternItemSpecial[randomItemSpecial].itemType_Left[slotIndex].y);
                        j = 0;
                    }
                }

                j++;
            }
        }
    }

    void SetPosItem_Left_For_TypeSpecial(int i, int j, int countItem, QueueFloor floor, float height)
    {
        item_Type_ScriptSpecial[j].itemList[countItem].transform.parent = floor.floorObj.transform;
        item_Type_ScriptSpecial[j].itemList[countItem].transform.localPosition = new Vector3(defaultPosItem_LeftSpecial[i].x, defaultPosItem_LeftSpecial[i].y + height, defaultPosItem_LeftSpecial[i].z);
        item_Type_ScriptSpecial[j].itemList[countItem].itemActive = true;
        floor.floorItemSlotClassSpecial.floor_Slot_Left[i] = true;
        floor.getItemSpecial.Add(item_Type_ScriptSpecial[j].itemList[countItem]);
        item_Type_ScriptSpecial[j].itemList.RemoveRange(countItem, 1);
    }

    void AddItemWihtType_MiddleSpecial(QueueFloor floor, int slotIndex, int type)
    {
        if (patternItemSpecial[randomItemSpecial].itemType_Middle[slotIndex].x == type && patternItemSpecial[randomItemSpecial].itemType_Middle[slotIndex] != null)
        {
            int j = 0;
            while (j < amount_Item_Pattern_MiddleSpecial[type - 1])
            {
                if (j < item_Type_ScriptSpecial[type - 1].itemList.Count)
                {
                    if (item_Type_ScriptSpecial[type - 1].itemList[j].itemActive == false
                       && floor.floorItemSlotClassSpecial.floor_Slot_Middle[slotIndex] == false)
                    {
                        SetPosItem_Middle_For_TypeSpecial(slotIndex, type - 1, j, floor, patternItemSpecial[randomItemSpecial].itemType_Middle[slotIndex].y);
                        j = 0;
                    }
                }

                j++;
            }
        }
    }

    void SetPosItem_Middle_For_TypeSpecial(int i, int j, int countItem, QueueFloor floor, float height)
    {
        item_Type_ScriptSpecial[j].itemList[countItem].transform.parent = floor.floorObj.transform;
        item_Type_ScriptSpecial[j].itemList[countItem].transform.localPosition = new Vector3(defaultPosItem_MiddleSpecial[i].x, defaultPosItem_MiddleSpecial[i].y + height, defaultPosItem_MiddleSpecial[i].z);
        item_Type_ScriptSpecial[j].itemList[countItem].itemActive = true;
        floor.floorItemSlotClassSpecial.floor_Slot_Middle[i] = true;
        floor.getItemSpecial.Add(item_Type_ScriptSpecial[j].itemList[countItem]);

        item_Type_ScriptSpecial[j].itemList.RemoveRange(countItem, 1);
    }

    void AddItemWihtType_RightSpecial(QueueFloor floor, int slotIndex, int type)
    {
        if (patternItemSpecial[randomItemSpecial].itemType_Right[slotIndex].x == type && patternItemSpecial[randomItemSpecial].itemType_Right[slotIndex] != null)
        {
            int j = 0;
            while (j < amount_Item_Pattern_RightSpecial[type - 1])
            {
                if (j < item_Type_ScriptSpecial[type - 1].itemList.Count)
                {
                    if (item_Type_ScriptSpecial[type - 1].itemList[j].itemActive == false
                       && floor.floorItemSlotClassSpecial.floor_Slot_Right[slotIndex] == false)
                    {
                        SetPosItem_Right_For_TypeSpecial(slotIndex, type - 1, j, floor, patternItemSpecial[randomItemSpecial].itemType_Right[slotIndex].y);
                        j = 0;
                    }
                }
                j++;
            }
        }
    }

    void SetPosItem_Right_For_TypeSpecial(int i, int j, int countItem, QueueFloor floor, float height)
    {
        item_Type_ScriptSpecial[j].itemList[countItem].transform.parent = floor.floorObj.transform;
        item_Type_ScriptSpecial[j].itemList[countItem].transform.localPosition = new Vector3(defaultPosItem_RightSpecial[i].x, defaultPosItem_RightSpecial[i].y + height, defaultPosItem_RightSpecial[i].z);
        item_Type_ScriptSpecial[j].itemList[countItem].itemActive = true;
        floor.floorItemSlotClassSpecial.floor_Slot_Right[i] = true;
        floor.getItemSpecial.Add(item_Type_ScriptSpecial[j].itemList[countItem]);

        item_Type_ScriptSpecial[j].itemList.RemoveRange(countItem, 1);
    }

    void CheckAddBuilding_Left(int i, int j, QueueFloor floor)
    {
        int index = 0;

        while (index < building_Pref.Count)
        {
            if (patternBuilding[randomPattern].stateBuilding_Left[j] == index + 1 && floor.floorClass.floor_Slot_Left[j] == false)
            {
                if (building_Script[i].buildingActive == false && building_Script[i].buildIndex == index)
                {
                    SetPosBuilding_Left(i, j, floor);
                    index = building_Pref.Count;
                }
            }
            index++;
        }
    }

    void CheckAddBuilding_Right(int i, int j, QueueFloor floor)
    {

        int index = 0;

        while (index < building_Pref.Count)
        {
            if (patternBuilding[randomPattern].stateBuilding_Right[j] == index + 1 && floor.floorClass.floor_Slot_Right[j] == false)
            {
                if (building_Script[i].buildingActive == false && building_Script[i].buildIndex == index)
                {
                    SetPosBuilding_Right(i, j, floor);
                    index = building_Pref.Count;
                }
            }
            index++;
        }

    }

    void SetPosBuilding_Left(int i, int j, QueueFloor floor)
    {
        building_Script[i].transform.parent = floor.floorObj.transform;
        building_Script[i].transform.localPosition = defaultPosBuilding_Left[j];
        building_Script[i].transform.eulerAngles = angleLeft;
        building_Script[i].buildingActive = true;
        floor.floorClass.floor_Slot_Left[j] = true;
        floor.getBuilding.Add(building_Script[i]);
    }

    void SetPosBuilding_Right(int i, int j, QueueFloor floor)
    {
        building_Script[i].transform.parent = floor.floorObj.transform;
        building_Script[i].transform.localPosition = defaultPosBuilding_Right[j];
        building_Script[i].transform.eulerAngles = angleRight;
        building_Script[i].buildingActive = true;
        floor.floorClass.floor_Slot_Right[j] = true;
        floor.getBuilding.Add(building_Script[i]);
    }

    void CheckAddBuildingSpecial_Left(int i, int j, QueueFloor floor)
    {
        int index = 0;

        while (index < building_Pref.Count)
        {
            if (patternBuildingSpecial[randomPatternbuildingSpecial].stateBuilding_Left[j] == index + 1 && floor.floorClassBuildingSpecial.floor_Slot_Left[j] == false)
            {
                if (buildingSpecial_Script[i].buildingActive == false && buildingSpecial_Script[i].buildIndex == index)
                {
                    SetPosBuildingSpecial_Left(i, j, floor);
                    index = building_Pref.Count;
                }
            }
            index++;
        }
    }

    void CheckAddBuildingSpecial_Right(int i, int j, QueueFloor floor)
    {

        int index = 0;

        while (index < building_Pref.Count)
        {
            if (patternBuildingSpecial[randomPatternbuildingSpecial].stateBuilding_Right[j] == index + 1 && floor.floorClassBuildingSpecial.floor_Slot_Right[j] == false)
            {
                if (buildingSpecial_Script[i].buildingActive == false && buildingSpecial_Script[i].buildIndex == index)
                {
                    SetPosBuildingSpecial_Right(i, j, floor);
                    index = building_Pref.Count;
                }
            }
            index++;
        }

    }

    void SetPosBuildingSpecial_Left(int i, int j, QueueFloor floor)
    {
        buildingSpecial_Script[i].transform.parent = floor.floorObj.transform;
        buildingSpecial_Script[i].transform.localPosition = defaultPosBuildingSpecial_Left[j];
        buildingSpecial_Script[i].transform.eulerAngles = angleLeft;
        buildingSpecial_Script[i].buildingActive = true;
        floor.floorClassBuildingSpecial.floor_Slot_Left[j] = true;
        floor.getBuildingSpecial.Add(buildingSpecial_Script[i]);
    }

    void SetPosBuildingSpecial_Right(int i, int j, QueueFloor floor)
    {
        buildingSpecial_Script[i].transform.parent = floor.floorObj.transform;
        buildingSpecial_Script[i].transform.localPosition = defaultPosBuildingSpecial_Right[j];
        buildingSpecial_Script[i].transform.eulerAngles = angleRight;
        buildingSpecial_Script[i].buildingActive = true;
        floor.floorClassBuildingSpecial.floor_Slot_Right[j] = true;
        floor.getBuildingSpecial.Add(buildingSpecial_Script[i]);
    }

    #endregion
}
