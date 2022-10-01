using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelsDb
{
    [Table(name: "currency")]
    public class CurrencyDb
    {
        [Key]
        [Column(name: "id")]
        public Guid Id { get; set; }

        [Column(name: "code")]
        public int Code { get; set; }

        [Column(name: "name")]
        public string Name { get; set; }

        [Column(name: "account_id")]
        public string AccountId { get; set; }

        [ForeignKey(nameof(AccountId))]
        public AccountDb AccountDb { get; set; }
    }
}