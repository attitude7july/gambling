using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gambling.Domain.Entities;
public class ApplicationUserBet
{
    [Key]
    public Guid Id { get; set; }

    [ForeignKey(nameof(UserId))]
    public virtual ApplicationUser User { get; set; }

    [Required]
    public Guid UserId { get; set; }

    [Required]
    public DateTime CreatedOn { get; set; }

    public DateTime? LastModifiedOn { get; set; }

    [Required]
    public double BetAmount { get; set; }

    [Required]
    public int NumberofChoice { get; set;}

    [Required]
    public int LotNumberSelected { get; set; }

    [Required]
    public Status Status { get; set; }


}
