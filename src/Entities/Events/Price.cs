using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Events
{
	public sealed class Price
	{
		public int Id { get; set; }
		public string Type { get; set; }
		[Column("Price")]
		public decimal Sum { get; set; }
	}
}
