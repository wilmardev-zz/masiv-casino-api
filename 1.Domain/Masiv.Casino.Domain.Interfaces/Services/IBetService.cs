﻿using Masiv.Casino.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Masiv.Casino.Domain.Interfaces.Services
{
    public interface IBetService
    {
        Task<GenericResponse> Create(Bet bet);

        Task<List<Bet>> Get();
    }
}