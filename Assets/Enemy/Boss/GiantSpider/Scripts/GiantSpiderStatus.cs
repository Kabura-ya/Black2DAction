using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

//����w偂̃X�e�[�^�X���܂Ƃ߂����́B
//�A�j���[�V�����̃N���b�v�����猻��Ԃ��擾�B
public class GiantSpiderStatus : MonoBehaviour
{
    [Header("�̗�"), SerializeField] private int maxHp = 200;
    public int MaxHp => maxHp;

    [Header("���s���x"),SerializeField] private float horSpeed = 5;
    public float HorSpeed => horSpeed;//�O���擾�����\
    [Header("�������x"), SerializeField] private float verSpeed = 10;
    public float VerSpeed => verSpeed;//�O���擾�����\

    [Header("�W�����v��"), SerializeField] private int jumpCount = 3;
    public int JumpCount => jumpCount;

    [Header("�ːi����"), SerializeField] private float tackleTime = 1.5f;
    public float TackleTime => tackleTime;

    [Header("�㌄"), SerializeField] private float coolTime = 0.3f;
    public float CoolTime => coolTime;

    [SerializeField] private Animator anim = null;

    private Transform playerTrans = null;
    public Transform PlayerTrans => playerTrans;

    private Transform cameraTrans = null;
    public Transform CameraTrans => cameraTrans;

    void Start()
    {
        playerTrans = GameObject.Find("Player").transform;
        cameraTrans = GameObject.Find("Main Camera").transform;
    }

    public bool IsSpawn()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("Spawn");
    }

    public bool IsStand()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("Stand");
    }

    public bool IsPreTackle()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("PreTackle");
    }
    public bool IsTackle()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("Tackle");
    }
    public bool IsPostTackle()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("PostTackle");
    }
    public void TackleSwitch(int i)
    {
        if(i > 0)
        {
            anim.SetBool("tackle", true);
        }
        else
        {
            anim.SetBool("tackle", false);
        }
    }

    public bool IsJump()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("Jump");
    }
    public void JumpSwitch(int i)
    {
        if (i > 0)
        {
            anim.SetBool("jump", true);
        }
        else
        {
            anim.SetBool("jump", false);
        }
    }

    public bool IsGuillotine()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("Guillotine");
    }
    public void GuillotineTrigger()
    {
        anim.SetTrigger("guillotine");
    }

    public bool IsPreWebBeem()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("PreWebBeem");
    }
    public void PreWebBeemTrigger()
    {
        anim.SetTrigger("prewebbeem");
    }

    public bool IsWebBeem()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("WebBeem");
    }
    public void WebBeemTrigger()
    {
        anim.SetTrigger("webbeem");
    }

    public bool IsStan()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("Stan");
    }
    public void StanPlay()
    {
        anim.Play("Stan");
    }
    public bool IsDead()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("Dead");
    }
    public void DeadPlay()
    {
        anim.Play("Dead");
    }
}
