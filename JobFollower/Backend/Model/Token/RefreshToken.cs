using System.ComponentModel.DataAnnotations;

namespace JobFollower.Backend.Model.Token
{
    public class RefreshToken
    {
        [Key]
        public int Id { get; set; }

        public required string TokenHash { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }

        public DateTime ExpiresAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsRevoked { get; set; } = false;

        public int? ReplacedByTokenId { get; set; }

    }
}
