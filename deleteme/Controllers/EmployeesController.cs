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
    public class EmployeesController : Controller
    {
        // Contexto de base de datos inyectado por dependency injection
        private readonly DataContext _context;

        // Constructor que recibe el contexto de base de datos
        public EmployeesController(DataContext context)
        {
            _context = context;
        }

        // GET: Employees
        // Lista todos los empleados registrados en el sistema
        public async Task<IActionResult> Index()
        {
            // ToListAsync ejecuta la consulta de forma asíncrona y devuelve todos los empleados
            return View(await _context.Employees.ToListAsync());
        }

        // GET: Employees/Details/5
        // Muestra los detalles completos de un empleado específico
        public async Task<IActionResult> Details(int? id)
        {
            // Validamos que se proporcione un ID
            if (id == null)
            {
                return NotFound();
            }

            // Buscamos el empleado por su ID
            var employee = await _context.Employees
                .FirstOrDefaultAsync(m => m.EmployeeId == id);
            
            // Si no existe, retornamos 404
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
        // Muestra el formulario para registrar un nuevo empleado
        public IActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        // Procesa el registro de un nuevo empleado
        [HttpPost]
        [ValidateAntiForgeryToken] // Protección contra ataques CSRF
        public async Task<IActionResult> Create(Employee employee)
        {
            // Verificamos que todos los campos requeridos estén completos y sean válidos
            if (ModelState.IsValid)
            {
                // Agregamos el empleado al contexto
                _context.Add(employee);
                // Guardamos en la base de datos
                await _context.SaveChangesAsync();
                // Redirigimos al listado
                return RedirectToAction(nameof(Index));
            }
            // Si hay errores, mostramos el formulario nuevamente con los datos ingresados
            return View(employee);
        }

        // GET: Employees/Edit/5
        // Muestra el formulario para editar un empleado existente
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // FindAsync busca por la clave primaria
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // POST: Employees/Edit/5
        // Procesa la actualización de los datos del empleado
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Employee employee)
        {
            // Verificamos que el ID coincida para evitar manipulación
            if (id != employee.EmployeeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Marcamos la entidad como modificada
                    _context.Update(employee);
                    // Guardamos los cambios
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Manejo de concurrencia cuando dos usuarios editan simultáneamente
                    if (!EmployeeExists(employee.EmployeeId))
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
            return View(employee);
        }

        // GET: Employees/Delete/5
        // Muestra la página de confirmación antes de eliminar
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .FirstOrDefaultAsync(m => m.EmployeeId == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        // Elimina definitivamente el empleado
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Método auxiliar para verificar si existe un empleado
        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.EmployeeId == id);
        }
    }
}