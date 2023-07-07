using Entities.Concrete;
using static StorageApplication.Controllers.HomeController;

namespace StorageApplication.Helpers
{
    public static class CoordinateHelper
    {
        public static double CalculateDistance(Coordinate coord1, Coordinate coord2)
        {
            const double earthRadius = 6371; // Dünya yarıçapı (km)

            var dLat = DegreesToRadians(coord2.Latitude - coord1.Latitude);
            var dLon = DegreesToRadians(coord2.Longitude - coord1.Longitude);

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(DegreesToRadians(coord1.Latitude)) * Math.Cos(DegreesToRadians(coord2.Latitude)) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var distance = earthRadius * c;

            return distance;
        }

        private static double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180;
        }
      

    }
}
