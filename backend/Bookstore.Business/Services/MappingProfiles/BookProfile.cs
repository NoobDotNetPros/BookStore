using AutoMapper;
using Bookstore.Models.DTOs;
using Bookstore.Models.Entities;

namespace Bookstore.Business.Services.MappingProfiles;

public class BookProfile : Profile
{
    public BookProfile()
    {
        CreateMap<Book, BookDto>();
    }
}
