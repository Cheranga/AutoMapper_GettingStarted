using Automapper.DataAccess.UnitTests.Dto;
using AutoMapper;
using AutoMapper.DataAccess;

namespace Automapper.DataAccess.UnitTests.Mappers
{
    public class InvoiceResolver : IValueResolver<Invoice, InvoiceDto, decimal>
    {
        public decimal Resolve(Invoice source, InvoiceDto destination, decimal destMember, ResolutionContext context)
        {
            if (source == null || destination == null)
            {
                return 0;
            }

            return source.TotalFromItemsBought + source.TotalTaxes;
        }
    }
}