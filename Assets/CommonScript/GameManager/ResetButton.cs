using UnityEngine;
using UnityEngine.SceneManagement;

public class ReplayButton : MonoBehaviour
{
    public void replay()
    {
        // ���݂̃V�[�����擾
        Scene scene = SceneManager.GetActiveScene();
        // ���݂̃V�[���̃r���h�ԍ����擾
        int buildIndex = scene.buildIndex;
        // �擾�����r���h�ԍ��̃V�[���i���݂̃V�[���j��ǂݍ���
        SceneManager.LoadScene(buildIndex);
    }
}
