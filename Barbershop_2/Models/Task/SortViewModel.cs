namespace Barbershop.Models.Task
{
	public class SortViewModel
	{
		public SortState NameSort { get; }
		public SortState PriceSort { get; }
		public SortState BarberSort { get; }
		public SortState Current { get; }

		public SortViewModel(SortState sortOrder)
		{
			NameSort = sortOrder == SortState.NameAsc ? SortState.NameDesc : SortState.NameAsc;
			PriceSort = sortOrder == SortState.PriceAsc ? SortState.PriceDesc : SortState.PriceAsc;
			BarberSort = sortOrder == SortState.BarberAsc ? SortState.BarberDesc : SortState.BarberAsc;
			Current = sortOrder;
		}
	}
}
