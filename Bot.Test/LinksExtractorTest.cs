using System;
using System.Collections.Generic;
using Bot.Utils;
using Xunit;

namespace Bot.Test
{
    public class LinksExtractorTest
    {
        [Theory]
        [InlineData(
            "Hello here is a link https://somewebsite.com/article.html",
            "https://somewebsite.com/article.html")]
        [InlineData(
            @"Hello here is a link https://somewebsite.com/article.html 
            and also please process this: https://stackoverflow.com/questions/16647140/return-value-using-string-result-command-executescalar-error-occurs-when-resul#16647283 thanks!",
            "https://stackoverflow.com/questions/16647140/return-value-using-string-result-command-executescalar-error-occurs-when-resul#16647283")]
        [InlineData(
            @"Hello here is a link https://somewebsite.com/article.html 
            and also please process this: https://stackoverflow.com/questions/16647140/return-value-using-string-result-command-executescalar-error-occurs-when-resul#16647283 thanks!",
            "https://somewebsite.com/article.html")]
        public void TestExtraction(string text, string expectedResult)
        {
            var extractor = new LinksExtractor(text);
            List<string> actualResult = extractor.ExtractLinks();
            bool contains = actualResult.Contains(expectedResult);
            Assert.True(contains);
        }

        [Theory]
        [InlineData(
            @"Hello here is a link https://somewebsite.com/article.html 
            and also please process this: https://stackoverflow.com/questions",
            2)]
        public void TestCount(string input, int expectedResult)
        {
            var extractor = new LinksExtractor(input);
            int actualResult = extractor.GetDetectedLinksCount();
            Assert.Equal(expectedResult, actualResult);
        }
    }
}
