using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestApp1.Database;
using TestApp1.Entities;
using TestApp1.Services;

namespace TestApp1.Controllers;

[ApiController] 
public class TestController: ControllerBase
{
    private DatabaseContext DatabaseContext;
        
    public TestController(DatabaseContext databaseContext, TestService testService) 
    {
        DatabaseContext = databaseContext;
    }
    
    [HttpGet("/getData")]
    public async Task<IActionResult> GetData() 
    {
        return Ok("Hello World");
    }

    [HttpPost("/createProduct")]
    public async Task<IActionResult> CreateProduct([FromBody] Product product)
    {
        DatabaseContext.Products.Add(product);
        await DatabaseContext.SaveChangesAsync();
        return Ok();
    }
    
    // реализовать метод, который создаст несколько продуктов? загуглить! 0,5 часа - Владу (массив продуктов)   
    [HttpPost("/createProducts")]
    public async Task<IActionResult> CreateProducts([FromBody] List<Product> products)
    {
        // прочитал, что лучше использовать AddRange, чтобы экономить ресурсы (?)
        DatabaseContext.Products.AddRange(products);
        await DatabaseContext.SaveChangesAsync();
        return Ok(products);
    }
    
    [HttpGet("/getProducts")]
    public async Task<IActionResult> GetProducts()
    {
        List<Product> result = await DatabaseContext.Products
            .ToListAsync();
        return Ok(result);
    }

    [HttpGet("/getProductById")]
    public async Task<IActionResult> GetProduct([FromQuery]int productId)
    {
        Product result = await DatabaseContext.Products
            .FirstOrDefaultAsync(product => product.Id == productId);
        
        // обработчик ошибки на отсутствие объекта в БД, код 204 
        if (result == null) 
        {
            return NoContent();
        }
        
        return Ok(result.Name);
    }

    [HttpDelete("/deleteProduct")]
    public async Task<IActionResult> DeleteProduct([FromQuery] string productName)
    {
        if (productName == null)
        {
            return BadRequest();
        }
        
        // Версия с прямым SQL-запросом в БД без загрузки объекта 
        await DatabaseContext.Products
            .Where(product => product.Name == productName)
            .ExecuteDeleteAsync();
        
        // Ниже представлена версия с загрузкой объекта в память.
        // Product productFromDB = await DatabaseContext.Products
        //     .FirstOrDefaultAsync(product => product.Name == productName);
        // DatabaseContext.Products.Remove(productFromDB);
        // await DatabaseContext.SaveChangesAsync();
        
        return Ok();
    }

    // реализовать метод, который удаляет массив продуктов.
    [HttpDelete("/deleteProducts")]
    public async Task<IActionResult> DeleteProducts([FromBody] List<string> productsToDelete)
    { 
        await DatabaseContext.Products
            .Where(product => productsToDelete.Contains(product.Name))
            .ExecuteDeleteAsync();
        return Ok();
        
        // вариант № 1 от которого я отказался, потому что узнал, что обращение к БД в цикле = хуйня ебаная
        // оставил просто чтобы было
        // foreach (string productName in productsToDelete)
        // {
        //     await DatabaseContext.Products
        //         .Where(product => product.Name == productName)
        //         .ExecuteDeleteAsync();
        // }
        // return Ok();
    }

    [HttpPatch("/updateProduct")]
    public async Task<IActionResult> UpdateProduct([FromQuery] string productName, [FromQuery] string productNewName)
    {
        Product product = await DatabaseContext.Products.FirstOrDefaultAsync(product => product.Name == productName);
        
        if (product == null)
        {
            return NoContent();
        }
        
        product.Name = productNewName;
        await DatabaseContext.SaveChangesAsync();
        return Ok();
    }
    
    // реализовать обновление нескольких продуктов
}