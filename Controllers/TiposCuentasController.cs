using ControlPresupuesto.Models;
using ControlPresupuesto.Services;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data.Common;

namespace ControlPresupuesto.Controllers
{
    public class TiposCuentasController: Controller
    {
        private readonly IRepositorioTiposCuentas repositorioTiposCuentas;
        private readonly IUsersService usersService;

        public TiposCuentasController(IRepositorioTiposCuentas repositorioTiposCuentas, IUsersService usersService)
        {
            this.repositorioTiposCuentas = repositorioTiposCuentas;
            this.usersService = usersService;
        }
        public IActionResult Crear()
        {

            return View();
        }

        [HttpGet]
        public async Task<ActionResult> Editar(int id)
        {
            var usuarioId = usersService.GetUserid();
            var tipoCuenta = await repositorioTiposCuentas.GetById(id, usuarioId);
            
            
            if(tipoCuenta == null)
            { 
                return RedirectToAction( "NoEncontrado","Home");
            }

            return View(tipoCuenta);
        }

        [HttpPost]
        public async Task<ActionResult> Editar(TipoCuenta tipoCuenta)
        {
            var usuarioId = usersService.GetUserid();
            var tipoCuentaExist = await repositorioTiposCuentas.GetById(tipoCuenta.Id, usuarioId);

            if(tipoCuentaExist is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            await repositorioTiposCuentas.Update(tipoCuenta);
            return RedirectToAction("Index");
        }

         [HttpPost]
        public async Task<IActionResult>Crear( TipoCuenta tipoCuenta )
        {
            if(!ModelState.IsValid)
            {
                return View(tipoCuenta);
            }

            tipoCuenta.UsuarioId = usersService.GetUserid();

            var existTipoCuenta =
                await repositorioTiposCuentas.Existe(tipoCuenta.Nombre, tipoCuenta.UsuarioId);
           
            if (existTipoCuenta)
            {
                ModelState.AddModelError(nameof(tipoCuenta.Nombre), $"El nombre {tipoCuenta.Nombre} ya existe.");
                return View(tipoCuenta);

            }

            await repositorioTiposCuentas.Crear( tipoCuenta );

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> VerifyExistTipoCuenta(string nombre)
        {
            var usuarioId = usersService.GetUserid();
            var existTipoCuenta = await repositorioTiposCuentas.Existe(nombre, usuarioId);
            if (existTipoCuenta) 
            {
                return Json($"El nombre {nombre} ya existe");
            }

            return Json(true);
        }

        public async Task<IActionResult> Index()
        {
            var usuarioid = usersService.GetUserid();
            var tiposCuentas = await repositorioTiposCuentas.Get(usuarioid);
            return View(tiposCuentas);
        }

        public async Task <IActionResult> Borrar(int id)
        {
            var usuarioId = usersService.GetUserid();
            var tipoCuenta = await repositorioTiposCuentas.GetById(id, usuarioId);
            if(tipoCuenta == null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            return View(tipoCuenta);
        }

        [HttpPost]
        public async Task<IActionResult> BorrarTipoCuenta(int id)
        {
            var usuarioId = usersService.GetUserid();
            var tipoCuenta = await repositorioTiposCuentas.GetById(id, usuarioId);

            if (tipoCuenta == null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            await repositorioTiposCuentas.Borrar(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Ordenar([FromBody] int[] ids)
        {
            var usuarioId = usersService.GetUserid();
            var tiposCuentas = await repositorioTiposCuentas.Get(usuarioId);
            var idsTiposCuentas = tiposCuentas.Select( x => x.Id);

            var idsTiposCuentasNoPertenecen = ids.Except(idsTiposCuentas).ToList();
            
            if(idsTiposCuentasNoPertenecen.Count > 0)
            {
                return Forbid();
            }

            var tiposCuentasOrdenados = ids.Select((valor, indice) => new TipoCuenta() { Id = valor, Orden = indice + 1 }).AsEnumerable();
            await repositorioTiposCuentas.Ordenar(tiposCuentasOrdenados);
            
            return Ok();
        }

    }
}
