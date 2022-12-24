namespace Barbershop.Models
{
	public class Barber
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public List<Client> Clients { get; set; } = new();
	}
}
