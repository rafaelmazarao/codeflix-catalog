using Codeflix.Catalog.Application.Interfaces;
using Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
using Codeflix.Catalog.Domain.Repository;
using Codeflix.Catalog.UnitTests.Common;
using Moq;
using Xunit;

namespace Codeflix.Catalog.UnitTests.Application.CreateCategory
{
    [CollectionDefinition(nameof(CreateCategoryTestFixture))]
    public class CreateCategoryTestFixtureCollection : ICollectionFixture<CreateCategoryTestFixture>
    {
    }

    public class CreateCategoryTestFixture : BaseFixture
    {
        public string GetValidCategoryName()
        {
            var categoryName = "";

            while (categoryName.Length < 3)
                categoryName = Faker.Commerce.Categories(1)[0];

            if (categoryName.Length > 255)
                categoryName = categoryName.Substring(0, 255);

            return categoryName;
        }

        public string GetValidCategoryDescription()
        {
            var categoryDescription = Faker.Commerce.ProductDescription();
            if (categoryDescription.Length > 1000)
                categoryDescription = categoryDescription.Substring(0, 1000);
            return categoryDescription;
        }

        public bool getRandomBoolean()
            => Faker.Random.Bool();

        public CreateCategoryInput GetInput()
            => new CreateCategoryInput(
                GetValidCategoryName(),
                GetValidCategoryDescription(),
                getRandomBoolean()
            );

        public CreateCategoryInput GetInvalidInputShortName()
        {
            var invalidInputShortName = GetInput();
            invalidInputShortName.Name = invalidInputShortName.Name.Substring(0, 2);
            return invalidInputShortName;            
        }

        public CreateCategoryInput GetInvalidInputTooLongName()
        {
            var invalidInputTooLongName = GetInput();
            invalidInputTooLongName.Name = Faker.Commerce.ProductName();
            while (invalidInputTooLongName.Name.Length <= 255)
                invalidInputTooLongName.Name = $"{invalidInputTooLongName.Name} {Faker.Commerce.ProductName()}";
            return invalidInputTooLongName;           
        }

        public CreateCategoryInput GetInvalidInputTooLongDescription()
        {
            var invalidInputTooLongDescription = GetInput();
            invalidInputTooLongDescription.Description = Faker.Commerce.ProductDescription();
            while (invalidInputTooLongDescription.Description.Length <= 10000)
                invalidInputTooLongDescription.Description = $"{invalidInputTooLongDescription.Description} {Faker.Commerce.ProductDescription()}";
            return invalidInputTooLongDescription;
        }

        public CreateCategoryInput GetInvalidInputNullDescription()
        {
            var invalidInputNullDescription = GetInput();
            invalidInputNullDescription.Description = null!;
            return invalidInputNullDescription;
        }

        public Mock<ICategoryRepository> GetRepositoryMock()
            => new Mock<ICategoryRepository>();

        public Mock<IUnitOfWork> GetUnitOfWorkMock() 
            => new Mock<IUnitOfWork>();       
    }
}
