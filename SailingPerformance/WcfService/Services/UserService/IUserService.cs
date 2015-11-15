using System.ServiceModel;

namespace WcfService.Services.UserService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IUserService" in both code and config file together.
    [ServiceContract]
    public interface IUserService
    {
        [OperationContract]
        bool CheckUser(string login, string password);
    }
}
