using DAMWeb.Shared.Models;
using MediatR;

namespace DAMWeb.Shared.Requests
{
    public class ForecastForDaysRequest : IRequest<ApiResponse>
    {
        public int Days { get; }

        public ForecastForDaysRequest(int days)
        {
            Days = days;
        }
    }
}
