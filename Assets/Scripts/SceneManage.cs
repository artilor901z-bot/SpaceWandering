using UnityEngine;
using UnityEngine.SceneManagement;

public class SimpleSceneLoader : MonoBehaviour
{
    public void LoadNextScene()
    {
        SceneManager.LoadScene("SampleScene");  // ������ĳ�����
        // �����ó���������SceneManager.LoadScene(1);  // 1�ǳ�����Build Settings�е�����
    }
}