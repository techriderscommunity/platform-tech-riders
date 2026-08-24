using Mapster;
using TechRiders.Application.DTOs.Responses.Intranet;
using TechRiders.Domain.Entities;

namespace TechRiders.Application.Mappings;

/// <summary>
/// Mapster configuration for Intranet domain (audit logs, settings, user categories)
/// </summary>
public class IntranetMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<IntranetAuditLog, IntranetAuditLogResponse>();
        config.NewConfig<IntranetSetting, IntranetSettingResponse>();
        config.NewConfig<IntranetUserCategory, IntranetUserCategoryResponse>();
    }
}
