using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using AutoMapper;
using Core.Entities;

namespace API.AutoMappers;

public class MapperProfile : Profile {

    public MapperProfile() {
        CreateMap<ProductoDTO, Producto>().ReverseMap().ForAllMembers(options => options.Condition((origen, destino, propiedad) => propiedad != null)); ;
        CreateMap<CategoriaDTO, Categoria>().ReverseMap();
        CreateMap<MarcaDTO, Marca>().ReverseMap();

        //Con este mapeo sirve para mapear manualmente los campos que no se llaman igual o ignorar
        CreateMap<Producto, ProductoListDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre))
            .ForMember(dest => dest.Precio, opt => opt.MapFrom(src => src.Precio))
            .ForMember(dest => dest.FechaCreacion, opt => opt.MapFrom(src => src.FechaCreacion))
            // .ForMember(dest => dest.Marca, opt => opt.MapFrom(src => src.Marca.Nombre)) //no estoy usando marca
            // .ForMember(dest => dest.Categoria, opt => opt.MapFrom(src => src.Categoria.Nombre)) //no estoy usando categoria
            .ReverseMap()
            .ForMember(dest => dest.Categoria, opt => opt.Ignore())
            .ForMember(dest => dest.Marca, opt => opt.Ignore());
            //cuando se aun mapeo desde ProductoListDTO a Producto, se ignora la categoria y la marca
    }
}
