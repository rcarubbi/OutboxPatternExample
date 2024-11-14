using ApplicationLayer.Commands;
using ApplicationLayer.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController(ISender sender, ILogger<ProductController> logger) : ApiController(sender)
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateProduct(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var productId = await sender.Send(request, cancellationToken);

        return CreatedAtRoute("GetProductById", new { id = productId }, null);
    }

    [HttpPatch("update-price")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdatePrice(UpdateProductPriceCommand request, CancellationToken cancellationToken)
    {
        await sender.Send(request, cancellationToken);

        return NoContent();
    }

    [HttpGet("{id:Guid}", Name = "GetProductById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var product = await sender.Send(new GetProductByIdQuery(id), cancellationToken);

        if (product is null)
        {
            return NotFound();
        }

        return Ok(product);
    }
}
