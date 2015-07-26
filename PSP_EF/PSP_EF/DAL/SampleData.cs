using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

using PSP_EF.Models;
using PSP_EF.BusinessService;
using System;

namespace PSP_EF.DAL
{
    public class SampleData : DropCreateDatabaseAlways<PetDbContext>
    {
        protected override void Seed(PetDbContext context)
        {
            PetClassLogic _logic = new PetClassLogic(context);
            PetLogic _petLogic = new PetLogic(context);

            var _countries = new List<Country>
            {
                new Country { Name = "Australia",Currency = "AUD"},
                new Country { Name = "USA",Currency = "USD"}
            };
            _countries.ForEach(c => context.Countries.Add(c));
            context.SaveChanges();

            var _citis = new List<City>
            {
               new City { Name = "Sydney",Country = _countries.Single(c=>c.Name=="Australia")},
               new City { Name = "Melbourne",Country = _countries.Single(c=>c.Name=="Australia") },
               new City { Name = "Chicago",Country = _countries.Single(c=>c.Name=="USA") }
            };
            _citis.ForEach(c => context.Cities.Add(c));
            context.SaveChanges();

            var _customers = new List<Customer>
                                {
                                    new Customer
                                        {
                                            FirstName = "A",
                                            LastName = "aa",
                                            Username = "Leo",
                                            Password = "123",
                                            Email = "123@gmail.com",
                                            Suburb = "Ashfild",
                                            Address = "2 main road",
                                            Description="bad felow",
                                            Phone = "1234567",
                                            City = _citis.Single(c => c.Name == "Sydney")
                                        },
                                    new Customer
                                        {
                                            FirstName = "B",
                                            LastName = "bb",
                                            Username ="king",
                                            Password = "321",
                                            Email = "321@gmail.com",
                                            Suburb = "Mosman",
                                            Description="good felow",
                                            Address = "23 second road",
                                            Phone = "1234567",
                                            City = _citis.Single(c => c.Name == "Chicago")
                                        }                                   
                                };
            _customers.ForEach(c => context.Customers.Add(c));
            context.SaveChanges();            
            
            var _services = new List<Service>
            {
                new Service 
                {   Customer = _customers.Single(c=>c.Username=="Leo"), 
                    Description="Good Sitter", 
                    Price=23, 
                    ProviderId=2, 
                    ServiceName = "Sitter the Good"
                },
                new Service 
                {   Customer = _customers.Single(c=>c.Username=="king"), 
                    Description="Good Walker", 
                    Price=333, 
                    ProviderId=1, 
                    ServiceName = "Walker the King"
                },
            };
            _services.ForEach(c => context.Services.Add(c));
            context.SaveChanges();

            var _transaction = new List<Transaction> 
            {
                new Transaction { CustomerId=2, ServiceId=1,Date=DateTime.Now, Status ="In Good shape"},
                new Transaction { CustomerId=1, ServiceId=2,Date=DateTime.Today, Status ="terrible service"}
            };
            _transaction.ForEach(c => context.Transactions.Add(c));
            context.SaveChanges();

            var _rating = new List<Rating> 
            {
                new Rating { Rate=23, Type =false, Comment =" the service was good", TransactionId=1 },
                new Rating { Rate=-4, Type =true,  Comment =" the customer didn't paid me" , TransactionId=2 }
            };
            _rating.ForEach(c => context.Ratings.Add(c));
            context.SaveChanges();

            //var _pet1 = new Pet { PetId=0, CustomerId = 1, Description = "brave and good", PetCategory = "German Sheperd", TransactionId = 1, Price = 23, HasClass = false };
            //var _pet2 = new Pet { PetId = 1, CustomerId = 1, Description = "Hilarious", PetCategory = "Puddle", TransactionId = 1, Price = 55, HasClass = false };
            //var _pet3 = new Pet { PetId = 2, CustomerId = 2, Description = "Good Friend", PetCategory = "Boxer", TransactionId = 2, Price = 44, HasClass = false };
            //_logic.CreatePet(_pet1);


            //var _pets = new List<Pet> 
            //{
            //    new Pet { CustomerId = 1, Description="brave and good", PetCategory="German Sheperd",TransactionId=1, Price=23, HasClass=false},
            //    new Pet { CustomerId = 1, Description="Hilarious", PetCategory="Puddle", TransactionId=1,Price=55, HasClass=false},
            //    new Pet { CustomerId = 2, Description="Good Friend", PetCategory="Boxer", TransactionId=2,Price=44, HasClass=false}
            //};

            //_petLogic.CreatePet(_pet1);
            //_petLogic.CreatePet(_pet2);
            //_petLogic.CreatePet(_pet3);
            //context.SaveChanges();
            
            var _petClasses = new List<PetClass> 
            {
                new PetClass 
                    { 
                        Name="Puppies 101", 
                        Description="First Step", 
                        Location="Town Hall", 
                        Capacity=8, 
                        Schedule=DateTime.Now                       
                    },
                new PetClass 
                    { 
                        Name="Kitten 101", 
                        Description="Mouse catching", 
                        Location="USYD Law School", 
                        Capacity=7,
                        Schedule=DateTime.Today
                    }
            };
            _petClasses.ForEach(c => context.PetClass.Add(c));
            context.SaveChanges();   
        }
    }
}