using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DooriconB : MonoBehaviour
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
        if (!FlagManager.Instance.GetFlag(FlagManager.FlagType.potopen)) return;
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
