using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialPass : MonoBehaviour
{
    [SerializeField] GameObject Dialpassword;
    private bool iszoom;

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
        }
        else if (!FlagManager.Instance.GetFlag(FlagManager.FlagType.GasCamera0) && iszoom)
        {
            iszoom = false;
            Dialpassword.SetActive(false);
        }
            
    }
}
