using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class ThreePasswordButton : MonoBehaviour
{
    private const int MaxNumber = 9;
    private const int MinNumber = 0;

    [SerializeField] private TMP_Text numberText;
    public int number { get; private set; }

    [SerializeField] private Renderer objectRenderer;
    [SerializeField] private GameObject BgThreePanel;

    private FlagManager flagManager;
    public GameObject BgPanel => BgThreePanel;

    private bool isSelected = false;

    private void Start()
    {
        InitializeButton();
        flagManager = FlagManager.Instance;
    }

    private void InitializeButton()
    {
        number = MinNumber;
        UpdateNumberDisplay();
        HideBGThreePanel(); // Initially hidden
    }

    private void UpdateNumberDisplay()
    {
        numberText.text = number.ToString();
    }

    public void OnClickThis()
    {
        if (flagManager.GetFlag(FlagManager.FlagType.CameraZoomObj) &&
            flagManager.GetFlag(FlagManager.FlagType.BdeskCamera) && isSelected)
        {
            IncrementNumber();
        }
    }

    private void IncrementNumber()
    {
        number = (number + 1) % (MaxNumber + 1);
        UpdateNumberDisplay();
    }

    public void ShowBGPanel()
    {
        BgThreePanel.SetActive(true);
        SetSelectedState(true);
    }

    public void HideBGThreePanel()
    {
        BgThreePanel.SetActive(false);
        SetSelectedState(false);
    }

    private void SetSelectedState(bool state)
    {
        isSelected = state;
    }
}
