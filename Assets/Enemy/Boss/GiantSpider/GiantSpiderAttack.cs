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
    [SerializeField] private Transform webShotPos = null;

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
        Instantiate(webShot, webShotPos.position, Quaternion.identity);
    }
}
