using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileHider.Data.Models
{
    public class User
    {
        public int Id { get; init; }
        [Column("first_name")]
        public string FirstName { get; init; }
        [Column("last_name")]
        public string LastName { get; init; }

        public User(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }
    }
}
