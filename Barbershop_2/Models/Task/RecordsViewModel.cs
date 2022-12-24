namespace Barbershop.Models.Task
{
	public class RecordsViewModel
	{
		public IEnumerable<Client> Clients { get; }
		public PageViewModel PageViewModel { get; }
		public FilterViewModel FilterViewModel { get; }
		public SortViewModel SortViewModel { get; }
		public RecordsViewModel(IEnumerable<Client> slaves, PageViewModel pageViewModel,
			FilterViewModel filterViewModel, SortViewModel sortViewModel)
		{
			Clients = slaves;
			PageViewModel = pageViewModel;
			FilterViewModel = filterViewModel;
			SortViewModel = sortViewModel;
		}
	}
}
