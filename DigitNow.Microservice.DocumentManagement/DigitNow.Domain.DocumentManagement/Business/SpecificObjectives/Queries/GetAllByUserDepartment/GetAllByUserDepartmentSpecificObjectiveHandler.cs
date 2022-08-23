using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using HTSS.Platform.Core.CQRS;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.SpecificObjectives.Queries.GetAll
{
    public class GetAllByUserDepartmentSpecificObjectiveHandler
        : IQueryHandler<GetAllByUserDepartmentSpecificObjectiveQuery, List<GetAllByUserDepartmentSpecificObjectiveResponse>>
    {
        private readonly IMapper _mapper;
        private readonly ISpecificObjectiveService _specificObjectiveService;
        private readonly IIdentityService _identityService;

        public GetAllByUserDepartmentSpecificObjectiveHandler(IMapper mapper,
            ISpecificObjectiveService specificObjectiveService,
            IIdentityService identityService)
        {
            _mapper = mapper;
            _specificObjectiveService = specificObjectiveService;
            _identityService = identityService;
        }

        public async Task<List<GetAllByUserDepartmentSpecificObjectiveResponse>> Handle(GetAllByUserDepartmentSpecificObjectiveQuery request, CancellationToken cancellationToken)
        {
            var currentUser = await _identityService.GetCurrentUserAsync(cancellationToken);
            var userDepartments = currentUser.Departments.Select(x => x.Id).ToList();

            var specificObjectives = await _specificObjectiveService.FindQuery()
                .Where(item => item.CreatedBy == currentUser.Id || userDepartments.Contains(item.DepartmentId))
                .Include(item => item.Objective)
                .ToListAsync(cancellationToken);

            if (specificObjectives == null) return null;

            return _mapper.Map<List<GetAllByUserDepartmentSpecificObjectiveResponse>>(specificObjectives);
        }
    }
}
