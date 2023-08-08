using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace NotesWebApi.Domains.Entities;

public class NotesGroup
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public User Author { get; set; } = null!;
    public DateTime LastUpdateTime { get; set; }
    public List<Note> Notes { get; set; } = null!; 
}
public class NotesGroupConfiguration : IEntityTypeConfiguration<NotesGroup>
{
    public void Configure(EntityTypeBuilder<NotesGroup> builder)
    {
        builder.HasKey(g => g.Id);
        
        builder.Property(g => g.Title).IsRequired();
        builder.Property(g => g.LastUpdateTime).IsRequired().HasColumnType("datetime");

        builder.HasOne(g => g.Author).WithMany(u => u.Groups).HasForeignKey("UserId").OnDelete(DeleteBehavior.Cascade).IsRequired();

        builder.HasAlternateKey("Title", "UserId");
    }
}