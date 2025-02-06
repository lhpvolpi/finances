using Finances.Core.Contexts.SharedContext.Entities;

namespace Finances.Core.Contexts.ConsolidationContext.UseCases.Get;

public class GetConsolidationResponse : Response
{
    public const string DefaultMessageSuccess = "Consolidation successfully created";

    public GetConsolidationResponse(int statusCode, bool success, object data, string message) : base(statusCode, success, data, message) { }

    public static GetConsolidationResponse CreateSuccess(int statusCode, object data = null, string message = DefaultMessageSuccess) => new(statusCode, true, data, message);
}