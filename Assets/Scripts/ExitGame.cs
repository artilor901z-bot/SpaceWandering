using UnityEngine;

public class GameExit : MonoBehaviour
{
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;  // �ڱ༭����ֹͣ����
#else
            Application.Quit();  // �ڹ����õ���Ϸ���˳�
#endif
    }
}