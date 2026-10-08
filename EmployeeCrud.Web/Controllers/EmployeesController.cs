using EmployeeCrud.Web.Data;
using EmployeeCrud.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeCrud.Web.Controllers;

public class EmployeesController : Controller
{
    private readonly AppDbContext _context;

    public EmployeesController(AppDbContext context)
    {
        _context = context;
    }

    // READ - list (with search)
    public async Task<IActionResult> Index(string? search)
    {
        var query = _context.Employees.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(e => e.EmployeeName.Contains(search)
                                  || e.EmployeeDepartment.Contains(search)
                                  || e.EmployeeEmail.Contains(search));
        }

        ViewData["Search"] = search;
        return View(await query.OrderByDescending(e => e.Id).ToListAsync());
    }

    // READ - single
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);
        if (employee == null) return NotFound();

        return View(employee);
    }

    // CREATE - GET
    public IActionResult Create()
    {
        return View(new Employee { CheckInTime = DateTime.Now });
    }

    // CREATE - POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Employee employee)
    {
        if (!ModelState.IsValid) return View(employee);

        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();
        TempData["Success"] = "Employee add ho gaya.";
        return RedirectToAction(nameof(Index));
    }

    // UPDATE - GET
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var employee = await _context.Employees.FindAsync(id);
        if (employee == null) return NotFound();

        return View(employee);
    }

    // UPDATE - POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Employee employee)
    {
        if (id != employee.Id) return NotFound();
        if (!ModelState.IsValid) return View(employee);

        try
        {
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.Employees.AnyAsync(e => e.Id == id)) return NotFound();
            throw;
        }

        TempData["Success"] = "Employee update ho gaya.";
        return RedirectToAction(nameof(Index));
    }

    // DELETE - GET (confirmation page)
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);
        if (employee == null) return NotFound();

        return View(employee);
    }

    // DELETE - POST
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var employee = await _context.Employees.FindAsync(id);
        if (employee != null)
        {
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Employee delete ho gaya.";
        }
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Error() => Content("Kuch ghalat ho gaya.");
}
