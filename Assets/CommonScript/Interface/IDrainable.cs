using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDrainable
{
    //�G�ɂ��Ă����āA�v���C���[���G�i�W�[�h���C���\���������֐�
    // Start is called before the first frame updat
    public bool Drain();//���ꂪtrue�̎��̓G�i�W�[�z���ł���
    public bool SuperDrain();//���ꂪtrue�̂Ƃ��̓X�[�p�[�_�b�V���Ńh���C���ł���
                             //�i�ʏ�̃_�b�V���ŉ���ł���U���������Ȃ��G�Ȃ��Drain()�̌��ʂ����̂܂ܕԂ��΂悢�j
}
