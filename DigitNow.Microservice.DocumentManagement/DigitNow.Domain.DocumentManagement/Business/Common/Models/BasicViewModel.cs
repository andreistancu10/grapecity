using Microsoft.EntityFrameworkCore.Query.Internal;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Models;

public class BasicViewModel
{
    public long Id { get; set; }
    public string Name { get; set; }

    public BasicViewModel(long id, string name)
    {
        Id = id;
        Name = name;
    }
}