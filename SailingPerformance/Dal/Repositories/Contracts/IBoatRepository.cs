using System;

namespace Dal.Repositories.Contracts
{
    public interface IBoatRepository:IRepository<Boat>
    {
        Guid GetGuidBoat(string model, string name);
        Guid GetGuidLastBoat();
    }
}
