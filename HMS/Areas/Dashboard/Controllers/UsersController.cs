using HMS.Areas.Dashboard.ViewModel;
using HMS.Entities;
using HMS.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace HMS.Areas.Dashboard.Controllers
{
    public class UsersController : Controller
    {
        private HMSSignInManager _signInManager;
        private HMSUserManager _userManager;

        public UsersController()
        {
        }

        public UsersController(HMSUserManager userManager, HMSSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public HMSSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<HMSSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public HMSUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<HMSUserManager>();
            }

            private set
            {
                _userManager = value;
            }
        }

        public ActionResult Index(string searchTerm, string roleID, int page = 1)
        {
            int recordSize = 1;
            //            page = page ?? 1;
            UsersListingModel model = new UsersListingModel();
            model.SearchTerm = searchTerm;
            model.RoleID = roleID;
            //model.Role = accomodationPackageService.GetAllAccomodationPackages();
            model.Users = SearchUsers(searchTerm, roleID, page, recordSize);
            var totalRecord = SearchUsersCount(searchTerm, roleID);//accomodationService.SearchAccomodationCount(searchTerm, roleID);
            model.Pager = new Pager(totalRecord, page, recordSize);
            return View(model);
        }

        public IEnumerable<HMSUser> SearchUsers(string searchTerm, string roleID, int page, int recordSize)
        {
            var users = UserManager.Users.AsQueryable();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                users = users.Where(x => x.Email.ToLower().Contains(searchTerm.ToLower()));
            }
            if (!string.IsNullOrEmpty(roleID))
            {
            //    users = users.Where(x => x.Email.ToLower().Contains(searchTerm.ToLower()));
            }

            //skip = 1-1 * 3 = 0
            //skip = 2-1 * 3 = 3
            //skip = 3-1 * 3 = 6
            var skip = (page - 1) * recordSize;
            return users.OrderBy(x => x.Email).Skip(skip).Take(recordSize).ToList();
        }
        public int SearchUsersCount(string searchTerm, string roleID)
        {
            var users = UserManager.Users.AsQueryable();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                users = users.Where(x => x.Email.ToLower().Contains(searchTerm.ToLower()));
            }
            if (!string.IsNullOrEmpty(roleID))
            {
                //    users = users.Where(x => x.Email.ToLower().Contains(searchTerm.ToLower()));
            }
            return users.Count();
        }
        /// <summary>
        /// use Action For Create and Update Action
        /// </summary>
        /// <returns></returns>
        ///

        [HttpGet]
        public async Task<ActionResult> Action(string ID)
        {
            UsersActionModel model = new UsersActionModel();

            if (!string.IsNullOrEmpty(ID))//We are trying to edit a Record
            {
                 var user = await UserManager.FindByIdAsync(ID);
                model.ID = user.Id;
                model.FullName = user.FullName;
                model.Email = user.Email;
                model.UserName = user.UserName;
                model.Country = user.Country;
                model.Address = user.Address;
            }
             return PartialView("_Action", model);
        }

        [HttpPost]
        public async Task<JsonResult> Action(UsersActionModel model)
        {
            JsonResult jsonResult = new JsonResult();
            IdentityResult result = null;

            if (!string.IsNullOrEmpty(model.ID))// Edit
            {
                var user = await UserManager.FindByIdAsync(model.ID);
                user.Id = model.ID;
                user.FullName = model.FullName;
                user.Email = model.Email;
                user.UserName = model.UserName;
                user.Country = model.Country;
                user.Address = model.Address;
                result = await UserManager.UpdateAsync(user);
            }
            else//Add/Create
            {
                var user = new HMSUser();
                //user.Id = model.ID;
                user.FullName = model.FullName;
                user.Email = model.Email;
                user.UserName = model.UserName;
                user.Country = model.Country;
                user.Address = model.Address;
                result = await UserManager.CreateAsync(user);
            }
           jsonResult.Data = new { Success = result.Succeeded, Message = string.Join(", ", result.Errors) };
            return jsonResult;
        }

        [HttpGet]
        public async Task<ActionResult> Delete(string ID)
        {
            UsersActionModel model = new UsersActionModel();
            var user = await UserManager.FindByIdAsync(ID);
            model.ID = user.Id;
            return PartialView("_Delete", model);
        }
        [HttpPost]
        public async Task<JsonResult> Delete(UsersActionModel model)
        {
            JsonResult jsonResult = new JsonResult();
            IdentityResult result = null;

            if (!string.IsNullOrEmpty(model.ID))// Delete
            {
                var user = await UserManager.FindByIdAsync(model.ID);
                result = await UserManager.DeleteAsync(user);
                jsonResult.Data = new { Success = result.Succeeded, Message = string.Join(", ", result.Errors) };
            }
            else
            {
                jsonResult.Data = new { Success = false, Message = "No User found/ Invalid Users" };
            }

            return jsonResult;
        }

    }
}