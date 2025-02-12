using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//攻撃オブジェクトを出したりオンオフしたり。
public class GiantSpiderAttack : MonoBehaviour
{
    [SerializeField] private GiantSpiderStatus giantSpiderStatus = null;
    [SerializeField] private GameObject tackle = null;
    [SerializeField] private GameObject guillotine = null;
    [SerializeField] private GameObject webShot = null;
    [SerializeField] private Transform webShotTrans = null;
    [SerializeField] private GameObject webBeem = null;

    private List<GameObject> generateObjects = new List<GameObject> ();

    void Awake()
    {
        TackleOff();
    }

    public void TackleOn()
    {
        tackle.SetActive(true);
    }
    public void TackleOff()
    {
        tackle.SetActive(false);
    }

    public void GenerateGuillotine()
    {
        if (giantSpiderStatus.PlayerTrans == null)
        {
            Instantiate(guillotine, this.transform.position, Quaternion.Euler(0f, 0f, 0f));
        }
        else
        {
            Instantiate(guillotine, giantSpiderStatus.PlayerTrans.position, Quaternion.Euler(0f, 0f, 0f));
        }
    }

    public void GenerateWebShot()
    {
        Instantiate(webShot, webShotTrans.position, Quaternion.Euler(0f, 0f, 80f));
        Instantiate(webShot, webShotTrans.position, Quaternion.Euler(0f, 0f, 100f));
    }

    public void GenerateWebBeem()
    {
        GameObject beem = Instantiate(webBeem, webShotTrans.position, Quaternion.Euler(0f, 0f, 90f));
        generateObjects.Add(beem);
    }

    //独立している攻撃オブジェクトをすべて破壊する。
    public void AllClear()
    {
        for(int i = 0; i < generateObjects.Count; i++)
        {
            GameObject destroyObject = generateObjects[0];
            generateObjects.Remove(destroyObject);
            Destroy(destroyObject);
        }
    }
}
