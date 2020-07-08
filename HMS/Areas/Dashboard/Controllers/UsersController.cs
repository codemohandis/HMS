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
        private HMSRoleManager _roleManager;


        public UsersController()
        {
        }

        public UsersController(HMSUserManager userManager, HMSSignInManager signInManager,HMSRoleManager roleManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            RoleManager = roleManager;

        }

        public HMSRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<HMSRoleManager>();
            }

            private set
            {
                _roleManager = value;
            }
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
            model.Roles = RoleManager.Roles.ToList();
            model.Users = SearchUsers(searchTerm, roleID, page, recordSize);
            var totalRecord = SearchUsersCount(searchTerm, roleID);//accomodationService.SearchAccomodationCount(searchTerm, roleID);
            model.Pager = new Pager(totalRecord, page, recordSize);
            return View(model);
        }

        public IEnumerable<IdentityRoles> SearchUsers(string searchTerm, string roleID, int page, int recordSize)
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
                var user = new IdentityRoles();
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

        [HttpGet]
        public async Task<ActionResult> UserRoles(string ID)
        {
            UsersRoleModel model = new UsersRoleModel();
            model.UserID = ID;
            //Find  user using userId
            var user = await UserManager.FindByIdAsync(ID);
            // is user k against jitny role hain unki Ids select kr rha hai
            var userRoleIDs = user.Roles.Select(x => x.RoleId).ToList();
            // phr yahan jo role user ko deye huwy hain unko total role say match krwa kr return kar rha hai
            model.UserRoles = RoleManager.Roles.Where(x => userRoleIDs.Contains(x.Id)).ToList();

            model.Roles = RoleManager.Roles.Where(x=>!userRoleIDs.Contains(x.Id)).ToList();
            return PartialView("_UserRoles", model);
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="operation">Type of operation to perform e.g: Assign or Delete</param>
        /// <param name="userID"></param>
        /// <param name="roleID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> UserRoleOperation(string userID,string roleID, bool isDelete = false)
        {
            JsonResult json = new JsonResult();
            var user =await UserManager.FindByIdAsync(userID);
            var role = await RoleManager.FindByIdAsync(roleID);
            if (user !=null && role!=null)
            {
                IdentityResult result = null;
                if (!isDelete)
                {
                    //UserId And role Name Parameter
                     result = await UserManager.AddToRoleAsync(userID, role.Name);
                }
                else
               {
                     result = await UserManager.RemoveFromRolesAsync(userID, role.Name);

                }
                json.Data = new { Success = result.Succeeded, Message = string.Join(",", result.Errors) };
            }
            else
            {
                json.Data = new { Success = false, Message = "Invalid Operations" };
            }
            return json;
        }

    }
}