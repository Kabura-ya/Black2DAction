using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrainTrue : MonoBehaviour, IDrainable
{
    // Start is called before the first frame update
    //��Ƀh���C���\�Ȃ��̂ɂ���p�ADrain()�֐���true��Ԃ�����
    public bool Drain()
    {
        return true;
    }
}
