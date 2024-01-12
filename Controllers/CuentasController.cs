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

        public CuentasController(IRepositorioTiposCuentas repositorioTiposCuentas, IUsersService usersService, IRepositorioCuentas repositorioCuentas )
        {
            this.repositorioTiposCuentas = repositorioTiposCuentas;
            this.usersService = usersService;
            this.repositorioCuentas = repositorioCuentas;
        }

        [HttpGet]
        public async Task<IActionResult> Crear()
        {
            var usuarioId = usersService.GetUserid();
            var model = new CuentaCreateDTO();
            model.TiposCuentas = await ObtenerTiposCuentas(usuarioId);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Crear(CuentaCreateDTO cuenta)
        {
            var usuarioId = usersService.GetUserid();
            var tipoCuneta = await repositorioTiposCuentas.GetById(cuenta.TipoCuentaId, usuarioId);
            
            if( tipoCuneta is null )
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            if(!ModelState.IsValid)
            {
                cuenta.TiposCuentas = await ObtenerTiposCuentas(usuarioId);
                return View(cuenta);
            }

            await repositorioCuentas.Crear(cuenta);
            return RedirectToAction("Index");
        }


        private async Task<IEnumerable<SelectListItem>> ObtenerTiposCuentas(int usuarioId)
        {
            var tiposCuentas = await repositorioTiposCuentas.Get(usuarioId);
            return tiposCuentas.Select(x => new SelectListItem(x.Nombre, x.Id.ToString()));

        }
    }
}
