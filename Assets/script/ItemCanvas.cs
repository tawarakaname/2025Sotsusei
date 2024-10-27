using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // シーン遷移させる場合に必要

public class ItemCanvas : MonoBehaviour
{
    [SerializeField] private GameObject Canvas;
    private FlagManager flagManager;

    // Start is called before the first frame update
    void Start()
    {
        Canvas.SetActive(false);
        DontDestroyOnLoad(gameObject); // シーンを切り替えても削除しない
        flagManager = FlagManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        // フラグがすべて false のときにのみ Fire0 ボタンを受け付ける
        bool isAllFlagsOff =
            !flagManager.GetFlag(FlagManager.FlagType.CameraZoomObj) &&
            !flagManager.GetFlag(FlagManager.FlagType.Textbox) &&
            !flagManager.GetFlag(FlagManager.FlagType.Itemgetpanel);

        // PS4コントローラーの ▫︎ ボタンは「Fire0」として認識されます
        if (isAllFlagsOff && Input.GetButtonDown("Fire0"))
        {
            Canvas.SetActive(true);
            FlagManager.Instance.SetFlag(FlagManager.FlagType.itembox, true);
        }

        bool isitemboxFlagOn = flagManager.GetFlag(FlagManager.FlagType.itembox);
        bool isZoomPanelFlagOff = !flagManager.GetFlag(FlagManager.FlagType.zoompanel);

        // itemboxflag が true、なおかつ zoompanelflag が false だった場合
        if (isitemboxFlagOn && isZoomPanelFlagOff && Input.GetButtonDown("Fire1"))
        {
            ClosePanel();
        }
    }

    public void ClosePanel()
    {
        Canvas.SetActive(false);
        FlagManager.Instance.SetFlag(FlagManager.FlagType.itembox, false);
    }
}
