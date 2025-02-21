using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//����w偂��A�j���[�V�������䂷�邽�߂Ɏg���B
//�A�j���[�^�[�R���g���[���[��t�����I�u�W�F�N�g�ɕt����B
public class GiantSpiderAnimationEvent : MonoBehaviour
{
    [SerializeField] private GiantSpiderAttack giantSpiderAttack = null;
    [SerializeField] private GameObject web = null;
    [SerializeField] private GameObject brake = null;

    void Awake()
    {
        WebOff();
        BrakeOff();
    }

    void TackleOn()
    {
        giantSpiderAttack.TackleOn();
    }
    void TackleOff()
    {
        giantSpiderAttack.TackleOff();
    }
    void BrakeOn()
    {
        brake.SetActive(true);
    }
    void BrakeOff()
    {
        brake.SetActive(false);
    }

    void GuillotineAttack()
    {
        giantSpiderAttack.GenerateGuillotine();
    }

    void WebBeemAttack()
    {
        giantSpiderAttack.GenerateWebBeem();
    }

    void WebOn()
    {
        web.SetActive(true);
    }
    void WebOff()
    {
        web.SetActive(false);
    }

    void PreDead()
    {
        WebOff();
        TackleOff();
        BrakeOff();
        giantSpiderAttack.AllClear();
    }
    void Dead()
    {
        Destroy(transform.root.gameObject);
    }
}
