using System.ComponentModel.DataAnnotations;

namespace JobFollower.Backend.Helpers
{
    public class ValidationHelper
    {
        public static List<ValidationResult> Validate<T>(T obj)
        {
            var context = new ValidationContext(obj!);
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(obj!, context, results, validateAllProperties: true);
            return results;
        }
        public static Dictionary<string, string[]> ToValidationDictionary(List<ValidationResult> results)
        {
            return results.SelectMany(x => x.MemberNames, (result, membernames) => new { membernames, result.ErrorMessage }).GroupBy(x => x.membernames).ToDictionary(g => g.Key, g => g.Select(x => x.ErrorMessage).ToArray());
        }
    }
}
