using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�\�����ꂽ��A�j���[�V�������f�t�H���g�X�e�[�g�ɖ߂��B
public class AnimReset : MonoBehaviour
{
    private Animator anim = null;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void OnEnable()
    {
        anim.Rebind();//�\�����ꂽ��ŏ�����Đ��ł���悤��
    }
}
