using Library.Core.Dto;
using Library.Core.ServiceInterface;
using Library.Data;
using LibraryLINQ.Models.Loan;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryLINQ.Controllers
{
    public class LoanController : Controller
    {
        private readonly ILoanServices _loanServices;
        private readonly LibraryContext _Context;

        public LoanController
            (
                ILoanServices loanServices,
                LibraryContext Context
            )
        {
            _loanServices = loanServices;
            _Context = Context;
        }

        [HttpGet]
        public IActionResult Create()
        {
            LoanDropdowns();

            LoanCreateViewModel vm = new()
            {
                LoanDate = DateTime.Now,
            };

            return View(vm);
        }

        public IActionResult Index()
        {
            var result = _Context.Loans
                .Select(x => new LoanIndexViewModel
                {
                    Id = x.Id,
                    BookId = x.BookId,
                    Title = x.Book.Title,
                    ReaderId = x.ReaderId,
                    FullName = x.Reader.FullName,
                    LoanDate = x.LoanDate,
                    ReturnDate = x.ReturnDate
                }).ToList();

            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(LoanCreateViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var bookExists = await _Context.Books
                    .AnyAsync(x => x.Id == vm.BookId && x.IsAvailable);

                var readerExists = await _Context.Readers
                    .AnyAsync(x => x.Id == vm.ReaderId);

                if (!bookExists || !readerExists)
                {
                    ModelState.AddModelError
                        (
                        "",
                        "Pleaes select a valid book and reader"
                        );
                }
                else
                {
                    var dto = new LoanDto
                    {
                        Id = vm.Id,
                        BookId = vm.BookId,
                        ReaderId = vm.ReaderId,
                        LoanDate = DateTime.Now,
                        ReturnDate = null
                    };

                    await _loanServices.Create(dto);

                    return RedirectToAction(nameof(Index));
                }
            }

            LoanDropdowns();
            return View(vm);
        }

        private void LoanDropdowns()
        {
            ViewBag.Books = _Context.Books
                .Where(x => x.IsAvailable)
                .OrderBy(x => x.Title)
                .ToList();

            ViewBag.Readers = _Context.Readers
                .OrderBy(x => x.FullName)
                .ToList();
        }

        [HttpGet]
        public async Task<IActionResult> Return(Guid id)
        {
            var loan = await _Context.Loans
                .Include(L => L.Book)
                .Include(L => L.Reader)
                .FirstOrDefaultAsync(L => L.Id == id);

            if (loan == null || loan.ReturnDate != null)
            {
                return NotFound();
            }

            var vm = new LoanReturnViewModel
            {
                Id = loan.Id,
                BookId = loan.BookId,
                ReaderId = loan.ReaderId,
                Title = loan.Book.Title,
                FullName = loan.Reader.FullName,
                LoanDate = loan.LoanDate,
                ReturnDate = loan.ReturnDate
            };

            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Return(LoanReturnViewModel vm)
        {
            var dto = new LoanDto
            {
                Id = vm.Id,
            };

            var result = await _loanServices.BookReturn(dto);

            if (result == null)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
