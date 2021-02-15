using RBC_GAM.Model;
using RBC_GAM.ModelDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RBC_GAM.Repositories
{
    public interface IUserRepository
    {
        Task<int> AddAsync(User user);

        Task<UserDTO> GetAsync(int id);
        Task<List<UserDTO>> GetAsync();        

    }
}
