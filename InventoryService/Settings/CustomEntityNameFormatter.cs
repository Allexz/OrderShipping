using MassTransit;

namespace InventoryService.Settings;

public class CustomEntityNameFormatter : IEntityNameFormatter
{
    public string FormatEntityName<T>()
    {
        // Retorna apenas o nome simples da classe (sem namespace)
        return typeof(T).Name;
    }
}
