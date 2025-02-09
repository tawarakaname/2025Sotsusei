using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dooricon : MonoBehaviour
{
    [SerializeField] private Collider triggerCollider; // コライダー
    [SerializeField] private GameObject targetImage;   // 表示・非表示を制御するImage


    void Start()
    {
        if (targetImage != null)
        {
            targetImage.SetActive(false);
        }
    }

    void Update()
    {
        if (FlagManager.Instance.GetFlag(FlagManager.FlagType.Result_A)) return;
        if (!FlagManager.Instance.GetFlag(FlagManager.FlagType.playballoon)) return;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (targetImage != null)
            {
                targetImage.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (targetImage != null)
            {
                targetImage.SetActive(false);
            }
        }
    }
}
