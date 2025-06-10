using BidService.Models;
using BidService.DTO;
using BidService.Services.Interfaces;
using Wolverine;

public class BidCreateService
{
    private readonly IBidRepository _bidRepository;
    private readonly IMessageBus _bus;

    public BidCreateService(IBidRepository bidRepository, IMessageBus bus)
    {
        _bidRepository = bidRepository;
        _bus = bus;
    }

    public async Task<BidDto> CreateBidAsync(CreateBidDto dto)
    {
        var bid = new Bid
        {
            userName = dto.userName,
            material = dto.material,
            technique = dto.technique
        };

        await _bidRepository.AddAsync(bid);

        var message = new BidMessageDto
        {
            BidId = bid.Id,
            material = bid.material,
            technique = bid.technique
        };
        await _bus.SendAsync(message);

        return new BidDto
        {
            Id = bid.Id,
            userName = bid.userName,
            material = bid.material,
            technique = bid.technique,
            result = bid.result
        };
    }
}

