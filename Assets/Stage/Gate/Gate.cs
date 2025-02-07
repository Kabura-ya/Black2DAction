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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerEnter && Input.GetKeyDown(KeyCode.E)) { SceneManager.LoadScene(sceneName); }
    }

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((openBossnum == -1 || GameManager.instance.DefeatedBosses[openBossnum]) &&  collision.gameObject.tag == "Player" ) {
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
