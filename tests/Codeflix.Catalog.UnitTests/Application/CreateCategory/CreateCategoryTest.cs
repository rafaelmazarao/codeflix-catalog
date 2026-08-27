using Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
using Codeflix.Catalog.Domain.Entity;
using Codeflix.Catalog.Domain.Exceptions;
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

        [Theory(DisplayName = nameof(ThrowWhenCantInstantiateCategory))]
        [Trait("Application", "CreateCategory - Use Cases")]
        [MemberData(nameof(CreateCategoryTestDataGenerator.GetInvalidInputs),
            parameters: 36,
            MemberType = typeof(CreateCategoryTestDataGenerator))]
        public async Task ThrowWhenCantInstantiateCategory(CreateCategoryInput input, string exceptionMessage)
        {
            var useCase = new UseCases.CreateCategory(_fixture.GetRepositoryMock().Object, _fixture.GetUnitOfWorkMock().Object);           

            Func<Task> task = async () => await useCase.Handle(input, CancellationToken.None);

            await task.Should().ThrowAsync<EntityValidationException>().WithMessage(exceptionMessage);
        }        
    }
}
