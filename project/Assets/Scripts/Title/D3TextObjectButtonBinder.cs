using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class D3TextObjectButtonBinder : MonoBehaviour
{
    [SerializeField] private Button totalLifeButton;
    [SerializeField] private Button totalHoverBoardButton;

    private D3TitleCharacter titleCharacter;

    private void OnEnable()
    {
        Bind(titleCharacter);
    }

    public void Bind(D3TitleCharacter title)
    {
        titleCharacter = title != null ? title : D3TitleCharacter.instance;

        if (titleCharacter == null)
        {
            return;
        }

        BindButton(totalLifeButton, titleCharacter.OpenNoLifeWindow, "TotalLIFE");
        BindButton(totalHoverBoardButton, titleCharacter.OpenNoLifeWindow, "TotalHoverBoard");
    }

    private void BindButton(Button button, UnityAction action, string buttonName)
    {
        if (button == null)
        {
            Debug.LogWarning("TextObject button is not assigned: " + buttonName);
            return;
        }

        button.onClick.RemoveListener(action);
        button.onClick.AddListener(action);
    }
}
