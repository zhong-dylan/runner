using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(D3EnemyController))]
public class D3EnemyEditor : Editor
{
    public Object instance;
    D3EnemyController itemTarget;
    public Object targetPref;



    private void OnEnable()
    {
        itemTarget = target as D3EnemyController;
      
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
        GUILayout.Label("Enemy Editor", style);
        GUILayout.EndVertical();
        if (itemTarget)
        {
            GUILayout.Space(10f);
            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
            GUILayout.Space(10f);
            GUILayout.Label("Basic", style);
            EditorGUILayout.TextArea("Initial speed, gravity and jump distance", GUI.skin.GetStyle("HelpBox"));

            itemTarget.decreaseLife = EditorGUILayout.IntField("Decrease Life Player: ", itemTarget.decreaseLife);
            GUILayout.Space(10f);

            itemTarget.DistanceEnemy = EditorGUILayout.FloatField("Player distance: ", itemTarget.DistanceEnemy);

            GUILayout.Space(10f);
            EditorGUILayout.TextArea("The enemy will get closer to the player as he collides with the obstacles, here you can modify the distance that the enemy will get when the player collides, the first collision of the player the enemy gets a little closer, the third collision (Random) the enemy approaches completely and takes life away from the player.", GUI.skin.GetStyle("HelpBox"));

            GUILayout.Space(10f);

            itemTarget.FirsHitPlayerDistanceEnemy = EditorGUILayout.FloatField("Player's First Collision (Distance): ", itemTarget.FirsHitPlayerDistanceEnemy);

            GUILayout.Space(10f);

            itemTarget.SecondHitPlayerDistanceEnemy = EditorGUILayout.FloatField("Player's Second Collision (Distance): ", itemTarget.SecondHitPlayerDistanceEnemy);

            GUILayout.Space(10f);

            itemTarget.ThirdHitPlayerDistanceEnemy = EditorGUILayout.FloatField("Player's Third Collision (Distance): ", itemTarget.ThirdHitPlayerDistanceEnemy);

            GUILayout.Space(10f);

            itemTarget.PosEnemyWhenArrestPlayer = EditorGUILayout.Vector3Field("Pos Enemy When Arrest Player: ", itemTarget.PosEnemyWhenArrestPlayer);


            GUILayout.Space(10f);
            GUILayout.EndVertical();

            GUILayout.Space(10f);
            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));

            GUILayout.Space(10f);
            GUILayout.Label("Audio", style);

            GUILayout.Space(10f);
            itemTarget.FarPolice = EditorGUILayout.ObjectField("Sound: Far Police: ", itemTarget.FarPolice, typeof(AudioClip), true) as AudioClip;
            GUILayout.Space(10f);

            itemTarget.ArrestPlayer = EditorGUILayout.ObjectField("Sound: ArrestPlayer: ", itemTarget.ArrestPlayer, typeof(AudioClip), true) as AudioClip;
            GUILayout.Space(10f);


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