using HMS.Areas.Dashboard.ViewModel;
using HMS.Entities;
using HMS.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace HMS.Areas.Dashboard.Controllers
{
    public class RolesController : Controller
    {
        private HMSSignInManager _signInManager;
        private HMSUserManager _userManager;
        private HMSRoleManager _roleManager;

        public RolesController()
        {
        }

        public RolesController(HMSUserManager userManager, HMSSignInManager signInManager,HMSRoleManager roleManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            RoleManager = roleManager;
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

        public ActionResult Index(string searchTerm,int page = 1)
        {
            int recordSize = 10;
            RolesListingModel model = new RolesListingModel();
            model.SearchTerm = searchTerm;
            model.Roles = SearchRoles(searchTerm,page, recordSize);
            var totalRecord = SearchRolesCount(searchTerm);
            model.Pager = new Pager(totalRecord, page, recordSize);
            return View(model);
        }

        public IEnumerable<IdentityRole> SearchRoles(string searchTerm, int page, int recordSize)
        {
            var roles = RoleManager.Roles.AsQueryable();
            if (!string.IsNullOrEmpty(searchTerm))
             {
                roles = roles.Where(x => x.Name.ToLower().Contains(searchTerm.ToLower()));
             }
            var skip = (page - 1) * recordSize;
            return roles.OrderBy(x => x.Name).Skip(skip).Take(recordSize).ToList();
        }
        public int SearchRolesCount(string searchTerm)
        {
            var roles = RoleManager.Roles.AsQueryable();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                roles = roles.Where(x => x.Name.ToLower().Contains(searchTerm.ToLower()));
            }
            return roles.Count();
        }
        /// <summary>
        /// use Action For Create and Update Action
        /// </summary>
        /// <returns></returns>
        ///

        [HttpGet]
        public async Task<ActionResult> Action(string ID)
        {
            RolesActionModel model = new RolesActionModel();

            if (!string.IsNullOrEmpty(ID))//We are trying to edit a Record
            {
                var role = await RoleManager.FindByIdAsync(ID);
                model.ID = role.Id;
                model.Name = role.Name;
             }
            return PartialView("_Action", model);
        }

        [HttpPost]
        public async Task<JsonResult> Action(RolesActionModel model)
        {
            JsonResult jsonResult = new JsonResult();
            IdentityResult result = null;

            if (!string.IsNullOrEmpty(model.ID))// Edit
            {
                var role = await RoleManager.FindByIdAsync(model.ID);
                role.Name = model.Name;
                result = await RoleManager.UpdateAsync(role);

            }
            else//Add/Create
            {
                var role = new IdentityRole();
                role.Name = model.Name;
               result = await RoleManager.CreateAsync(role);
            }
            jsonResult.Data = new { Success = result.Succeeded, Message = string.Join(", ", result.Errors) };
            return jsonResult;
        }

        [HttpGet]
        public async Task<ActionResult> Delete(string ID)
        {
            RolesActionModel model = new RolesActionModel();
            var role = await RoleManager.FindByIdAsync(ID);
            model.ID = role.Id;
            return PartialView("_Delete", model);
        }
        [HttpPost]
        public async Task<JsonResult> Delete(RolesActionModel model)
        {
            JsonResult jsonResult = new JsonResult();
            IdentityResult result = null;

            if (!string.IsNullOrEmpty(model.ID))// Delete
            {
             var role = await RoleManager.FindByIdAsync(model.ID);
             result = await RoleManager.DeleteAsync(role);
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