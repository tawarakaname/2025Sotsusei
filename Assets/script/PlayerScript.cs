using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    // 移動速度
    public float speed = 5f;
    public GameObject sphereObject; // Sphere オブジェクトの参照を格納するための変数

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        {
            // キーボード入力を取得
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            // 移動量を計算
            Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput) * speed * Time.deltaTime;

            // プレイヤーを移動させる
            transform.Translate(movement);
        }

        // マウスの左ボタンがクリックされたら
        if (Input.GetMouseButtonDown(0))
        {
            // マウスの位置から Ray を作成
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Ray が Cube に当たった場合
            if (Physics.Raycast(ray, out hit) && hit.collider.CompareTag("cube"))
            {
                // Sphere オブジェクトを表示する
                sphereObject.SetActive(true);

                // Player オブジェクトの位置と Cube の位置を比較し、Cube の前にいるか判定
                Vector3 playerPos = transform.position;
                Vector3 cubePos = hit.collider.transform.position;
                Debug.Log("見つかりません!");
                if (playerPos.z < cubePos.z)
                {
                    Debug.Log("cube の前に立っています！");
                    // ここに Cube の前に立ったときの処理を追加する
                   
                }
            }
        }

    }

}
