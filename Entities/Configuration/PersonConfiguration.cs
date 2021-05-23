using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Configuration
{
    public class PersonConfiguration : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.HasData
            (
                new Person
                {
                    Id = new Guid("53a1237a-3ed3-4462-b9f0-5a7bb1056a33"),
                    Name = "Vince Gilligan",
                    BirthDate = new DateTime(1967, 02, 10),
                    BirthPlace = "Richmond, Virginia, USA",
                    Height = 1.83
                },
                new Person
                {
                    Id = new Guid("86dba8c0-d178-41e7-938c-ed49778fb52a"),
                    Name = "Bryan Cranston",
                    BirthDate = new DateTime(1965, 03, 07),
                    BirthPlace = "San Fernando, California, USA",
                    Height = 1.79
                },
                new Person
                {
                    Id = new Guid("021ca3c1-0deb-4afd-ae94-2159a8479811"),
                    Name = "Aaron Paul",
                    BirthDate = new DateTime(1979, 08, 27),
                    BirthPlace = "Emmett, Idaho, USA",
                    Height = 1.73
                }
            );
        }
    }
}

