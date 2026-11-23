using Library.Core.Domain;
using Library.Core.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Core.ServiceInterface
{
    public interface IBookServices
    {
        Task<Book> Create(BookDto dto);
        Task<Book> Update(BookDto dto);
        Task<Book> Delete(Guid Id);
    }
}
