using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IllustPassword : MonoBehaviour
{
    //*正解すると、外部の関数を実行する(Unity Event)
    // *public UnityEvent ClearEvent;

    //正解
    [SerializeField] int[] correctNumbers;

    //現在の数値:colorpasswordButtonのnumberを見ればいい
    [SerializeField] IllustPasswordButton[] IllustpasswordButtons;

    //クリックするたびに現在のパネルの数値と正解を比較
    //一致するならクリアログ

    public void CheckClear()
    {
        if (IsClear())
        {
            // フラグを設定する
            FlagManager.Instance.SetFlag(FlagManager.FlagType.IllustPasswordclear, true);
            Debug.Log("IllustPasswordclearFlagON");
        }
    }

    bool IsClear()
    {
        //正解しているかどうか　
        // =>　一つでも一致しなければfalse
        // => 全てのチェックをクリアすればtrue
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
 