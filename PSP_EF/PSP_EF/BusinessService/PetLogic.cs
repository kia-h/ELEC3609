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
    public class PetLogic
    {
        public PetDbContext _petDb { get; set; }

        public PetLogic(PetDbContext db = null)
        {
            _petDb = db ?? new PetDbContext();
        }

        public Pet GetPetById(int id)
        {
            return _petDb.Pets.SingleOrDefault(c => c.PetId == id);
        }

        public IEnumerable<Pet> GetPetByCategory (string category)
        {
            return _petDb.Pets.Where(p => p.PetCategory == category.ToLower());
        }

        public List<Pet> GetAllPets(bool includeCustomer)
        {
            return includeCustomer? _petDb.Pets.Include(c=> c.Customer).Include(c=> c.PetClass).ToList() : _petDb.Pets.Include(c=> c.PetClass).ToList();
        }

        public int CreatePet(Pet entity)
        {
            try
            {
                if (entity == null)
                    return 0;

                _petDb.Pets.Add(entity);
                return _petDb.SaveChanges()==1 ? entity.PetId : 0;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public bool AddPet(Pet entity)
        {
            try
            {
                _petDb.Pets.Add(entity);
                _petDb.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool UpdatePet(Pet entity)
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

        public string UpdatePetAjaxService(int id, string value, int columnPosition)
        {

            try
            {
               Pet targetPet = GetPetById(id);
                if (targetPet != null)
                {
                    switch (columnPosition)
                    {
                        case 1:
                            targetPet.PetCategory = value;
                            break;
                        case 2:
                            targetPet.Price = Convert.ToDecimal(value);
                            break;
                        case 3:
                            targetPet.Description = value;
                            break;
                     
                       
                        default:
                            return "This columnIndex does not exist or is not editable.";

                    }

                    if (_petDb.SaveChanges() == 1) return value;
                    else return "Internal error refresh the page and try again";
                }
                else return string.Format("Fatal Error.The given Pet id{0} does not exist in our DataBase.The id of Pet is passed automaticly by plugin on updating.So its a fatal Error if you can see the Pet in Pet list, but the system cant relocate it.Or your a Hacker and tring to do some malicius ", id);
            }
            catch (Exception exp)
            {
                return exp.Message;
            }
        }

        public bool DeletePet(int id=0)
        {
            //EF5 only
            var pet = new Pet {PetId = id};
            try
            {
                //var obj = _petDb.Pets.SingleOrDefault(p => p.PetId == id);
                //_petDb.Pets.Remove(obj);
                //_petDb.SaveChanges();

                //EF 5 only
                _petDb.Pets.Attach(pet);
                _petDb.Entry(pet).State = EntityState.Deleted;
                _petDb.SaveChanges();
                
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        //As we dont have CustomersLogic yet
        public IEnumerable<Customer> GetAllCustomers()
        {
            return _petDb.Customers;
        }

        public Customer GetCustomerById(int id)
        {
            return _petDb.Customers.SingleOrDefault(c => c.CustomerId == id);
        }

        public void Dispose()
        {
            if (_petDb == null) return;
            _petDb.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
