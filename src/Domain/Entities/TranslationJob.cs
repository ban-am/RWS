using Domain.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public class TranslationJob
{
    public int Id { get; set; }
    public string CustomerName { get; set; }
    public JobStatus Status { get; set; }
    public string OriginalContent { get; set; }
    public string TranslatedContent { get; set; }
    public double Price { get; set; }

    public int? TranslatorId { get; set; }
    public Translator Translator { get; set; }
}