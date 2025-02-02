using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Titl_GameManager : MonoBehaviour
{
    public string loadSceneName;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Test1());
        StartCoroutine(Test2());
    }

    IEnumerator Test1()
    {
        yield return new WaitForSeconds(10);
        Debug.Log("Test1");
    }

    IEnumerator Test2()
    {
        yield return new WaitForSeconds(10);
        Debug.Log("Test2");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartButton()
    {
        SceneManager.LoadScene(loadSceneName);
    }
}
