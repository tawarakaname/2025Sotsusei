using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class ZoomPanel : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Transform objParent;
    GameObject zoomObj;
    private Image imageComponent;
    private Sprite originalSprite;
    private Sprite zoomSprite;
    private bool isZoomSprite = false;

    private void Start()
    {
        panel.SetActive(false);
    }
    void Update()
    {
        // PS4コントローラーの三角ボタンは「Fire3」として認識されます
        if (Input.GetButtonDown("Fire3"))
        {
            if (panel.activeSelf && imageComponent != null)
            {
                ToggleImage();
            }
            else
            {
                ShowPanel();
            }
            Debug.Log("三角ボタンが押されました！");
        }

        // PS4コントローラーのバツボタンは「Fire1」として認識されます
        if (Input.GetButtonDown("Fire1"))
        {
            ClosePanel();
            Debug.Log("バツボタンが押されました！");
        }
    }
    //やること
    //(アイテムを選択していたら)Zoomボタンが押されたらBigパネル表示
    public void ShowPanel()
    {
        Item item = Itembox.instance.GetSelectedItem();
        if (item != null)
        {
            Destroy(zoomObj);
            panel.SetActive(true);
            //アイテムを表示
            // 新しいGameObjectを作成し、それにImageコンポーネントを追加してSpriteを設定する
            zoomObj = new GameObject("ZoomItem");

            imageComponent = zoomObj.AddComponent<Image>();

            originalSprite = item.zoomObj;
            zoomSprite = item.zoomsprite;

            // 最初はzoomObjが表示されるようにする
            imageComponent.sprite = originalSprite;

            zoomObj.transform.SetParent(objParent, false);

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
        Destroy(zoomObj);
    }
}



