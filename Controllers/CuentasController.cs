using AutoMapper;
using ControlPresupuesto.Models;
using ControlPresupuesto.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ControlPresupuesto.Controllers
{
    public class CuentasController : Controller
    {
        private readonly IRepositorioTiposCuentas repositorioTiposCuentas;
        private readonly IUsersService usersService;
        private readonly IRepositorioCuentas repositorioCuentas;
        private readonly IMapper mapper;
        private readonly IRepositorioTransacciones repositorioTransacciones;
        private readonly IServicioReportes servicioReportes;

        public CuentasController(IRepositorioTiposCuentas repositorioTiposCuentas, IUsersService usersService,
                                 IRepositorioCuentas repositorioCuentas, IMapper mapper,
                                 IRepositorioTransacciones repositorioTransacciones,
                                 IServicioReportes servicioReportes)
        {
            this.repositorioTiposCuentas = repositorioTiposCuentas;
            this.usersService = usersService;
            this.repositorioCuentas = repositorioCuentas;
            this.mapper = mapper;
            this.repositorioTransacciones = repositorioTransacciones;
            this.servicioReportes = servicioReportes;
        }

        public async Task<IActionResult> Detalle(int id, int mes, int año)
        {
            int usuarioId = usersService.GetUserid();
            Cuenta cuenta = await repositorioCuentas.GetById(id, usuarioId);

            if (cuenta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            ViewBag.Cuenta = cuenta.Nombre;

            dynamic modelo = await servicioReportes
                .ObtenerReporteTransaccionesDetalladasPorCuenta(usuarioId, id, mes, año, ViewBag);

            return View(modelo);
        }

        [HttpGet]
        public async Task<IActionResult> Crear()
    {
            int usuarioId = usersService.GetUserid();
             CuentaCreateModelView model = new()
             {
                TiposCuentas = await ObtenerTiposCuentas(usuarioId)
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Crear(CuentaCreateModelView cuenta)
        {
            int usuarioId = usersService.GetUserid();
            TipoCuenta tipoCuenta = await repositorioTiposCuentas.GetById(cuenta.TipoCuentaId, usuarioId);

            if (tipoCuenta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            if (!ModelState.IsValid)
            {
                cuenta.TiposCuentas = await ObtenerTiposCuentas(usuarioId);
                return View(cuenta);
            }

            await repositorioCuentas.Crear(cuenta);
            return RedirectToAction("Index");
        }


        private async Task<IEnumerable<SelectListItem>> ObtenerTiposCuentas(int usuarioId)
        {
            IEnumerable<TipoCuenta> tiposCuentas = await repositorioTiposCuentas.Get(usuarioId);
            return tiposCuentas.Select(x => new SelectListItem(x.Nombre, x.Id.ToString()));

        }

        public async Task<IActionResult> Index()
        {
            int usuarioId = usersService.GetUserid();
            IEnumerable<Cuenta> CuentasConTipos = await repositorioCuentas.Buscar(usuarioId);
            List<IndiceCuentasViewModel> model = CuentasConTipos
                        .GroupBy(x=> x.TipoCuenta).Select(grupo => new IndiceCuentasViewModel
                        {
                            TipoCuenta = grupo.Key,
                            Cuentas = grupo.AsEnumerable()
                        }).ToList();

            return View(model);
        }
        public async Task<IActionResult> Editar (int id)
        {
            int usuarioId = usersService.GetUserid();
            Cuenta cuenta = await repositorioCuentas.GetById(id, usuarioId);
            if (cuenta is null)
            {
                RedirectToAction("NoEncontrado", "Home");
            }

            CuentaCreateModelView model = mapper.Map<CuentaCreateModelView>(cuenta);
          

            model.TiposCuentas = await ObtenerTiposCuentas(usuarioId);
            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> Editar(CuentaCreateModelView cuentaEditar)
        {
            int usuarioId = usersService.GetUserid();
            Cuenta cuenta = await repositorioCuentas.GetById(cuentaEditar.Id, usuarioId);
            if (cuenta is null)
            { 
                RedirectToAction("NoEncontrado", "Home");
            }

            TipoCuenta tipoCuenta = await repositorioTiposCuentas.GetById(cuentaEditar.TipoCuentaId, usuarioId);
       
            if(tipoCuenta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            await repositorioCuentas.Actualizar(cuentaEditar);
            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> Borrar (int id)
        {
            int usuarioId = usersService.GetUserid();
            Cuenta cuenta = await repositorioCuentas.GetById(id, usuarioId);

            if(cuenta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            return View(cuenta);
        }

        [HttpPost]
        public async Task<IActionResult> BorrarCuenta (int id)
        {
            int usuarioId = usersService.GetUserid();
            Cuenta cuenta = await repositorioCuentas.GetById (id, usuarioId);

            if(cuenta is null)
            {
                return RedirectToAction("NoEncontrado",  "Home");

            }

            await repositorioCuentas.Borrar(id);
            return RedirectToAction ("Index");
        }

    }
}
