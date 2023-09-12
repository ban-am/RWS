using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Configurations;

public class DatabaseSettings : IValidatableObject
{
    public const string Key = nameof(DatabaseSettings);

    public string ConnectionString { get; set; } = string.Empty;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrEmpty(ConnectionString))
        {
            yield return new ValidationResult($"{nameof(DatabaseSettings)}.{nameof(ConnectionString)} is not configured", new[] { nameof(ConnectionString) });
        }
    }
}
