using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IDashHit//�v���C���[���_�b�V���łԂ���Ɖ����N������̂ɂ��̃C���^�[�t�F�[�X������i�_�b�V���ɂ��G�i�W�[�z����IDrainable�C���^�[�t�F�[�X�j
{
    //�v���C���[���ʏ�_�b�V���œG�̒e�ɂԂ���ƒe�𔽎˂���Ƃ��A�`���[�W�_�b�V���ňꕔ�̍U���p�^�[�����̓G�ɂԂ���ƓG���X�^����������Ƃ��̂��߂Ɏg�p����
    public void NormalDashHit();//�v���C���[�͒ʏ�_�b�V���łԂ���������ɂ��̊֐������s����B
    public void SuperDashHit();//�v���C���[�̓X�[�p�[�_�b�V���łԂ���������ɂ��̊֐������s����B
}
