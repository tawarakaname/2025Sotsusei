using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCanvas : MonoBehaviour
{
    [SerializeField] private GameObject itemObject;
    // Start is called before the first frame update
    void Start()
    {
        itemObject.SetActive(false);
    }

        // Update is called once per frame
        void Update()
    {
        // PS4コントローラーの▫︎ボタンは「Fire0」として認識されます
        if (Input.GetButtonDown("Fire0"))
        {
            itemObject.SetActive(true);
            Debug.Log("▫︎ボタンが押されました！");
        }
    }
   
}
