using AutoMapper;
using ControlPresupuesto.Models;

namespace ControlPresupuesto.Services
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles() 
        {
            CreateMap<Cuenta, CuentaCreateModelView>();
            CreateMap<TransaccionActualizacionModelView, Transaccion>().ReverseMap();
        }
    }
}
