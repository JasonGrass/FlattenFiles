# FlattenFiles

展平所有的文件夹，将文件都放到一个文件夹中。

## 功能

将输入的文件夹（包括所有子文件夹）中的文件，全部复制或移动到一个单一文件夹

### 后缀过滤

如果填写了后缀名，则只复制/移动指定后缀名的文件（大小写不敏感），没有填写则处理全部的文件。

格式：`.jpg;.png`

### 输出文件夹

程序会在指定的输出文件夹中，新建一个文件夹 `Flatten_xxx`，将所有文件放在这个文件夹中。

## 错误处理

### 1 相同的文件（MD5相同）

相同的文件只会复制/移动一次，后面的直接忽略。因为要计算文件的 MD5 值，所以如果文件比较多，比较大，则速度会比较慢。

### 2 重名文件（MD5不同）

后续的文件，则自动在文件名后面添加一个随机字符串。如 `good.png` -> `good_f3s1f5g7.png`

### 3 复制或移动失败

可能是文件权限问题或者其他问题，会输出到日志中。`非预期情况，文件复制失败。xxx`

可以在日志中搜索 `非预期情况` 定位哪些文件处理失败了。

## 开发

这个工具只是用来临时使用的，不接受新需求。可以自行下载添加新功能。

开发环境：  

- Visual Studio 2022 社区版 [下载 Visual Studio Tools](https://visualstudio.microsoft.com/zh-hans/downloads/ )
- net8.0 SDK [Download .NET 8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0 )
- C# & WPF
