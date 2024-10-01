using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Events
{
	public sealed class Price
	{
		public int Id { get; set; }
		public string Type { get; set; }
		[Column("price")]
		public decimal Sum { get; set; }
	}
}
