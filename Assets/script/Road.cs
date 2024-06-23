using UnityEngine;
using UnityEngine.SceneManagement;

public class Road : MonoBehaviour
{
    [SerializeField] string sceneName;
    public void Action()
    {
        SceneManager.LoadScene(sceneName);
    }
}
