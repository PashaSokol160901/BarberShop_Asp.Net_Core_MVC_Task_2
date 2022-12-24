using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Barbershop.Data;
using Barbershop.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Barbershop.Models.Task;

namespace Barbershop.Controllers
{
	[Authorize]
	public class RecordsController : Controller
	{
		private readonly BarbershopContext _context;

		public RecordsController(BarbershopContext context)
		{
			_context = context;
		}

		public async Task<IActionResult> Index(string name, int barber = 0, int page = 1,
			SortState sortOrder = SortState.NameAsc)
		{
			int pageSize = 4;

			//фильтрация
			IQueryable<Client> clients = _context.Client.Include(x => x.Barber);

			if (barber != 0)
			{
				clients = clients.Where(p => p.BarberId == barber);
			}
			if (!string.IsNullOrEmpty(name))
			{
				clients = clients.Where(p => p.Name == name);
			}

			// сортировка
			switch (sortOrder)
			{
				case SortState.NameDesc:
					clients = clients.OrderByDescending(s => s.Name);
					break;
				case SortState.PriceAsc:
					clients = clients.OrderBy(s => s.Price);
					break;
				case SortState.PriceDesc:
					clients = clients.OrderByDescending(s => s.Price);
					break;
				case SortState.BarberAsc:
					clients = clients.OrderBy(s => s.Barber!.Name);
					break;
				case SortState.BarberDesc:
					clients = clients.OrderByDescending(s => s.Barber!.Name);
					break;
				default:
					clients = clients.OrderBy(s => s.Name);
					break;
			}

			// пагинация
			var count = await clients.CountAsync();
			var items = await clients.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

			// формируем модель представления
			RecordsViewModel viewModel = new RecordsViewModel(
			items,
				new PageViewModel(count, page, pageSize),
				new FilterViewModel(await _context.Barber.ToListAsync(), barber, name),
				new SortViewModel(sortOrder)
			);
			return View(viewModel);
		}

		public async Task<IActionResult> Details(int? id)
		{
			if (id == null || _context.Client == null)
			{
				return NotFound();
			}

			var product = await _context.Client
				.Include(p => p.Barber)
				.FirstOrDefaultAsync(m => m.Id == id);
			if (product == null)
			{
				return NotFound();
			}

			return View(product);
		}

		[Authorize(Roles = "admin")]
		public IActionResult Create()
		{
			ViewData["BarberId"] = new SelectList(_context.Set<Barber>(), "Id", "Name");
			return View();
		}

		[Authorize(Roles = "admin")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Id,Name,Price,BarberId")] Client client)
		{
			if (ModelState.IsValid)
			{
				_context.Add(client);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			ViewData["BarberId"] = new SelectList(_context.Set<Barber>(), "Id", "Id", client.BarberId);
			return View(client);
		}

		[Authorize(Roles = "admin")]
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null || _context.Client == null)
			{
				return NotFound();
			}

			var product = await _context.Client.FindAsync(id);
			if (product == null)
			{
				return NotFound();
			}
			ViewData["BarberId"] = new SelectList(_context.Set<Barber>(), "Id", "Name", product.BarberId);
			return View(product);
		}

		[Authorize(Roles = "admin")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,BarberId")] Client client)
		{
			if (id != client.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(client);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!ClientExists(client.Id))
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
			ViewData["BarberId"] = new SelectList(_context.Set<Barber>(), "Id", "Id", client.BarberId);
			return View(client);
		}

		[Authorize(Roles = "admin")]
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null || _context.Client == null)
			{
				return NotFound();
			}

			var product = await _context.Client
				.Include(p => p.Barber)
				.FirstOrDefaultAsync(m => m.Id == id);
			if (product == null)
			{
				return NotFound();
			}

			return View(product);
		}

		[Authorize(Roles = "admin")]
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			if (_context.Client == null)
			{
				return Problem("Entity set 'BarbershopContext.Client'  is null.");
			}
			var product = await _context.Client.FindAsync(id);
			if (product != null)
			{
				_context.Client.Remove(product);
			}

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool ClientExists(int id)
		{
			return _context.Client.Any(e => e.Id == id);
		}
	}
}
