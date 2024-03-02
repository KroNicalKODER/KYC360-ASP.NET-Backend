using KYC360.Data;
using KYC360.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KYC360
{
    public class Seed
    {
        private readonly DataContext dataContext;

        public Seed(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public void SeedDataContext()
        {
            if (!dataContext.Entities.Any())
            {
                var entities = new List<Entity>
                {
                    new Entity
                    {
                        Id = "1",
                        Gender = "Male",
                        Deceased = false,
                        Addresses = new List<Address>
                        {
                            new Address
                            {
                                Id = "1",
                                AddressLine = "123 Main St",
                                City = "Cityville",
                                Country = "Countryland",
                                EntityId = "1"
                            }
                        },
                        Dates = new List<Date>
                        {
                            new Date
                            {
                                Id = "1",
                                DateType = "Birth",
                                Dat = new DateTime(1990, 1, 1),
                                EntityId = "1"
                            }
                        },
                        Names = new List<Name>
                        {
                            new Name
                            {
                                Id = "1",
                                FirstName = "John",
                                MiddleName = "Athena",
                                LastName = "Doe",
                                EntityId= "1"
                            }
                        }
                    },
                    new Entity
                    {
                        Id = "2",
                        Gender = "Female",
                        Deceased = true,
                        Addresses = new List<Address>
                        {
                            new Address
                            {
                                Id = "2",
                                AddressLine = "456 Oak St",
                                City = "Townsville",
                                Country = "Countryland",
                                EntityId = "2"
                            }
                        },
                        Dates = new List<Date>
                        {
                            new Date
                            {
                                Id = "2",
                                DateType = "Birth",
                                Dat = new DateTime(1985, 5, 10),
                                EntityId = "2"
                            },
                            new Date
                            {
                                Id = "3",
                                DateType = "Death",
                                Dat = new DateTime(2020, 12, 31),
                                EntityId = "2"
                            }
                        },
                        Names = new List<Name>
                        {
                            new Name
                            {
                                Id = "2",
                                FirstName = "Jane",
                                MiddleName = "Carlos",
                                LastName = "Smith",
                                EntityId = "2"
                            }
                        }
                    }
                    // Add more entities as needed
                };

                dataContext.Entities.AddRange(entities);
                dataContext.SaveChanges();
            }
        }
    }
}
