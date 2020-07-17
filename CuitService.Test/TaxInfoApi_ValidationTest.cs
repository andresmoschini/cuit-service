using AutoFixture;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace CuitService.Test
{
    public class TaxInfoApi_ValidationTest : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;
        private readonly Fixture _specimenBuilders;

        public TaxInfoApi_ValidationTest(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _specimenBuilders = new Fixture();
        }

        [Theory]
        [InlineData("20-31111111-8")]
        [InlineData("20-31111111-6")]
        [InlineData("20-31111111-1")]
        public async Task GET_taxinfo_by_cuit_with_an_invalid_verification_digit_should_return_400_BadRequest(string cuit)
        {
            // Arrange
            var client = _factory.WithBypassAuthorization().CreateClient();

            // Act
            var response = await client.GetAsync($"https://custom.domain.com/taxinfo/by-cuit/{cuit}");

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            // TODO: verify that the provider has not been called

            var content = await response.Content.ReadAsStringAsync();
            var problemDetail = JsonSerializer.Deserialize<JsonElement>(content);
            Assert.Equal("One or more validation errors occurred.", problemDetail.GetProperty("title").GetString());
            Assert.Collection(problemDetail.GetProperty("errors").EnumerateObject(),
                item =>
                {
                    Assert.Equal("cuit", item.Name);
                    Assert.Equal(1, item.Value.GetArrayLength());
                    Assert.Equal("The CUIT's verification digit is wrong.", item.Value.EnumerateArray().First().GetString());
                });
        }

        [Theory]
        [InlineData("20-3111111-8")]
        [InlineData("20-311111111-6")]
        public async Task GET_taxinfo_by_cuit_with_wrong_length_should_return_400_BadRequest(string cuit)
        {
            // Arrange
            var client = _factory.WithBypassAuthorization().CreateClient();

            // Act
            var response = await client.GetAsync($"https://custom.domain.com/taxinfo/by-cuit/{cuit}");

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            // TODO: verify that the provider has not been called

            var content = await response.Content.ReadAsStringAsync();
            var problemDetail = JsonSerializer.Deserialize<JsonElement>(content);
            Assert.Equal("One or more validation errors occurred.", problemDetail.GetProperty("title").GetString());
            Assert.Collection(problemDetail.GetProperty("errors").EnumerateObject(),
                item =>
                {
                    Assert.Equal("cuit", item.Name);
                    Assert.Equal(1, item.Value.GetArrayLength());
                    Assert.Equal("The CUIT number cannot have less than 11 numbers.", item.Value.EnumerateArray().First().GetString());
                });
        }

        [Theory]
        [InlineData("%20%20")]
        [InlineData("%20 %20%20")]
        public async Task GET_taxinfo_by_cuit_with_spaces_should_return_400_BadRequest(string cuit)
        {
            // Arrange
            var client = _factory.WithBypassAuthorization().CreateClient();

            // Act
            var response = await client.GetAsync($"https://custom.domain.com/taxinfo/by-cuit/{cuit}");

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            // TODO: verify that the provider has not been called

            var content = await response.Content.ReadAsStringAsync();
            var problemDetail = JsonSerializer.Deserialize<JsonElement>(content);
            Assert.Equal("One or more validation errors occurred.", problemDetail.GetProperty("title").GetString());
            Assert.Collection(problemDetail.GetProperty("errors").EnumerateObject(),
                item =>
                {
                    Assert.Equal("cuit", item.Name);
                    Assert.Equal(1, item.Value.GetArrayLength());
                    Assert.Equal("The cuit field is required.", item.Value.EnumerateArray().First().GetString());
                });
        }

        [Theory]
        [InlineData("-")]
        [InlineData("-----")]
        [InlineData("%20%20-")]
        [InlineData("-%20%20-")]
        public async Task GET_taxinfo_by_cuit_with_dashes_or_spaces_should_return_400_BadRequest(string cuit)
        {
            // Arrange
            var client = _factory.WithBypassAuthorization().CreateClient();

            // Act
            var response = await client.GetAsync($"https://custom.domain.com/taxinfo/by-cuit/{cuit}");

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            // TODO: verify that the provider has not been called

            var content = await response.Content.ReadAsStringAsync();
            var problemDetail = JsonSerializer.Deserialize<JsonElement>(content);
            Assert.Equal("One or more validation errors occurred.", problemDetail.GetProperty("title").GetString());
            Assert.Collection(problemDetail.GetProperty("errors").EnumerateObject(),
                item =>
                {
                    Assert.Equal("cuit", item.Name);
                    Assert.Equal(1, item.Value.GetArrayLength());
                    Assert.Equal("The CUIT number cannot be empty.", item.Value.EnumerateArray().First().GetString());
                });
        }

        [Theory]
        [InlineData("1234a5890")]
        [InlineData("1234a")]
        [InlineData("20 31111111 7")]
        [InlineData("20x31111111x7")]
        [InlineData("20,31111111,7")]
        [InlineData("20_31111111_7")]
        public async Task GET_taxinfo_by_cuit_with_invalid_characters_should_return_400_BadRequest(string cuit)
        {
            // Arrange
            var client = _factory.WithBypassAuthorization().CreateClient();

            // Act
            var response = await client.GetAsync($"https://custom.domain.com/taxinfo/by-cuit/{cuit}");

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            // TODO: verify that the provider has not been called

            var content = await response.Content.ReadAsStringAsync();
            var problemDetail = JsonSerializer.Deserialize<JsonElement>(content);
            Assert.Equal("One or more validation errors occurred.", problemDetail.GetProperty("title").GetString());
            Assert.Collection(problemDetail.GetProperty("errors").EnumerateObject(),
                item =>
                {
                    Assert.Equal("cuit", item.Name);
                    Assert.Equal(1, item.Value.GetArrayLength());
                    Assert.Equal("The CUIT number cannot have other characters than numbers and dashes.", item.Value.EnumerateArray().First().GetString());
                });
        }

        [Theory]
        [InlineData("")]
        [InlineData("     ")]
        public async Task GET_taxinfo_by_cuit_without_segment_should_return_404_NotFound(string cuit)
        {
            // Arrange
            var client = _factory.WithBypassAuthorization().CreateClient();

            // Act
            var response = await client.GetAsync($"https://custom.domain.com/taxinfo/by-cuit/{cuit}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            // TODO: verify that the provider has not been called
        }


        [Theory]
        [InlineData("20311111117")]
        [InlineData("33123456780")]
        [InlineData("20-31111111-7")]
        [InlineData("3-3-1-2-3-4-5-6-7-8-0")]
        public async Task GET_taxinfo_by_cuit_should_accept_valid_cuit_numbers(string cuit)
        {
            // Arrange
            var client = _factory.WithBypassAuthorization().CreateClient();

            // Act
            var response = await client.GetAsync($"https://custom.domain.com/taxinfo/by-cuit/{cuit}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            // TODO: verify that the provider has been called
        }
    }
}
