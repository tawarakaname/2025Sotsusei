using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ItemCanvas : MonoBehaviour
{
    [SerializeField] private GameObject Canvas;
    [SerializeField] private GameObject OptionCanvas;
    private FlagManager flagManager;
    private bool currentZoomPanelFlag;
    private bool hasClosedOptionPanel = false; // オプションパネルを閉じたかどうかを記録
    [SerializeField] private Image inventryImage;
    private TextManager textManager;

    void Start()
    {
        textManager = GameObject.FindWithTag("TextManager").GetComponent<TextManager>();
        Canvas.SetActive(false);
        OptionCanvas.SetActive(false);
        flagManager = FlagManager.Instance;
    }

    private void Awake()
    {
        CheckSingleton();
    }

    private void CheckSingleton()
    {
        var target = GameObject.FindGameObjectWithTag(gameObject.tag);
        var checkResult = target != null && target != gameObject;

        if (checkResult)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        bool isAllFlagsOff =
            !flagManager.GetFlag(FlagManager.FlagType.CameraZoomObj) &&
            !flagManager.GetFlag(FlagManager.FlagType.Textbox) &&
            !flagManager.GetFlag(FlagManager.FlagType.Telop) &&
            !flagManager.GetFlag(FlagManager.FlagType.Nowanim) &&
            !flagManager.GetFlag(FlagManager.FlagType.specialnowanim) &&
            !flagManager.GetFlag(FlagManager.FlagType.Option) &&
            !flagManager.GetFlag(FlagManager.FlagType.Itemgetpanel);

        bool isAlloptionFlagsOff =
            !flagManager.GetFlag(FlagManager.FlagType.CameraZoomObj) &&
            !flagManager.GetFlag(FlagManager.FlagType.Textbox) &&
            !flagManager.GetFlag(FlagManager.FlagType.Telop) &&
            !flagManager.GetFlag(FlagManager.FlagType.Nowanim) &&
            !flagManager.GetFlag(FlagManager.FlagType.specialnowanim) &&
            !flagManager.GetFlag(FlagManager.FlagType.itembox) &&
            !flagManager.GetFlag(FlagManager.FlagType.Itemgetpanel);

        inventryImage.enabled = isAllFlagsOff;

        if (isAllFlagsOff && Input.GetButtonDown("Fire0"))
        {
            Canvas.SetActive(true);
            FlagManager.Instance.SetFlag(FlagManager.FlagType.itembox, true);
        }

        bool isitemboxFlagOn = flagManager.GetFlag(FlagManager.FlagType.itembox);

        if (isitemboxFlagOn && !currentZoomPanelFlag && Input.GetButtonDown("Fire1"))
        {
            ClosePanel();
        }
        currentZoomPanelFlag = flagManager.GetFlag(FlagManager.FlagType.zoompanel);

        if (isAlloptionFlagsOff && Input.GetButtonDown("Fire-Option"))
        {
            OptionCanvas.SetActive(true);
            FlagManager.Instance.SetFlag(FlagManager.FlagType.Option, true);
        }

        bool isOptionFlagOn = flagManager.GetFlag(FlagManager.FlagType.Option);
        bool isZukanFlagOn = !flagManager.GetFlag(FlagManager.FlagType.Zukan);
        bool isOperationFlagOn = !flagManager.GetFlag(FlagManager.FlagType.Operation);
        bool isHomeFlagOn = !flagManager.GetFlag(FlagManager.FlagType.Home);

        if (isOptionFlagOn && isZukanFlagOn && isOperationFlagOn && isHomeFlagOn && Input.GetButtonDown("Fire1"))
        {
            CloseOptionPanel();
        }

        if (flagManager.GetFlag(FlagManager.FlagType.Gotitle) && !hasClosedOptionPanel)
        {
            CloseOptionPanel();
            hasClosedOptionPanel = true; // 一度閉じたことを記録する
        }
    }

    public void ClosePanel()
    {
        Canvas.SetActive(false);
        FlagManager.Instance.SetFlag(FlagManager.FlagType.itembox, false);
    }

    public void CloseOptionPanel()
    {
        OptionCanvas.SetActive(false);
        FlagManager.Instance.SetFlag(FlagManager.FlagType.Option, false);
    }
}
