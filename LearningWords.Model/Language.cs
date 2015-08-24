using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningWords.Model
{
    [Table("Language")]
    public partial class Language
    {
        public Language()
        {
            WordLearneds = new HashSet<WordLearned>();
        }

        public int ID { get; set; }

        [StringLength(25)]
        public string LanguageCode { get; set; }

        [Column(TypeName = "text")]
        public string Description { get; set; }

        public virtual ICollection<WordLearned> WordLearneds { get; set; }
    }
}
