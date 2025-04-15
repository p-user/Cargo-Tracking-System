namespace Order.Api.Models
{
    public class CargoDetails //value object
    {
        public string? Description { get; private set; }
        public double WeightKg { get; private set; }
        public string Dimensions { get; private set; }

        public static CargoDetails Create(string? description, double weightKg, string dimensions)
        {

            if (weightKg <= 0)
                throw new ArgumentOutOfRangeException(nameof(weightKg), "Weight must be positive.");

            if (string.IsNullOrWhiteSpace(dimensions))
                throw new ArgumentException("Dimensions are required.");

            return new CargoDetails
            {
                Description = description,
                WeightKg = weightKg,
                Dimensions = dimensions
            };
        }
    }
}
