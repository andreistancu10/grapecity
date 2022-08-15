using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Contracts.Objectives;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities.Objectives;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Core.Errors;

namespace DigitNow.Domain.DocumentManagement.Business.GeneralObjectives.Commands.Create
{
    public class CreateGeneralObjectiveHandler : ICommandHandler<CreateGeneralObjectiveCommand, ResultObject>
    {
        private readonly DocumentManagementDbContext _dbContext;
        private readonly IObjectiveService _objectiveService;

        public CreateGeneralObjectiveHandler(DocumentManagementDbContext dbContext, IObjectiveService objectiveService)
        {
            _dbContext = dbContext;
            _objectiveService = objectiveService;
        }
        public async Task<ResultObject> Handle(CreateGeneralObjectiveCommand request, CancellationToken cancellationToken)
        {

            var generalObjective = new GeneralObjective{};
           
            try
            {
                generalObjective.Objective = new Objective
                {
                    State = ObjectiveState.Active,
                    Title = request.Title,
                    Details = request.Details
                };

                await _objectiveService.AddAsync(generalObjective.Objective, cancellationToken);
                await _dbContext.AddAsync(generalObjective, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);

            } catch (Exception ex)
            {
                return ResultObject.Error(new ErrorMessage
                {
                    Message = ex.InnerException?.Message
                });
            }

            return ResultObject.Created(generalObjective.ObjectiveId);
        }

    }
}
