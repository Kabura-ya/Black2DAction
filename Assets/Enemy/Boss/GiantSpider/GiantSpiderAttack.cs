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

    void Awake()
    {
        TackleSwitch(0);
    }

    public void TackleSwitch(int i)
    {
        if(i > 0)
        {
            tackle.SetActive(true);
        }
        else
        {
            tackle.SetActive(false);
        }
    }

    public void GenerateGuillotine()
    {
        if (giantSpiderStatus.PlayerTrans == null)
        {
            Instantiate(guillotine, this.transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(guillotine, giantSpiderStatus.PlayerTrans.position, Quaternion.identity);
        }
    }

    public void GenerateWebShot()
    {
        Instantiate(webShot, webShotTrans.position, Quaternion.Euler(0f, 0f, 80f));
        Instantiate(webShot, webShotTrans.position, Quaternion.Euler(0f, 0f, 100f));
    }

    public void GenerateWebBeem()
    {
        Instantiate(webBeem, webShotTrans.position, Quaternion.Euler(0f, 0f, 90f));
    }
}
