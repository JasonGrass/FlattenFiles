using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FlattenFiles
{
    internal static class Utils
    {
        public static string CalaMD5(string filePath)
        {
            // 使用using语句确保MD5实例在完成后被正确释放
            using var md5 = MD5.Create();
            // 打开指定文件的FileStream
            using var stream = File.OpenRead(filePath);
            // 计算文件的MD5哈希值
            var hash = md5.ComputeHash(stream);
            // 将字节数组转换成十六进制字符串
            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }
    }
}
