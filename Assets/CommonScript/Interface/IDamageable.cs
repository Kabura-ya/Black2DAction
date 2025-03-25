using UnityEngine;

public interface IDamageable
{
    public void Damage(int value/*ダメージの値*/, Vector2 vector/*ノックバックの方向*/, int type/*ダメージの種類*/);
    //敵がプレイヤーに攻撃する場合：ダメージの種類は、0は普通のダッシュで回避可能で、1はチャージダッシュでのみ回避可能の赤攻撃、2は回避不可能な地形などのダメージ
    //プレイヤーが敵に攻撃する場合：ダメージの種類は、0は通常攻撃、1はエネルギー攻撃、2はチャージダッシュでぶつかる攻撃

    //上の引数3つのやつだけ中身の処理を書けばよい。下のこれは疑似的にデフォルト引数を使うためにやっている。
    void Damage(int value) => Damage(value, Vector2.zero, 0);
    void Damage(int value, Vector2 vector) => Damage(value, vector, 0);

    
}