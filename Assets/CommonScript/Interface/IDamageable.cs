using UnityEngine;

public interface IDamageable
{

    public void Damage(int value);

    public void Damage(int value, Vector2 vector);
    //�_���[�W�̎�ނ�Player�ɗ^����_���[�W�ł����g��Ȃ��\��ł͂��邪�A��̎����l�����Ĉꉞ����Ă���
    public void Damage(int value/*�_���[�W�̒l*/, Vector2 vector/*�m�b�N�o�b�N�̕���*/, int type/*�_���[�W�̎��*/);
    //�_���[�W�̎�ނ́A0�͕��ʂ̃_�b�V���ŉ���\�ŁA1�̓`���[�W�_�b�V���ł̂݉���\�̐ԍU���A2�͉��s�\�Ȓn�`�Ȃǂ̃_���[�W
    /*//���̂���3�s���R�s�y���āA��ԉ��̈���3�̂�������g�̏����������΂���΂悢�B�^���I�Ƀf�t�H���g�������g�����߂ɂ���Ă���
    public void Damage(int value){Damage(value, Vector2.zero);}
    public void Damage(int value, Vector2 vector){Damage(value, vector, 0);}
    public void Damage(int value, Vector2 vector, int type){//�����Ƀ_���[�W���󂯂����̏����������ihp�����炷�Ƃ�hp��0�ɂȂ����玀�ʂƂ��j}
     */
}