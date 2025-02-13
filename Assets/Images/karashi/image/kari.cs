using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kari : MonoBehaviour
{
    [SerializeField] private GameObject karicanvas; // 親 Canvas
    [SerializeField] private GameObject panel; // 子の Panel
    private bool hasActivated = false;

    void Update()
    {
        if (hasActivated ||!FlagManager.Instance.GetFlag(FlagManager.FlagType.Belt1move))
        {
            return;
        }

        if (FlagManager.Instance.GetFlag(FlagManager.FlagType.Belt2move))
        {
            karicanvas.SetActive(true); // 親 Canvas を表示
            panel.SetActive(true); // 子 Panel を表示

            // Panel を Canvas 内で最前面に移動
            panel.transform.SetAsLastSibling();

            hasActivated = true;
        }
    }
}
