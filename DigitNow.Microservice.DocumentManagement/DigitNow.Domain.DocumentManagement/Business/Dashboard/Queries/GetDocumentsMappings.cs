using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.Dashboard.Queries;

public class GetDocumentsMappings : Profile
{
    public GetDocumentsMappings()
    {
        CreateMap<IncomingDocument, DashboardDocumentViewModel>();
        CreateMap<OutgoingDocument, DashboardDocumentViewModel>();
        CreateMap<InternalDocument, DashboardDocumentViewModel>();
    }
}
