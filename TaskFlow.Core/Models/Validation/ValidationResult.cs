namespace TaskFlow.Core.Models.Validation;

public class ValidationResult
{
    public bool IsValid => Errors.Count == 0;
    public List<string> Errors { get; } = new();

    public void AddError(string error)
    {
        Errors.Add(error);
    }

    public static ValidationResult Success()
    {
        return new ValidationResult();
    }

    public static ValidationResult Error(string error)
    {
        var result = new ValidationResult();
        result.AddError(error);
        return result;
    }

    public static ValidationResult Error(IEnumerable<string> errors)
    {
        var result = new ValidationResult();
        foreach (var error in errors)
        {
            result.AddError(error);
        }
        return result;
    }
}
