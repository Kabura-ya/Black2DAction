using UnityEngine;

public interface IDamageable
{

    public void Damage(int value);

    public void Damage(int value, Vector2 vector);
    //ダメージの種類はPlayerに与えるダメージでしか使わない予定ではあるが、後の事も考慮して一応いれておく
    public void Damage(int value/*ダメージの値*/, Vector2 vector/*ノックバックの方向*/, int type/*ダメージの種類*/);
    //ダメージの種類は、0は普通のダッシュで回避可能で、1はチャージダッシュでのみ回避可能の赤攻撃、2は回避不可能な地形などのダメージ
    /*//下のこの3行をコピペして、一番下の引数3つのやつだけ中身の処理を書けばすればよい。疑似的にデフォルト引数を使うためにやっている
    public void Damage(int value){Damage(value, Vector2.zero);}
    public void Damage(int value, Vector2 vector){Damage(value, vector, 0);}
    public void Damage(int value, Vector2 vector, int type){//ここにダメージを受けた時の処理を書く（hpを減らすとかhpが0になったら死ぬとか）}
     */
}