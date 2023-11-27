﻿using System.ComponentModel.DataAnnotations;

namespace Companies.API.Dtos.CompaniesDtos
{
    public record CompanyDto()
    {
        public Guid Id { get; init; }
        public string? Name { get; init; }
        public string? Address { get; init; } 
        public string? Country { get; init; } 
    }
}
