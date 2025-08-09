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
    public class InvoiceDetailsController : Controller
    {
        // Contexto de base de datos para acceder a las tablas
        private readonly DataContext _context;

        // Constructor con inyección de dependencias
        public InvoiceDetailsController(DataContext context)
        {
            _context = context;
        }

        // GET: InvoiceDetails
        // Lista todos los detalles de facturas con sus relaciones
        public async Task<IActionResult> Index()
        {
            // Include carga las relaciones para mostrar información completa
            var invoiceDetails = await _context.InvoiceDetails
                .Include(i => i.Invoice)           // Incluye la factura relacionada
                .Include(i => i.ProductOrService)  // Incluye el producto/servicio
                .ToListAsync();
                
            return View(invoiceDetails);
        }

        // GET: InvoiceDetails/Details/5
        // Muestra los detalles completos de una línea de factura
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Cargamos el detalle con todas sus relaciones
            var invoiceDetail = await _context.InvoiceDetails
                .Include(i => i.Invoice)
                    .ThenInclude(inv => inv.Client)  // También incluimos el cliente de la factura
                .Include(i => i.ProductOrService)
                .FirstOrDefaultAsync(m => m.InvoiceDetailId == id);
                
            if (invoiceDetail == null)
            {
                return NotFound();
            }

            return View(invoiceDetail);
        }

        // GET: InvoiceDetails/Create
        // Muestra el formulario para agregar un detalle a una factura
        public IActionResult Create()
        {
            // Preparamos las listas desplegables para facturas y productos/servicios
            ViewData["InvoiceId"] = new SelectList(_context.Invoices, "InvoiceId", "InvoiceId");
            ViewData["ProductOrServiceId"] = new SelectList(_context.ProductsOrServices, "ProductOrServiceId", "Name");
            
            return View();
        }

        // POST: InvoiceDetails/Create
        // Procesa la creación de un nuevo detalle de factura
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InvoiceDetail invoiceDetail)
        {
            if (ModelState.IsValid)
            {
                // Obtenemos el producto/servicio para calcular el precio y los impuestos
                var productOrService = await _context.ProductsOrServices.FindAsync(invoiceDetail.ProductOrServiceId);
                
                if (productOrService != null)
                {
                    // Si no se especificó el precio, usar el precio del producto
                    if (invoiceDetail.Price == 0)
                    {
                        invoiceDetail.Price = productOrService.Price;
                    }
                    
                    // Calculamos los impuestos basados en el porcentaje del producto
                    decimal subtotal = invoiceDetail.Price * invoiceDetail.Quantity;
                    invoiceDetail.Taxes = subtotal * (decimal)(productOrService.PercentTax / 100);
                    
                    // Calculamos el precio total (subtotal + impuestos)
                    invoiceDetail.TotalPrice = subtotal + invoiceDetail.Taxes;
                    
                    // Actualizamos el stock si es un producto
                    if (productOrService.ServiceType == "Producto" || productOrService.ServiceType == "Repuesto")
                    {
                        productOrService.Stock -= invoiceDetail.Quantity;
                        _context.Update(productOrService);
                    }
                }
                
                _context.Add(invoiceDetail);
                await _context.SaveChangesAsync();
                
                // Actualizamos los totales de la factura
                await UpdateInvoiceTotals(invoiceDetail.InvoiceId);
                
                return RedirectToAction(nameof(Index));
            }
            
            ViewData["InvoiceId"] = new SelectList(_context.Invoices, "InvoiceId", "InvoiceId", invoiceDetail.InvoiceId);
            ViewData["ProductOrServiceId"] = new SelectList(_context.ProductsOrServices, "ProductOrServiceId", "Name", invoiceDetail.ProductOrServiceId);
            
            return View(invoiceDetail);
        }

        // GET: InvoiceDetails/Edit/5
        // Muestra el formulario para editar un detalle existente
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoiceDetail = await _context.InvoiceDetails.FindAsync(id);
            if (invoiceDetail == null)
            {
                return NotFound();
            }
            
            ViewData["InvoiceId"] = new SelectList(_context.Invoices, "InvoiceId", "InvoiceId", invoiceDetail.InvoiceId);
            ViewData["ProductOrServiceId"] = new SelectList(_context.ProductsOrServices, "ProductOrServiceId", "Name", invoiceDetail.ProductOrServiceId);
            
            return View(invoiceDetail);
        }

        // POST: InvoiceDetails/Edit/5
        // Procesa la actualización de un detalle de factura
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, InvoiceDetail invoiceDetail)
        {
            if (id != invoiceDetail.InvoiceDetailId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Obtenemos el producto para recalcular impuestos
                    var productOrService = await _context.ProductsOrServices.FindAsync(invoiceDetail.ProductOrServiceId);
                    
                    if (productOrService != null)
                    {
                        // Recalculamos los valores
                        decimal subtotal = invoiceDetail.Price * invoiceDetail.Quantity;
                        invoiceDetail.Taxes = subtotal * (decimal)(productOrService.PercentTax / 100);
                        invoiceDetail.TotalPrice = subtotal + invoiceDetail.Taxes;
                    }
                    
                    _context.Update(invoiceDetail);
                    await _context.SaveChangesAsync();
                    
                    // Actualizamos los totales de la factura
                    await UpdateInvoiceTotals(invoiceDetail.InvoiceId);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InvoiceDetailExists(invoiceDetail.InvoiceDetailId))
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
            
            ViewData["InvoiceId"] = new SelectList(_context.Invoices, "InvoiceId", "InvoiceId", invoiceDetail.InvoiceId);
            ViewData["ProductOrServiceId"] = new SelectList(_context.ProductsOrServices, "ProductOrServiceId", "Name", invoiceDetail.ProductOrServiceId);
            
            return View(invoiceDetail);
        }

        // GET: InvoiceDetails/Delete/5
        // Muestra la página de confirmación antes de eliminar
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoiceDetail = await _context.InvoiceDetails
                .Include(i => i.Invoice)
                .Include(i => i.ProductOrService)
                .FirstOrDefaultAsync(m => m.InvoiceDetailId == id);
                
            if (invoiceDetail == null)
            {
                return NotFound();
            }

            return View(invoiceDetail);
        }

        // POST: InvoiceDetails/Delete/5
        // Elimina definitivamente el detalle de factura
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var invoiceDetail = await _context.InvoiceDetails.FindAsync(id);
            if (invoiceDetail != null)
            {
                int invoiceId = invoiceDetail.InvoiceId;
                
                // Devolvemos el stock si era un producto
                var productOrService = await _context.ProductsOrServices.FindAsync(invoiceDetail.ProductOrServiceId);
                if (productOrService != null && (productOrService.ServiceType == "Producto" || productOrService.ServiceType == "Repuesto"))
                {
                    productOrService.Stock += invoiceDetail.Quantity;
                    _context.Update(productOrService);
                }
                
                _context.InvoiceDetails.Remove(invoiceDetail);
                await _context.SaveChangesAsync();
                
                // Actualizamos los totales de la factura
                await UpdateInvoiceTotals(invoiceId);
            }

            return RedirectToAction(nameof(Index));
        }

        // Método auxiliar para verificar si existe un detalle
        private bool InvoiceDetailExists(int id)
        {
            return _context.InvoiceDetails.Any(e => e.InvoiceDetailId == id);
        }
        
        // Método auxiliar para actualizar los totales de una factura
        // Se llama después de agregar, editar o eliminar detalles
        private async Task UpdateInvoiceTotals(int invoiceId)
        {
            var invoice = await _context.Invoices
                .Include(i => i.InvoiceDetails)
                .FirstOrDefaultAsync(i => i.InvoiceId == invoiceId);
                
            if (invoice != null)
            {
                // Recalculamos los totales basados en los detalles
                invoice.Subtotal = invoice.InvoiceDetails.Sum(d => d.Price * d.Quantity);
                invoice.TotalTaxes = invoice.InvoiceDetails.Sum(d => d.Taxes);
                invoice.TotalAmount = invoice.InvoiceDetails.Sum(d => d.TotalPrice);
                
                _context.Update(invoice);
                await _context.SaveChangesAsync();
            }
        }
    }
}