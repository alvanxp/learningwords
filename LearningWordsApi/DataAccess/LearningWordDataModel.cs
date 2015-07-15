using LearningWordsApi.Models;

namespace LearningWordsApi.DataAccess
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class LearningWordDataModel : DbContext
    {
        public LearningWordDataModel()
            : base("name=LearningWordDataModel1")
        {
        }

        public virtual DbSet<Language> Languages { get; set; }
        public virtual DbSet<WordLearned> WordLearneds { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Language>()
                .Property(e => e.LanguageCode)
                .IsUnicode(false);

            modelBuilder.Entity<Language>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Language>()
                .HasMany(e => e.WordLearneds)
                .WithRequired(e => e.Language)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<WordLearned>()
                .Property(e => e.Description)
                .IsUnicode(false);
        }
    }
}
