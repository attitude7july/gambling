using System.ComponentModel.DataAnnotations;

namespace gambling.Domain.Entities;


public class ApplicationUser 
{
    [Key]
    public Guid UserId { get; set; }
    [Required]
    public DateTime CreatedOn{ get; set; }
    public DateTime? LastModifiedOn{ get; set; }
    public string UserName { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }

    [Required]
    public double Balance { get; set; }

    public virtual ICollection<ApplicationUserBet> Bets { get; set; }

}
