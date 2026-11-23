using Library.Core.Domain;
using Library.Core.Dto;
using Library.Core.ServiceInterface;
using Library.Data;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;



namespace Library.ApplicationServices.Services
{
    public class LoanServices : ILoanServices
    {
        private readonly LibraryContext _context;

        public LoanServices
            (
            LibraryContext context
            )
        {
            _context = context;
        }

        [HttpGet]
        public async Task<Loan> Create(LoanDto dto)
        {
            Loan domain = new();

            domain.Id = Guid.NewGuid();
            domain.BookId = dto.BookId;
            domain.ReaderId = dto.ReaderId;
            domain.LoanDate = dto.LoanDate;
            domain.ReturnDate = dto.ReturnDate;

            var book = await _context.Books.FindAsync(dto.BookId);

            if (book != null && book.IsAvailable)
            {
                book.IsAvailable = false;

                await _context.Loans.AddAsync(domain);
                await _context.SaveChangesAsync();
            }

            return domain;
        }
        
        public async Task<Loan> BookReturn(LoanDto dto)
        {
            var loan = await _context.Loans.FindAsync(dto.Id);

            if (loan == null || loan.ReturnDate != null)
            {
                return null;
            }

            var books = await _context.Books.FindAsync(loan.BookId);

            loan.ReturnDate = DateTime.Now;

            if (books != null)
            {
                books.IsAvailable = true;
            }

            await _context.SaveChangesAsync();

            return loan;
        }
    }
}
