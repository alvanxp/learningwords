namespace LearningWordsApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Language")]
    public partial class Language
    {
        public Language()
        {
            WordLearneds = new HashSet<WordLearned>();
        }

        public int ID { get; set; }

        [Required]
        [StringLength(10)]
        public string LanguageCode { get; set; }

        [StringLength(250)]
        public string Description { get; set; }

        public virtual ICollection<WordLearned> WordLearneds { get; set; }
    }
}
