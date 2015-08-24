using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace LearningWords.Model
{
    [Table("WordLearned")]
    public partial class WordLearned
    {
        public long ID { get; set; }

        public Guid WordID { get; set; }

        [Microsoft.Build.Framework.Required]
        [StringLength(150)]
        public string Word { get; set; }

        public int LanguageID { get; set; }

        [Column(TypeName = "text")]
        public string Description { get; set; }
        [JsonIgnore]
        public virtual Language Language { get; set; }
    }
}