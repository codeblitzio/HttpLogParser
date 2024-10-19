using HttpLogParser.Models;
using HttpLogParser.Repositories;
using Microsoft.Extensions.Logging;
using Moq;

namespace HttpLogParser.Tests;

public class RepositoryTests
{
    readonly ILogger<InMemoryRepository> _logger = new Mock<ILogger<InMemoryRepository>>().Object;

    [Fact]
    public void Repository_Returns_Unique_IP_Count()
    {
        // arrange
        var sut = new InMemoryRepository(_logger);

        var httpLogEntry1 = new HttpLogEntry
        {   
             Ip = "127.0.0.1"
        };

        var httpLogEntry2 = new HttpLogEntry
        {   
             Ip = "127.0.0.1"
        };

        var httpLogEntry3 = new HttpLogEntry
        {   
             Ip = "127.0.0.2"
        };

        // act
        sut.AddHttpLogEntry(httpLogEntry1);
        sut.AddHttpLogEntry(httpLogEntry2);
        sut.AddHttpLogEntry(httpLogEntry3);

        var result = sut.GetUniqueIpCount;

        // assert
        Assert.Equal(2, result);
    }

    [Fact]
    public void Repository_Returns_Top_3_Most_Active_Ips()
    {
        // arrange
        var sut = new InMemoryRepository(_logger);

        var httpLogEntry1 = new HttpLogEntry
        {      
             Ip = "127.0.0.1"
        };

        var httpLogEntry2 = new HttpLogEntry
        {   
             Ip = "127.0.0.1"
        };

        var httpLogEntry3 = new HttpLogEntry
        {   
             Ip = "127.0.0.1"
        };

        var httpLogEntry4 = new HttpLogEntry
        {   
             Ip = "127.0.0.2"
        };

        var httpLogEntry5 = new HttpLogEntry
        {   
             Ip = "127.0.0.2"
        };

        var httpLogEntry6 = new HttpLogEntry
        {   
             Ip = "127.0.0.3"
        };

        var httpLogEntry7 = new HttpLogEntry
        {   
             Ip = "127.0.0.3"
        };

        var httpLogEntry8 = new HttpLogEntry
        {   
             Ip = "127.0.0.4"
        };

        // act
        sut.AddHttpLogEntry(httpLogEntry1);
        sut.AddHttpLogEntry(httpLogEntry2);
        sut.AddHttpLogEntry(httpLogEntry3);
        sut.AddHttpLogEntry(httpLogEntry4);
        sut.AddHttpLogEntry(httpLogEntry5);
        sut.AddHttpLogEntry(httpLogEntry6);
        sut.AddHttpLogEntry(httpLogEntry7);
        sut.AddHttpLogEntry(httpLogEntry8);

        var result = sut.MostActiveIps;

        // assert
        Assert.Equal("127.0.0.1", result.ToArray()[0]);
        Assert.Equal("127.0.0.2", result.ToArray()[1]);
        Assert.Equal("127.0.0.3", result.ToArray()[2]);
    }

        [Fact]
    public void Repository_Returns_Top_3_Most_Visited_Urls()
    {
        // arrange
        var sut = new InMemoryRepository(_logger);

        var httpLogEntry1 = new HttpLogEntry
        {      
              Uri = "/index.js"
        };

        var httpLogEntry2 = new HttpLogEntry
        {   
             Uri = "/index.js"
        };

        var httpLogEntry3 = new HttpLogEntry
        {   
             Uri = "/index.js"
        };

        var httpLogEntry4 = new HttpLogEntry
        {   
             Uri = "/"
        };

        var httpLogEntry5 = new HttpLogEntry
        {   
             Uri = "/"
        };

        var httpLogEntry6 = new HttpLogEntry
        {   
             Uri = "/about"
        };

        var httpLogEntry7 = new HttpLogEntry
        {   
             Uri = "/about"
        };

        var httpLogEntry8 = new HttpLogEntry
        {   
             Uri = "http://google.com"
        };

        // act
        sut.AddHttpLogEntry(httpLogEntry1);
        sut.AddHttpLogEntry(httpLogEntry2);
        sut.AddHttpLogEntry(httpLogEntry3);
        sut.AddHttpLogEntry(httpLogEntry4);
        sut.AddHttpLogEntry(httpLogEntry5);
        sut.AddHttpLogEntry(httpLogEntry6);
        sut.AddHttpLogEntry(httpLogEntry7);
        sut.AddHttpLogEntry(httpLogEntry8);

        var result = sut.MostVisitedUrls;

        // assert
        Assert.Equal("/index.js", result.ToArray()[0]);
        Assert.Equal("/", result.ToArray()[1]);
        Assert.Equal("/about", result.ToArray()[2]);
    }
}