using ApiTicketAWS.Models;

namespace ApiTicketAWS.Services
{
    public interface ITicketService
    {
        public Task<GetResponseModel> getRequest(string accessToken);
    }
}
