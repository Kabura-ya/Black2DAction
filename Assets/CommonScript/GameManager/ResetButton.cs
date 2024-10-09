using UnityEngine;
using UnityEngine.SceneManagement;

public class ReplayButton : MonoBehaviour
{
    public void replay()
    {
        // 現在のシーンを取得
        Scene scene = SceneManager.GetActiveScene();
        // 現在のシーンのビルド番号を取得
        int buildIndex = scene.buildIndex;
        // 取得したビルド番号のシーン（現在のシーン）を読み込む
        SceneManager.LoadScene(buildIndex);
    }
}
