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

    // Update is called once per frame
    void Update()
    {
        // フラグがすべて false のときにのみ Fire0 ボタンを受け付ける
        bool isAllFlagsOff =
            !flagManager.GetFlag(FlagManager.FlagType.CameraZoomObj) &&
            !flagManager.GetFlag(FlagManager.FlagType.Textbox) &&
            !flagManager.GetFlag(FlagManager.FlagType.Telop) &&
            !flagManager.GetFlag(FlagManager.FlagType.Nowanim) &&
            !flagManager.GetFlag(FlagManager.FlagType.Option) &&
            !flagManager.GetFlag(FlagManager.FlagType.Itemgetpanel);

        bool isAlloptionFlagsOff =
            !flagManager.GetFlag(FlagManager.FlagType.CameraZoomObj) &&
            !flagManager.GetFlag(FlagManager.FlagType.Textbox) &&
            !flagManager.GetFlag(FlagManager.FlagType.Telop) &&
            !flagManager.GetFlag(FlagManager.FlagType.Nowanim) &&
            !flagManager.GetFlag(FlagManager.FlagType.itembox) &&
            !flagManager.GetFlag(FlagManager.FlagType.Itemgetpanel);


        inventryImage.enabled = isAllFlagsOff;

        // PS4コントローラーの ▫︎ ボタンは「Fire0」として認識されます
        if (isAllFlagsOff && Input.GetButtonDown("Fire0"))
        {
            Canvas.SetActive(true);
            FlagManager.Instance.SetFlag(FlagManager.FlagType.itembox, true);
        }

        bool isitemboxFlagOn = flagManager.GetFlag(FlagManager.FlagType.itembox);

        // itemboxflag が true、なおかつ zoompanelflag が false だった場合
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
        // Optionflag が true、なおかつ ZukanOflag が false だった場合
        if (isOptionFlagOn && isZukanFlagOn && isOperationFlagOn && isHomeFlagOn && Input.GetButtonDown("Fire1"))
        {
            CloseOptionPanel();
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
