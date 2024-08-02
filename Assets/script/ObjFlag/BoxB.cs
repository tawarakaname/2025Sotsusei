using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxB : MonoBehaviour
{
    public void OnClickObj()
    {
        // フラグを設定する
        FlagManager.Instance.SetFlag(FlagManager.FlagType.BoxB, true);
       
    }
}

