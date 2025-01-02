using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Player player;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameOver()
    {
        SceneManager.LoadScene("Title");
    }

    public void GameClear()
    {
        StartCoroutine(ClearC());
    }

    IEnumerator ClearC()
    {
        yield return new WaitForSeconds(4);
        SceneManager.LoadScene("Title");
    }
}
