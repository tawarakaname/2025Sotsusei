using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialPass : MonoBehaviour
{
    [SerializeField] GameObject Dialpassword;
    private bool iszoom;
    private DialPasswordButton dialPasswordButton; // DialPasswordButton スクリプトへの参照

    private void Start()
    {
        Dialpassword.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        if(FlagManager.Instance.GetFlag(FlagManager.FlagType.GasCamera0)&&!iszoom)
        {
            iszoom = true;
            Dialpassword.SetActive(true);
            DialPasswordButton[] dialPasswordButtons = Dialpassword.GetComponentsInChildren<DialPasswordButton>();
            foreach (var button in dialPasswordButtons)
            {
                button.ResetNumber(); // 数字をリセット
            }
        }
        else if (!FlagManager.Instance.GetFlag(FlagManager.FlagType.GasCamera0) && iszoom)
        {
            iszoom = false;
            Dialpassword.SetActive(false);
        }
            
    }
}
