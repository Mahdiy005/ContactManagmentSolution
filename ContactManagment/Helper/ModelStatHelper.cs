using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ContactManagment.Helper
{
    public static class ModelStatHelper
    {
        public static Dictionary<string, string[]> GetErrors(this ModelStateDictionary modelState)
        {
            return modelState
                .Where(ms => ms.Value?.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                );
        }
    }
}
