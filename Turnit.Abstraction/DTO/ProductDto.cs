namespace Turnit.Abstraction.DTO;

public class ProductDto
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public IEnumerable<AvailabilityDto> Availability { get; set; }
}