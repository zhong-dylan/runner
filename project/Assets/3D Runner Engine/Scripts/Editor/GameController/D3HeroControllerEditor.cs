using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.UI;
using TMPro;

[CustomEditor(typeof(D3HeroController))]
public class D3HeroControllerEditor : Editor
{

    D3HeroController itemTarget;
    SerializedObject TargetS;


    private void OnEnable()
    {
        itemTarget = target as D3HeroController;
        if (itemTarget)
        {
            TargetS = new SerializedObject(itemTarget);

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
        GUILayout.Label("Hero Upgrades Editor", style);
        GUILayout.EndVertical();

        if (itemTarget)
        {

            GUILayout.Space(10f);
            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
            GUILayout.Space(10f);
            GUILayout.Label("Basic", style);

            GUILayout.Space(10f);
            itemTarget.ShopContoller = EditorGUILayout.ObjectField("Component Shop Contoller: ", itemTarget.ShopContoller, typeof(D3ShopCharacter), true) as D3ShopCharacter;

            GUILayout.Space(10f);
            itemTarget.NamePlayerText = EditorGUILayout.ObjectField("Player Name Text: ", itemTarget.NamePlayerText, typeof(Text), true) as Text;

            GUILayout.Space(10f);
            itemTarget.ImageButtonBuy = EditorGUILayout.ObjectField("Image Button Buy: ", itemTarget.ImageButtonBuy, typeof(Sprite), true) as Sprite;

            GUILayout.Space(10f);
            itemTarget.ImageButtonNoCoin = EditorGUILayout.ObjectField("Image Button No Coin: ", itemTarget.ImageButtonNoCoin, typeof(Sprite), true) as Sprite;

            GUILayout.Space(10f);
            itemTarget.BuyText = EditorGUILayout.TextField("Buy Text: ", itemTarget.BuyText);

            GUILayout.Space(10f);
            itemTarget.NOCoinText = EditorGUILayout.TextField("NO Coin Text: ", itemTarget.NOCoinText);

            GUILayout.Space(10f);
            itemTarget.MultiplyPriceWhenPurchasing = EditorGUILayout.IntField("Cant Multiply Price When Purchasing :", itemTarget.MultiplyPriceWhenPurchasing);

            GUILayout.Space(10f);
            GUILayout.EndVertical();


            GUILayout.Space(10f);
            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
            GUILayout.Space(10f);
            GUILayout.Label("Upgrades", style);

            GUILayout.Space(10f);
            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
            GUILayout.Space(10f);
            GUILayout.Label("Sprint", style);
            GUILayout.Space(10f);
            itemTarget.PriceTextSprintTime = EditorGUILayout.ObjectField("Object Text Price: ", itemTarget.PriceTextSprintTime, typeof(Text), true) as Text;
            GUILayout.Space(10f);
            itemTarget.ScrollbarAddTimeSprintTime = EditorGUILayout.ObjectField("Object Slider: ", itemTarget.ScrollbarAddTimeSprintTime, typeof(Slider), true) as Slider;
            GUILayout.Space(10f);
            itemTarget.TextAddTimeSprintTime = EditorGUILayout.ObjectField("Object Text Add Time: ", itemTarget.TextAddTimeSprintTime, typeof(TextMeshProUGUI), true) as TextMeshProUGUI;
            GUILayout.Space(10f);
            itemTarget.ButtonSprintTime = EditorGUILayout.ObjectField("Object Button: ", itemTarget.ButtonSprintTime, typeof(Button), true) as Button;
            GUILayout.Space(10f);
            itemTarget.ButtonTextSprintTime = EditorGUILayout.ObjectField("Object Text Button: ", itemTarget.ButtonTextSprintTime, typeof(Text), true) as Text;
            GUILayout.Space(10f);
            GUILayout.EndVertical();

            GUILayout.Space(10f);
            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
            GUILayout.Space(10f);
            GUILayout.Label("Special", style);
            GUILayout.Space(10f);
            itemTarget.PriceTextSpecialTime = EditorGUILayout.ObjectField("Object Text Price: ", itemTarget.PriceTextSpecialTime, typeof(Text), true) as Text;
            GUILayout.Space(10f);
            itemTarget.ScrollbarAddTimeSpecialTime = EditorGUILayout.ObjectField("Object Slider: ", itemTarget.ScrollbarAddTimeSpecialTime, typeof(Slider), true) as Slider;
            GUILayout.Space(10f);
            itemTarget.TextAddTimeSpecialTime = EditorGUILayout.ObjectField("Object Text Add Time: ", itemTarget.TextAddTimeSpecialTime, typeof(TextMeshProUGUI), true) as TextMeshProUGUI;
            GUILayout.Space(10f);
            itemTarget.ButtonSpecialTime = EditorGUILayout.ObjectField("Object Button: ", itemTarget.ButtonSpecialTime, typeof(Button), true) as Button;
            GUILayout.Space(10f);
            itemTarget.ButtonTextSpecialTime = EditorGUILayout.ObjectField("Object Text Button: ", itemTarget.ButtonTextSpecialTime, typeof(Text), true) as Text;
            GUILayout.Space(10f);
            GUILayout.EndVertical();

            GUILayout.Space(10f);
            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
            GUILayout.Space(10f);
            GUILayout.Label("Multiply", style);
            GUILayout.Space(10f);
            itemTarget.PriceTextMultiplyTime = EditorGUILayout.ObjectField("Object Text Price: ", itemTarget.PriceTextMultiplyTime, typeof(Text), true) as Text;
            GUILayout.Space(10f);
            itemTarget.ScrollbarAddTimeMultiplyTime = EditorGUILayout.ObjectField("Object Slider: ", itemTarget.ScrollbarAddTimeMultiplyTime, typeof(Slider), true) as Slider;
            GUILayout.Space(10f);
            itemTarget.TextAddTimeMultiplyTime = EditorGUILayout.ObjectField("Object Text Add Time: ", itemTarget.TextAddTimeMultiplyTime, typeof(TextMeshProUGUI), true) as TextMeshProUGUI;
            GUILayout.Space(10f);
            itemTarget.ButtonMultiplyTime = EditorGUILayout.ObjectField("Object Button: ", itemTarget.ButtonMultiplyTime, typeof(Button), true) as Button;
            GUILayout.Space(10f);
            itemTarget.ButtonTextMultiplyTime = EditorGUILayout.ObjectField("Object Text Button: ", itemTarget.ButtonTextMultiplyTime, typeof(Text), true) as Text;
            GUILayout.Space(10f);
            GUILayout.EndVertical();

            GUILayout.Space(10f);
            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
            GUILayout.Space(10f);
            GUILayout.Label("Magnet", style);
            GUILayout.Space(10f);
            itemTarget.PriceTextMagnetTime = EditorGUILayout.ObjectField("Object Text Price: ", itemTarget.PriceTextMagnetTime, typeof(Text), true) as Text;
            GUILayout.Space(10f);
            itemTarget.ScrollbarAddTimeMagnetTime = EditorGUILayout.ObjectField("Object Slider: ", itemTarget.ScrollbarAddTimeMagnetTime, typeof(Slider), true) as Slider;
            GUILayout.Space(10f);
            itemTarget.TextAddTimeMagnetTime = EditorGUILayout.ObjectField("Object Text Add Time: ", itemTarget.TextAddTimeMagnetTime, typeof(TextMeshProUGUI), true) as TextMeshProUGUI;
            GUILayout.Space(10f);
            itemTarget.ButtonMagnetTime = EditorGUILayout.ObjectField("Object Button: ", itemTarget.ButtonMagnetTime, typeof(Button), true) as Button;
            GUILayout.Space(10f);
            itemTarget.ButtonTextMagnetTime = EditorGUILayout.ObjectField("Object Text Button: ", itemTarget.ButtonTextMagnetTime, typeof(Text), true) as Text;
            GUILayout.Space(10f);
            GUILayout.EndVertical();

            GUILayout.Space(10f);
            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
            GUILayout.Space(10f);
            GUILayout.Label("Shield", style);
            GUILayout.Space(10f);
            itemTarget.PriceTextShieldTime = EditorGUILayout.ObjectField("Object Text Price: ", itemTarget.PriceTextShieldTime, typeof(Text), true) as Text;
            GUILayout.Space(10f);
            itemTarget.ScrollbarAddTimeShieldTime = EditorGUILayout.ObjectField("Object Slider: ", itemTarget.ScrollbarAddTimeShieldTime, typeof(Slider), true) as Slider;
            GUILayout.Space(10f);
            itemTarget.TextAddTimeShieldTime = EditorGUILayout.ObjectField("Object Text Add Time: ", itemTarget.TextAddTimeShieldTime, typeof(TextMeshProUGUI), true) as TextMeshProUGUI;
            GUILayout.Space(10f);
            itemTarget.ButtonShieldTime = EditorGUILayout.ObjectField("Object Button: ", itemTarget.ButtonShieldTime, typeof(Button), true) as Button;
            GUILayout.Space(10f);
            itemTarget.ButtonTextShieldTime = EditorGUILayout.ObjectField("Object Text Button: ", itemTarget.ButtonTextShieldTime, typeof(Text), true) as Text;
            GUILayout.Space(10f);
            GUILayout.EndVertical();


            GUILayout.Space(10f);
            GUILayout.EndVertical();


        }

        if (EditorGUI.EndChangeCheck())
        {
            if (!Application.isPlaying)
            {
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }

        }
    }
}
