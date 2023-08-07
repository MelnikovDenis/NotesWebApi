using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace NotesWebApi.Domains.Entities;

public class Note
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Text { get; set; } = null!;
    public DateTime CreationTime { get; set; }
    public DateTime LastUpdateTime { get; set; }
    public NotesGroup Group { get; set; } = null!;
}
public class NoteConfiguration : IEntityTypeConfiguration<Note>
{
    public void Configure(EntityTypeBuilder<Note> builder)
    {
        builder.HasKey(n => n.Id);

        builder.Property(n => n.Title).IsRequired();
        builder.Property(n => n.Text).IsRequired();
        builder.Property(n => n.CreationTime).IsRequired().HasColumnType("datetime");
        builder.Property(n => n.LastUpdateTime).IsRequired().HasColumnType("datetime");

        builder.HasOne(n => n.Group).WithMany(g => g.Notes).HasForeignKey("GroupId").OnDelete(DeleteBehavior.Cascade);
    }
}