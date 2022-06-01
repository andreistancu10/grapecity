using System.Linq;
using FluentValidation;

namespace DigitNow.Domain.DocumentManagement.extensions.Validators
{
    public static class RuleBuilderExtensions
    {
        /// <summary>
        ///     Ensures that the specified property does not contain numbers.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ruleBuilder"></param>
        /// <returns></returns>
        public static IRuleBuilder<T, string> NoNumbers<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Must(value => !value.Any(char.IsDigit))
                .WithMessage("TenantNotification.common.noNumbers");
        }
    }
}