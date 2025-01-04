using System;
using System.Collections.Generic;
using System.Text;

namespace ZSN.Utils.Core.Utils
{
    public class GeoUtils
    {
        private const double EarthRadiusKm = 6371.0;

        /// <summary>
        /// 计算每个点与输入点的距离,返回单位千米
        /// </summary>
        /// <param name="lat1"></param>
        /// <param name="lon1"></param>
        /// <param name="lat2"></param>
        /// <param name="lon2"></param>
        /// <returns></returns>
        public static double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            double dLat = ToRadians(lat2 - lat1);
            double dLon = ToRadians(lon2 - lon1);

            lat1 = ToRadians(lat1);
            lat2 = ToRadians(lat2);

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                        Math.Cos(lat1) * Math.Cos(lat2) *
                        Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return EarthRadiusKm * c;
        }

        private static double ToRadians(double angle)
        {
            return Math.PI * angle / 180.0;
        }
    }
}
