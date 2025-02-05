using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

//����w偂̃X�e�[�^�X���܂Ƃ߂����́B
//�A�j���[�V�����̃N���b�v�����猻��Ԃ��擾�B
public class GiantSpiderStatus : MonoBehaviour
{
    [Header("���s���x"),SerializeField] private float horSpeed = 5;
    public float HorSpeed => horSpeed;//�O���擾�����\
    [Header("�������x"), SerializeField] private float verSpeed = 10;
    public float VerSpeed => verSpeed;//�O���擾�����\

    [SerializeField] private Animator anim = null;

    public bool IsSpawn()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("Spawn");
    }

    public bool IsStand()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("Stand");
    }

    public bool IsWalk()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("Walk");
    }

    public bool IsJump()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("Jump");
    }

    public bool IsDead()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("Dead");
    }
}
