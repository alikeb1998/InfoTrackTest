using System.Net;
using Moq;
using Moq.Protected;
using SECrawler.Application.Services.Implementations;

namespace SECrawler.Tests.Services;

[TestFixture]
public class HttpClientWrapperTests
{
    private Mock<HttpMessageHandler> _httpMessageHandlerMock = null!;
    private HttpClientWrapper _clientWrapper = null!;

    [SetUp]
    public void SetUp()
    {
        _httpMessageHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

        var client = new HttpClient(_httpMessageHandlerMock.Object)
        {
            BaseAddress = new System.Uri("https://example.com")
        };

        _clientWrapper = new HttpClientWrapper(client);
    }

    [Test]
    public async Task GetStringAsync_ShouldSendRequestWithHeaders_AndReturnResponseContent()
    {
        // Arrange
        var expectedContent = "response body";
        var requestUrl = "https://example.com/test";
        var headers = new Dictionary<string, string>
        {
            { "User-Agent", "TestAgent" },
            { "Authorization", "Bearer token" }
        };

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get &&
                    req.RequestUri!.ToString() == requestUrl &&
                    req.Headers.UserAgent.ToString() == "TestAgent" &&
                    req.Headers.Authorization!.ToString() == "Bearer token"
                ),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(expectedContent)
            });

        // Act
        var result = await _clientWrapper.GetStringAsync(requestUrl, headers);

        // Assert
        Assert.That(result, Is.EqualTo(expectedContent));
    }
}