using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Gate : MonoBehaviour
{
    public string sceneName;
    private bool playerEnter = false;
    public GameObject guide;//�ǂ̃{�^���������ƃ{�X�킪�n�܂邩�̈ē��Ƃ��{�X�̉摜�Ƃ�
    public int openBossnum = 1;//���̔ԍ��̃{�X���|����Ă���Δ�����������(-1�Ȃ�΍ŏ�������)

    //���͊֌W
    private PlayerInput playerInput_;
    // Start is called before the first frame update
    void Start()
    {
        playerInput_ = new PlayerInput();
        playerInput_.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerEnter && playerInput_.Player.GateIn.triggered) { SceneManager.LoadScene(sceneName); }
    }

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((openBossnum == -1 || GameManager.instance.DefeatedBosses[openBossnum]) &&  collision.gameObject.tag == "Player" ) {
            if (openBossnum != -1) {
                Debug.Log("DefeatedBosses[openBossnum]");
                Debug.Log(GameManager.instance.DefeatedBosses[openBossnum]);
                Debug.Log(openBossnum);
            }
            playerEnter = true;
            guide.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player"){
            playerEnter = false;
            guide.SetActive(false);
        }
    }
}
