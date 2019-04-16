using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LittleLibrary.Data;
using LittleLibrary.Repositories;
using LittleLibrary.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LittleLibrary.Controllers
{
    [Authorize (Roles = "Admin")]
    public class RoleController : Controller
    {
        ApplicationDbContext _context;

        public RoleController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Role
        public ActionResult Index()
        {
            RoleRepo roleRepo = new RoleRepo(_context);
            return View(roleRepo.GetAllRoles());
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(RoleVM roleVM)
        {
            if (ModelState.IsValid)
            {
                RoleRepo roleRepo = new RoleRepo(_context);
                var success = roleRepo.CreateRole(roleVM.RoleName);
                if (success)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            ViewBag.Error = "An error occurred while creating this role. Please try again.";
            return View();
        }

        public  ActionResult Delete(string id)
        {
            if (id != null)
            {
                RoleRepo rr = new RoleRepo(_context);
                if (rr.DeleteRole(id))
                {
                    return RedirectToAction(nameof(Index));
                }

                ViewBag.Error = "An error occurred while deleting this role. Please try again.";
                return View();
            }

            ViewBag.Error = "An error occurred while deleting this role. Please try again.";
            return View();
        }

    }

}