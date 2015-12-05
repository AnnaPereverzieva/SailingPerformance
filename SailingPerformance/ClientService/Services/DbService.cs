using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dal;

namespace ClientService.Services
{
   public static class GuidExtensions
    {
        static int[] byteOrder = { 15, 14, 13, 12, 11, 10, 9, 8, 6, 7, 4, 5, 0, 1, 2, 3 };
        public  static Guid LastGuid=new Guid("00000000 - 0000 - 0000 - 0000 - 000000000001"); 
        public static Guid Increment(Guid guid)
        {
            var bytes = guid.ToByteArray();
            var canIncrement = byteOrder.Any(i => ++bytes[i] != 0);
            return new Guid(canIncrement ? bytes : new byte[16]);
        }
    }

    public class DbService
    {
        public DbService()
        {
            //using (var db = new SailingContext())
            //{
            //    var boat = new Boat
            //    {
            //        Model ="RSX45", Name = "Titanik1", IdBoat = GuidExtensions.Increment(new Guid("00000000-0000-0000-0000-000000000001"))

            //    };
            //    db.Boats.Add(boat);
            //    db.SaveChanges();
            //}
        }
      
    }
}
