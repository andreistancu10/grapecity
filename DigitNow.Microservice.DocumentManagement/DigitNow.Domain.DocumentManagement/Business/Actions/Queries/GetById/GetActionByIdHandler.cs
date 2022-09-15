using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using HTSS.Platform.Core.CQRS;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.Actions.Queries.GetById
{
    public class GetActionByIdHandler : IQueryHandler<GetActionByIdQuery, GetActionByIdResponse>
    {
        private readonly IMapper _mapper;
        private readonly IActionService _actionService;

        public GetActionByIdHandler(
            IMapper mapper,
            IActionService actionService)
        {
            _mapper = mapper;
            _actionService = actionService;
        }

        public async Task<GetActionByIdResponse> Handle(GetActionByIdQuery request, CancellationToken cancellationToken)
        {
            var action = await _actionService.FindQuery()
                .Include(c => c.AssociatedActivity)
                .AsNoTracking()
                .Include(x => x.ActionFunctionaries)
                .FirstOrDefaultAsync(item => item.Id == request.Id, cancellationToken);

            if (action == null)
            {
                return null;
            }

            return _mapper.Map<GetActionByIdResponse>(action);
        }
    }
}
