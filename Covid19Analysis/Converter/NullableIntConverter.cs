using System;
using Windows.UI.Xaml.Data;

namespace Covid19Analysis.Converter
{
    /// <summary>
    /// </summary>
    public class NullableIntConverter : IValueConverter
    {
        #region Methods

        /// <summary>
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value;
        }

        /// <summary>
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            int temp;
            if (string.IsNullOrEmpty((string) value) || !int.TryParse((string) value, out temp))
            {
                return null;
            }

            return temp;
        }

        #endregion
    }
}