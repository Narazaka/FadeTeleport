# Fade Teleport

フェードしてテレポートするやつ

## Install

### VCC用インストーラーunitypackageによる方法（おすすめ）

https://github.com/Narazaka/FadeTeleport/releases/latest から `net.narazaka.vrchat.fade-teleport-installer.zip` をダウンロードして解凍し、対象のプロジェクトにインポートする。

### VCCによる方法

1. https://vpm.narazaka.net/ から「Add to VCC」ボタンを押してリポジトリをVCCにインストールします。
2. VCCでSettings→Packages→Installed Repositoriesの一覧中で「Narazaka VPM Listing」にチェックが付いていることを確認します。
3. アバタープロジェクトの「Manage Project」から「Fade Teleport」をインストールします。

## Usage

`FadeTeleporter`プレハブを**1つだけ**シーンに置く。

以下のAPIを呼ぶ（フェード時間内に短時間に呼んでもそれっぽくハンドリングします）。

```csharp
void ReserveTeleportTo(Vector3 position, Quaternion rotation);
void ReserveTeleportTo(Vector3 position, Quaternion rotation, bool lockPosition);
void ReserveRespawn();
void ReserveRespawn(bool lockPosition);
```

Interactなどで手軽にやりたい場合は`FadeTeleportTo`や`FadeRespawn`などをAdd Componentしてください。

## 更新履歴

- 0.1.0: リリース

## License

[Zlib License](LICENSE.txt)
