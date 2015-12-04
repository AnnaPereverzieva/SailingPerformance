namespace Dal.Repositories.Contracts
{
    public interface IBoatRepository:IRepository<Boat>
    {
        bool CheckBoat(string model, string name);
    }
}
