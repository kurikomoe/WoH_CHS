## 介绍
游戏名：魔法使之夜 (Steam)

游戏链接：https://store.steampowered.com/app/2052410/

汉化：澄空汉化 Switch 版搬运，[传送门](https://mahotsukaicn.github.io/)


## 版权声明

所有版权归游戏开发商和原汉化组所有。


## 目录说明：

switch: 澄空switch版 dump，提取澄空汉化文本

Game: 用于测试的游戏放这儿，Archive 里面删除了所有的 hfa 文件

patches: 用于开发汉化补丁

tools: 各种解包和字库生成

unpack: steam 版解包文件，主要放文本和字库图

Analysis: 从各个角落收集的分析



##  分析说明

一些分析文件放在了 Analysis 中，包含 x64dbg db, ida pro 数据库和 note.h

具体的封包格式需要参见 psp-hunex-xx 的 py 文件。

目前 CG 封包的 bmpType == 0xC 还没分析，不过有生之年吧

WoH 启动的时候会检查 steam 连接，之后做二次启动，根据 api 规范，在根目录添加 steam_api64.txt 并写上游戏 appid 可绕过。

bin2cpp 真好用.jpg，希望下次能直接把 dll 也丢进去，减少分发的文件数量。
> 20250217 直接 enigma-virutalbox-engine 打包算了（


## 使用说明

```shell
tools/ccit 目录下，导出 font ccit 映射信息
> cargo run -- --ccits ..\font3.ccit.bin --ccits ..\H.ccit.bin --ccits ..\italic.ccit.bin --ccits ..\normal.ccit.bin --output orig_ccit.json

tools 目录下,重建字库映射，重建字库图，拷贝到目标文件夹
> python3 build_mapping.py &&  python3 draw_font.py && cp font_rebuild/* ../Game/fonts/ -r

Launcher 目录下，编译补丁并启动游戏测试
> just run 
```



