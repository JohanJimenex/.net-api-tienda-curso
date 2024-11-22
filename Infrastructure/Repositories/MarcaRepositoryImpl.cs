using Core.Entities;
using Core.Interfaces;
using Infrastructure.data;

namespace Infrastructure.Repositories;



public class MarcaRepositoryImpl : BaseRepositoryImpl<Marca>, IMarcaRepository {

    public MarcaRepositoryImpl(TiendaContext context) : base(context) { }
}