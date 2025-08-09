using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using deleteme.Models.Entities;

namespace deleteme.Controllers
{
    public class ProductsOrServicesController : Controller
    {
        // Contexto de base de datos para acceder a las tablas
        private readonly DataContext _context;

        // Constructor con inyección de dependencias
        public ProductsOrServicesController(DataContext context)
        {
            _context = context;
        }

        // GET: ProductsOrServices
        // Lista todos los productos y servicios disponibles
        public async Task<IActionResult> Index()
        {
            // Obtenemos todos los productos/servicios de la base de datos
            return View(await _context.ProductsOrServices.ToListAsync());
        }

        // GET: ProductsOrServices/Details/5
        // Muestra los detalles de un producto o servicio específico
        public async Task<IActionResult> Details(int? id)
        {
            // Validamos que se proporcione un ID
            if (id == null)
            {
                return NotFound();
            }

            // Buscamos el producto/servicio por ID
            var productOrService = await _context.ProductsOrServices
                .FirstOrDefaultAsync(m => m.ProductOrServiceId == id);
            
            // Si no existe, retornamos 404
            if (productOrService == null)
            {
                return NotFound();
            }

            return View(productOrService);
        }

        // GET: ProductsOrServices/Create
        // Muestra el formulario para crear un nuevo producto o servicio
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProductsOrServices/Create
        // Procesa el registro de un nuevo producto o servicio
        [HttpPost]
        [ValidateAntiForgeryToken] // Protección CSRF
        public async Task<IActionResult> Create(ProductOrService productOrService)
        {
            // Verificamos que el modelo sea válido
            if (ModelState.IsValid)
            {
                // Si no se especifica el porcentaje de impuesto, usar 16% por defecto
                if (productOrService.PercentTax == 0)
                {
                    productOrService.PercentTax = 16;
                }
                
                // Agregamos el producto/servicio al contexto
                _context.Add(productOrService);
                // Guardamos en la base de datos
                await _context.SaveChangesAsync();
                // Redirigimos al listado
                return RedirectToAction(nameof(Index));
            }
            // Si hay errores, mostramos el formulario con los datos ingresados
            return View(productOrService);
        }

        // GET: ProductsOrServices/Edit/5
        // Muestra el formulario para editar un producto o servicio existente
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Buscamos el producto/servicio por su clave primaria
            var productOrService = await _context.ProductsOrServices.FindAsync(id);
            if (productOrService == null)
            {
                return NotFound();
            }
            return View(productOrService);
        }

        // POST: ProductsOrServices/Edit/5
        // Procesa la actualización de un producto o servicio
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductOrService productOrService)
        {
            // Verificamos que el ID coincida
            if (id != productOrService.ProductOrServiceId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Marcamos la entidad como modificada
                    _context.Update(productOrService);
                    // Guardamos los cambios
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Manejo de concurrencia
                    if (!ProductOrServiceExists(productOrService.ProductOrServiceId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(productOrService);
        }

        // GET: ProductsOrServices/Delete/5
        // Muestra la página de confirmación antes de eliminar
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productOrService = await _context.ProductsOrServices
                .FirstOrDefaultAsync(m => m.ProductOrServiceId == id);
            if (productOrService == null)
            {
                return NotFound();
            }

            return View(productOrService);
        }

        // POST: ProductsOrServices/Delete/5
        // Elimina definitivamente el producto o servicio
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productOrService = await _context.ProductsOrServices.FindAsync(id);
            if (productOrService != null)
            {
                _context.ProductsOrServices.Remove(productOrService);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Método auxiliar para verificar si existe un producto/servicio
        private bool ProductOrServiceExists(int id)
        {
            return _context.ProductsOrServices.Any(e => e.ProductOrServiceId == id);
        }
    }
}