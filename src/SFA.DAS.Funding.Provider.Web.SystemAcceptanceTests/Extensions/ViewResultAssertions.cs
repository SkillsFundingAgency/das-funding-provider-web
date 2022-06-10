using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.Funding.Provider.Web.SystemAcceptanceTests.Extensions
{
    public class ViewResultAssertions : ReferenceTypeAssertions<ViewResult, ViewResultAssertions>
    {
        public ViewResultAssertions(ViewResult instance) : base(instance)
        {
        }

        protected override string Identifier => "ViewResult";

        public AndConstraint<ViewResultAssertions> ContainError(string modelStateKey, string errorMessage, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
            .BecauseOf(because, becauseArgs)
            .ForCondition(!string.IsNullOrEmpty(modelStateKey) && !string.IsNullOrEmpty(errorMessage))
            .FailWith("ModelStateKey and error message not provided")
            .Then
            .Given(() => Subject.ViewData.ModelState)
            .ForCondition(e => e.ContainsKey(modelStateKey) && e[modelStateKey]!.Errors.Any(i => i.ErrorMessage == errorMessage))
            .FailWith("Expected {context:ModelState} to contain item with key {0} with error collection containing error {1} but found none",
                _ => modelStateKey, _ => errorMessage);

            return new AndConstraint<ViewResultAssertions>(this);
        }
    }
}
