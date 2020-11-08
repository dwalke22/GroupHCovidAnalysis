using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Covid19Analysis.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class CollectionExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> collection)
        {
            return new ObservableCollection<T>(collection);
        }
    }
}
