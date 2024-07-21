using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Motosenitem : MonoBehaviour
{
    void Start()
    {
        Debug.Log("Start called");
        gameObject.SetActive(false);
    }


    void Update()
    {
        Debug.Log("Update called");

        bool isMotosenFlagOn = FlagManager.Instance.GetFlag(FlagManager.FlagType.Motosen);
        Debug.Log("Motosen flag status: " + isMotosenFlagOn);

        if (isMotosenFlagOn)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}