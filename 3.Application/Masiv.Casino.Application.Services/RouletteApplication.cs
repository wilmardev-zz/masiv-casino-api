using Masiv.Casino.Application.Interfaces;
using Masiv.Casino.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Masiv.Casino.Application.Services
{
    public class RouletteApplication : IRouletteApplication
    {
        public Task<List<GenericResponse>> Close(Roulette roulette)
        {
            throw new NotImplementedException();
        }

        public Task<GenericResponse> Create()
        {
            throw new NotImplementedException();
        }

        public Task<List<GenericResponse>> Get()
        {
            throw new NotImplementedException();
        }

        public Task<GenericResponse> Open(Roulette roulette)
        {
            throw new NotImplementedException();
        }
    }
}