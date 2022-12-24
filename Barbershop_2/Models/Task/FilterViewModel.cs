using Microsoft.AspNetCore.Mvc.Rendering;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Barbershop.Models.Task
{
	public class FilterViewModel
	{
		public FilterViewModel(List<Barber> barbers, int barber, string name)
		{
			barbers.Insert(0, new Barber { Name = "All", Id = 0 });
			Barbers = new SelectList(barbers, "Id", "Name", barber);
			SelectedBarber = barber;
			SelectedName = name;
		}
		public SelectList Barbers { get; }
		public int SelectedBarber { get; }
		public string SelectedName { get; }
	}
}
