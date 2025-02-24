# UnityRoomのURL
https://unityroom.com/games/boss_dash

ここから遊べるのは私が1人で制作を進めた分です。

# 操作

移動[左右キー]

ジャンプ[Z]

近距離攻撃[X]

通常ダッシュ[C]

チャージダッシュ[D]長押し～離す

エネルギー弾[S]（左上の下のゲージが紫色のときのみ）

# アクション等の基本仕様

攻撃は近距離攻撃が基本

ダッシュ回避中に**あえて敵の攻撃に当たりにいく**ことにメリットがあるゲーム性

ダッシュ回避中に攻撃に当たるとMPがたまり、MPを消費して大技を放てる

## ドレインダッシュ（ダッシュ回避）

ドレインダッシュ（ダッシュ回避）で敵や一部の攻撃をすりぬけると**MP**がたまる（ドレイン）

MPを消費して**大技**（エネルギー弾とか）を放てる

**赤い攻撃**は通常のダッシュ回避では避けられず、ちゃんとかわす必要がある

## 一部ボスの討伐などで追加されるようにしたいアクション（今はアクションの解放部分を開発中なので最初からある）

### チャージダッシュ(今は最初からあるが、途中のボス討伐で解放されるようにしたい)
スクリプトだとSuperDashとか書いてある（ChargeDashとか書いてるとチャージ中なのかダッシュ中なのかどっちの話をしてるのかよくわからないから）

赤い攻撃も回避できる

通常の**2倍**の量のMP吸収できる（通常の攻撃もチャージダッシュで当たれば2倍）

通常のダッシュで回避可能な攻撃は**全て**チャージダッシュでも回避可能

# プログラミング等での重要事項

## Gitの利用法とかブランチの扱い方とか

**他人のブランチに勝手にマージしない**相談の上マージしましょう。

自分のブランチにマージするのは基本developから。

ブランチの管理手法はこれを参考にしています。

https://qiita.com/Sansuusetto/items/90b602f25f49016188d3

## ジャンプ

床にはタグ**Ground**をつける

Groundタグにプレイヤーの足元のコライダーが触れたかで着地判定を行う。Groundタグのついてない足場ではジャンプできない

## インターフェース

### ダメージ用インターフェース（必須、一番重要）

CommonScript/Interface/IDamageable.cs

public void Damage(int value/*ダメージの値*/, Vector2 vector/*ノックバックの方向*/, int type/*ダメージの種類*/);

プレイヤーが受けるダメージ:ダメージの種類typeは、0なら通常ダッシュで避けられる、1は赤攻撃用で、通常ダッシュでは避けられずチャージダッシュを使えば避けられる、2なら何があっても避けられない（現状敵の攻撃に使うつもりはなく、地形などのダメージを想定）。

敵が受けるダメージ：ダメージの種類typeは現在使用していないが、今後ダメージエフェクトを通常攻撃とダッシュで別のものにしたり、チャージダッシュでぶつかった場合だけダメージを受ける状態などを作ることを想定して、敵が受けるダメージにも使用している。攻撃の方向vectorは、ノックバック以外にも、盾を持っている敵など、ある方向からの攻撃のダメージは受けない敵を実装することも想定している。

敵がダメージを受けたり倒された時にはダメージエフェクトを出すように

エフェクトを出すには、public GameObject damageEffect;　のようにGameobject 型のpublic変数を作り、変数にエフェクト用のプレファブを入れ、Damage関数にの中で　Instantiate(damageEffect, transform.position, transform.rotation);　を実行すればよい

Enemy/Effect/DamageEffect/Effect_Damage_Enemy.prefab がダメージを受けた時のエフェクト

Enemy/Effect/DamageEffect/Effect_Defeat.prefab　が倒したときのエフェクト

用意されているエフェクトがボスの絵に合わないと感じたら自分で作り直してもよい。

### ドレイン(ダッシュで当たるとMP吸収ができるもの)用インターフェース（なくても動くが基本つけてほしい）

CommonScript/Interface/IDrainable.cs

敵や敵の攻撃など、プレイヤーがダッシュで当たるとＭＰ吸収可能なものにつける。

Drain()がtrueのものは通常ダッシュでもチャージダッシュでもドレイン可能

チャージダッシュでのみドレイン可能な赤い攻撃などは、SuperDrain()のみtrueでDrain()がfalseにする

接触ダメージが無いものでもドレイン可能な敵を制作する事を想定して、ダメージとは別にインターフェースを作っている。

#### プレイヤーがダッシュでぶつかった相手に何かするための利用

##### スタン

ボスの一部の攻撃中にダッシュでぶつかるとスタンし、一定時間攻撃を加えるチャンスができるようにする。

##### 敵の弾をダッシュで反射

ダッシュで敵の弾をはじくと敵に飛んで行ってダメージを与えるようにする。あえて敵の弾にぶつかりにいくのはスリリングでいいかもしれない。

## ファイル等の位置

各ボスのプレファブやスクリプトやアニメーションなどは、Assets/Enemy/Boss の中に新しくフォルダーを作ってその中に置いておく

各Sceneは Assets/Scenes フォルダの中に保存

## ステータス調整

行動頻度、HP、攻撃力、移動速度などはpublic変数にしておき、後から難易度調整をしやすくしてください。

## 敵の攻撃の予備動作

ゲーム的に理不尽にならないように、敵のアニメーションを見てから攻撃を回避できるようにしてください。

特に赤攻撃は、1秒以上は予備動作のアニメーションや予告エフェクトを出す時間を挟むようにしてください。

## 基本の数値・難易度

敵のダメージは基本1、高くても3まで。

ノックバックはx方向に10、y方向に5。

赤攻撃の予備動作は1秒以上。

ボス討伐時間は1~3分、長くても5分

難易度は初プレイの人が5回以内にクリアできるぐらい、難しくても10回ぐらい。

## 未実装の要素(必須)

### 1:ワールドマップ（実装はできたがもう少し使い勝手をよくしたい）

ボスを討伐すると次のボスの扉が解放される機能。現在は扉のオブジェクト側で、どのボスが倒されたら扉を解放されるかを設定しているが、GameManagerで一括で管理できるようにしたい。

### 2:能力解放

ボス討伐による能力の増加。どのボスが討伐されたかをGameManagerに記録する機能は実装できているため、十分実装可能。

### 3:データのセーブ

どのボスが討伐されているかの情報の保存。宮本がセーブの実装法を知らない

### 4:ボスのHPゲージ

### 5:赤攻撃の予告エフェクト

# その他の作業効率が上がるかもしれない情報

## コルーチンによる遅延処理

敵の行動などで、何秒後にこの行動をする、といった遅延処理こコルーチンが使える。

## 頻繁に使う内容を書いたスクリプト

CommonScript/Useful に、触れたものにダメージを与えるだけ、まっすぐ飛ぶだけ、出現から一定時間後に消滅などの、敵の弾などで頻繁に使いそうなものをいくつか用意してある。
