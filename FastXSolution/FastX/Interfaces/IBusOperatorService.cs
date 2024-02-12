﻿using FastX.Models.DTOs;

namespace FastX.Interfaces
{
    public interface IBusOperatorService
    {
        public Task<List<BusDTOForOperator>> GetAllBuses(int busOperatorId);

    }
}
