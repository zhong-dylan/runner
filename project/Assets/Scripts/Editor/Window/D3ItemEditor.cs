using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using static D3Item;

[CustomEditor(typeof(D3Item))]
public class D3ItemEditor : Editor {
	
	public Object instance;
	D3Item itemTarget;
	public Object targetPref;


    private void OnEnable()
    {
        itemTarget = target as D3Item; 
    }


    public override void OnInspectorGUI ()
	{
        EditorGUI.BeginChangeCheck();
        var style = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };
        GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true), GUILayout.Height(170f));
        Texture2D m_Logo = (Texture2D)Resources.Load("Img/D3Icon", typeof(Texture2D));
        GUILayout.Label(m_Logo, style, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        GUILayout.EndVertical();
        GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
        GUILayout.Label("Item Editor", style);
        GUILayout.EndVertical();
        if (itemTarget)
        {
            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
            itemTarget.distanceZ = EditorGUILayout.FloatField("Size Object: ", itemTarget.distanceZ);
            EditorGUILayout.TextArea("Please adjust until the purple gizmo takes up the entire size of the item.", GUI.skin.GetStyle("HelpBox"));
            GUILayout.Space(10f);

            itemTarget.colorPattern = EditorGUILayout.ColorField("Color in Pattern and Gizmo", itemTarget.colorPattern);


            GUILayout.Space(10f);
            itemTarget.ShowGizmo = GUILayout.Toggle(itemTarget.ShowGizmo, "Show Gizmo");
            GUILayout.EndVertical();

            GUILayout.Space(10f);
            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
            GUILayout.Space(10f);

			itemTarget.typeItem = (TypeItem)EditorGUILayout.EnumPopup("Type Item: ", itemTarget.typeItem);
            GUILayout.EndVertical();


            GUILayout.Space(10f);

            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
            GUILayout.Label("Config", style);

            if (itemTarget.typeItem == TypeItem.Coin)
            {
                GUILayout.Space(10f);
                EditorGUILayout.TextArea("Add money if item = coin", GUI.skin.GetStyle("HelpBox"));
                itemTarget.scoreAdd = EditorGUILayout.FloatField("Score Add: ", itemTarget.scoreAdd);
            }

            if (itemTarget.typeItem == TypeItem.Obstacle || itemTarget.typeItem == TypeItem.Obstacle_Roll || itemTarget.typeItem == TypeItem.Moving_Obstacle)
            {
                GUILayout.Space(10f);
                EditorGUILayout.TextArea("decrease life if item = obstacle", GUI.skin.GetStyle("HelpBox"));
                itemTarget.decreaseLife = EditorGUILayout.IntField("Decrease Life: ", itemTarget.decreaseLife);

            }

            if (itemTarget.typeItem == TypeItem.Moving_Obstacle)
            {
                GUILayout.Space(10f);
                EditorGUILayout.TextArea("Speed move for moving obstacle", GUI.skin.GetStyle("HelpBox"));
                itemTarget.speedMove = EditorGUILayout.FloatField("Speed Move: ", itemTarget.speedMove);

            }

            if (itemTarget.typeItem == TypeItem.ItemSprint )
            {
                GUILayout.Space(10f);
                EditorGUILayout.TextArea("effect value Max to add (20)", GUI.skin.GetStyle("HelpBox"));
                itemTarget.itemEffectValue = EditorGUILayout.FloatField("Speed Effect Value: ", itemTarget.itemEffectValue);

            }
            if (itemTarget.typeItem == TypeItem.ItemSpecial)
            {
                GUILayout.Space(10f);
                EditorGUILayout.TextArea("effect value Max to add (20)", GUI.skin.GetStyle("HelpBox"));
                itemTarget.itemEffectValue = EditorGUILayout.FloatField("Speed Effect Value: ", itemTarget.itemEffectValue);
               

            }

            if (itemTarget.typeItem == TypeItem.ItemMultiply)
            {
                GUILayout.Space(10f);
                EditorGUILayout.TextArea("effect value Max to add (20)", GUI.skin.GetStyle("HelpBox"));
                itemTarget.itemEffectValue = EditorGUILayout.FloatField("Multiply Coin Value: ", itemTarget.itemEffectValue);
                
            }
            if (itemTarget.itemEffectValue > 20)
            {
                itemTarget.itemEffectValue = 20;
            }

            if (itemTarget.typeItem == TypeItem.ItemSprint || itemTarget.typeItem == TypeItem.ItemMultiply || itemTarget.typeItem == TypeItem.ItemJump || itemTarget.typeItem == TypeItem.ItemShield || itemTarget.typeItem == TypeItem.ItemMagnet || itemTarget.typeItem == TypeItem.ItemSpecial)
            {
                GUILayout.Space(10f);
                EditorGUILayout.TextArea("duration item", GUI.skin.GetStyle("HelpBox"));
                itemTarget.duration = EditorGUILayout.FloatField("Duration: ", itemTarget.duration);

            }

            if (itemTarget.typeItem == TypeItem.ItemSprint || itemTarget.typeItem == TypeItem.ItemMultiply || itemTarget.typeItem == TypeItem.ItemJump || itemTarget.typeItem == TypeItem.ItemShield || itemTarget.typeItem == TypeItem.ItemMagnet || itemTarget.typeItem == TypeItem.Coin || itemTarget.typeItem == TypeItem.ItemSpecial)
            {
                GUILayout.Space(10f);
                EditorGUILayout.TextArea("rotate item// first add script D3 Item Rotate", GUI.skin.GetStyle("HelpBox"));
                itemTarget.itemRotate = EditorGUILayout.ObjectField("Item Rotate: ", itemTarget.itemRotate, typeof(D3ItemCoinRotate), true) as D3ItemCoinRotate;

            }


            GUILayout.Space(10f);
            EditorGUILayout.TextArea("effect when hit item", GUI.skin.GetStyle("HelpBox"));
            itemTarget.effectHit = EditorGUILayout.ObjectField("Effect Hit: ", itemTarget.effectHit, typeof(GameObject), true) as GameObject;

            if (itemTarget.typeItem == TypeItem.Moving_Obstacle)
            {
                GUILayout.Space(10f);
                EditorGUILayout.TextArea("Lights and sound with proximity to the player", GUI.skin.GetStyle("HelpBox"));
                itemTarget.listLights = EditorGUILayout.ObjectField("Lightst: ", itemTarget.listLights, typeof(GameObject), true) as GameObject;
                GUILayout.Space(10f);
                itemTarget.audioHorn = EditorGUILayout.ObjectField("Audio: ", itemTarget.audioHorn, typeof(AudioClip), true) as AudioClip;
                GUILayout.Space(10f);
                itemTarget.percentPlay = EditorGUILayout.IntField("Percent Play: ", itemTarget.percentPlay);
                GUILayout.Space(10f);
            }

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

        if (itemTarget.isEditing == false){
			if(PrefabUtility.GetCorrespondingObjectFromOriginalSource(target) == null && PrefabUtility.GetCorrespondingObjectFromOriginalSource(target)  != null){
				if(GUILayout.Button("Setting Item")){
					OpenWindowSettingItem();
				}
			}
		}else{
			if(GUILayout.Button("Apply")){
				CloseWindowSettingItem();
			}
		}
	}

 	private void OpenWindowSettingItem(){

		string scenePath = SceneManager.GetActiveScene().name;
		EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects);
		RenderSettings.ambientLight = Color.white;
		instance =  Instantiate(target, Vector3.zero, Quaternion.identity);
		instance.name = target.name;
		targetPref = target;
		Selection.activeObject = instance;
		itemTarget = instance as D3Item;
		itemTarget.isEditing = true;
		itemTarget.targetPref = targetPref;
		itemTarget.scenePath = scenePath;


	}

	private void CloseWindowSettingItem(){
		itemTarget.isEditing = false;
		if(itemTarget.transform.parent != null){
			PrefabUtility.SaveAsPrefabAssetAndConnect(itemTarget.transform.parent.gameObject, itemTarget.targetPref.name, InteractionMode.AutomatedAction);
			SceneManager.LoadScene(itemTarget.scenePath);
		}else{
			PrefabUtility.SaveAsPrefabAssetAndConnect(itemTarget.gameObject, itemTarget.targetPref.name, InteractionMode.AutomatedAction);
			SceneManager.LoadScene(itemTarget.scenePath);
		}
	}

}
