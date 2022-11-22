using Microsoft.AspNetCore.Mvc;
using BulkyBook.Models;
using BulkyBook.DataAcess.Repository.IRepository;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CoverTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CoverTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<CoverType> result = _unitOfWork.CoverType.GetAll();
            return View(result);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CoverType obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.CoverType.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "CoverType Created Sucessfully";

                return RedirectToAction(nameof(Index));
            }

            return View(obj);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            CoverType result = _unitOfWork.CoverType.GetById(id.Value);

            return View(result);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Delete(CoverType obj)
        {
            _unitOfWork.CoverType.Remove(obj);
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var result = _unitOfWork.CoverType.GetById(id.Value);
            return View(result);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(CoverType obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.CoverType.Update(obj);
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }

            return View(obj);
        }
    }
}
