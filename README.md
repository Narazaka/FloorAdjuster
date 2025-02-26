# FloorAdjuster

Floor Adjuster

## 概要

非破壊にアバターの上下位置を変更するツールです。

Modular Avatarと互換性があります。

## インストール

### VCC用インストーラーunitypackageによる方法（VRChatプロジェクトおすすめ）

https://github.com/Narazaka/FloorAdjuster/releases/latest から `net.narazaka.vrchat.floor-adjuster-installer.zip` をダウンロードして解凍し、対象のプロジェクトにインポートする。

### VCCによる方法

0. https://modular-avatar.nadena.dev/ja から「ダウンロード（VCC経由）」ボタンを押してリポジトリをVCCにインストールします。
1. [https://vpm.narazaka.net/](https://vpm.narazaka.net/?q=net.narazaka.vrchat.floor-adjuster) から「Add to VCC」ボタンを押してリポジトリをVCCにインストールします。
2. VCCでSettings→Packages→Installed Repositoriesの一覧中で「Narazaka VPM Listing」にチェックが付いていることを確認します。
3. アバタープロジェクトの「Manage Project」から「Floor Adjuster」をインストールします。

## 使い方

1. アバターのArmatureのオブジェクトを選択し、「Add Component」から「Floor Adjuster」を選んで付けます。

2. 地面高さを設定します。

3. アップロードするとたぶん上下位置が動いてるはず

## 注意

アバターのArmatureのスケールを調整してHipsボーンを移動させる＆Hipsボーンにスケールの逆数をかけることで大きさを保つ手法をとっています。

このため厳密にはボーンの配置に寄りますが一般的にアバターの上下位置だけでなく前後位置も移動します。これを打ち消すためにViewPointの上下+前後位置も動かしています。

## 更新履歴

- 1.0.5
  - VCCインストーラーを追加
  - changelogUrlをマニフェストに追加
- 1.0.4
  - VCCでのUnity 2022プロジェクトへのインストールでUnityバージョン警告がでないように
- 1.0.3
  - MA依存では無くNDMF依存にする by @anatawa12
  - 誤ってるっぽいところ（アバタールートなど）にコンポーネントを付けたときに警告
  - スケールが非標準のArmatureで動作するようにする by @lunetoiles
- 1.0.2
  - asmdef修正
- 1.0.1
  - アバターViewPointの前後位置ズレを修正
- 1.0.0
  - リリース

## License

[Zlib License](LICENSE.txt)
