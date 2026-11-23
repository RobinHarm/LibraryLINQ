using Library.Core.Domain;
using Library.Core.Dto;
using Library.Core.ServiceInterface;
using Library.Data;
using LibraryLINQ.LINQ;
using LibraryLINQ.Models.Book;
using Microsoft.AspNetCore.Mvc;

namespace LibraryLINQ.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookServices _bookServices;
        private readonly LibraryContext _libraryContext;

        public BookController
            (
            IBookServices bookServices,
            LibraryContext libraryContext
            )
        {
            _bookServices = bookServices;
            _libraryContext = libraryContext;
        }

        [HttpGet]
        public IActionResult Create()
        {
            BookCreateViewModel vm = new();

            return View(vm);
        }

        public IActionResult Index(string? year, string? title, bool borrowedOnly = false)
        {
            var books = _libraryContext.Books.ToList();
            var sorting = new LINQclass();

            if (!string.IsNullOrWhiteSpace(year))
            {
                books = sorting.BooksYearFind(books, year);
            }

            if(!string.IsNullOrWhiteSpace(title))
            {
                books = sorting.BooksTitle(books, title.Trim());
            }

            if(borrowedOnly)
            {
                books = sorting.Unavailablebooks(books);
            }

            var result = books.Select(x => new BookIndexViewModel
            {
                Id = x.Id,
                Title = x.Title,
                Author = x.Author,
                PublishedYear = x.PublishedYear,
                IsAvailable = x.IsAvailable,
            }).ToList();

            ViewBag.Year = year;
            ViewBag.TitleSearch = title;
            ViewBag.BorrowedOnly = borrowedOnly;

            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(BookCreateViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var dto = new BookDto
                {
                    Id = vm.Id,
                    Title = vm.Title,
                    Author = vm.Author,
                    PublishedYear = vm.PublishedYear
                };

                var result = await _bookServices.Create(dto);

                return RedirectToAction(nameof(Index));

            }

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var book = await _libraryContext.Books.FindAsync(id);

            if(book == null)
            {
                return NotFound();
            }

            var vm = new BookUpdateViewModel();
            {
                vm.Id = book.Id;
                vm.Title = book.Title;

                return View(vm);
            }
        }
        [HttpPost]

        public async Task<IActionResult> Update(BookUpdateViewModel vm)
        {
            var dto = new BookDto()
            {
                Id = vm.Id,
                Title = vm.Title
            };

            var result = await _bookServices.Update(dto);

            if(result == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var book = await _libraryContext.Books.FindAsync(id);

            if (book == null)
            { 
                return NotFound(); 
            }

            var vm = new BookDeleteViewModel
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                PublishedYear = book.PublishedYear,
                IsAvailable = book.IsAvailable

            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmation(Guid id)
        {
            var result = await _bookServices.Delete(id);

            if (result == null)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
