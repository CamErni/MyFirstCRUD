﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RepositoryPattern.API.Domain.Contracts;
using RepositoryPattern.API.Domain.Entities;
using RepositoryPattern.API.Repositories;

namespace RepositoryPattern.API.Controllers
{
    [ApiController]
    [Route("api/")]
    public class BookController: ControllerBase
    {
        private readonly IBaseRepository<Book> _bookRepository;
        private readonly IMapper _mapper;

        public BookController(IBaseRepository<Book> bookRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
        }
        [HttpGet("books")]
        public async Task<IActionResult> GetBooks()
        {
            var books = await _bookRepository.GetAll(a=>a.Author);
            var bookDtos=_mapper.Map<List<GetBookDto>>(books);
            return Ok(bookDtos);
        }

        [HttpGet("books/{id}")]
        public async Task<IActionResult> GetBook(Guid id)
        {
            var book = await _bookRepository.Get(id, a=>a.Author);
            if (book == null)
            {
                return NotFound();
            }
            var bookDto=_mapper.Map<GetBookDto>(book);
            return Ok(bookDto);
        }
        [HttpPost("books")]
        public async Task<IActionResult> CreateBook([FromBody] CreateBook createbook)
        {
            var book = _mapper.Map<Book>(createbook);
            var createdBook=await _bookRepository.Add(book);
            var bookDto = _mapper.Map<GetBookDto>(createdBook);
            return CreatedAtAction(nameof(GetBook), new { id = bookDto.Id },bookDto);
        }
    }
}
