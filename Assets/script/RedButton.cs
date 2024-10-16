using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedButton : MonoBehaviour
{
    private FlagManager flagManager;
    [SerializeField] private GameObject TextBox; // TextBoxの参照


    // Start is called before the first frame update
    void Start()
    {
        flagManager = FlagManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        // 両方のフラグがtrueかどうかを確認
        if (flagManager != null &&
            flagManager.GetFlag(FlagManager.FlagType.ThreePasswordclear) &&
            flagManager.GetFlag(FlagManager.FlagType.BTB))
        {
            Debug.Log("ThreePasswordclear と BTB の両方のフラグが true です！");
        }
    }
}
