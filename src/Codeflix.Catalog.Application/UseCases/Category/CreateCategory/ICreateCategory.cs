using MediatR;

namespace Codeflix.Catalog.Application.UseCases.Category.CreateCategory
{
    public interface ICreateCategory : IRequestHandler<CreateCategoryInput, CreateCategoryOutput>
    {
    }
}
