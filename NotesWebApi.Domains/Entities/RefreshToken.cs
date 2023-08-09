using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace NotesWebApi.Domains.Entities;

public class RefreshToken
{
    public Guid Token { get; set; }
    public DateTime Created { get; set; } = DateTime.UtcNow;
    public DateTime Expires { get; set; }
    public User Owner { get; set; } = null!;
}

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.HasKey(t => t.Token);

        builder.Property(t => t.Created).IsRequired().HasColumnType("datetime");
        builder.Property(t => t.Expires).IsRequired().HasColumnType("datetime");

        builder.HasOne(t => t.Owner).WithMany(u => u.RefreshTokens).HasForeignKey("UserId").OnDelete(DeleteBehavior.Cascade).IsRequired();
    }
}
