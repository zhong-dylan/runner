using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using static D3ItemShop;
using static D3ListItemShop;

[CustomEditor(typeof(D3ListItemShop))]
public class D3TemplateListEditor : Editor
{
    D3ListItemShop itemTarget;
    private void OnEnable()
    {
        itemTarget = target as D3ListItemShop;
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
        GUILayout.Label("Template List Item Window Editor", style);
        GUILayout.EndVertical();

        if (itemTarget)
        {
            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));

            itemTarget.TypeTemplate = (D3TypeTemplate)EditorGUILayout.EnumPopup("Type Template: ", itemTarget.TypeTemplate);
            EditorGUILayout.TextArea("This template is the one used to instantiate the items", GUI.skin.GetStyle("HelpBox"));

            GUILayout.EndVertical();

            GUILayout.Space(10);

           
            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
            GUILayout.Space(10f);
            GUILayout.Label("Basic Object", style);
            GUILayout.Space(10f);

            itemTarget.NameText = EditorGUILayout.ObjectField("Name Text: ", itemTarget.NameText, typeof(Text), true) as Text;

            itemTarget.Image = EditorGUILayout.ObjectField("Image: ", itemTarget.Image, typeof(Image), true) as Image;

            itemTarget.PriceText = EditorGUILayout.ObjectField("Price Text: ", itemTarget.PriceText, typeof(Text), true) as Text;

            GUILayout.EndVertical();

            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
            GUILayout.Space(10f);
            GUILayout.Label("Button", style);
            GUILayout.Space(10f);

            itemTarget.Button = EditorGUILayout.ObjectField("Button: ", itemTarget.Button, typeof(Button), true) as Button;

            itemTarget.ButtonText = EditorGUILayout.ObjectField("Button Text: ", itemTarget.ButtonText, typeof(Text), true) as Text;

            GUILayout.Space(10f);

            itemTarget.ImageButtonBuy = EditorGUILayout.ObjectField("Sprite Button Buy: ", itemTarget.ImageButtonBuy, typeof(Sprite), true) as Sprite;

            itemTarget.ImageButtonSelect = EditorGUILayout.ObjectField("Sprite Button Select: ", itemTarget.ImageButtonSelect, typeof(Sprite), true) as Sprite;

            GUILayout.EndVertical();

            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
            GUILayout.Space(10f);
            GUILayout.Label("Text For Button", style);
            GUILayout.Space(10f);

            itemTarget.BuyText = EditorGUILayout.TextField("Buy: ", itemTarget.BuyText);
            itemTarget.NOCoinText = EditorGUILayout.TextField("NO Coin: ", itemTarget.NOCoinText);
            itemTarget.SelectText = EditorGUILayout.TextField("Select: ", itemTarget.SelectText);
            itemTarget.InUseText = EditorGUILayout.TextField("In Use: ", itemTarget.InUseText);


            GUILayout.EndVertical();

            GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
            GUILayout.Space(10f);
            GUILayout.Label("Icon Money", style);
            GUILayout.Space(10f);

            itemTarget.ImageIconCoin = EditorGUILayout.ObjectField("Image Icon Coin: ", itemTarget.ImageIconCoin, typeof(Image), true) as Image;

            itemTarget.ImageIconVirtualCoin = EditorGUILayout.ObjectField("Image Icon Virtual Coin: ", itemTarget.ImageIconVirtualCoin, typeof(Sprite), true) as Sprite;

            itemTarget.ImageIconRealCoin = EditorGUILayout.ObjectField("Image Icon Real Coin: ", itemTarget.ImageIconRealCoin, typeof(Sprite), true) as Sprite;


            GUILayout.EndVertical();


            if (itemTarget.TypeTemplate == D3TypeTemplate.VirtualOrRealMoney)
            {
                GUILayout.BeginVertical("GroupBox", GUILayout.ExpandWidth(true));
                GUILayout.Space(10f);
                GUILayout.Label("For Player Statistics", style);
                GUILayout.Space(10f);

                itemTarget.ContentStatistics = EditorGUILayout.ObjectField("Content Statistics: ", itemTarget.ContentStatistics, typeof(GameObject), true) as GameObject;

                GUILayout.Space(10f);

                itemTarget.ScrollbarAddSprintTime = EditorGUILayout.ObjectField("Scrollbar Add Sprint Time: ", itemTarget.ScrollbarAddSprintTime, typeof(Slider), true) as Slider;

                itemTarget.TextAddSprintTime = EditorGUILayout.ObjectField("Text Add Sprint Time: ", itemTarget.TextAddSprintTime, typeof(TextMeshProUGUI), true) as TextMeshProUGUI;

                GUILayout.Space(10f);

                itemTarget.ScrollbarAddSpecialTime = EditorGUILayout.ObjectField("Scrollbar Add Special Time: ", itemTarget.ScrollbarAddSpecialTime, typeof(Slider), true) as Slider;

                itemTarget.TextAddSpecialTime = EditorGUILayout.ObjectField("Text Add Special Time: ", itemTarget.TextAddSpecialTime, typeof(TextMeshProUGUI), true) as TextMeshProUGUI;

                GUILayout.Space(10f);

                itemTarget.ScrollbarAddMultiplyTime = EditorGUILayout.ObjectField("Scrollbar Add Multiply Time: ", itemTarget.ScrollbarAddMultiplyTime, typeof(Slider), true) as Slider;

                itemTarget.TextAddMultiplyTime = EditorGUILayout.ObjectField("Text Add Multiply Time: ", itemTarget.TextAddMultiplyTime, typeof(TextMeshProUGUI), true) as TextMeshProUGUI;

                GUILayout.Space(10f);

                itemTarget.ScrollbarAddMagnetTime = EditorGUILayout.ObjectField("Scrollbar Add Magnet Time: ", itemTarget.ScrollbarAddMagnetTime, typeof(Slider), true) as Slider;

                itemTarget.TextAddMagnetTime = EditorGUILayout.ObjectField("Text Add Magnet Time: ", itemTarget.TextAddMagnetTime, typeof(TextMeshProUGUI), true) as TextMeshProUGUI;

                GUILayout.Space(10f);

                itemTarget.ScrollbarAddShieldTime = EditorGUILayout.ObjectField("Scrollbar Add Shield Time: ", itemTarget.ScrollbarAddShieldTime, typeof(Slider), true) as Slider;

                itemTarget.TextAddShieldTime = EditorGUILayout.ObjectField("Text Add Shield Time: ", itemTarget.TextAddShieldTime, typeof(TextMeshProUGUI), true) as TextMeshProUGUI;

                
                GUILayout.EndVertical();
            }
           


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
