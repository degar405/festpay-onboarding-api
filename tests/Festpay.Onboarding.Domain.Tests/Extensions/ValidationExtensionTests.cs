using Festpay.Onboarding.Domain.Extensions;

namespace Festpay.Onboarding.Domain.Tests.Extensions
{
    public class ValidationExtensionTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("john.doeexample.com")]
        [InlineData("john@doe@domain.com")]
        public void IsValidEmail_ReturnsFalse_ForNullEmptyOrInvalid(string email)
        {
            var result = ValidationExtension.IsValidEmail(email!);
            Assert.False(result);
        }

        [Theory]
        [InlineData("john.doe@example.com")]
        [InlineData("user+tag@sub.domain.com")]
        public void IsValidEmail_ReturnsTrue_ForValidEmails(string email)
        {
            var result = ValidationExtension.IsValidEmail(email);
            Assert.True(result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("123")]
        [InlineData("1199999999")]    
        [InlineData("119999999999")]  
        [InlineData("11a99999999")]   
        public void IsValidPhone_ReturnsFalse_ForNullEmptyOrInvalid(string phone)
        {
            var result = ValidationExtension.IsValidPhone(phone);
            Assert.False(result);
        }

        [Theory]
        [InlineData("11999999999")]
        [InlineData("(11999999999)")]
        public void IsValidPhone_ReturnsTrue_ForValidPhones(string phone)
        {
            var result = ValidationExtension.IsValidPhone(phone);
            Assert.True(result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("123")]                
        [InlineData("00000000000")]        
        [InlineData("00000000000000")]     
        public void IsValidDocument_ReturnsFalse_ForNullEmptyOrInvalid(string document)
        {
            var result = ValidationExtension.IsValidDocument(document);
            Assert.False(result);
        }

        [Theory]
        [InlineData("16670073607")]        
        [InlineData("166.700.736-07")]     
        [InlineData("11222333000181")]     
        [InlineData("11.222.333/0001-81")] 
        public void IsValidDocument_ReturnsTrue_ForValidCpfOrCnpj(string document)
        {
            var result = ValidationExtension.IsValidDocument(document);
            Assert.True(result);
        }
    }
}
