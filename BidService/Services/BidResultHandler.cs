using BidService.DTO;
using BidService.Services.Interfaces;
using ConstructionProject.Contracts;

namespace BidService.Services
{
    public class BidResultHandler
    {
        private readonly IBidRepository _bidRepository;
        private readonly ILogger<BidResultHandler> _logger;

        public BidResultHandler(IBidRepository bidRepository, ILogger<BidResultHandler> logger)
        {
            _bidRepository = bidRepository;
            _logger = logger;
        }

        public async Task Consume(BidResultDto message)
        {
            _logger.LogInformation("Получено сообщение о результате для заявки {BidId}", message.BidId);

            var bid = await _bidRepository.GetByIdAsync(message.BidId);

            if (bid is null)
            {
                _logger.LogWarning("Заявка с ID {BidId} не найдена.", message.BidId);
                return;
            }

            bid.Result = message.result;

            await _bidRepository.UpdateAsync(bid);

            _logger.LogInformation("Статус заявки {BidId} обновлен на '{Status}'", message.BidId, message.result);
        }
    }
}
