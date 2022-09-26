using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelsDb
{
    [Table(name: "client")]
    public class ClientDb
    {
        [Key]
        [Column(name: "id")]
        public Guid Id { get; set; }

        [Column(name: "first_name")]
        public string FirstName { get; set; }

        [Column(name: "last_name")]
        public string LastName { get; set; }

        [Column(name: "date_of_birth")]
        public DateTime DateOfBirth { get; set; }

        [Column(name: "phone_number")]
        public int PhoneNumber { get; set; }

        [Column(name: "passport_number")]
        public int PassportNumber { get; set; }

        [Column(name: "bonus_count")]
        public int BonusCount { get; set; }

        public ICollection<AccountDb> AccountDbCollection { get; set; }
    }
}