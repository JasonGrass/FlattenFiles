using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FlattenFiles
{
    internal class Flatten
    {
        private readonly FlattenOptions _options;
        private IList<string> _exts = new List<string>(0);

        private HashSet<string> _filesMd5 = new HashSet<string>();
        private HashSet<string> _filesNames = new HashSet<string>();

        private HashSet<string> _allExts = new HashSet<string>();

        private Action<string> _logger;

        public Flatten(FlattenOptions options, Action<string> logger)
        {
            _options  = options;
            _logger = logger;
            CheckOptions();
        }

        public void FlattenFiles()
        {
            var files = ReadFiles(_options.InputFolder);

            var output = Path.Combine(_options.OutputFolder, $"Flatten_{DateTime.Now:yyyyMMddHHmmss}");
            Directory.CreateDirectory(output);

            var successCount = 0;
            var failCount = 0;

            foreach (var file in files)
            {
                var fileExt = Path.GetExtension(file).ToLower();
                _allExts.Add(fileExt);
                if (_exts.Count > 0 && !_exts.Contains(fileExt))
                {
                    // 后缀名过滤
                    continue;
                }

                // 处理相同文件
                if (!_filesMd5.Add(Utils.CalaMD5(file)))
                {
                    // 相同文件则忽略
                    _logger($"MD5 相同，忽略 {file}");
                    continue;
                }

                // 处理重名
                var filename = Path.GetFileName(file).ToLower();
                if (!_filesNames.Add(filename))
                {
                    do
                    {
                        filename = Path.GetFileNameWithoutExtension(file) + "_" +
                                   Guid.NewGuid().ToString("N").Substring(0, 8) + Path.GetExtension(file);
                        filename = filename.ToLower();
                    } while (!_filesNames.Add(filename));

                    _logger($"重名文件，重命名 {Path.GetFileName(file)} -> {filename}");
                }

                if (_options.IsCopy)
                {
                    try
                    {
                        File.Copy(file, Path.Combine(output, filename));
                        successCount++;
                    }
                    catch (Exception ex)
                    {
                        _logger($"非预期情况，文件复制失败。 {ex.GetType().Name} - {ex.Message} {file}");
                        failCount++;
                    }
                }
                else
                {
                    try
                    {
                        File.Move(file, Path.Combine(output, filename));
                        successCount++;
                    }
                    catch (Exception ex)
                    {
                        _logger($"非预期情况，文件移动失败。 {ex.GetType().Name} - {ex.Message} {file}");
                        failCount++;
                    }
                }

            }

            _logger($"所有文件的后缀为：{string.Join(";", _allExts)}");
            _logger($"一共 {files.Count} 个文件，移动/复制了 {successCount} 个，失败 {failCount} 个");
        }


        private IList<string> ReadFiles(string path)
        {
            var files = Directory.GetFiles(path).ToList();
            var dirs = Directory.GetDirectories(path);
            foreach (var dir in dirs)
            {
                var f = ReadFiles(dir);
                files.AddRange(f);
            }

            return files;
        }

        private void CheckOptions()
        {
            if (!string.IsNullOrWhiteSpace(_options.ExtFilter))
            {
                _exts = _options.ExtFilter.Split(";")
                    .Where(ext => !string.IsNullOrWhiteSpace(ext))
                    .Select(ext => ext.Trim().ToLower())
                    .ToList();
            }

            if (!Directory.Exists(_options.InputFolder))
            {
                throw new DirectoryNotFoundException($"没有找到输入文件夹 {_options.InputFolder}");
            }
            if (!Directory.Exists(_options.OutputFolder))
            {
                throw new DirectoryNotFoundException($"没有找到输出文件夹 {_options.OutputFolder}");
            }
        }



    }
}
