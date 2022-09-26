using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelsDb
{
    [Table(name: "account")]
    public class AccountDb
    {
        [Key]
        [Column(name: "id")]
        public Guid Id { get; set; }

        [Column(name: "currency_name")]
        public string CurrencyName { get; set; }

        [Column(name: "amount")]
        public double Amount { get; set; }

        [ForeignKey("client_id")]
        [Column(name: "client_id")]
        public Guid ClientId { get; set; }

        public ClientDb ClientDb { get; set; }
    }
}