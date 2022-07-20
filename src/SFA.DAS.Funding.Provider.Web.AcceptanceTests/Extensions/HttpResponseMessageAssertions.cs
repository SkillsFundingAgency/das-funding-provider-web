using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;

namespace SFA.DAS.Funding.Provider.Web.AcceptanceTests.Extensions
{
    public class HttpResponseMessageAssertions : ReferenceTypeAssertions<HttpResponseMessage, HttpResponseMessageAssertions>
    {
        private readonly IHtmlDocument _document;

        public HttpResponseMessageAssertions(HttpResponseMessage instance) : base(instance)
        {
            var parser = new HtmlParser();
            _document = parser.ParseDocument(instance.Content.ReadAsStringAsync().Result);
        }

        protected override string Identifier => "HttpResponseMessage";

        public AndConstraint<HttpResponseMessageAssertions> HaveTitle(string title, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
             .BecauseOf(because, becauseArgs)
             .ForCondition(!string.IsNullOrEmpty(title))
             .FailWith("Title to assert on not provided")
             .Then
             .Given(() => _document.Title)
             .ForCondition(t => t.Equals(title))
             .FailWith("Expected {context:Title} to contain {0} but found {1}",
                 _ => title, item => item);

            return new AndConstraint<HttpResponseMessageAssertions>(this);
        }

        public AndConstraint<HttpResponseMessageAssertions> HaveBackLink(string link, string because = "", params object[] becauseArgs)
        {
            return HaveLink(".govuk-back-link", link, because, becauseArgs);
        }

        public AndConstraint<HttpResponseMessageAssertions> HaveLink(string selector, string link, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
            .BecauseOf(because, becauseArgs)
            .ForCondition(!string.IsNullOrEmpty(link))
            .FailWith("Link to assert on not provided")
            .Then
            .Given(() => _document.DocumentElement.QuerySelector(selector)?.Attributes["href"]?.Value)
            .ForCondition(t => _document.DocumentElement.QuerySelector(selector)?.Attributes["href"]?.Value == link)
            .FailWith("Expected {context:DocumentElement} to contain {0} but found {1}",
                _ => link, item => item);

            return new AndConstraint<HttpResponseMessageAssertions>(this);
        }

        public AndConstraint<HttpResponseMessageAssertions> HaveButton(string selector, string buttonText, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
            .BecauseOf(because, becauseArgs)
            .ForCondition(!string.IsNullOrEmpty(buttonText))
            .FailWith("Button to assert on not provided")
            .Then
            .Given(() => _document.DocumentElement.QuerySelector(selector)?.InnerHtml)
            .ForCondition(t => _document.DocumentElement.QuerySelector(selector)?.InnerHtml?.Trim() == buttonText.Trim())
            .FailWith("Expected {context:DocumentElement} to contain {0} but found {1}",
                _ => buttonText, item => item);

            return new AndConstraint<HttpResponseMessageAssertions>(this);
        }
        public AndConstraint<HttpResponseMessageAssertions> HaveForm(string action, string because = "", params object[] becauseArgs)
        {            
            Execute.Assertion
            .BecauseOf(because, becauseArgs)
            .ForCondition(!string.IsNullOrEmpty(action))
            .FailWith("Button to assert on not provided")
            .Then
            .Given(() => _document.DocumentElement.QuerySelector("form[method=\"post\"]")?.Attributes["action"]?.Value)
            .ForCondition(t => _document.DocumentElement.QuerySelector("form[method=\"post\"]")?.Attributes["action"]?.Value == action)
            .FailWith("Expected {context:DocumentElement} to contain {0} but found {1}",
                _ => action, item => item);

            return new AndConstraint<HttpResponseMessageAssertions>(this);
        }

        public AndConstraint<HttpResponseMessageAssertions> HaveInnerHtml(string selector, string innerHtml, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
            .BecauseOf(because, becauseArgs)
            .ForCondition(!string.IsNullOrEmpty(innerHtml))
            .FailWith("InnerHtml to assert on not provided")
            .Then
            .Given(() => _document.DocumentElement.QuerySelector(selector))
            .ForCondition(t => _document.DocumentElement.QuerySelector(selector).InnerHtml == innerHtml)
            .FailWith("Expected {context:DocumentElement} to contain {0} but found {1}",
                _ => innerHtml, item => item.InnerHtml);

            return new AndConstraint<HttpResponseMessageAssertions>(this);
        }

        public AndConstraint<HttpResponseMessageAssertions> NotHaveInnerHtml(string selector, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(!string.IsNullOrEmpty(selector))
                .FailWith("Selector not provided")
                .Then
                .Given(() => _document.DocumentElement.QuerySelector(selector))
                .ForCondition(t => _document.DocumentElement.QuerySelector(selector)?.InnerHtml == null)
                .FailWith("Expected {context:DocumentElement} not to contain {0}",
                    _ => selector);

            return new AndConstraint<HttpResponseMessageAssertions>(this);
        }

        public AndConstraint<HttpResponseMessageAssertions> NotHaveLink(string link, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
            .BecauseOf(because, becauseArgs)
            .ForCondition(!string.IsNullOrEmpty(link))
            .FailWith("Link to assert on not provided")
            .Then
            .Given(() => _document.DocumentElement.QuerySelectorAll(".govuk-link").Select(i => i.Attributes["href"]!.Value))
            .ForCondition(t => !_document.DocumentElement.QuerySelectorAll(".govuk-link").Select(i => i.Attributes["href"].Value).Contains(link))
            .FailWith("Expected {context:DocumentElement} to not contain {0} link but one was found", _ => link);

            return new AndConstraint<HttpResponseMessageAssertions>(this);
        }

        public AndConstraint<HttpResponseMessageAssertions> HavePathAndQuery(string pathAndQuery, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
             .BecauseOf(because, becauseArgs)
             .ForCondition(!string.IsNullOrEmpty(pathAndQuery))
             .FailWith("PathAndQuery to assert on not provided")
             .Then
             .Given(() => Subject.RequestMessage?.RequestUri?.PathAndQuery)
             .ForCondition(t => t.Equals(pathAndQuery))
             .FailWith("Expected {context:PathAndQuery} to contain {0} but found {1}",
                 _ => pathAndQuery, item => item);

            return new AndConstraint<HttpResponseMessageAssertions>(this);
        }
    }
}
