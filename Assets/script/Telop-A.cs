using System.Collections;
using UnityEngine;

public class Telop_A : MonoBehaviour
{
    [SerializeField] private MonoBehaviour playerScript;  // Playerスクリプトの参照
    [SerializeField] private MonoBehaviour itemBoxScript; // ItemBoxスクリプトの参照

    // Start is called before the first frame update
    void Start()
    {
        // スクリプトを無効化
        playerScript.enabled = false;
        itemBoxScript.enabled = false;

        // ItemBoxがアタッチされているGameObjectを非アクティブ化
        itemBoxScript.gameObject.SetActive(false);

        // 15秒後にスクリプトとGameObjectを有効化
        StartCoroutine(EnableScriptsAfterDelay(10f));
    }

    private IEnumerator EnableScriptsAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);  // 指定された秒数待機

        // スクリプトとGameObjectを有効化
        playerScript.enabled = true;
        itemBoxScript.enabled = true;
        itemBoxScript.gameObject.SetActive(true);
    }
}
