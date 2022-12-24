using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Barbershop.Models;

namespace Barbershop.Data
{
	public class BarbershopContext : DbContext
	{
		public BarbershopContext(DbContextOptions<BarbershopContext> options)
			: base(options)
		{
			//Database.EnsureDeleted();
			Database.EnsureCreated();

			if (!Barber.Any())
			{
				Barber Sasha = new Barber { Name = "Sasha" };
				Barber Olena = new Barber { Name = "Olena" };

				Client c1 = new Client { Name = "Nick", Barber = Sasha, Price = 200 };
				Client c2 = new Client { Name = "Brian", Barber = Sasha, Price = 250 };
				Client c3 = new Client { Name = "Sirgay", Barber = Olena, Price = 150 };
				Client c4 = new Client { Name = "Den", Barber = Sasha, Price = 250 };
				Client c5 = new Client { Name = "Pasha", Barber = Olena, Price = 150 };
				Barber.AddRange(Olena, Sasha);
				Client.AddRange(c1, c2, c3, c4, c5);
				SaveChanges();
			}
		}
		public DbSet<Barbershop.Models.Client> Client { get; set; } = default!;
		public DbSet<Barbershop.Models.Barber> Barber { get; set; } = default!;
	}
}
