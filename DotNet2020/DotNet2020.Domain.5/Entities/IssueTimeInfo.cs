using System;

namespace DotNet2020.Domain._5.Entities
{
    public struct IssueTimeInfo
    {
        public int? EstimatedTime { get; private set; }
        public int? SpentTime { get; private set; }

        public IssueTimeInfo(int? estimatedTime, int? spentTime)
        {
            if (estimatedTime.HasValue && estimatedTime < 0)
                throw new ArgumentException("Must be equal to or greater than 0!", "EstimatedTime");
            if (spentTime.HasValue && spentTime < 0)
                throw new ArgumentException("Must be equal to or greater than 0!", "SpentTime");

            EstimatedTime = estimatedTime;
            SpentTime = spentTime;
        }

        /// <summary>
        /// Получить коэффициент ошибки
        /// </summary>
        public double? GetErrorCoef()
        {
            return EstimatedTime.HasValue && SpentTime.HasValue
                ? (double)EstimatedTime / SpentTime
                : null;
        }

        /// <summary>
        /// Получить ошибку в часах
        /// </summary>
        public int? GetErrorHours()
        {
            return Math.Abs((EstimatedTime - SpentTime).Value);
        }
    }
}
