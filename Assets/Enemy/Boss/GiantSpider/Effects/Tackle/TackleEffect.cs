using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TackleEffect : MonoBehaviour
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
