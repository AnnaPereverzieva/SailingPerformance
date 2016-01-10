using System;
using System.Collections.Generic;

namespace Dal.Repositories.Contracts
{
    public interface IBoatRepository:IRepository<Boat>
    {
        Guid GetGuidBoat(string model, string name);
        List<Boat> GetBoats();
    }
}
