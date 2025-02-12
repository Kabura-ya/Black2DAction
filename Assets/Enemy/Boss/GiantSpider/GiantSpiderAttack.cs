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

    //�U���I�u�W�F�N�g�͎��S�A�X�^�����ɏ������������B
    public void DestroyRegist(GameObject attackObject)
    {
        clearEvent.AddListener(() => { Destroy(attackObject); });
    }

    //�Ɨ����Ă���U���I�u�W�F�N�g�����ׂĔj�󂷂�B
    public void AllClear()
    {
        clearEvent.Invoke();
    }
}
