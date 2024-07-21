using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotosenFlagOn : MonoBehaviour
{
    public void OnClickObj()
    {
        gameObject.SetActive(false);

        // フラグを設定する
        FlagManager.Instance.SetFlag(FlagManager.FlagType.Motosen, true);
        Debug.Log("MotosenFlagON");
    }
}
