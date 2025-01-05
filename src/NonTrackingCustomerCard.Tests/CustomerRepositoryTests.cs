using FluentAssertions;

using Microsoft.JSInterop;

using NonTrackingCustomerCard.Client.Contracts;
using NonTrackingCustomerCard.Client.Repositories;

using NSubstitute;

namespace NonTrackingCustomerCard.Tests.Client.Repositories
{
    public class CustomersRepositoryTest
    {
        [Fact]
        public async Task GetCustomerOfVendorDataAsync_ReturnsCustomerOfVendorData_WhenCustomerExistsInLocalStorage()
        {
            // Arrange
            var mockJSRuntime = Substitute.For<IJSRuntime>();
            var customerJson = """
                    {"ov":{"vn":"Test","puk":"MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAstD9xZOY6OAWziUa7\u002BeW5iFgBD/Ls3ntzqWuwxikswvLphsKEOfSQL341KP0TnMA6UFZKJ9MDiXC/aZrJ9D2qhrPrqCgQP3CRS2xzThsLa9hX2gu6VgnR/BbDZscmyuImi70dswZc5yC0aorGvlmPOlQEEPTMgPaCo2I8q6UtnhVDJ88SsIz2kNY/7YwNGZcWBbViY9btvOEF0n3DxgTpg0bSpTc8IuoQsNwqb5vEYwqCMziJNCnPQ3QON1XDG2nE5lFlBcQFjg8jEsXkhjvCIcdLD8WKdbMxs/jWWA28IHk/hDFAhtNcVATUbtb9rJ2sNkGRu9hIw7m7qAVY9NnyQIDAQAB"},"c":{"pts":40},"s":"kTAPB1o8Mj\u002BAh4uLLghWrIBwCsg260Bw/1IUAsBgU1P\u002BmnXUmEhaANbAFxXMBU81SXST\u002Bu7Po9oxx6aqcXqX4yHQFgE71e5I5xSx/0UUQOmCGzfiWk79TbIcI8QxMV2iGc1dGx3TdoM62JiHpCyn3kPry9yRo29ropUpVcvxcw3ckhIQSGAG14ELGbrK/sv0r\u002Bat/56BnVPazlL\u002B1CBzk928A2Vt7bxT1aZTLyINOnpD6AZfNqAxloGafqnJerHfp7wKqemLEB0YAkw6\u002BCh4KXUttJTL8YFqBTaQFxrMYRmyJ3VaR8RdPI1AQ61ArW3yh082\u002BJZv8mQNASGvcKxEtQ=="}
                """;
            mockJSRuntime
                .ArrangeLocalStorageGetItemForKeyReturns("NonTrackingCustomerCard.Client.Customer", customerJson);

            var repository = new LocalStorageCustomersRepository(mockJSRuntime);

            // Act
            var result = await repository.GetCustomerOfVendorDataAsync();

            // Assert
            await mockJSRuntime.AssertLocalStorageGetCallReceived("NonTrackingCustomerCard.Client.Customer");

            result.Should().NotBeNull();
            result.OfVendor.Should().NotBeNull();
            result.OfVendor.Name.Should().Be("Test");
            result
                .OfVendor.PublicKey.Should()
                .Be(
                    "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAstD9xZOY6OAWziUa7\u002BeW5iFgBD/Ls3ntzqWuwxikswvLphsKEOfSQL341KP0TnMA6UFZKJ9MDiXC/aZrJ9D2qhrPrqCgQP3CRS2xzThsLa9hX2gu6VgnR/BbDZscmyuImi70dswZc5yC0aorGvlmPOlQEEPTMgPaCo2I8q6UtnhVDJ88SsIz2kNY/7YwNGZcWBbViY9btvOEF0n3DxgTpg0bSpTc8IuoQsNwqb5vEYwqCMziJNCnPQ3QON1XDG2nE5lFlBcQFjg8jEsXkhjvCIcdLD8WKdbMxs/jWWA28IHk/hDFAhtNcVATUbtb9rJ2sNkGRu9hIw7m7qAVY9NnyQIDAQAB"
                );
            result.Customer.Points.Should().Be(40);
            result
                .Signature.Should()
                .Be(
                    "kTAPB1o8Mj\u002BAh4uLLghWrIBwCsg260Bw/1IUAsBgU1P\u002BmnXUmEhaANbAFxXMBU81SXST\u002Bu7Po9oxx6aqcXqX4yHQFgE71e5I5xSx/0UUQOmCGzfiWk79TbIcI8QxMV2iGc1dGx3TdoM62JiHpCyn3kPry9yRo29ropUpVcvxcw3ckhIQSGAG14ELGbrK/sv0r\u002Bat/56BnVPazlL\u002B1CBzk928A2Vt7bxT1aZTLyINOnpD6AZfNqAxloGafqnJerHfp7wKqemLEB0YAkw6\u002BCh4KXUttJTL8YFqBTaQFxrMYRmyJ3VaR8RdPI1AQ61ArW3yh082\u002BJZv8mQNASGvcKxEtQ=="
                );
        }

        [Fact]
        public async Task GetCustomerOfVendorDataAsync_ReturnsNewCustomerOfVendorData_WhenCustomerDoesNotExistInLocalStorage()
        {
            // Arrange
            var mockJSRuntime = Substitute.For<IJSRuntime>();
            mockJSRuntime
                .ArrangeLocalStorageGetItemForKeyReturns("NonTrackingCustomerCard.Client.Customer", (string)null);

            var repository = new LocalStorageCustomersRepository(mockJSRuntime);

            // Act
            var result = await repository.GetCustomerOfVendorDataAsync();

            // Assert
            await mockJSRuntime.AssertLocalStorageGetCallReceived("NonTrackingCustomerCard.Client.Customer");

            result.Should().NotBeNull();
            result.OfVendor.Should().NotBeNull();
            result.OfVendor.Name.Should().BeEmpty();
            result.OfVendor.PublicKey.Should().BeEmpty();
            result.Customer.Should().NotBeNull();
            result.Customer.Points.Should().Be(0);
            result.Signature.Should().BeEmpty();
        }

        [Fact]
        public async Task GetCustomerDataWithSignatureAsync_ReturnsEmptyCustomerDataWithSignature_WhenLocalStorageIsEmpty()
        {
            // Arrange
            var mockJSRuntime = Substitute.For<IJSRuntime>();
            mockJSRuntime
                .ArrangeLocalStorageGetItemForKeyReturns("NonTrackingCustomerCard.Client.Customer", (string)null);

            var repository = new LocalStorageCustomersRepository(mockJSRuntime);

            // Act
            var result = await repository.GetCustomerDataWithSignatureAsync();

            // Assert
            await mockJSRuntime.AssertLocalStorageGetCallReceived("NonTrackingCustomerCard.Client.Customer");

            result.Should().NotBeNull();
            result.Customer.Should().NotBeNull();
            result.Customer.Points.Should().Be(0);
            result.Signature.Should().BeEmpty();
        }

        [Fact]
        public async Task GetCustomerDataWithSignatureAsync_ReturnsCustomerDataWithSignature_WhenLocalStorageHasData()
        {
            // Arrange
            var mockJSRuntime = Substitute.For<IJSRuntime>();
            var customerJson = """
                    {"c":{"pts":40},"s":"kTAPB1o8Mj\u002BAh4uLLghWrIBwCsg260Bw/1IUAsBgU1P\u002BmnXUmEhaANbAFxXMBU81SXST\u002Bu7Po9oxx6aqcXqX4yHQFgE71e5I5xSx/0UUQOmCGzfiWk79TbIcI8QxMV2iGc1dGx3TdoM62JiHpCyn3kPry9yRo29ropUpVcvxcw3ckhIQSGAG14ELGbrK/sv0r\u002Bat/56BnVPazlL\u002B1CBzk928A2Vt7bxT1aZTLyINOnpD6AZfNqAxloGafqnJerHfp7wKqemLEB0YAkw6\u002BCh4KXUttJTL8YFqBTaQFxrMYRmyJ3VaR8RdPI1AQ61ArW3yh082\u002BJZv8mQNASGvcKxEtQ=="}
                """;

            mockJSRuntime
                .ArrangeLocalStorageGetItemForKeyReturns("NonTrackingCustomerCard.Client.Customer", customerJson);

            var repository = new LocalStorageCustomersRepository(mockJSRuntime);

            // Act
            var result = await repository.GetCustomerDataWithSignatureAsync();

            // Assert
            await mockJSRuntime.AssertLocalStorageGetCallReceived("NonTrackingCustomerCard.Client.Customer");

            result.Should().NotBeNull();
            result.Customer.Should().NotBeNull();
            result.Customer.Points.Should().Be(40);
            result
                .Signature.Should()
                .Be(
                    "kTAPB1o8Mj\u002BAh4uLLghWrIBwCsg260Bw/1IUAsBgU1P\u002BmnXUmEhaANbAFxXMBU81SXST\u002Bu7Po9oxx6aqcXqX4yHQFgE71e5I5xSx/0UUQOmCGzfiWk79TbIcI8QxMV2iGc1dGx3TdoM62JiHpCyn3kPry9yRo29ropUpVcvxcw3ckhIQSGAG14ELGbrK/sv0r\u002Bat/56BnVPazlL\u002B1CBzk928A2Vt7bxT1aZTLyINOnpD6AZfNqAxloGafqnJerHfp7wKqemLEB0YAkw6\u002BCh4KXUttJTL8YFqBTaQFxrMYRmyJ3VaR8RdPI1AQ61ArW3yh082\u002BJZv8mQNASGvcKxEtQ=="
                );
        }

        [Fact]
        public async Task SaveCustomerDataAsync_SavesCustomerDataToLocalStorage()
        {
            // Arrange
            var mockJSRuntime = Substitute.For<IJSRuntime>();
            var customerData = new CustomerOfVendorData
            {
                OfVendor = new VendorPublicData { Name = "Test", PublicKey = "test-publickey" },
                Customer = new CustomerData { Points = 40 },
                Signature = "test-signature",
            };
            var customerJson = System.Text.Json.JsonSerializer.Serialize(customerData);
            var repository = new LocalStorageCustomersRepository(mockJSRuntime);

            // Act
            await repository.SaveCustomerDataWithSignatureAsync(customerData);

            // Assert
            await mockJSRuntime
                .AssertLocalStorageSetCallReceived("NonTrackingCustomerCard.Client.Customer", customerJson);
        }

        [Fact]
        public async Task TryGetCustomerJson_ReturnsCustomerJson_WhenCustomerExistsInLocalStorage()
        {
            // Arrange
            var mockJSRuntime = Substitute.For<IJSRuntime>();
            var customerJson = """
                    {"OfVendor":{"Name":"Test","PublicKey":"MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAstD9xZOY6OAWziUa7\u002BeW5iFgBD/Ls3ntzqWuwxikswvLphsKEOfSQL341KP0TnMA6UFZKJ9MDiXC/aZrJ9D2qhrPrqCgQP3CRS2xzThsLa9hX2gu6VgnR/BbDZscmyuImi70dswZc5yC0aorGvlmPOlQEEPTMgPaCo2I8q6UtnhVDJ88SsIz2kNY/7YwNGZcWBbViY9btvOEF0n3DxgTpg0bSpTc8IuoQsNwqb5vEYwqCMziJNCnPQ3QON1XDG2nE5lFlBcQFjg8jEsXkhjvCIcdLD8WKdbMxs/jWWA28IHk/hDFAhtNcVATUbtb9rJ2sNkGRu9hIw7m7qAVY9NnyQIDAQAB"},"Customer":{"Points":40},"Signature":"kTAPB1o8Mj\u002BAh4uLLghWrIBwCsg260Bw/1IUAsBgU1P\u002BmnXUmEhaANbAFxXMBU81SXST\u002Bu7Po9oxx6aqcXqX4yHQFgE71e5I5xSx/0UUQOmCGzfiWk79TbIcI8QxMV2iGc1dGx3TdoM62JiHpCyn3kPry9yRo29ropUpVcvxcw3ckhIQSGAG14ELGbrK/sv0r\u002Bat/56BnVPazlL\u002B1CBzk928A2Vt7bxT1aZTLyINOnpD6AZfNqAxloGafqnJerHfp7wKqemLEB0YAkw6\u002BCh4KXUttJTL8YFqBTaQFxrMYRmyJ3VaR8RdPI1AQ61ArW3yh082\u002BJZv8mQNASGvcKxEtQ==}
                """;
            mockJSRuntime
                .ArrangeLocalStorageGetItemForKeyReturns("NonTrackingCustomerCard.Client.Customer", customerJson);

            var repository = new LocalStorageCustomersRepository(mockJSRuntime);

            // Act
            var (hasData, resultJson) = await repository.TryGetCustomerJson();

            // Assert
            await mockJSRuntime.AssertLocalStorageGetCallReceived("NonTrackingCustomerCard.Client.Customer");

            hasData.Should().BeTrue();
            resultJson.Should().Be(customerJson);
        }

        [Fact]
        public async Task TryGetCustomerJson_ReturnsFalse_WhenCustomerDoesNotExistInLocalStorage()
        {
            // Arrange
            var mockJSRuntime = Substitute.For<IJSRuntime>();
            mockJSRuntime
                .ArrangeLocalStorageGetItemForKeyReturns("NonTrackingCustomerCard.Client.Customer", (string)null);

            var repository = new LocalStorageCustomersRepository(mockJSRuntime);

            // Act
            var (hasData, resultJson) = await repository.TryGetCustomerJson();

            // Assert
            await mockJSRuntime.AssertLocalStorageGetCallReceived("NonTrackingCustomerCard.Client.Customer");

            hasData.Should().BeFalse();
            resultJson.Should().BeEmpty();
        }
    }
}
