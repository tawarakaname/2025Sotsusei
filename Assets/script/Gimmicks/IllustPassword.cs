using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IllustPassword : MonoBehaviour
{
    // 正解の番号
    [SerializeField] int[] correctNumbers;

    // 現在のパネルの数値
    [SerializeField] IllustPasswordButton[] IllustpasswordButtons;

    // 最初に非表示のオブジェクト
    [SerializeField] private GameObject capsuleD;


    // クリックするたびに現在のパネルの数値と正解を比較
    public void CheckClear()
    {
        if (IsClear())
        {
            // フラグを設定する
            FlagManager.Instance.SetFlag(FlagManager.FlagType.IllustPasswordclear, true);
            Debug.Log("IllustPasswordclearFlagON");

            // フラグが設定されたので capsuleD を表示する
            capsuleD.SetActive(true);
        }
    }

    // 全てのパネルが正解かどうかチェックする
    bool IsClear()
    {
        for (int i = 0; i < correctNumbers.Length; i++)
        {
            if (IllustpasswordButtons[i].number != correctNumbers[i])
            {
                return false;
            }
        }
        return true;
    }
}

 