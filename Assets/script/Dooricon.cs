using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dooricon : MonoBehaviour
{
    [SerializeField] private Collider triggerCollider; // コライダー
    [SerializeField] private GameObject targetImage;   // 表示・非表示を制御するImage
    [SerializeField] private GameObject firsttargetImage;   // 表示・非表示を制御するImage


    void Start()
    {
        if (targetImage != null)
        {
            targetImage.SetActive(false);
        }

        if (firsttargetImage != null)
        {
            firsttargetImage.SetActive(true);
        }
    }

    void Update()
    {
        if (FlagManager.Instance.GetFlag(FlagManager.FlagType.FireParticleON))
        {
            return;
        }

        if (FlagManager.Instance.GetFlag(FlagManager.FlagType.DialPasswordclear))
        {
            if (firsttargetImage != null)
            {
                firsttargetImage.SetActive(false);
            }
        }

    }
        private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 条件を満たしたときのみ targetImage を表示
            if (!FlagManager.Instance.GetFlag(FlagManager.FlagType.Result_A) &&
                FlagManager.Instance.GetFlag(FlagManager.FlagType.playballoon))
            {
                if (targetImage != null)
                {
                    targetImage.SetActive(true);
                }
            }
            // 条件を満たしたときのみ targetImage を表示
            if (!FlagManager.Instance.GetFlag(FlagManager.FlagType.Result_B) &&
                FlagManager.Instance.GetFlag(FlagManager.FlagType.Pillaropen))
            {
                if (targetImage != null)
                {
                    targetImage.SetActive(true);
                }
            }

            if (!FlagManager.Instance.GetFlag(FlagManager.FlagType.Result_C) &&
               FlagManager.Instance.GetFlag(FlagManager.FlagType.yappiclear))
            {
                if (targetImage != null)
                {
                    targetImage.SetActive(true);
                }
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
