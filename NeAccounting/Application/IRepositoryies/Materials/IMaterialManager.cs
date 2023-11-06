﻿using Domain.NovinEntity.Materials;
using DomainShared.Pun;
using DomainShared.ViewModels;
using NeApplication.Common;

namespace NeApplication.IRepositoryies.Materials
{
    public interface IMaterialManager : IRepository<Material>
    {
        Task<List<SuggestBoxViewModel<int>>> GetMaterails();
        Task<List<PunListDto>> GetMaterails(string name , string serial);
    }
}
