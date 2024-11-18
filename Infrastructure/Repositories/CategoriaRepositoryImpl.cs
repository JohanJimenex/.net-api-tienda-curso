using Core.Entities;
using Core.Interfaces;
using Infrastructure.data;

namespace Infrastructure.Repositories;



public class CategoriaRepositoryImpl : BaseRepository<Categoria>, ICategoriaRepository {

    public CategoriaRepositoryImpl(TiendaContext context) : base(context) { }
}
