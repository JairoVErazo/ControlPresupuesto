using ControlPresupuesto.Models;
using ControlPresupuesto.Services;
using Microsoft.AspNetCore.Mvc;

namespace ControlPresupuesto.Controllers
{
    public class CategoriasController : Controller
    {
        private readonly IRepositorioCategorias repositorioCategorias;
        private readonly IUsersService usersService;

        public CategoriasController( IRepositorioCategorias repositorioCategorias, IUsersService usersService)
        {
            this.repositorioCategorias = repositorioCategorias;
            this.usersService = usersService;
        }

        [HttpPost]
        public async Task<IActionResult> Crear(Categoria categoria) 
        {
            if (!ModelState.IsValid)
            {
                return View(categoria);
            }
            int usuarioId = usersService.GetUserid();
            categoria.UsuarioId = usuarioId;
            await repositorioCategorias.Crear(categoria);
            return RedirectToAction("Index");

        }

        public async Task<IActionResult> Index()
        {
            int usuarioId = usersService.GetUserid();
            IEnumerable<Categoria> categorias = await repositorioCategorias.Obtener(usuarioId);
            return View(categorias);
        }

        public IActionResult Crear()
        {
            return View();
        }

        public async Task<IActionResult> Editar(int id)
        {
            int usuarioId = usersService.GetUserid();
            Categoria categoria = await repositorioCategorias.GeTById(id, usuarioId);

            if (categoria is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            return View(categoria);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(Categoria categoriaEditar)
        {
            if (!ModelState.IsValid)
            {
                return View(categoriaEditar);
            }

            int usuarioId = usersService.GetUserid();
            Categoria categoria = await repositorioCategorias.GeTById(categoriaEditar.Id, usuarioId);

            if (categoria is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            categoriaEditar.UsuarioId = usuarioId;
            await repositorioCategorias.Actualizar(categoriaEditar);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Borrar(int id)
        {
            int usuarioId = usersService.GetUserid();
            Categoria categoria = await repositorioCategorias.GeTById(id, usuarioId);

            if (categoria is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            return View(categoria);
        }

        [HttpPost]
        public async Task<IActionResult> BorrarCategoria(int id)
        {
            int usuarioId = usersService.GetUserid();
            Categoria categoria = await repositorioCategorias.GeTById(id, usuarioId);

            if (categoria is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            await repositorioCategorias.Borrar(id);
            return RedirectToAction("Index");
        }

    }
}
