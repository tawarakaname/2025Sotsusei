using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// *using UnityEngine.Events;

public class Password : MonoBehaviour
{
    //*正解すると、外部の関数を実行する(Unity Event)
    // *public UnityEvent ClearEvent;
    
    //正解
    [SerializeField] int[] correctNumbers;

    //現在の数値:passwordButtonのnumberを見ればいい
    [SerializeField] PasswordButton[] passwordButtons;

    //クリックするたびに現在のパネルの数値と正解を比較
    //一致するならクリアログ

    public void CheckClear()
    {
        if (IsClear())
        {
            Debug.Log("クリア");
            //*クリアの時に発動
            //*ClearEvent.Invoke();
        }
    }

    bool IsClear()
    {
        //正解しているかどうか　
        // =>　一つでも一致しなければfalse
        // => 全てのチェックをクリアすればtrue
        for(int i=0; i< correctNumbers.Length; i++)
        {
            if (passwordButtons[i].number != correctNumbers[i])
            {
                return false;
            }
        }
        return true;
    }
 
}
