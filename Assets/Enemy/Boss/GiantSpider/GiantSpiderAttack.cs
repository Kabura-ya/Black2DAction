using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//攻撃オブジェクトを出したりオンオフしたり。
public class GiantSpiderAttack : MonoBehaviour
{
    [SerializeField] private GiantSpiderStatus giantSpiderStatus = null;
    [SerializeField] private GameObject tackle = null;
    [SerializeField] private GameObject guillotine = null;
    [SerializeField] private GameObject webShot = null;
    [SerializeField] private Transform webShotTrans = null;
    [SerializeField] private GameObject webBeem = null;

    public UnityEvent clearEvent;

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
        GameObject shot = Instantiate(webShot, webShotTrans.position, Quaternion.Euler(0f, 0f, 80f));
        shot.GetComponent<WebShot>().RegistGSA(this);
        DestroyRegist(shot);

        shot = Instantiate(webShot, webShotTrans.position, Quaternion.Euler(0f, 0f, 100f));
        shot.GetComponent<WebShot>().RegistGSA(this);
        DestroyRegist(shot);
    }

    public void GenerateWebBeem()
    {
        GameObject beem = Instantiate(webBeem, webShotTrans.position, Quaternion.Euler(0f, 0f, 90f));
        DestroyRegist(beem);
    }

    //攻撃オブジェクトは死亡、スタン時に消去させたい。
    public void DestroyRegist(GameObject attackObject)
    {
        clearEvent.AddListener(() => { Destroy(attackObject); });
    }

    //独立している攻撃オブジェクトをすべて破壊する。
    public void AllClear()
    {
        clearEvent.Invoke();
    }
}
