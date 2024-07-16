using Moq;
using Moq.Protected;

namespace ApiAggregationService.Helpers;

public static class HttpClientMockHelper
{
    public static HttpClient CreateMockHttpClient(HttpResponseMessage responseMessage)
    {
        var handlerMock = new Mock<HttpMessageHandler>();

        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(responseMessage);

        var client = new HttpClient(handlerMock.Object);

        return client;
    }
}
