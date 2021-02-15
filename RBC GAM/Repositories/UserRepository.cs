using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using RBC_GAM.Data;
using RBC_GAM.Model;
using RBC_GAM.ModelDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RBC_GAM.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly FinInstContext _dbContext;
        private readonly IMapper _mapper;

        public UserRepository(FinInstContext dbContext,
                              IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<bool> AddAsync(User user)
        {
            var dbUser = _dbContext.User.SingleOrDefault(x => x.Id == user.Id);
            if (dbUser == null)
            {
                await _dbContext.AddAsync(user);
                var result = await _dbContext.SaveChangesAsync();
                return result > 0;
            }

            return false;
        }

        public async Task<UserDTO> GetAsync(int id)
        {
            var user = await _dbContext.User
                .Include(x => x.FinancialInstrumentUsers)
                .Include(x => x.Thresholds)
                    .ThenInclude(x => x.Triggers)
                .SingleOrDefaultAsync(x => x.Id == id);

            return _mapper.Map<UserDTO>(user);
        }

        public async Task<List<UserDTO>> GetAsync()
        {
            return await _dbContext.User
                .ProjectTo<UserDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }
    }
}
