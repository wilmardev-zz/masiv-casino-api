using Masiv.Casino.Application.Interfaces;
using Masiv.Casino.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Masiv.Casino.Application.Services
{
    public class BetApplication : IBetApplication
    {
        public Task<GenericResponse> Create(Bet bet)
        {
            throw new NotImplementedException();
        }
    }
}