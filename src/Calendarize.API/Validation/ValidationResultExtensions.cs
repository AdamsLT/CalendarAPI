using FluentValidation.Results;
using System.Collections.Generic;

namespace Calendarize.API.Validation
{
    public static class ValidationResultExtensions
    {
        public static Dictionary<string, object> ToErrorDictionary(this ValidationResult validationResult)
        {
            var elementsList = new Dictionary<string, object>();

            foreach (var failure in validationResult.Errors)
            {
                var key = failure.PropertyName;
                if (!elementsList.ContainsKey(key))
                {
                    var messageObject = new List<object>
                    {
                        new
                        {
                            reason = failure.CustomState.ToString(),
                            message = failure.ErrorMessage
                        }
                    };

                    elementsList.Add(key, messageObject);
                }
                else
                {
                    ((List<object>)elementsList[key]).Add(new
                    {
                        reason = failure.CustomState.ToString(),
                        message = failure.ErrorMessage
                    });
                }
            }

            return elementsList;
        }
    }
}
