# Honkai3AbyssUploader

## 介绍
崩坏三深渊战场共享播报机，由于缺少获取深渊战场的API，并且操作崩坏三主程序后截图并不是一个很好的解决方案，所以想出了这个共同上传共同受益的方案。

总体思路就是大家的Bot去连接一个中心服务器，当有一个人提供本期深渊或战场截图时，便广播到所有连接到服务器的Bot上

## 特性
- 一期深渊或战场只广播第一个发送的人
- 可以多次上传同一期深渊战场，后续查询时将随机发送一份

## 指令
- 深渊快报：查询当期深渊
- 战场快报：查询当期战场
- 上传深渊快报：上传当期深渊信息
- 上传战场快报：上传当期战场信息

## 配置
位于数据目录下`me.cqp.luohuaming.AbyssUploader\Config.json`，配置需重载后生效
|键|默认值|含义|
|----|----|----|
|WebSocketURL|ws://abyss.hellobaka.xyz/ws|消息服务器Websocket|
|ReconnectTimeout|3000|重连等待时长|
|HeartBeatTimeout|30000|心跳等待时长|
|APIWaitTimeout|10000|调用等待时长|
|EnableGroup| |启用的群，使用`\|`分割|
|QueryAbyssOrder|深渊快报|查询深渊的指令|
|QueryMemoryFieldOrder|战场快报|查询战场的指令|
|UploadAbyssOrder|上传深渊快报|上传深渊的指令|
|UploadMemoryFieldOrder|上传战场快报|上传战场的指令|
|AbyssRemarkEnable|false|是否启用深渊额外备注功能|
|MemoryFieldRemarkEnable|false|是否启用战场额外备注功能|

## 问题
由于性质类似公共留言板，不可避免会出现有人想要发送不合适的消息，由于本服务现在是试行阶段，没有对应的规制手段，希望使用本服务的大家能够只上传游戏内截图

## 兼容性
如果你并不想使用CQ插件，也可以自己写一个插件。连接原理就是WebSocket，包体足够简单，看着代码就能很快搓一个出来。如果有需要详细的通信协议的请留言，我会考虑写一份出来的。

## 画的饼
- [ ] 审核
- [ ] 拉黑群
- [ ] 举报
- [ ] 查询历史记录
- [ ] 贡献排行榜
