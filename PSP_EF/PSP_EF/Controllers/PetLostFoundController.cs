using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PSP_EF.Models;
using PSP_EF.DAL;

namespace PSP_EF.Controllers
{
    public class PetLostFoundController : Controller
    {
        private PetDbContext db = new PetDbContext();

        // GET : /PetLostFound/AddPetLostInfo
        // get initial page for posting a lost pet
        public ActionResult AddPetLostInfo()
        {
            ViewBag.citylist = getCities();
            return View();
        }

        // POST : /PetLostFound/AddPetLostInfo
        // Save the lost pet info to the database
        [HttpPost, ValidateInput(false)]
        public ActionResult AddPetLostInfo(LostFound petLost)
        {
            ViewBag.citylist = getCities();
            var cityId = Request.Form["citylist"];

            var cUser = (Customer)Session["currentUser"];
            petLost.CustomerId = cUser.CustomerId;
            petLost.City = Convert.ToInt16(cityId);

            if (petLost.Description == null)
            {
                petLost.Description = "";
            }

            db.LostFounds.Add(petLost);
            db.SaveChanges();
            ViewBag.Message = "Your lost pet information has been posted. Your lost report ID is " + petLost.LostFoundId + ".";
            ViewBag.Message2 = "Please do not lose your report ID. You need it to keep track your lost report.";
            ViewBag.Message3 = "Thank you for using our service.";
            return View();
        }

        // GET : /PetLostFound/PetLostHelpInfo
        public ActionResult PetLostHelpInfo()
        {
            ViewBag.citylist = getCities();
            return View();
        }

        // POST : /PetLostFound/PetLostHelpInfoResult
        [HttpPost]
        public ActionResult PetLostHelpInfoResult(LostFound petLost)
        {
            ViewBag.citylist = getCities();
            var cityId = Convert.ToInt32(Request.Form["citylist"]);

            var petLostQuery = (from theOwner in db.Customers
                                join theLostInfo in db.LostFounds on theOwner.CustomerId equals theLostInfo.CustomerId
                                join theCity in db.Cities on theLostInfo.City equals theCity.CityId
                                where theLostInfo.PetCategory.Equals(petLost.PetCategory)
                                where theLostInfo.City.Equals(cityId)
                                select new SearchLostPetResult
                                {
                                    lostInfoId = theLostInfo.LostFoundId,
                                    pet_owner_name = theOwner.FirstName + " " + theOwner.LastName,
                                    pet_category = theLostInfo.PetCategory,
                                    city = theCity.Name,
                                    pet_desc = theLostInfo.Description
                                }).Distinct();

            //int abc = petLostQuery.Count();
            return View(petLostQuery.AsEnumerable());
        }

        // GET : /PetLostFound/PostComment
        public ActionResult PostComment(int lostInfoId = 0)
        {
            ViewBag.message = lostInfoId;
            LostFound lostPet = db.LostFounds.Find(lostInfoId);
            Session["LostFoundId"] = lostPet.LostFoundId;
            ViewBag.reportId = lostPet.LostFoundId;
            ViewBag.petCategory = lostPet.PetCategory;
            ViewBag.description = lostPet.Description;


            return View();
        }

        // POST: /PetLostFound/SaveComment
        [HttpPost]
        public ActionResult SaveComment(LF_Comment theComment)
        {
            var cUser = (Customer)Session["currentUser"];
            int LostFoundId = Convert.ToInt32(Session["LostFoundId"]);
            theComment.InfoId = LostFoundId;
            theComment.CommenterId = cUser.CustomerId;

            db.LF_Comment.Add(theComment);
            db.SaveChanges();
            Session["LostFoundId"] = null;
            return View();
        }

        // GET: /PetLostFound/TrackReport
        public ActionResult TrackReport()
        {
            return View();
        }

        // POST: /PetLostFound/TrackReport
        // Get report regarding the lost pet 
        [HttpPost]
        public ActionResult TrackReportResult(LostFound petLost)
        {
            LostFound thePetLost = db.LostFounds.Find(petLost.LostFoundId);
            var cUser = (Customer)Session["currentUser"];

            // Check whether the user is a valid and authorized user
            if (thePetLost != null && (Session["currentUser"] != null))
            {
                if (thePetLost.CustomerId == cUser.CustomerId)
                {
                    ViewBag.ReportId = thePetLost.LostFoundId;
                    ViewBag.PetCategory = thePetLost.PetCategory;
                    ViewBag.Description = thePetLost.Description;
                    var comment = (from theComment in db.LF_Comment
                                   join theOwner in db.Customers on theComment.CommenterId equals theOwner.CustomerId
                                   where theComment.InfoId.Equals(petLost.LostFoundId)
                                   select new SearchLostPetResult
                                   {
                                       lostInfoId = theComment.InfoId,
                                       pet_owner_name = theOwner.FirstName + " " + theOwner.LastName,
                                       pet_category = "",
                                       city = "",
                                       pet_desc = theComment.Comment
                                   }).Distinct();

                    return View(comment.AsEnumerable());
                }
                else
                {
                    ViewBag.error = "You are not authorized to view this Report";
                }
            }
            else
            {
                ViewBag.error = "No such report exists.";
            }
            return View();
        }

        // GET: /PetLostFound/PostFinding
        // get page for posting information when finding a lost pet
        public ActionResult PostFinding()
        {
            return View();
        }

        // POST: /PetLostFound/PostFinding
        // post the information regarding a newly found pet
        [HttpPost]
        public ActionResult PostFinding(Pet cPet)
        {
            Customer currentUser = (Customer)Session["currentUser"];
            int customerId = currentUser.CustomerId;

            cPet.CustomerId = customerId;
            cPet.Price = 0;
            db.Pets.Add(cPet);
            db.SaveChanges();

            Service newService = new Service();
            newService.ProviderId = customerId;
            newService.PetId = cPet.PetId;
            newService.ServiceName = "A lost pet founded by " + currentUser.FirstName + " " + currentUser.LastName;
            newService.Description = "Found a lost pet";
            newService.Price = 0;
            db.Services.Add(newService);
            db.SaveChanges();

            ViewBag.Message = "Your pet info has been posted. Thank you for using our service.";

            return View();
        }

        // GET:/PetLostFound/ClaimPet/
        // get the result of all pets that are possible to be claimed
        public ActionResult ClaimPet()
        {
            // get list of all pets which is available for claim
            var petForClaim = (from thePets in db.Pets
                               join theOwner in db.Customers on thePets.CustomerId equals theOwner.CustomerId
                               join service in db.Services on thePets.PetId equals service.PetId
                               where service.ServiceName.StartsWith("A lost pet founded by ")
                               where !thePets.TransactionId.HasValue
                               select new SearchPetResult
                               {
                                   service_id = service.ServiceId,
                                   pet_owner_name = theOwner.FirstName + " " + theOwner.LastName,
                                   pet_category = thePets.PetCategory,
                                   pet_desc = thePets.Description,
                                   pet_price = 0
                               }).Distinct();

            return View(petForClaim.AsEnumerable());
        }

        // GET:/PetLostFound/ClaimProcess/
        // Claim back one's pet
        public ActionResult ClaimProcess(int serviceId = 0)
        {
            int petId = 0;
            var cUser = (Customer)Session["currentUser"];
            Transaction trans = new Transaction();

            trans.CustomerId = cUser.CustomerId;
            trans.ServiceId = serviceId;
            trans.Date = System.DateTime.Now;
            trans.Status = "Pet claim initiated";

            if (ModelState.IsValid)
            {
                db.Transactions.Add(trans);
                db.SaveChanges();
            }

            var getPetId = (from service in db.Services
                            where service.ServiceId.Equals(serviceId)
                            select new { service.PetId });

            foreach (var c in getPetId)
            {
                petId = (int)c.PetId;
            }

            Pet thePet = new Pet();
            thePet = db.Pets.Find(petId);
            thePet.TransactionId = trans.TransactionId;

            if (ModelState.IsValid)
            {
                db.Entry(thePet).State = EntityState.Modified;
                db.SaveChanges();
            }

            return View();
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        // Get list of all cities in the database
        public List<SelectListItem> getCities()
        {
            var cities = from p in db.Cities
                         select p;

            List<SelectListItem> list = new List<SelectListItem>();

            foreach (var c in cities)
            {
                list.Add(new SelectListItem
                {
                    Text = c.Name,
                    Value = c.CityId.ToString(),
                    Selected = (c.CityId == 1)
                });
            }

            return list;
        }
    }
}