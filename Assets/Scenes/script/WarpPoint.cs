using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpPoint : MonoBehaviour
{

    public Vector3 teleportDestination;
    private void OnTriggerEnter(Collider other)
    {
        // �v���C���[���g���K�[�]�[���ɓ��������ǂ������m�F
        if (other.CompareTag("Player"))
        {
            // �v���C���[�̈ʒu�����[�v��ɐݒ�
            other.transform.position = teleportDestination;
        }
    }
}
