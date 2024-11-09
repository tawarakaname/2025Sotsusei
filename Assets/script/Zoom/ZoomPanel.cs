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
    [SerializeField] AudioSource audioSource;
    private bool canReceiveInput = true;

    private void Start()
    {
        panel.SetActive(false);
        flagManager = FlagManager.Instance;

        // ZoomObjectの初期化
        zoomObj = new GameObject("ZoomItem");
        imageComponent = zoomObj.AddComponent<Image>();
        zoomObj.transform.SetParent(objParent, false);
        zoomObj.SetActive(false);  // 初期状態で非表示
        audioSource = GetComponent<AudioSource>();
    }

    private void Awake()
    {
        CheckSingleton();
    }
    void Update()
    {
        if (!canReceiveInput) return; // ディレイ中は入力を無視

        bool isItemboxFlagOn = flagManager.GetFlag(FlagManager.FlagType.itembox);
        bool isZoomPanelFlagOn = flagManager.GetFlag(FlagManager.FlagType.zoompanel);
        bool isslotbgFlagOn = flagManager.GetFlag(FlagManager.FlagType.slotbg);

        if (isItemboxFlagOn && isslotbgFlagOn && Input.GetButtonDown("Fire3"))
        {
            audioSource.Play();
            FlagManager.Instance.SetFlag(FlagManager.FlagType.zoompanel, true);

            if (panel.activeSelf && imageComponent != null)
            {
                ToggleImage();
            }
            else
            {
                ShowPanel();
                StartCoroutine(InputDelay(0.2f)); // 入力の受付を一時停止
            }
        }

        if (isZoomPanelFlagOn && Input.GetButtonDown("Fire1"))
        {
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

            // ここでisZoomSpriteをリセット
            isZoomSprite = false;
        }
    }


    // 画像を切り替えるメソッド
    private void ToggleImage()
    {
        if (isZoomSprite)
        {
            imageComponent.sprite = originalSprite;
            audioSource.Play(); // 鳴らしたいタイミングで再生
        }
        else
        {
            imageComponent.sprite = zoomSprite;
            audioSource.Play(); // 鳴らしたいタイミングで再生
        }
        isZoomSprite = !isZoomSprite;
    }

    // Closeボタンが押されたらBigパネルを非表示
    public void ClosePanel()
    {
        panel.SetActive(false);
        zoomObj.SetActive(false); // ZoomObjectを非表示にするだけで済む
    }

    private IEnumerator InputDelay(float delay)
    {
        canReceiveInput = false;
        yield return new WaitForSeconds(delay);
        canReceiveInput = true;
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
}



