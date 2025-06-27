using ResourceService.DTO;
using ResourceService.Models;
using ResourceService.Services.Interfaces;
using Wolverine;
using ConstructionProject.Contracts;

namespace ResourceService.Services
{
    public class BidMessageHandler
    {
        private readonly IResourceRepository _repository;
        private readonly ILogger<BidMessageHandler> _logger;

        public BidMessageHandler(IResourceRepository repository, ILogger<BidMessageHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task Handle(BidMessageDto message, IMessageBus bus)
        {
            _logger.LogInformation("--> Получена заявка {BidId} на проверку ресурсов...", message.BidId);

            var unavailableReasons = new List<string>();
            var reservedTechniques = new List<Technique>();
            var reservedMaterials = new List<(Material material, decimal quantity)>();

            foreach (var techName in message.RequiredTechniques)
            {
                var technique = await _repository.GetTechniqueByNameAsync(techName);
                if (technique == null)
                {
                    unavailableReasons.Add($"Техника '{techName}' не найдена.");
                }
                else if (!technique.IsAvailable)
                {
                    unavailableReasons.Add($"Техника '{techName}' занята.");
                }
                else
                {
                    reservedTechniques.Add(technique);
                }
            }

            foreach (var item in message.RequiredMaterials)
            {
                var material = await _repository.GetMaterialByNameAsync(item.Name);
                if (material == null)
                {
                    unavailableReasons.Add($"Материал '{item.Name}' не найден.");
                }
                else if (material.Quantity < (double)item.Quantity)
                {
                    unavailableReasons.Add($"Недостаточно материала '{item.Name}'. Требуется: {item.Quantity}, в наличии: {material.Quantity}.");
                }
                else
                {
                    reservedMaterials.Add((material, item.Quantity));
                }
            }

            string resultMessage;

            if (unavailableReasons.Any())
            {
                resultMessage = "Отклонено: " + string.Join(" ", unavailableReasons);
                _logger.LogWarning("--> Заявка {BidId} отклонена. Причина: {Reason}", message.BidId, resultMessage);
            }
            else
            {
                _logger.LogInformation("--> Все ресурсы для заявки {BidId} доступны. Начинаем резервирование...", message.BidId);

                foreach (var technique in reservedTechniques)
                {
                    technique.IsAvailable = false;
                    await _repository.UpdateTechniqueAsync(technique);
                }

                foreach (var (material, quantity) in reservedMaterials)
                {
                    material.Quantity -= (double)quantity;
                    await _repository.UpdateMaterialAsync(material);
                }

                resultMessage = "Одобрено: Все ресурсы успешно зарезервированы.";
                _logger.LogInformation("--> Заявка {BidId} одобрена.", message.BidId);
            }

            var response = new BidResultDto
            {
                BidId = message.BidId,
                result = resultMessage
            };

            await bus.PublishAsync(response);
            _logger.LogInformation("--> Результат по заявке {BidId} отправлен.", message.BidId);
        }
    }
}