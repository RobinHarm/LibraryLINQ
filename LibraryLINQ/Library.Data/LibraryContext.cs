using Library.Core.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Data
{
    // LibraryContext on andmebaasi konteksti klass,
    // mille kaudu rakendus suhtleb andmebaasiga.
    public class LibraryContext : DbContext
    {
        // Konstruktor võtab vastu andmebaasiühenduse seadistused.
        public LibraryContext(DbContextOptions<LibraryContext> options) : base(options)
        {

        }
        // DbSet määrab andmebaasi tabelid.
        public DbSet<Book> Books  { get; set; }
        public DbSet<Loan> Loans  { get; set; }
        public DbSet<Reader> Readers  { get; set; }
    }
}
