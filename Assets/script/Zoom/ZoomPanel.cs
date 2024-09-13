using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ZoomPanel : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Transform objParent;
    private GameObject zoomObj;
    private Image imageComponent;
    private Sprite originalSprite;
    private Sprite zoomSprite;
    private bool isZoomSprite = false;
    private FlagManager flagManager;

    private void Start()
    {
        panel.SetActive(false);
        DontDestroyOnLoad(gameObject);
        flagManager = FlagManager.Instance;

        // ZoomObjectの初期化
        zoomObj = new GameObject("ZoomItem");
        imageComponent = zoomObj.AddComponent<Image>();
        zoomObj.transform.SetParent(objParent, false);
        zoomObj.SetActive(false);  // 初期状態で非表示
    }

    void Update()
    {
        bool isItemboxFlagOn = flagManager.GetFlag(FlagManager.FlagType.itembox);
        bool isZoomPanelFlagOn = flagManager.GetFlag(FlagManager.FlagType.zoompanel);

        // PS4コントローラーの三角ボタンは「Fire3」として認識されます
        if (isItemboxFlagOn && Input.GetButtonDown("Fire3"))
        {
            FlagManager.Instance.SetFlag(FlagManager.FlagType.zoompanel, true);
            if (panel.activeSelf && imageComponent != null)
            {
                ToggleImage();
            }
            else
            {
                ShowPanel();
            }
        }

        // PS4コントローラーのバツボタンは「Fire1」として認識されます
        if (isZoomPanelFlagOn && Input.GetButtonDown("Fire1"))
        {
            // ZoomPanel フラグを false に設定する
            FlagManager.Instance.SetFlag(FlagManager.FlagType.zoompanel, false);
            ClosePanel();
        }
    }

    // やること
    // (アイテムを選択していたら)Zoomボタンが押されたらBigパネル表示
    public void ShowPanel()
    {
        bool isitemboxFlagOn = flagManager.GetFlag(FlagManager.FlagType.itembox);
        if (!isitemboxFlagOn)
        {
            panel.SetActive(false); // itemboxフラグがfalseの場合はパネルを非表示のままにする
            return;
        }

        Item item = Itembox.instance.GetSelectedItem();
        if (item != null)
        {
            // ZoomObjectの表示準備
            originalSprite = item.zoomObj;
            zoomSprite = item.zoomsprite;

            // アイテムを表示
            imageComponent.sprite = originalSprite;
            zoomObj.SetActive(true); // 非表示状態から表示状態へ変更

            panel.SetActive(true);
        }
    }

    // 画像を切り替えるメソッド
    private void ToggleImage()
    {
        if (isZoomSprite)
        {
            imageComponent.sprite = originalSprite;
        }
        else
        {
            imageComponent.sprite = zoomSprite;
        }
        isZoomSprite = !isZoomSprite;
    }

    // Closeボタンが押されたらBigパネルを非表示
    public void ClosePanel()
    {
        panel.SetActive(false);
        zoomObj.SetActive(false); // ZoomObjectを非表示にするだけで済む
    }
}



