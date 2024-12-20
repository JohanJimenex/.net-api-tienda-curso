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

        CreateMap<ProductoDTO, Producto>().ReverseMap().ForAllMembers(options => options.Condition((origen, destino, propiedad) => propiedad != null));
        CreateMap<CategoriaDTO, Categoria>().ReverseMap();
        CreateMap<MarcaDTO, Marca>().ReverseMap();

        //OJO esto no es necesario si las propeidades se llaman iguales y/o contiene las mismas propiedades
        //Esto es para hacer el mapeo manualmente los campos que no se llaman igual o campos a ignorar si un campo no existe ne uno de los modelos
        CreateMap<Producto, ProductoListDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre))
            .ForMember(dest => dest.Precio, opt => opt.MapFrom(src => src.Precio))
            .ForMember(dest => dest.FechaCreacion, opt => opt.MapFrom(src => src.FechaCreacion))
            // .ForMember(dest => dest.Marca, opt => opt.MapFrom(src => src.Marca.Nombre)) //no estoy usando marca en mi dto
            // .ForMember(dest => dest.Categoria, opt => opt.MapFrom(src => src.Categoria.Nombre)) //no estoy usando categoria en mi dto
            .ReverseMap()
            .ForMember(dest => dest.Categoria, opt => opt.Ignore())
            .ForMember(dest => dest.Marca, opt => opt.Ignore());
        //cuando se aun mapeo desde ProductoListDTO a Producto, se ignora la categoria y la marca

        CreateMap<RegisterDTO, Usuario>()
        .ForMember(dest => dest.Roles, opt => opt.Ignore())
        .ReverseMap();

        CreateMap<Usuario, DatosUsuarioDTO>()
        .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.Roles.Select(r => r.Nombre).ToList()));
    }
}
