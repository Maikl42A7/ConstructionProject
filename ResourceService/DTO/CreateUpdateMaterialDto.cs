namespace ResourceService.DTO
{
    public class CreateUpdateMaterialDto
    {
        public string Name { get; set; } = string.Empty;
        public string Unit { get; set; } = string.Empty;
        public double Quantity { get; set; }
    }
}
