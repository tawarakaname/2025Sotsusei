using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kari : MonoBehaviour
{
    [SerializeField] private GameObject[] deleteObjs; // 非アクティブにするオブジェクトの配列
    [SerializeField] private GameObject[] nodeleteObjs; // 非アクティブにするオブジェクトの配列
    [SerializeField] private GameObject karicanvas; // 親 Canvas
    [SerializeField] private GameObject panel; // 子の Panel
    private bool hasActivated = false;

    void Update()
    {
        if (hasActivated || !FlagManager.Instance.GetFlag(FlagManager.FlagType.Belt1move))
        {
            return;
        }

        //if (FlagManager.Instance.GetFlag(FlagManager.FlagType.Belt2move))
        //{
        //    karicanvas.SetActive(true); // 親 Canvas を表示
        //    panel.SetActive(true); // 子 Panel を表示

        //    // Panel を Canvas 内で最前面に移動
        //    panel.transform.SetAsLastSibling();

        //    hasActivated = true;
        //}

        if (FlagManager.Instance.GetFlag(FlagManager.FlagType.switch2ON))
        {
            foreach (GameObject obj in deleteObjs)
            {
                if (obj != null)
                {
                    obj.SetActive(false); // 各オブジェクトを非表示にする
                }
            }
            foreach (GameObject noobj in nodeleteObjs)
            {
                if (noobj != null)
                {
                    noobj.SetActive(true); // 各オブジェクトを非表示にする
                }
            }
        }
    }
}
