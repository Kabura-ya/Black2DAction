using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrainSuperTrue : MonoBehaviour, IDrainable
{
    // Start is called before the first frame update
    //�ԍU���ȂǁA��ɃX�[�p�[�_�b�V���Ńh���C���\�Ȃ��̂ɂ���p�ADrain()�֐���true��Ԃ�����
    public bool Drain()
    {
        return false;
    }

    public bool SuperDrain()
    {
        return true;
    }
}

