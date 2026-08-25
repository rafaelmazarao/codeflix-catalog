using Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
using Codeflix.Catalog.Domain.Entity;
using Codeflix.Catalog.Domain.Exceptions;
using Codeflix.Catalog.UnitTests.Domain.Entity.Category;
using FluentAssertions;
using Moq;
using Xunit;
using UseCases = Codeflix.Catalog.Application.UseCases.Category.CreateCategory;

namespace Codeflix.Catalog.UnitTests.Application.CreateCategory
{
    [Collection(nameof(CreateCategoryTestFixture))]
    public class CreateCategoryTest
    {
        private readonly CreateCategoryTestFixture _fixture;

        public CreateCategoryTest(CreateCategoryTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory(DisplayName = nameof(CreateCategory))]
        [Trait("Application", "CreateCategory - Use Cases")]
        [MemberData(nameof(GetCategoriesToCreate))]
        public async Task CreateCategory(CreateCategoryInput input)
        {
            var repositoryMock = _fixture.GetRepositoryMock();
            var unitOfWorkMock = _fixture.GetUnitOfWorkMock();

            var useCase = new UseCases.CreateCategory(repositoryMock.Object, unitOfWorkMock.Object);

            var output = await useCase.Handle(input, CancellationToken.None);

            repositoryMock.Verify(
                repository => repository.Insert(
                    It.IsAny<Category>(),
                    It.IsAny<CancellationToken>()
                    ),
                Times.Once
            );

            unitOfWorkMock.Verify(
                uow => uow.Commit(It.IsAny<CancellationToken>()),
                Times.Once
            );

            output.Should().NotBeNull();
            output.Name.Should().Be(input.Name);
            output.Description.Should().Be(input.Description);
            output.IsActive.Should().Be(input.IsActive);
            output.Id.Should().NotBeEmpty();
            output.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
        }

        private static IEnumerable<object[]> GetCategoriesToCreate()
        {
            var fixture = new CreateCategoryTestFixture();

            return new List<object[]>
            {
                new object[] { fixture.GetInput() },
                new object[] { new CreateCategoryInput(fixture.GetValidCategoryName()) },
                new object[] { new CreateCategoryInput(fixture.GetValidCategoryName(), fixture.GetValidCategoryDescription()) }
            };
        }

        [Theory(DisplayName = nameof(ThrowWhenCantInstantiateAggregate))]
        [Trait("Application", "CreateCategory - Use Cases")]
        [MemberData(nameof(GetInvalidInputs))]
        public async Task ThrowWhenCantInstantiateAggregate(CreateCategoryInput input, string exceptionMessage)
        {
            var useCase = new UseCases.CreateCategory(_fixture.GetRepositoryMock().Object, _fixture.GetUnitOfWorkMock().Object);           

            Func<Task> task = async () => await useCase.Handle(input, CancellationToken.None);

            await task.Should().ThrowAsync<EntityValidationException>().WithMessage(exceptionMessage);
        }

        private static IEnumerable<object[]> GetInvalidInputs()
        {
            var fixture = new CreateCategoryTestFixture();
            var invalidInputsList = new List<object[]>();

            var invalidInputShortName = fixture.GetInput();
            invalidInputShortName.Name = invalidInputShortName.Name.Substring(0, 2);
            invalidInputsList.Add(new object[] {
                invalidInputShortName,
                "Name should be at least 3 characters long"
            });

            var invalidInputTooLongName = fixture.GetInput();
            var tooLongNameForCategory = fixture.Faker.Commerce.ProductName();
            while (tooLongNameForCategory.Length <= 255)
                tooLongNameForCategory = $"{tooLongNameForCategory} {fixture.Faker.Commerce.ProductName()}";

            invalidInputTooLongName.Name = tooLongNameForCategory;
            invalidInputsList.Add(new object[] {
                invalidInputTooLongName,
                $"Name should be less or equal 255 characters long"
            });

            var invalidInputDescriptionNull = fixture.GetInput();
            invalidInputDescriptionNull.Description = null!;
            invalidInputsList.Add(new object[] {
                invalidInputDescriptionNull,
                "Description should not be null"
            });

            var invalidInputTooLongDescription = fixture.GetInput();
            var tooLongDescriptionForCategory = fixture.Faker.Commerce.ProductDescription();
            while (tooLongDescriptionForCategory.Length <= 10000)
                tooLongDescriptionForCategory = $"{tooLongDescriptionForCategory} {fixture.Faker.Commerce.ProductDescription()}";

            invalidInputTooLongDescription.Description = tooLongDescriptionForCategory;
            invalidInputsList.Add(new object[] {
                invalidInputTooLongDescription,
                $"Description should be less or equal 10000 characters long"
            });

            return invalidInputsList;
        }
    }
}
