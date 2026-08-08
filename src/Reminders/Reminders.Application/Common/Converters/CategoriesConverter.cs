using Reminders.Application.Common.Interfaces;
using Reminders.Application.TransferModels;

using Mapster;

namespace Reminders.Application.Common.Converters;

public class CategoriesConverter : IDtoConverter<CategoryCreateDTO, CategoryRequestDTO>
{
    public CategoryCreateDTO ToModel(CategoryRequestDTO dto)
    {
        if (dto == null)
            return null;
        
        return dto.Adapt<CategoryCreateDTO>();
    }

    public CategoryRequestDTO ToDto(CategoryCreateDTO model)
    {
        if (model == null)
            return null;
        
        return model.Adapt<CategoryRequestDTO>();
    }
}