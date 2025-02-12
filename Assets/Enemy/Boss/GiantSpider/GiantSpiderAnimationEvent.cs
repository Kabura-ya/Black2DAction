using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//����w偂��A�j���[�V�������䂷�邽�߂Ɏg���B
//�A�j���[�^�[�R���g���[���[��t�����I�u�W�F�N�g�ɕt����B
public class GiantSpiderAnimationEvent : MonoBehaviour
{
    [SerializeField] private GiantSpiderAttack giantSpiderAttack = null;
    
    void GuillotineAttack()
    {
        giantSpiderAttack.GenerateGuillotine();
    }

    void WebBeemAttack()
    {
        giantSpiderAttack.GenerateWebBeem();
    }
}
