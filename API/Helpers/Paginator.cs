using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Helpers;

public class Paginator<T> {

    public int PageIndex { get; set; } // se obtiene de la URL, ejemplo: /productos?pageIndex=1
    public int PageSize { get; set; } // Cantidad de elementos por pagina, ejemplo : 10, 20, 30
    public int TotalCount { get; set; } // Cantidad total de elementos, se obtiene de la base de datos
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize); // Cantidad total de paginas, se obtiene calculando TotalCount / PageSize
    public bool HasPreviousPage => PageIndex > 1; // Si la pagina actual es mayor a 1, entonces hay una pagina anterior
    public bool HasNextPage => PageIndex < TotalPages; // Si la pagina actual es menor a la cantidad total de paginas, entonces hay una pagina siguiente

    public List<T> Items { get; set; } // Lista de elementos que se van a mostrar en la pagina, ejemplo: List<Producto> 

    public Paginator(int pageIndex, int pageSize, int count, List<T> items) {
        PageIndex = pageIndex;
        PageSize = pageSize;
        TotalCount = count;
        Items = items;
    }

}
