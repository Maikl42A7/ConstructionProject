using BidService.DTO;
using BidService.Models;
using BidService.Services.Interfaces;
using Wolverine;
using ConstructionProject.Contracts;

namespace BidService.Services
{
    public class BidService
    {
        private readonly IBidRepository _bidRepository;
        private readonly IMessageBus _messageBus;

        public BidService(IBidRepository bidRepository, IMessageBus messageBus)
        {
            _bidRepository = bidRepository;
            _messageBus = messageBus;
        }

        public async Task<List<BidDto>> GetAllBidsAsync()
        {
            var bids = await _bidRepository.GetAllAsync();

            return bids.Select(bid => new BidDto
            {
                Id = bid.Id,
                userName = bid.UserName,
                material = string.Join(", ", bid.RequiredMaterials.Select(m => $"{m.Name} ({m.Quantity})")),
                technique = string.Join(", ", bid.RequiredTechniques.Select(t => t.Name)),
                result = bid.Result
            }).ToList();
        }

        public async Task<BidDto?> GetBidByIdAsync(Guid id)
        {
            var bid = await _bidRepository.GetByIdAsync(id);

            if (bid == null)
            {
                return null;
            }

            return new BidDto
            {
                Id = bid.Id,
                userName = bid.UserName,
                material = string.Join(", ", bid.RequiredMaterials.Select(m => $"{m.Name} ({m.Quantity})")),
                technique = string.Join(", ", bid.RequiredTechniques.Select(t => t.Name)),
                result = bid.Result
            };
        }

        public async Task<Bid> CreateBidAsync(CreateBidDto dto)
        {
            var bid = new Bid
            {
                UserName = dto.UserName,
                RequiredMaterials = dto.Materials.Select(m => new RequiredMaterial
                {
                    Name = m.Name,
                    Quantity = m.Quantity
                }).ToList(),
                RequiredTechniques = dto.Techniques.Select(t => new RequiredTechnique
                {
                    Name = t
                }).ToList(),
                Result = "В обработке"
            };

            await _bidRepository.AddAsync(bid);

            var message = new BidMessageDto
            {
                BidId = bid.Id,
                RequiredMaterials = dto.Materials.ToList(),
                RequiredTechniques = dto.Techniques.ToList()
            };

            await _messageBus.PublishAsync(message);

            return bid;
        }

        public async Task ClearAllBidsAsync()
        {
            await _bidRepository.ClearAllAsync();
        }
    }
}
