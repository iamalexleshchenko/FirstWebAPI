using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestApp1.Database;
using TestApp1.Entities;
using TestApp1.Services;

namespace TestApp1.Controllers;


[ApiController]
public class BookControllers: ControllerBase
{
    private DatabaseContext DatabaseContext;
        
    public BookControllers(DatabaseContext databaseContext, TestService testService) 
    {
        DatabaseContext = databaseContext;
    }
    
    [HttpPost("/createBook")]
    public async Task<IActionResult> CreateBook([FromBody] Book book)
    {
        DatabaseContext.Books.Add(book);
        await DatabaseContext.SaveChangesAsync();
        return Ok();
    }
    
    [HttpGet("/getListBook")]
    public async Task<IActionResult> GetBooks()
    {
        var result = await DatabaseContext.Books
            .ToListAsync();
        return Ok(result);
    }
    
     // ниже приведен аналог SQL на LINQ
    // SELECT title, author, price, amount 
    // FROM book
    // WHERE (price < 500 OR price > 600) AND price * amount >= 5000;
    [HttpGet ("/getBooks_1")]
    public async Task<IActionResult> Test()
    {
        List<BookDTO> result = await DatabaseContext.Books
            .Where(x => (x.Price < 500 || x.Price > 600) && x.Price * x.Amount >= 5000)
            .Select(x => new BookDTO()
            {
                Title = x.Title,
                Author = x.Author,
                Price = x.Price,
                Amount = x.Amount
            })
            .ToListAsync();
        return Ok(result);
    }
    
    // ниже приведен аналог SQL на LINQ
    // SELECT author, title
    // FROM book
    // WHERE amount BETWEEN 2 AND 14
    // ORDER BY author DESC, title ASC; 
    [HttpGet ("/getBooks_2")]
    public async Task<IActionResult> Test2()
    {
        var result = await DatabaseContext.Books
            .Where(x => (x.Amount >= 2 && x.Amount <= 14))
            .Select(x => new BookDTO()
            {
                Author = x.Author,
                Title = x.Title,
            })
            .OrderByDescending(x => x.Author)
            .ThenBy(x => x.Title)
            .ToListAsync();
        return Ok(result);
    }
    
    // ниже приведен аналог SQL на LINQ
    // SELECT title, author, price
    // FROM book
    // WHERE author LIKE "_%М.%"
    // ORDER BY price DESC;
    [HttpGet ("/GetBooks_3")]
    public async Task<IActionResult> Test3()
    {
        var result = await DatabaseContext.Books
            .Where(x => x.Author.Contains("М."))
            .Select(x => new BookDTO()
            {
                Title = x.Title,
                Author = x.Author,
                Price = x.Price,
            })
            .OrderByDescending(x => x.Price)
            .ToListAsync();
        return Ok(result);
    }
    
    // ниже приведен аналог SQL на LINQ
    // SELECT DISTINCT amount
    // FROM book;
    
    [HttpGet("/GetBooks_4")]
    public async Task<IActionResult> Test4()
    {
        var result = await DatabaseContext.Books
            .Select(x => x.Amount)
            .Distinct()
            .ToListAsync();
        return Ok(result);
    }
    
    // SELECT author AS Автор, COUNT(author) AS Различных_книг, SUM(amount) AS Количество_экземпляров
    // FROM book
    // GROUP BY author;

    [HttpGet("/GetBooks_5")]
    public async Task<IActionResult> Test5()
    {
        var result = await DatabaseContext.Books
            .GroupBy(x => x.Author)
            .Select(x => new 
            {
                Автор = x.Key,
                Различных_книг = x.Count(),
                Количество_экземпляров = x.Sum(y => y.Amount)
            })
            .ToListAsync();
        return Ok(result);
    }
    
    // SELECT author, title, price
    // FROM book 
    // WHERE (price - (SELECT MIN(price) FROM book)) <= 150
    // ORDER BY price;

    [HttpGet("/GetBooks_6")]
    public async Task<IActionResult> Test6()
    {
        var result =  await DatabaseContext.Books
            .Where(element => element.Price - DatabaseContext.Books.Min(books => books.Price) <= 150)
            .OrderBy(price => price)
            .Select(output => new
            {
                Author = output.Author,
                Title = output.Title,
                Price = output.Price,
                
            })
            .ToListAsync();
        return Ok(result);
    }
    
    
    // SELECT author, title, amount
    // FROM book
    // WHERE amount IN (
    //     SELECT amount
    //     FROM book 
    //     GROUP BY amount
    //     HAVING COUNT(amount)=1);

    [HttpGet("/GetBooks_7")]
    public async Task<IActionResult> Test7()
    {
        var result = await DatabaseContext.Books
            .Where(element =>
                DatabaseContext.Books
                    .GroupBy(element => element.Amount)
                    .Where(amount => amount.Count() == 1)
                    .Select(x => x.Key)
                    .Contains(element.Amount))
            .Select(result => new
            {
                result.Author,
                result.Title,
                result.Amount
            })
            .ToListAsync();
        return Ok(result);
    }

    class BookDTO // отдельный класс
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public decimal Price { get; set; }
        public int Amount { get; set; }
    }
}