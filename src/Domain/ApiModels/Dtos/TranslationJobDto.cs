using Domain.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.ApiModels.Dtos;

public class TranslationJobDto
{
    public int Id { get; set; }
    public string CustomerName { get; set; }
    public JobStatus Status { get; set; }
    public string OriginalContent { get; set; }
    public string TranslatedContent { get; set; }
    public double Price { get; set; }
}
