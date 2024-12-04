using UnityEngine;

public class GameExit : MonoBehaviour
{
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;  // 在编辑器中停止运行
#else
            Application.Quit();  // 在构建好的游戏中退出
#endif
    }
}