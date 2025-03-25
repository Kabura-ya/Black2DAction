using UnityEngine;

public interface IDamageable
{
    public void Damage(int value/*�_���[�W�̒l*/, Vector2 vector/*�m�b�N�o�b�N�̕���*/, int type/*�_���[�W�̎��*/);
    //�G���v���C���[�ɍU������ꍇ�F�_���[�W�̎�ނ́A0�͕��ʂ̃_�b�V���ŉ���\�ŁA1�̓`���[�W�_�b�V���ł̂݉���\�̐ԍU���A2�͉��s�\�Ȓn�`�Ȃǂ̃_���[�W
    //�v���C���[���G�ɍU������ꍇ�F�_���[�W�̎�ނ́A0�͒ʏ�U���A1�̓G�l���M�[�U���A2�̓`���[�W�_�b�V���łԂ���U��

    //��̈���3�̂�������g�̏����������΂悢�B���̂���͋^���I�Ƀf�t�H���g�������g�����߂ɂ���Ă���B
    void Damage(int value) => Damage(value, Vector2.zero, 0);
    void Damage(int value, Vector2 vector) => Damage(value, vector, 0);

    
}