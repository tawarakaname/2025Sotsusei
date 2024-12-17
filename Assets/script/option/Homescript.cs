using UnityEngine;
using UnityEngine.UI;

public class HomeScript : MonoBehaviour
{
    [SerializeField] private Image contentImage; // 内容を表示するImage


    public void Jump()
    {
        ShowContent();
    }

    private void ShowContent()
    {
        contentImage.gameObject.SetActive(true); // 内容Imageを表示
    }
}

