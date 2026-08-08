namespace Reminders.Application.Common.Interfaces;

public interface IDtoConverter<TModel, TDto>
{
    TModel ToModel(TDto dto);
    TDto ToDto(TModel model);
}