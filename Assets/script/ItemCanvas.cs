using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ItemCanvas : MonoBehaviour
{
    [SerializeField] private GameObject Canvas;
    private FlagManager flagManager;
    private bool currentZoomPanelFlag;
    [SerializeField] private Image inventryImage;
    private TextManager textManager;

    private void OnEnable()
    {
        // シーンがロードされるたびに OnSceneLoaded を呼び出す
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // イベントの登録解除
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {
        Canvas.SetActive(false);
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

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 新しいシーンがロードされたときに TextManager の参照を再取得
        textManager = FindObjectOfType<TextManager>();

        // Itembox インスタンスが存在する場合に TextManager をセット
        if (Itembox.instance != null)
        {
            Itembox.instance.SetTextManager(textManager);
        }
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
    }

    public void ClosePanel()
    {
        Canvas.SetActive(false);
        FlagManager.Instance.SetFlag(FlagManager.FlagType.itembox, false);
    }

}