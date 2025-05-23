﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Rent_A_Car.DbContext;
using Rent_A_Car.Models;

namespace Rent_A_Car.Controllers
{
	[Authorize(Roles = "Admin")]
	public class CarController : Controller
	{
		private readonly AppDbContext _context;

		public CarController(AppDbContext context)
		{
			_context = context;
		}

		// GET: Car
		public async Task<IActionResult> Index()
		{
			return View(await _context.Car.ToListAsync());
		}

		// GET: Car/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var car = await _context.Car
				.FirstOrDefaultAsync(m => m.Id == id);
			if (car == null)
			{
				return NotFound();
			}

			return View(car);
		}

		// GET: Car/Create
		public IActionResult Create()
		{
			return View();
		}

		// POST: Car/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Id,Brand,Model,YearOfProduction,Seats,Description,PricePerDay")] Car car)
		{
			var existingCar = await _context.Car.FirstOrDefaultAsync(e => e.Model == car.Model);

			if (existingCar == null)
			{
				_context.Add(car);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			else if (existingCar.YearOfProduction != car.YearOfProduction)
			{
				_context.Add(car);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			else
			{
				ModelState.AddModelError(string.Empty, "This car already exists.");
			}
			return View(car);
		}

		// GET: Car/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var car = await _context.Car.FindAsync(id);
			if (car == null)
			{
				return NotFound();
			}
			return View(car);
		}

		// POST: Car/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("Id,Brand,Model,YearOfProduction,Seats,Description,PricePerDay")] Car car)
		{
			if (id != car.Id)
			{
				return NotFound();
			}
			try
			{
				var existingCar = await _context.Car.FindAsync(id);

				if (existingCar == null)
				{
					return NotFound();
				}

				existingCar.Brand = car.Brand;
				existingCar.Model = car.Model;
				existingCar.YearOfProduction = car.YearOfProduction;
				existingCar.Seats = car.Seats;
				existingCar.Description = car.Description;
				existingCar.PricePerDay = car.PricePerDay;

				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!CarExists(car.Id))
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
		// GET: Car/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var car = await _context.Car
				.FirstOrDefaultAsync(m => m.Id == id);
			if (car == null)
			{
				return NotFound();
			}

			return View(car);
		}

		// POST: Car/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var car = await _context.Car.FindAsync(id);
			if (car != null)
			{
				_context.Car.Remove(car);
			}

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool CarExists(int id)
		{
			return _context.Car.Any(e => e.Id == id);
		}
	}
}
