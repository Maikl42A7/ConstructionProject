using BidService.DTO;
using BidService.Services.Interfaces;
using Wolverine;

public class BidResultService
{
    private readonly IBidRepository _bidRepository;

    public BidResultService(IBidRepository bidRepository)
    {
        _bidRepository = bidRepository;
    }

    public async Task Handle(BidResultDto message)
    {
        var bid = await _bidRepository.GetByIdAsync(message.BidId);
        if (bid != null)
        {
            bid.result = message.result;
            await _bidRepository.UpdateAsync(bid);
        }
    }
}

