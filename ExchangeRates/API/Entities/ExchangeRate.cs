namespace API.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class ExchangeRate
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Code { get; set; }

        [Range(0, double.MaxValue)]
        public double Rate { get; set; }

        public DateTime Date { get; set; }
    }
}
