using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    private bool isItemGetPanelActive;

    // Start is called before the first frame update
    void Start()
    {
        // 初期値を設定
        isItemGetPanelActive = GameObject.FindWithTag("Itemgetpanel") != null;
    }

    // Update is called once per frame
    void Update()
    {
        // タグがアクティブかどうか確認
        isItemGetPanelActive = GameObject.FindWithTag("Itemgetpanel") != null;

        // ボタン操作を制御
        if (isItemGetPanelActive)
        {
            DisableButtons();
        }
        else
        {
            return;
        }
    }

    private void DisableButtons()
    {
        if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire0")|| Input.GetButtonDown("Fire3"))
        {
            // 何も行わない（入力を無視）
            return;
        }
    }

    
}

