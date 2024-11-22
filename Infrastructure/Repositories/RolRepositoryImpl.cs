using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.data;

namespace Infrastructure.Repositories;

public class RolRepositoryImpl : BaseRepositoryImpl<Rol>, IRolRepository {

    public RolRepositoryImpl(TiendaContext context) : base(context) { }

}
