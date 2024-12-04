using UnityEngine;
using UnityEngine.SceneManagement;

public class SimpleSceneLoader : MonoBehaviour
{
    public void LoadNextScene()
    {
        SceneManager.LoadScene("SampleScene");  // 填入你的场景名
        // 或者用场景索引：SceneManager.LoadScene(1);  // 1是场景在Build Settings中的索引
    }
}