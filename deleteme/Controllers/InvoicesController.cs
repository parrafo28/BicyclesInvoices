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
    public class InvoicesController : Controller
    {
        // _context es la instancia del DataContext que nos permite acceder a la base de datos
        // Se inyecta mediante dependency injection en el constructor
        private readonly DataContext _context;

        // Constructor que recibe el contexto de base de datos por inyección de dependencias
        // Esto es un patrón estándar en ASP.NET Core para manejar la conexión a la base de datos
        public InvoicesController(DataContext context)
        {
            _context = context;
        }

        // GET: Invoices
        // Acción para mostrar la lista de todas las facturas
        // Include() carga las relaciones para mostrar nombres en vez de solo IDs
        public async Task<IActionResult> Index()
        {
            // Obtenemos todas las facturas de la base de datos
            // Include() hace un JOIN para traer los datos relacionados y evitar el problema N+1
            var invoices = await _context.Invoices
                .Include(i => i.Client)     // Incluye los datos del cliente
                .Include(i => i.Employee)    // Incluye los datos del empleado
                .Include(i => i.Bicycle)     // Incluye los datos de la bicicleta
                .ToListAsync();              // Ejecuta la consulta de forma asíncrona
            
            return View(invoices);
        }

        // GET: Invoices/Details/5
        // Muestra los detalles completos de una factura específica
        public async Task<IActionResult> Details(int? id)
        {
            // Validamos que se haya proporcionado un ID
            if (id == null)
            {
                return NotFound();
            }

            // Buscamos la factura con todas sus relaciones
            var invoice = await _context.Invoices
                .Include(i => i.Client)      // Cliente asociado
                .Include(i => i.Employee)     // Empleado que creó la factura
                .Include(i => i.Bicycle)      // Bicicleta facturada
                .Include(i => i.InvoiceDetails) // Detalles de la factura (líneas de items)
                    .ThenInclude(d => d.ProductOrService) // Para cada detalle, incluir el producto/servicio
                .FirstOrDefaultAsync(m => m.InvoiceId == id); // Busca la primera que coincida con el ID
            
            // Si no se encuentra la factura, retornamos 404
            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        // GET: Invoices/Create
        // Muestra el formulario para crear una nueva factura
        public IActionResult Create()
        {
            // ViewData se usa para pasar datos adicionales a la vista
            // SelectList crea listas desplegables para los formularios
            // El primer parámetro es la fuente de datos
            // El segundo es el campo value (lo que se guarda)
            // El tercero es el campo text (lo que se muestra al usuario)
            
            ViewData["ClientId"] = new SelectList(_context.Clients, "ClientId", "Name");
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "Name");
            ViewData["BicycleId"] = new SelectList(_context.Bicycles, "BicycleId", "Model");
            
            return View();
        }

        // POST: Invoices/Create
        // Procesa el formulario cuando el usuario envía una nueva factura
        [HttpPost] // Indica que solo responde a peticiones POST
        [ValidateAntiForgeryToken] // Protección contra ataques CSRF
        public async Task<IActionResult> Create(Invoice invoice)
        {
            // ModelState.IsValid verifica que todos los campos requeridos estén presentes
            // y que cumplan con las validaciones definidas en el modelo
            if (ModelState.IsValid)
            {
                // Si no se especificó fecha, usar la fecha actual
                if (invoice.Date == default(DateTime))
                {
                    invoice.Date = DateTime.Now;
                }
                
                // Calculamos los totales basados en el subtotal
                // Esta es una lógica simple, puedes ajustar los porcentajes según necesites
                if (invoice.Subtotal > 0)
                {
                    // Calculamos impuestos como 16% del subtotal (IVA por ejemplo)
                    invoice.TotalTaxes = invoice.Subtotal * 0.16m;
                    // El total es subtotal más impuestos
                    invoice.TotalAmount = invoice.Subtotal + invoice.TotalTaxes;
                }
                
                // Agregamos la nueva factura al contexto
                _context.Add(invoice);
                // Guardamos los cambios en la base de datos
                await _context.SaveChangesAsync();
                // Redirigimos al índice después de guardar exitosamente
                return RedirectToAction(nameof(Index));
            }
            
            // Si el modelo no es válido, volvemos a mostrar el formulario con los datos ingresados
            // y recargamos las listas desplegables
            ViewData["ClientId"] = new SelectList(_context.Clients, "ClientId", "Name", invoice.ClientId);
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "Name", invoice.EmployeeId);
            ViewData["BicycleId"] = new SelectList(_context.Bicycles, "BicycleId", "Model", invoice.BicycleId);
            
            return View(invoice);
        }

        // GET: Invoices/Edit/5
        // Muestra el formulario para editar una factura existente
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // FindAsync es más eficiente que FirstOrDefault cuando buscas por ID principal
            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice == null)
            {
                return NotFound();
            }
            
            // Preparamos las listas desplegables con el valor actual seleccionado
            ViewData["ClientId"] = new SelectList(_context.Clients, "ClientId", "Name", invoice.ClientId);
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "Name", invoice.EmployeeId);
            ViewData["BicycleId"] = new SelectList(_context.Bicycles, "BicycleId", "Model", invoice.BicycleId);
            
            return View(invoice);
        }

        // POST: Invoices/Edit/5
        // Procesa los cambios cuando el usuario actualiza una factura
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Invoice invoice)
        {
            // Verificamos que el ID de la URL coincida con el ID del objeto
            // Esto previene manipulación maliciosa
            if (id != invoice.InvoiceId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Recalculamos los totales si el subtotal cambió
                    if (invoice.Subtotal > 0)
                    {
                        invoice.TotalTaxes = invoice.Subtotal * 0.16m;
                        invoice.TotalAmount = invoice.Subtotal + invoice.TotalTaxes;
                    }
                    
                    // Update marca la entidad como modificada
                    _context.Update(invoice);
                    // SaveChangesAsync ejecuta el UPDATE en la base de datos
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Esta excepción ocurre cuando dos usuarios intentan editar el mismo registro simultáneamente
                    if (!InvoiceExists(invoice.InvoiceId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        // Si existe pero hay un conflicto, relanzamos la excepción
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            
            // Si hay errores de validación, mostramos el formulario nuevamente
            ViewData["ClientId"] = new SelectList(_context.Clients, "ClientId", "Name", invoice.ClientId);
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "Name", invoice.EmployeeId);
            ViewData["BicycleId"] = new SelectList(_context.Bicycles, "BicycleId", "Model", invoice.BicycleId);
            
            return View(invoice);
        }

        // GET: Invoices/Delete/5
        // Muestra la página de confirmación antes de eliminar una factura
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Cargamos la factura con sus relaciones para mostrar información completa
            var invoice = await _context.Invoices
                .Include(i => i.Client)
                .Include(i => i.Employee)
                .Include(i => i.Bicycle)
                .FirstOrDefaultAsync(m => m.InvoiceId == id);
                
            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        // POST: Invoices/Delete/5
        // Elimina definitivamente la factura después de la confirmación
        [HttpPost, ActionName("Delete")] // ActionName permite que el método se llame DeleteConfirmed pero responda a la acción Delete
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Buscamos la factura a eliminar
            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice != null)
            {
                // Remove marca la entidad para eliminación
                _context.Invoices.Remove(invoice);
            }
            
            // SaveChangesAsync ejecuta el DELETE en la base de datos
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Método auxiliar para verificar si una factura existe
        // Se usa para manejar concurrencia en las actualizaciones
        private bool InvoiceExists(int id)
        {
            // Any() es más eficiente que Count() > 0 cuando solo necesitas saber si existe
            return _context.Invoices.Any(e => e.InvoiceId == id);
        }
    }
}