namespace Barbershop.Models
{
	public class Client
	{
		public int Id { get; set; }
		public string? Name { get; set; }
		public int Price { get; set; }
		public int? BarberId { get; set; }
		public Barber? Barber { get; set; }
	}
}
