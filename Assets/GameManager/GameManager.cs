using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Player player;
    public string loseLoadSceneName;
    public string winLoadSceneName;
    public bool[] DefeatedBosses = new bool[50];//倒したボスの番号のところをtrueにする
    private int numOfBosses = 20;//ボスが何体いるか
    public static GameManager instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            for (int i = 0; i < numOfBosses; i++)
            {
                DefeatedBosses[i] = false;
            }
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameOver()
    {
        SceneManager.LoadScene(loseLoadSceneName);
    }

    public void GameClear(int bossNum)//ボスを倒したら各ボスの番号を引数としてこの関数が呼ばれる
    {
        DefeatedBosses[bossNum] =true;//
        Debug.Log("DefeatBoss");
        Debug.Log(bossNum);
        StartCoroutine(ClearC());
    }

    IEnumerator ClearC()
    {
        yield return new WaitForSeconds(4);
        SceneManager.LoadScene(winLoadSceneName);
    }
}
