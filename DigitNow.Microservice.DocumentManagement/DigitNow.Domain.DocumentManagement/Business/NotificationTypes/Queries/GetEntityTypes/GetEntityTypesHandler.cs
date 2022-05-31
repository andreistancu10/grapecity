using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.NotificationTypes.Helpers;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.NotificationTypes.Queries.GetEntityTypes
{
    internal sealed class GetEntityTypesHandler : IQueryHandler<GetEntityTypesQuery, IList<GetEntityTypesResponse>>
    {
        private readonly IMapper _mapper;

        public GetEntityTypesHandler(IMapper mapper)
        {
            _mapper = mapper;
        }

        public Task<IList<GetEntityTypesResponse>> Handle(GetEntityTypesQuery request, CancellationToken cancellationToken)
        {
            var entityTypes = NotificationEntityTypeHelper.GetNotificationEntityTypes();
            return Task.FromResult(_mapper.Map<IList<GetEntityTypesResponse>>(entityTypes));
        }
    }
}
