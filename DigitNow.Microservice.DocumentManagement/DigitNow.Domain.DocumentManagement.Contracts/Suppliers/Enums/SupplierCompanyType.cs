using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Contracts.Suppliers.Enums
{
    public enum SupplierCompanyType
    {
        LLC = 1, //SRL (Limited Liability Company) 
        SA = 2, // Public limited company (SA)
        GP = 3, //General partnership - Societate in nume colectiv (SNC)
        LP = 4, // Limited partnership- Societate in comandita simpla (SCS)
        LJSPS = 5, //Limited joint-stock partnership Societate in comandita pe actiuni (SCA)
        SP = 6,//Sole proprietorship (PFA) 
        IC = 7, //Individual company (II)
        FA = 8, //Family Association (AF)
        Other = 9

    }
}
