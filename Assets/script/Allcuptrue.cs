using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Allcuptrue : MonoBehaviour
{
    [SerializeField] private GameObject huta;
    [SerializeField] private GameObject allcup1;

    private bool hasExecuted = false; // ★処理が実行されたかを管理

    void Update()
    {
        // すでに処理が実行済みなら何もしない
        if (hasExecuted)
        {
            return;
        }

        if (!FlagManager.Instance.GetFlag(FlagManager.FlagType.Belt2move))
        {
            return;
        }
        if (FlagManager.Instance.GetFlagByType(Item.Type.Allcupwater))
        {
            return;
        }

        if (FlagManager.Instance.GetFlag(FlagManager.FlagType.Belt2move) &&
            FlagManager.Instance.GetFlag(FlagManager.FlagType.cupCCamera) &&
            !FlagManager.Instance.GetFlag(FlagManager.FlagType.getallcup))
        {
            if (huta != null)
            {
                huta.SetActive(false);
            }
            if (allcup1 != null)
            {
                allcup1.SetActive(true);
            }

            // ★一度実行されたらフラグを true にする
            hasExecuted = true;
        }
    }
}
