using Finances.Core.Contexts.MessageContext.Data;
using Finances.Core.Contexts.MessageContext.Entities;

namespace Finances.Infra.Data;

public class MessageRepository : Repository<Message, FinancesDbContext>, IMessageRepository
{
    public MessageRepository(FinancesDbContext context) : base(context) { }
}

