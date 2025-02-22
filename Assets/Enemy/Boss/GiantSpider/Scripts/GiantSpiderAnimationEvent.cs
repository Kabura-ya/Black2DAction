using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//����w偂��A�j���[�V�������䂷�邽�߂Ɏg���B
//�A�j���[�^�[�R���g���[���[��t�����I�u�W�F�N�g�ɕt����B
public class GiantSpiderAnimationEvent : MonoBehaviour
{
    [SerializeField] private GiantSpiderAttack giantSpiderAttack = null;
    [SerializeField] private GiantSpiderEffect giantSpiderEffect = null;
    private Animator anim = null;

    void Awake()
    {
        anim = GetComponent<Animator>();
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
        giantSpiderEffect.BrakeOn();
    }
    void BrakeOff()
    {
        giantSpiderEffect.BrakeOff();
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
        giantSpiderEffect.WebOn();
    }
    void WebOff()
    {
        giantSpiderEffect.WebOff();
    }

    void AllClear()
    {
        anim.ResetTrigger("guillotine");
        anim.ResetTrigger("prewebbeem");
        anim.ResetTrigger("webbeem");
        anim.SetBool("jump", false);
        anim.SetBool("tackle", false);
        giantSpiderAttack.AllClear();
        giantSpiderEffect.AllClear();
    }
    void Dead()
    {
        Destroy(transform.parent.gameObject);
    }
}
