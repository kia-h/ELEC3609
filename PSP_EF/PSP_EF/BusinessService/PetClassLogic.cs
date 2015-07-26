using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using PSP_EF.DAL;
using PSP_EF.Models;

namespace PSP_EF.BusinessService
{
    public class PetClassLogic
    {
        public PetDbContext _petDb { get; set; }

        public PetClassLogic(PetDbContext db = null)
        {
            _petDb = db ?? new PetDbContext();
        }

        public PetClass GetPetClassById(int id)
        {
            return _petDb.PetClass.SingleOrDefault(c => c.PetClassId == id);
        }

        public IEnumerable<PetClass> GetPetByLocation(string location)
        {
            return _petDb.PetClass.Where(p => p.Location == location.ToLower());
        }

        public IEnumerable<PetClass> GetPetByDate(DateTime schedule)
        {
            return _petDb.PetClass.Where(p => p.Schedule == schedule);
        }

        /// <summary>
        /// Get All customers who have at least one class booked for at least one of his/her pets.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<Customer> GetAllBookedCustomers(int id)
        {
            return _petDb.Customers.Where(c => c.PetClasses.Count > 0);
        }
        public IEnumerable<Customer> GetCustomersOfThisClassRoom(int classId)
        {
            try
            {
                return _petDb.PetClass.SingleOrDefault(c => c.PetClassId.Equals(classId)).BookedCustomers;
            }
            catch (Exception)
            {

                return null;
            }
         
        }

        public IEnumerable<Pet> GetAllPetsOfThisClassRoom(int classId)
        {
            return _petDb.PetClass.SingleOrDefault(c => c.PetClassId.Equals(classId)).Pets;
        }

        public IEnumerable<PetClass> GetAllClasses(bool onlyAvailables)
        {
            return onlyAvailables ? _petDb.PetClass.Where(c=>(int)c.Capacity - c.Pets.Count >0) : _petDb.PetClass;
        }

        public bool HasEnoughCapacity(PetClass target, int inComming)
        {
            return (target.Pets.Count + inComming) <= target.Capacity;
        }

        public bool BookThisClassForMyPet(int classId, int petId)
        {
            Pet pet =  _petDb.Pets.SingleOrDefault(c => c.PetId==petId);
           Customer owner =pet.Customer;
            return BookThisClassForMyPet(classId, pet, owner);
        }

        public bool BookThisClassForMyPet(int classId, Pet pet, Customer owner)
        {
           PetClass target = GetPetClassById(classId);
           return BookThisClassForMyPet(target, pet, owner);
            
        }

        public bool BookThisClassForMyPet( PetClass target,Pet pet, Customer owner)
        {
               pet.HasClass = true;
               target.Pets.Add(pet);
               target.BookedCustomers.Add(owner);
               return UpdatePetClass(target);
                  
           
        }
        /// <summary>
      /// Subscribe all pets of gienCustomer to a given class,before calling this method, make sure to include the pets to Customer.
      /// </summary>
      /// <param name="classId"></param>
      /// <param name="owner"></param>
      /// <returns>true on success, false if thers no space for all incoming pets</returns>
        public bool BookAllMyPetsToThisClass(PetClass target, Customer owner)
        {
            if(HasEnoughCapacity(target, owner.Pets.Count))
            {
              foreach (Pet p in owner.Pets)
                {
                    target.Pets.Add(p);
                }
                target.BookedCustomers.Add(owner);
                return UpdatePetClass(target);
            }
            return false;
        }

        public bool RemoveMyPetFromThisClass(PetClass target, int petId, int ownerId)
        {
           
                target.Pets.Remove(new Pet() { PetId = petId });
                target.BookedCustomers.Remove(new Customer() { CustomerId = ownerId });
                return UpdatePetClass(target);
          
        }

        public bool RemoveAllMyPetsFromThisClass(PetClass target, int ownerId)
        {
            for (int i = 0; i < target.Pets.Count; i++)
            {
                int currentPetId = target.Pets.ElementAt(i).PetId;
                if (target.Pets.ElementAt(i).CustomerId.Equals(ownerId))
                    target.Pets.Remove(new Pet() { PetId = currentPetId });
            }
            target.BookedCustomers.Remove(new Customer() { CustomerId = ownerId });
            return UpdatePetClass(target);
        }
        
        public int CreatePet(PetClass entity)
        {
            try
            {
                if (entity == null)
                    return 0;

                _petDb.PetClass.Add(entity);
                return _petDb.SaveChanges() == 1 ? entity.PetClassId : 0;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public bool AddPetClass(PetClass entity)
        {
            try
            {
                _petDb.PetClass.Add(entity);
                _petDb.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool UpdatePetClass(PetClass entity)
        {
            try
            {
                _petDb.Entry(entity).State = EntityState.Modified;
                _petDb.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeletePetClass(int id = 0)
        {
            //EF5 only
            var petClass = new PetClass { PetClassId = id };
            try
            {
                //var obj = _petDb.Pets.SingleOrDefault(p => p.PetId == id);
                //_petDb.Pets.Remove(obj);
                //_petDb.SaveChanges();

                //EF 5 only
                _petDb.PetClass.Attach(petClass);
                _petDb.Entry(petClass).State = EntityState.Deleted;
                _petDb.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void Dispose()
        {
            if (_petDb == null) return;
            _petDb.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}