using UnityEngine;

public interface IDamageable
{

    public void Damage(int value);

    public void Damage(int value, Vector2 vector);
    //�_���[�W�̎�ނ�Player�ɂ����g��Ȃ��\��ł͂��邪�A��̎����l�����Ĉꉞ����Ă���type��1���ƃ_�b�V���ł悯���āA2���Ɣ������Ȃ�
    public void Damage(int value/*�_���[�W�̒l*/, Vector2 vector/*�m�b�N�o�b�N�̕���*/, int type/*�_���[�W�̎��*/);
    /*//�����O�̂���������Ă�����R�s�y����΂悢�A�^���I�Ƀf�t�H���g�������g�����߂ɂ���Ă���
    public void Damage(int value){Damage(value, Vector2.zero);}
    public void Damage(int value, Vector2 vector){Damage(value, vector, 0);}
     */
    //public void Death();//�ォ�������
}