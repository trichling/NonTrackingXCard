using FluentAssertions;

using Microsoft.JSInterop;

using NonTrackingCustomerCard.Client.Contracts;
using NonTrackingCustomerCard.Client.Repositories;

using NSubstitute;

namespace NonTrackingCustomerCard.Tests.Client.Repositories
{
    public class VendorRepositoryTest
    {
        [Fact]
        public async Task GetVendorPublicData_ReturnsPublicVendorData_WhenVendorExistsInLocalStorage()
        {
            // Arrange
            var mockJSRuntime = Substitute.For<IJSRuntime>();
            var vendorJson = """
                    {"prk":"MIIEvQIBADANBgkqhkiG9w0BAQEFAASCBKcwggSjAgEAAoIBAQCyoCozOw/xRsFnGukYMbifaIkwhb5NsdQX5fwOJlebsIIsxtHuItR9LW2KucJCjV3tv/L7pZMwO7CW8hfdvUcb68s1gJwk9gq50o6uzRKpZwEzIWBKqtaF8YzzuBznJZiUTAH0fWm/Z9RWZPpD5\u002B/jlEr65sUFHROhmtJaxfAZdxvGg4jpErgWTaI/XmYfJOUYu\u002B1ri\u002Bow1aCILm8iNw41CWZ5IkZ4a/Hj8ufEHI/2DDrIaSf3uctOpOA5iP414XCPK5j/IkjSHRzKajcf8on6T1ChsCuAbYbrk0dbOkaGPaRf5EjGAQ9S4WKNGqeWL/WuALgq\u002BwRtYs/3g36Mra33AgMBAAECggEAWUTrIiRLwOCEClOsF0/N/Tigi1Pjue972B\u002BzepzV3rR1MgyA4NqeSBraamCQgXMl9Iof1Hy4lPvXsnA11jbgYUdTjJ8EKgKedKSSczCAuZGFS3jMJzS\u002Btjz0HN8v7qLe4Iol1fqRTTuGJlEbs7EntZwoZsDKyNXxuCtoIj5W0lparZ0uyo/2EO0i4RfL3R8Wi3FqRijop61TzimfrjFzX3me/5FlU/WNjZ\u002BvAJwExazIdAwlTkLT2XBpL/sHmlYJ7MB29PXWkOBsTWVSwjDqxtHjCng/9Kmqk0qTjqKOVORWRbX/lkgkiUx45bQbj9B2jS6Oj1ujpStXvymzCksGUQKBgQDjaTXz3qQog6OAbc6\u002B074UYElmwTJqlRjmz3R/TXCbqGOUelhc7vj1129LH6BHxsUBRLBKncZ7ZO6dwvfqJbi/dQEZYkKbELn42xrTStA4ZiDO2sh8q0zDZoAUJDMMYfGKW4/ciJGqdgSpAjzXpjsgaaYxIF/LUlgVdimlmo\u002By5wKBgQDJFOQXdLvtr0kxd/9063dLYj0qxRJeXYo6pFucuRPUybE5be\u002BVTooW0mcZ8BJkmVT068FDqYauiaMa4/3lEry4gXs6\u002BLY1rmrdgyUKZUZsA\u002Bi1mne4bHbYd7JGx/mNjYCBCLVpY4eMJrynRE1B5umwL5LLXw0M4hHaiTfobgHacQKBgFq5xL6QWmmXawl3xX24OCMk7uPBu/1tkkuzBEUtffl7yo6X6NzashRSaKJN8cHw\u002BPOylaqPG3prIA9skz\u002Bk4PnxEgkwfGYk3Cz\u002BLMuTE1MM25XrDwU9yhhCsmJWts7/3D9YnCJdkc\u002Bx2dYbgG6AcSJTQ\u002BxhgBY7ucyGIcNh0GaNAoGACVoXYZ/kPaGxXcOe9ekdmzvubFbOC\u002BV90Exkll1lNrhKrckXI3KLqZQPh31K3bLj6KAuVN9FjEiKdW21GTpN3Kbo/E0k7Eo4XiUDTAB5zfxAjnGor8Mbo\u002BWtPO2ABb1XBKlLBqPCYhpLG9xpW29J3w/XW\u002B4HxVmeSvOmzgCPxYECgYEAy4YmxAhL1jYnqE7nxKuDuOYV8u3hYSVjOnUpGoWiCZ129WgldGEX8kePm7iuSTULmYZMb\u002BM722YsYMKpP7eijejCYyK5RvDcebxhk0vOLL2Un1eI/kbof0\u002B3c7XLYUYB5hxu5YDSuhDrocoh8U8gq5bRqvsoGhrERL7SvEkp/uE=","vn":"Test","puk":"MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAsqAqMzsP8UbBZxrpGDG4n2iJMIW\u002BTbHUF\u002BX8DiZXm7CCLMbR7iLUfS1tirnCQo1d7b/y\u002B6WTMDuwlvIX3b1HG\u002BvLNYCcJPYKudKOrs0SqWcBMyFgSqrWhfGM87gc5yWYlEwB9H1pv2fUVmT6Q\u002Bfv45RK\u002BubFBR0ToZrSWsXwGXcbxoOI6RK4Fk2iP15mHyTlGLvta4vqMNWgiC5vIjcONQlmeSJGeGvx4/LnxByP9gw6yGkn97nLTqTgOYj\u002BNeFwjyuY/yJI0h0cymo3H/KJ\u002Bk9QobArgG2G65NHWzpGhj2kX\u002BRIxgEPUuFijRqnli/1rgC4KvsEbWLP94N\u002BjK2t9wIDAQAB"}
                """;

            mockJSRuntime
                .ArrangeLocalStorageGetItemForKeyReturns("NonTrackingCustomerCard.Client.Vendor", vendorJson);

            var repository = new LocalStorageVendorRepository(mockJSRuntime);

            // Act
            var result = await repository.GetVendorPublicData();

            // Assert
            await mockJSRuntime.AssertLocalStorageGetCallReceived("NonTrackingCustomerCard.Client.Vendor");

            result.Should().NotBeNull();
            result.Name.Should().NotBeNull();
            result.Name.Should().Be("Test");
            result
                .PublicKey.Should()
                .Be(
                    "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAsqAqMzsP8UbBZxrpGDG4n2iJMIW\u002BTbHUF\u002BX8DiZXm7CCLMbR7iLUfS1tirnCQo1d7b/y\u002B6WTMDuwlvIX3b1HG\u002BvLNYCcJPYKudKOrs0SqWcBMyFgSqrWhfGM87gc5yWYlEwB9H1pv2fUVmT6Q\u002Bfv45RK\u002BubFBR0ToZrSWsXwGXcbxoOI6RK4Fk2iP15mHyTlGLvta4vqMNWgiC5vIjcONQlmeSJGeGvx4/LnxByP9gw6yGkn97nLTqTgOYj\u002BNeFwjyuY/yJI0h0cymo3H/KJ\u002Bk9QobArgG2G65NHWzpGhj2kX\u002BRIxgEPUuFijRqnli/1rgC4KvsEbWLP94N\u002BjK2t9wIDAQAB"
                );
        }



        [Fact]
        public async Task GetVendorPublicData_ReturnsNewVendorPublicData_WhenVendorDoesNotExistInLocalStorage()
        {
            // Arrange
            var mockJSRuntime = Substitute.For<IJSRuntime>();
            mockJSRuntime
                .ArrangeLocalStorageGetItemForKeyReturns("NonTrackingCustomerCard.Client.Vendor", (string)null);

            var repository = new LocalStorageVendorRepository(mockJSRuntime);

            // Act
            var result = await repository.GetVendorPublicData();

            // Assert
            await mockJSRuntime.AssertLocalStorageGetCallReceived("NonTrackingCustomerCard.Client.Vendor");

            result.Should().NotBeNull();
            result.Should().BeOfType<VendorPublicData>();
            result.Name.Should().NotBeNull();
            result.Name.Should().BeEmpty();
            result.PublicKey.Should().NotBeNull();
            result.PublicKey.Should().BeEmpty();
        }

        [Fact]
        public async Task GetVendorData_ReturnsEmptyVendorData_WhenLocalStorageIsEmpty()
        {
            // Arrange
            var mockJSRuntime = Substitute.For<IJSRuntime>();
            mockJSRuntime
                .ArrangeLocalStorageGetItemForKeyReturns("NonTrackingCustomerCard.Client.Vendor", (string)null);

            var repository = new LocalStorageVendorRepository(mockJSRuntime);

            // Act
            var result = await repository.GetVendorData();

            // Assert
            await mockJSRuntime.AssertLocalStorageGetCallReceived("NonTrackingCustomerCard.Client.Vendor");

            result.Should().NotBeNull();
            result.Should().BeOfType<VendorData>();
            result.Name.Should().NotBeNull();
            result.PublicKey.Should().NotBeNull();
            result.PrivateKey.Should().NotBeNull();
        }

        [Fact]
        public async Task GetVendorData_ReturnsVendorData_WhenLocalStorageHasData()
        {
            // Arrange
            var mockJSRuntime = Substitute.For<IJSRuntime>();
            var vendorJson = """
                    {"prk":"MIIEvQIBADANBgkqhkiG9w0BAQEFAASCBKcwggSjAgEAAoIBAQCyoCozOw/xRsFnGukYMbifaIkwhb5NsdQX5fwOJlebsIIsxtHuItR9LW2KucJCjV3tv/L7pZMwO7CW8hfdvUcb68s1gJwk9gq50o6uzRKpZwEzIWBKqtaF8YzzuBznJZiUTAH0fWm/Z9RWZPpD5\u002B/jlEr65sUFHROhmtJaxfAZdxvGg4jpErgWTaI/XmYfJOUYu\u002B1ri\u002Bow1aCILm8iNw41CWZ5IkZ4a/Hj8ufEHI/2DDrIaSf3uctOpOA5iP414XCPK5j/IkjSHRzKajcf8on6T1ChsCuAbYbrk0dbOkaGPaRf5EjGAQ9S4WKNGqeWL/WuALgq\u002BwRtYs/3g36Mra33AgMBAAECggEAWUTrIiRLwOCEClOsF0/N/Tigi1Pjue972B\u002BzepzV3rR1MgyA4NqeSBraamCQgXMl9Iof1Hy4lPvXsnA11jbgYUdTjJ8EKgKedKSSczCAuZGFS3jMJzS\u002Btjz0HN8v7qLe4Iol1fqRTTuGJlEbs7EntZwoZsDKyNXxuCtoIj5W0lparZ0uyo/2EO0i4RfL3R8Wi3FqRijop61TzimfrjFzX3me/5FlU/WNjZ\u002BvAJwExazIdAwlTkLT2XBpL/sHmlYJ7MB29PXWkOBsTWVSwjDqxtHjCng/9Kmqk0qTjqKOVORWRbX/lkgkiUx45bQbj9B2jS6Oj1ujpStXvymzCksGUQKBgQDjaTXz3qQog6OAbc6\u002B074UYElmwTJqlRjmz3R/TXCbqGOUelhc7vj1129LH6BHxsUBRLBKncZ7ZO6dwvfqJbi/dQEZYkKbELn42xrTStA4ZiDO2sh8q0zDZoAUJDMMYfGKW4/ciJGqdgSpAjzXpjsgaaYxIF/LUlgVdimlmo\u002By5wKBgQDJFOQXdLvtr0kxd/9063dLYj0qxRJeXYo6pFucuRPUybE5be\u002BVTooW0mcZ8BJkmVT068FDqYauiaMa4/3lEry4gXs6\u002BLY1rmrdgyUKZUZsA\u002Bi1mne4bHbYd7JGx/mNjYCBCLVpY4eMJrynRE1B5umwL5LLXw0M4hHaiTfobgHacQKBgFq5xL6QWmmXawl3xX24OCMk7uPBu/1tkkuzBEUtffl7yo6X6NzashRSaKJN8cHw\u002BPOylaqPG3prIA9skz\u002Bk4PnxEgkwfGYk3Cz\u002BLMuTE1MM25XrDwU9yhhCsmJWts7/3D9YnCJdkc\u002Bx2dYbgG6AcSJTQ\u002BxhgBY7ucyGIcNh0GaNAoGACVoXYZ/kPaGxXcOe9ekdmzvubFbOC\u002BV90Exkll1lNrhKrckXI3KLqZQPh31K3bLj6KAuVN9FjEiKdW21GTpN3Kbo/E0k7Eo4XiUDTAB5zfxAjnGor8Mbo\u002BWtPO2ABb1XBKlLBqPCYhpLG9xpW29J3w/XW\u002B4HxVmeSvOmzgCPxYECgYEAy4YmxAhL1jYnqE7nxKuDuOYV8u3hYSVjOnUpGoWiCZ129WgldGEX8kePm7iuSTULmYZMb\u002BM722YsYMKpP7eijejCYyK5RvDcebxhk0vOLL2Un1eI/kbof0\u002B3c7XLYUYB5hxu5YDSuhDrocoh8U8gq5bRqvsoGhrERL7SvEkp/uE=","vn":"Test","puk":"MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAsqAqMzsP8UbBZxrpGDG4n2iJMIW\u002BTbHUF\u002BX8DiZXm7CCLMbR7iLUfS1tirnCQo1d7b/y\u002B6WTMDuwlvIX3b1HG\u002BvLNYCcJPYKudKOrs0SqWcBMyFgSqrWhfGM87gc5yWYlEwB9H1pv2fUVmT6Q\u002Bfv45RK\u002BubFBR0ToZrSWsXwGXcbxoOI6RK4Fk2iP15mHyTlGLvta4vqMNWgiC5vIjcONQlmeSJGeGvx4/LnxByP9gw6yGkn97nLTqTgOYj\u002BNeFwjyuY/yJI0h0cymo3H/KJ\u002Bk9QobArgG2G65NHWzpGhj2kX\u002BRIxgEPUuFijRqnli/1rgC4KvsEbWLP94N\u002BjK2t9wIDAQAB"}
                """;

            mockJSRuntime.
                ArrangeLocalStorageGetItemForKeyReturns("NonTrackingCustomerCard.Client.Vendor", vendorJson);

            var repository = new LocalStorageVendorRepository(mockJSRuntime);

            // Act
            var result = await repository.GetVendorData();

            // Assert
            await mockJSRuntime.AssertLocalStorageGetCallReceived("NonTrackingCustomerCard.Client.Vendor");

            result.Should().NotBeNull();
            result.Should().BeOfType<VendorData>();
            result.Name.Should().Be("Test");
            result.PublicKey.Should().Be("MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAsqAqMzsP8UbBZxrpGDG4n2iJMIW\u002BTbHUF\u002BX8DiZXm7CCLMbR7iLUfS1tirnCQo1d7b/y\u002B6WTMDuwlvIX3b1HG\u002BvLNYCcJPYKudKOrs0SqWcBMyFgSqrWhfGM87gc5yWYlEwB9H1pv2fUVmT6Q\u002Bfv45RK\u002BubFBR0ToZrSWsXwGXcbxoOI6RK4Fk2iP15mHyTlGLvta4vqMNWgiC5vIjcONQlmeSJGeGvx4/LnxByP9gw6yGkn97nLTqTgOYj\u002BNeFwjyuY/yJI0h0cymo3H/KJ\u002Bk9QobArgG2G65NHWzpGhj2kX\u002BRIxgEPUuFijRqnli/1rgC4KvsEbWLP94N\u002BjK2t9wIDAQAB");
            result.PrivateKey.Should().Be("MIIEvQIBADANBgkqhkiG9w0BAQEFAASCBKcwggSjAgEAAoIBAQCyoCozOw/xRsFnGukYMbifaIkwhb5NsdQX5fwOJlebsIIsxtHuItR9LW2KucJCjV3tv/L7pZMwO7CW8hfdvUcb68s1gJwk9gq50o6uzRKpZwEzIWBKqtaF8YzzuBznJZiUTAH0fWm/Z9RWZPpD5\u002B/jlEr65sUFHROhmtJaxfAZdxvGg4jpErgWTaI/XmYfJOUYu\u002B1ri\u002Bow1aCILm8iNw41CWZ5IkZ4a/Hj8ufEHI/2DDrIaSf3uctOpOA5iP414XCPK5j/IkjSHRzKajcf8on6T1ChsCuAbYbrk0dbOkaGPaRf5EjGAQ9S4WKNGqeWL/WuALgq\u002BwRtYs/3g36Mra33AgMBAAECggEAWUTrIiRLwOCEClOsF0/N/Tigi1Pjue972B\u002BzepzV3rR1MgyA4NqeSBraamCQgXMl9Iof1Hy4lPvXsnA11jbgYUdTjJ8EKgKedKSSczCAuZGFS3jMJzS\u002Btjz0HN8v7qLe4Iol1fqRTTuGJlEbs7EntZwoZsDKyNXxuCtoIj5W0lparZ0uyo/2EO0i4RfL3R8Wi3FqRijop61TzimfrjFzX3me/5FlU/WNjZ\u002BvAJwExazIdAwlTkLT2XBpL/sHmlYJ7MB29PXWkOBsTWVSwjDqxtHjCng/9Kmqk0qTjqKOVORWRbX/lkgkiUx45bQbj9B2jS6Oj1ujpStXvymzCksGUQKBgQDjaTXz3qQog6OAbc6\u002B074UYElmwTJqlRjmz3R/TXCbqGOUelhc7vj1129LH6BHxsUBRLBKncZ7ZO6dwvfqJbi/dQEZYkKbELn42xrTStA4ZiDO2sh8q0zDZoAUJDMMYfGKW4/ciJGqdgSpAjzXpjsgaaYxIF/LUlgVdimlmo\u002By5wKBgQDJFOQXdLvtr0kxd/9063dLYj0qxRJeXYo6pFucuRPUybE5be\u002BVTooW0mcZ8BJkmVT068FDqYauiaMa4/3lEry4gXs6\u002BLY1rmrdgyUKZUZsA\u002Bi1mne4bHbYd7JGx/mNjYCBCLVpY4eMJrynRE1B5umwL5LLXw0M4hHaiTfobgHacQKBgFq5xL6QWmmXawl3xX24OCMk7uPBu/1tkkuzBEUtffl7yo6X6NzashRSaKJN8cHw\u002BPOylaqPG3prIA9skz\u002Bk4PnxEgkwfGYk3Cz\u002BLMuTE1MM25XrDwU9yhhCsmJWts7/3D9YnCJdkc\u002Bx2dYbgG6AcSJTQ\u002BxhgBY7ucyGIcNh0GaNAoGACVoXYZ/kPaGxXcOe9ekdmzvubFbOC\u002BV90Exkll1lNrhKrckXI3KLqZQPh31K3bLj6KAuVN9FjEiKdW21GTpN3Kbo/E0k7Eo4XiUDTAB5zfxAjnGor8Mbo\u002BWtPO2ABb1XBKlLBqPCYhpLG9xpW29J3w/XW\u002B4HxVmeSvOmzgCPxYECgYEAy4YmxAhL1jYnqE7nxKuDuOYV8u3hYSVjOnUpGoWiCZ129WgldGEX8kePm7iuSTULmYZMb\u002BM722YsYMKpP7eijejCYyK5RvDcebxhk0vOLL2Un1eI/kbof0\u002B3c7XLYUYB5hxu5YDSuhDrocoh8U8gq5bRqvsoGhrERL7SvEkp/uE=");
        }

        [Fact]
        public async Task SaveVendorDataAsync_SavesVendorDataToLocalStorage()
        {
            // Arrange
            var mockJSRuntime = Substitute.For<IJSRuntime>();
            var vendorData = new VendorData
            {
                Name = "Test",
                PublicKey = "test-publickey",
                PrivateKey = "test-privatekey"
            };
            var vendorJson = System.Text.Json.JsonSerializer.Serialize(vendorData);
            var repository = new LocalStorageVendorRepository(mockJSRuntime);

            // Act
            await repository.SaveVendorData(vendorData);

            // Assert
            await mockJSRuntime.
                AssertLocalStorageSetCallReceived("NonTrackingCustomerCard.Client.Vendor", vendorJson);
        }

        [Fact]
        public async Task TryGetVendorJson_ReturnsVendorJson_WhenVendorExistsInLocalStorage()
        {
            // Arrange
            var mockJSRuntime = Substitute.For<IJSRuntime>();
            var vendorJson = """
                    {"prk":"MIIEvQIBADANBgkqhkiG9w0BAQEFAASCBKcwggSjAgEAAoIBAQCyoCozOw/xRsFnGukYMbifaIkwhb5NsdQX5fwOJlebsIIsxtHuItR9LW2KucJCjV3tv/L7pZMwO7CW8hfdvUcb68s1gJwk9gq50o6uzRKpZwEzIWBKqtaF8YzzuBznJZiUTAH0fWm/Z9RWZPpD5\u002B/jlEr65sUFHROhmtJaxfAZdxvGg4jpErgWTaI/XmYfJOUYu\u002B1ri\u002Bow1aCILm8iNw41CWZ5IkZ4a/Hj8ufEHI/2DDrIaSf3uctOpOA5iP414XCPK5j/IkjSHRzKajcf8on6T1ChsCuAbYbrk0dbOkaGPaRf5EjGAQ9S4WKNGqeWL/WuALgq\u002BwRtYs/3g36Mra33AgMBAAECggEAWUTrIiRLwOCEClOsF0/N/Tigi1Pjue972B\u002BzepzV3rR1MgyA4NqeSBraamCQgXMl9Iof1Hy4lPvXsnA11jbgYUdTjJ8EKgKedKSSczCAuZGFS3jMJzS\u002Btjz0HN8v7qLe4Iol1fqRTTuGJlEbs7EntZwoZsDKyNXxuCtoIj5W0lparZ0uyo/2EO0i4RfL3R8Wi3FqRijop61TzimfrjFzX3me/5FlU/WNjZ\u002BvAJwExazIdAwlTkLT2XBpL/sHmlYJ7MB29PXWkOBsTWVSwjDqxtHjCng/9Kmqk0qTjqKOVORWRbX/lkgkiUx45bQbj9B2jS6Oj1ujpStXvymzCksGUQKBgQDjaTXz3qQog6OAbc6\u002B074UYElmwTJqlRjmz3R/TXCbqGOUelhc7vj1129LH6BHxsUBRLBKncZ7ZO6dwvfqJbi/dQEZYkKbELn42xrTStA4ZiDO2sh8q0zDZoAUJDMMYfGKW4/ciJGqdgSpAjzXpjsgaaYxIF/LUlgVdimlmo\u002By5wKBgQDJFOQXdLvtr0kxd/9063dLYj0qxRJeXYo6pFucuRPUybE5be\u002BVTooW0mcZ8BJkmVT068FDqYauiaMa4/3lEry4gXs6\u002BLY1rmrdgyUKZUZsA\u002Bi1mne4bHbYd7JGx/mNjYCBCLVpY4eMJrynRE1B5umwL5LLXw0M4hHaiTfobgHacQKBgFq5xL6QWmmXawl3xX24OCMk7uPBu/1tkkuzBEUtffl7yo6X6NzashRSaKJN8cHw\u002BPOylaqPG3prIA9skz\u002Bk4PnxEgkwfGYk3Cz\u002BLMuTE1MM25XrDwU9yhhCsmJWts7/3D9YnCJdkc\u002Bx2dYbgG6AcSJTQ\u002BxhgBY7ucyGIcNh0GaNAoGACVoXYZ/kPaGxXcOe9ekdmzvubFbOC\u002BV90Exkll1lNrhKrckXI3KLqZQPh31K3bLj6KAuVN9FjEiKdW21GTpN3Kbo/E0k7Eo4XiUDTAB5zfxAjnGor8Mbo\u002BWtPO2ABb1XBKlLBqPCYhpLG9xpW29J3w/XW\u002B4HxVmeSvOmzgCPxYECgYEAy4YmxAhL1jYnqE7nxKuDuOYV8u3hYSVjOnUpGoWiCZ129WgldGEX8kePm7iuSTULmYZMb\u002BM722YsYMKpP7eijejCYyK5RvDcebxhk0vOLL2Un1eI/kbof0\u002B3c7XLYUYB5hxu5YDSuhDrocoh8U8gq5bRqvsoGhrERL7SvEkp/uE=","vn":"Test","puk":"MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAsqAqMzsP8UbBZxrpGDG4n2iJMIW\u002BTbHUF\u002BX8DiZXm7CCLMbR7iLUfS1tirnCQo1d7b/y\u002B6WTMDuwlvIX3b1HG\u002BvLNYCcJPYKudKOrs0SqWcBMyFgSqrWhfGM87gc5yWYlEwB9H1pv2fUVmT6Q\u002Bfv45RK\u002BubFBR0ToZrSWsXwGXcbxoOI6RK4Fk2iP15mHyTlGLvta4vqMNWgiC5vIjcONQlmeSJGeGvx4/LnxByP9gw6yGkn97nLTqTgOYj\u002BNeFwjyuY/yJI0h0cymo3H/KJ\u002Bk9QobArgG2G65NHWzpGhj2kX\u002BRIxgEPUuFijRqnli/1rgC4KvsEbWLP94N\u002BjK2t9wIDAQAB"}
                """;

            mockJSRuntime.ArrangeLocalStorageGetItemForKeyReturns("NonTrackingCustomerCard.Client.Vendor", vendorJson);

            var repository = new LocalStorageVendorRepository(mockJSRuntime);

            // Act
            var (hasData, resultJson) = await repository.TryGetVendorJson();

            // Assert
            await mockJSRuntime.AssertLocalStorageGetCallReceived("NonTrackingCustomerCard.Client.Vendor");
            hasData.Should().BeTrue();
            resultJson.Should().Be(vendorJson);
        }

        [Fact]
        public async Task TryGetCustomerJson_ReturnsFalse_WhenCustomerDoesNotExistInLocalStorage()
        {
            // Arrange
            var mockJSRuntime = Substitute.For<IJSRuntime>();
            mockJSRuntime.ArrangeLocalStorageGetItemForKeyReturns("NonTrackingCustomerCard.Client.Customer", (string)null);

            var repository = new LocalStorageVendorRepository(mockJSRuntime);

            // Act
            var (hasData, resultJson) = await repository.TryGetVendorJson();

            // Assert
            await mockJSRuntime.AssertLocalStorageGetCallReceived("NonTrackingCustomerCard.Client.Vendor");
            hasData.Should().BeFalse();
            resultJson.Should().BeEmpty();
        }

    }
}
