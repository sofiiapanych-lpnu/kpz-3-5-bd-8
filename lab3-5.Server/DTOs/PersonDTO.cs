using lab3_5.Server.Models;

public class PersonDTO
{
    public int PersonId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public virtual Client? Client { get; set; }

    public virtual Courier? Courier { get; set; }
}