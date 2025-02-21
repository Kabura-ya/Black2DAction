using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//�U���I�u�W�F�N�g���o������I���I�t������B
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
        GameObject guill = null;
        if (giantSpiderStatus.PlayerTrans == null)
        {
            guill = Instantiate(guillotine, this.transform.position, Quaternion.Euler(0f, 0f, 0f));
            DestroyRegist(guill);
        }
        else
        {
            guill = Instantiate(guillotine, giantSpiderStatus.PlayerTrans.position, Quaternion.Euler(0f, 0f, 0f));
            DestroyRegist(guill);
        }
    }

    public void GenerateWebShot()
    {
        GameObject shot = Instantiate(webShot, webShotTrans.position, Quaternion.Euler(0f, 0f, 80f));
        shot.GetComponent<WebShot>().RegistGSA(this);
        DestroyRegist(shot);

        shot = Instantiate(webShot, webShotTrans.position, Quaternion.Euler(0f, 0f, 90f));
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

    //�U���I�u�W�F�N�g�͎��S�A�X�^�����ɏ������������B
    public void DestroyRegist(GameObject attackObject)
    {
        clearEvent.AddListener(() => { Destroy(attackObject); });
    }

    //�Ɨ����Ă���U���I�u�W�F�N�g�����ׂĔj�󂷂�B
    public void AllClear()
    {
        TackleOff();
        clearEvent.Invoke();
    }
}
