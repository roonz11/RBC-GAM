using RBC_GAM.ModelDTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RBC_GAM.Services
{
    public interface INotificationService
    {
        void DisplayNotification(UserNotification user);
        Task NotifyUsers(double prevPrice, int finIstId);
        Task<List<UserNotification>> UsersBuyAbove(double prevPrice, int finIstId);
        Task<List<UserNotification>> UsersBuyBelow(double prevPrice, int finIstId);
        Task<List<UserNotification>> UsersSellAbove(double prevPrice, int finIstId);
        Task<List<UserNotification>> UsersSellBelow(double prevPrice, int finIstId);
    }
}