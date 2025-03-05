using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace GeoHub.Entities;

public class BaseEntity
{
    public BaseEntity()
    {
        CreatedOn = DateTime.UtcNow;
        UpdatedOn = DateTime.UtcNow;
    }

    [Key]
    public int Id { get; set; }

    public DateTime CreatedOn { get; set; }


    public DateTime UpdatedOn { get; set; }
}