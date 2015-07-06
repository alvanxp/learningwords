namespace LearningWordsApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("WordLearned")]
    public partial class WordLearned
    {
        public int ID { get; set; }

        public Guid WordId { get; set; }

        [Required]
        [StringLength(150)]
        public string Word { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string Description { get; set; }

        public int? LanguageId { get; set; }

        public virtual Language Language { get; set; }
    }
}
