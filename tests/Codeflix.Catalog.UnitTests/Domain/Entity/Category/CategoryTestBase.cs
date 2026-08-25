using Xunit;

namespace Codeflix.Catalog.UnitTests.Domain.Entity.Category
{
    [Collection("CategoryTestFixture")]
    public class CategoryTestBase
    {
        private readonly CategoryTestFixture _categoryTestFixture;
    }
}