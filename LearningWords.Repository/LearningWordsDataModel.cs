namespace LearningWords.Repository
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class LearningWordsDataModel : DbContext
    {
        public LearningWordsDataModel()
            : base("name=LearningWordsDataModel")
        {
        }

        public virtual DbSet<WordLearned> WordLearneds { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WordLearned>()
                .Property(e => e.Word)
                .IsUnicode(false);

            modelBuilder.Entity<WordLearned>()
                .Property(e => e.Language)
                .IsFixedLength()
                .IsUnicode(false);
        }

        public void AddWord(string wordLearned, string language)
        {
            this.WordLearneds.Add(new WordLearned()
            {
                Language = language,
                Word = wordLearned
            });
        }



    }
}
