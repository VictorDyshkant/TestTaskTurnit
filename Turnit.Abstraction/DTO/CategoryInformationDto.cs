namespace Turnit.Abstraction.DTO;

public class CategoryInformationDto
{
    public Guid? CategoryId { get; set; }

    public IEnumerable<ProductDto> Products { get; set; }
}