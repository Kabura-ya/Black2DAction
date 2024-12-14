using UnityEngine;

public interface IDamageable
{

    public void Damage(int value);

    public void Damage(int value, Vector2 vector);
    //ダメージの種類はPlayerにしか使わない予定ではあるが、後の事も考慮して一応いれておくtypeが1だとダッシュでよけられて、2だと避けられない
    public void Damage(int value/*ダメージの値*/, Vector2 vector/*ノックバックの方向*/, int type/*ダメージの種類*/);
    /*//引数三つのやつだけ書いてこれをコピペすればよい、疑似的にデフォルト引数を使うためにやっている
    public void Damage(int value){Damage(value, Vector2.zero);}
    public void Damage(int value, Vector2 vector){Damage(value, vector, 0);}
     */
    //public void Death();//後から消した
}