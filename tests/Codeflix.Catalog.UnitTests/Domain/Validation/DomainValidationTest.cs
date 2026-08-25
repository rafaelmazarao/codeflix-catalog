using Bogus;
using Xunit;
using Codeflix.Catalog.Domain.Validation;
using FluentAssertions;
using Codeflix.Catalog.Domain.Exceptions;

namespace Codeflix.Catalog.UnitTests.Domain.Validation
{
    public class DomainValidationTest
    {
        private Faker _faker { get; set; } = new Faker();

        [Fact(DisplayName = nameof(NotNullOk))]
        [Trait("Domain", "Domain Validation")]
        public void NotNullOk()
        {
            string fieldName = _faker.Commerce.ProductName().Replace(" ", "");
            var value = _faker.Commerce.ProductName();

            Action action
                = () => DomainValidation.NotNull(value, fieldName);

            action.Should().NotThrow();

        }

        [Fact(DisplayName = nameof(NotNullThrowWhenNull))]
        [Trait("Domain", "Domain Validation")]
        public void NotNullThrowWhenNull()
        {
            string? value = null;
            string fieldName = _faker.Commerce.ProductName().Replace(" ", "");

            Action action
                = () => DomainValidation.NotNull(value, fieldName);

            action.Should().Throw<EntityValidationException>().WithMessage($"{fieldName} should not be null");

        }

        [Theory(DisplayName = nameof(NotNullOrEmptyThrowWhenEmpty))]
        [Trait("Domain", "Domain Validation")]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData(null)]
        public void NotNullOrEmptyThrowWhenEmpty(string? target)
        {
            string fieldName = _faker.Commerce.ProductName().Replace(" ", "");

            Action action
                = () => DomainValidation.NotNullOrEmpty(target, fieldName);

            action.Should().Throw<EntityValidationException>().WithMessage($"{fieldName} should not be empty or null");

        }

        [Fact(DisplayName = nameof(NotNullOrEmptyOk))]
        [Trait("Domain", "Domain Validation")]
        public void NotNullOrEmptyOk()
        {
            string fieldName = _faker.Commerce.ProductName().Replace(" ", "");

            var target = _faker.Commerce.ProductName();

            Action action
                = () => DomainValidation.NotNullOrEmpty(target, fieldName);

            action.Should().NotThrow();
        }

        [Theory(DisplayName = nameof(MinLengthThrowWhenLess))]
        [Trait("Domain", "Domain Validation")]
        [MemberData(nameof(GetValuesSmallerThanMin), parameters: 10)]
        public void MinLengthThrowWhenLess(string? target, int minLength)
        {
            string fieldName = _faker.Commerce.ProductName().Replace(" ", "");

            Action action
                = () => DomainValidation.MinLength(target, minLength, fieldName);

            action.Should().Throw<EntityValidationException>()
                .WithMessage($"{fieldName} should be at least {minLength} characters long");
        }

        public static IEnumerable<object[]> GetValuesSmallerThanMin(int numberOfTests = 5)
        {
            var faker = new Faker();
            for (int i = 0; i < numberOfTests; i++)
            {
                faker.Commerce.ProductName();
                var minLength = faker.Random.Int(2, 10);
                var target = faker.Random.String2(minLength - 1);
                yield return new object[] { target, minLength };
            }
        }

        [Theory(DisplayName = nameof(MinLengthOk))]
        [Trait("Domain", "Domain Validation")]
        [MemberData(nameof(GetValuesGreaterThanMin), parameters: 10)]
        public void MinLengthOk(string target, int minLength)
        {
            string fieldName = _faker.Commerce.ProductName().Replace(" ", "");

            Action action
                = () => DomainValidation.MinLength(target, minLength, fieldName);

            action.Should().NotThrow();
        }

        public static IEnumerable<object[]> GetValuesGreaterThanMin(int numberOfTests = 5)
        {
            var faker = new Faker();
            for (int i = 0; i < numberOfTests; i++)
            {
                faker.Commerce.ProductName();
                var minLength = faker.Random.Int(2, 10);
                var target = faker.Random.String2(minLength + 1);
                yield return new object[] { target, minLength };
            }
        }

        [Theory(DisplayName = nameof(MaxLengthThrowWhenGreater))]
        [Trait("Domain", "Domain Validation")]
        [MemberData(nameof(GetValuesGreaterThanMax), parameters: 10)]
        public void MaxLengthThrowWhenGreater(string target, int maxLength)
        {
            string fieldName = _faker.Commerce.ProductName().Replace(" ", "");

            Action action
                = () => DomainValidation.MaxLength(target, maxLength, fieldName);

            action.Should().Throw<EntityValidationException>().WithMessage($"{fieldName} should be less or equal {maxLength} characters long");
        }

        public static IEnumerable<object[]> GetValuesGreaterThanMax(int numberOfTests = 5)
        {
            var faker = new Faker();
            for (int i = 0; i < numberOfTests; i++)
            {
                faker.Commerce.ProductName();
                var maxLength = faker.Random.Int(1, 10);
                var target = faker.Random.String2(maxLength + 1);
                yield return new object[] { target, maxLength };
            }
        }

        [Theory(DisplayName = nameof(MaxLengthOk))]
        [Trait("Domain", "Domain Validation")]
        [MemberData(nameof(GetValuesLessThanMax), parameters: 10)]
        public void MaxLengthOk(string target, int maxLength)
        {
            string fieldName = _faker.Commerce.ProductName().Replace(" ", "");

            Action action
                = () => DomainValidation.MaxLength(target, maxLength, fieldName);

            action.Should().NotThrow();
        }

        public static IEnumerable<object[]> GetValuesLessThanMax(int numberOfTests = 5)
        {
            var faker = new Faker();
            for (int i = 0; i < numberOfTests; i++)
            {
                faker.Commerce.ProductName();
                var maxLength = faker.Random.Int(1, 10);
                var target = faker.Random.String2(faker.Random.Int(1, maxLength));
                yield return new object[] { target, maxLength };
            }
        }
    }
}
