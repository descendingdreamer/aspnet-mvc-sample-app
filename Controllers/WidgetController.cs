using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebForm.Data;
using WebForm.Models;

namespace WebForm.Controllers;

public class WidgetController : Controller
{
    private readonly WidgetContext _db;

    public WidgetController(WidgetContext context)
    {
        _db = context;
    }

    [ActionName("Index")]
    public async Task<IActionResult> Index()
    {
        return View(await _db.Widgets!.ToListAsync());
    }

    [ActionName("Create")]
    public IActionResult Create()
    {
        var widget = new Widget
        {
            Types = GetTypes()
        };
        return View(widget);
    }

    [HttpPost]
    [ActionName("Create")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> CreateAsync([Bind("Id,Name,Created,Type,SubType")] Widget widget)
    {
        try
        {
            _db.Widgets?.Add(widget);
            await _db.SaveChangesAsync();
            const string message = "Created";
            ViewBag.Message = message;
            return View("Success");
        } catch {
            return View(widget);
        }
    }

    [HttpPost]
    [ActionName("Edit")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> EditAsync([Bind("Id,Name,Created,Type,SubType")] Widget widget)
    {
        try
        {
            _db.Update(widget);
            await _db.SaveChangesAsync();
            const string message = "Edited";
            ViewBag.Message = message;
            return View("Success");
        } catch {
            return View(widget);
        }
    }

    [ActionName("Edit")]
    public async Task<ActionResult> EditAsync(int id)
    {
        var widget = await _db.Widgets!.SingleAsync(w => w.Id == id);
        widget.Types = GetTypes();
        widget.SubTypes = await GetSubTypes(widget.Type);
        return View(widget);
    }

    [ActionName("Delete")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        var widget = await _db.Widgets!.SingleAsync(w => w.Id == id);
        return View(widget);
    }

    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteConfirmedAsync(int id)
    {
        var widget = new Widget() { Id = id };
        _db.Widgets?.Attach(widget);
        _db.Widgets?.Remove(widget);
        await _db.SaveChangesAsync();
        const string message = "Deleted";
        ViewBag.Message = message;
        return View("Success");
    }
    
    public IEnumerable<SelectListItem> GetTypes()
    {
        return _db.Types!.Select(m => new SelectListItem() { Value = m.Type, Text = m.Type });
    }

    public async Task<List<SelectListItem>> GetSubTypes(string? type)
    {
        var subtypes = new List<SelectListItem>();
        var widgetType = await _db.Types!.SingleAsync(wt => wt.Type == type);
        var subtypeList = widgetType.SubTypes!.Split(", ");
        subtypes.AddRange(subtypeList.Select(str => new SelectListItem() { Text = str, Value = str }));
        return subtypes;
    }

    [HttpGet]
    [ActionName("GetSubTypes")]
    public async Task<JsonResult> GetSubTypesDdl(string? type)
    {
        var subtypes = new List<SelectListItem>();
        if (string.IsNullOrEmpty(type)) return Json(subtypes);
        var widgetType = await _db.Types!.SingleAsync(wt => wt.Type == type);
        var subtypeList = widgetType.SubTypes!.Split(", ");
        subtypes.AddRange(subtypeList.Select(str => new SelectListItem() { Text = str, Value = str }));
        return Json(subtypes);
    }
}