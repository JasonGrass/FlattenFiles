using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FlattenFiles
{
    public class FlattenOptions : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private string _extFilter;
        private string _inputFolder;
        private string _outputFolder;
        private bool _isMove;

        /// <summary>
        /// 包含文件的后缀名
        /// </summary>
        public string ExtFilter
        {
            get => _extFilter;
            set
            {
                _extFilter = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 输入文件夹
        /// </summary>
        public string InputFolder
        {
            get => _inputFolder;
            set
            {
                _inputFolder = value;
                OnPropertyChanged();
            } }

        /// <summary>
        /// 输出文件夹
        /// </summary>
        public string OutputFolder
        {
            get => _outputFolder;
            set
            {
                _outputFolder = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 是否移动，true: 移动；false: 复制
        /// </summary>
        public bool IsMove
        {
            get => _isMove;
            set
            {
                _isMove = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsCopy));
            }
        }

        public bool IsCopy
        {
            get => !_isMove;
            set
            {
                _isMove = !value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsMove));
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
