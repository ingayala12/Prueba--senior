using AutoMapper;
using PruebaCliente.Modelos;
using PruebaCliente.Modelos.DTOs;

namespace PruebaCliente.ClienteMapper
{
    public class ClienteMapping : Profile
    {
        public ClienteMapping()
        {
            CreateMap<Cliente, ClienteDTOs>().ReverseMap();
            CreateMap<Cliente, ClienteCrearDTOs>().ReverseMap();
        }
    }
}
