namespace Order.Api.Models
{
    public class CargoDetails //value object
    {
        public string? Description { get; private set; }
        public double WeightKg { get; private set; }
        public decimal LengthCm { get; private set; }
        public decimal WidthCm { get; private set; }
        public decimal HeightCm { get; private set; }

        public CargoDetails(string? description, double weightKg, decimal lengthCm, decimal widthCm, decimal heightCm)
        {

            if (weightKg <= 0)
                throw new ArgumentOutOfRangeException(nameof(weightKg), "Weight must be positive.");

            if (lengthCm <= 0)
                throw new ArgumentOutOfRangeException(nameof(lengthCm), "Length must be positive.");

            if (widthCm <= 0)
                throw new ArgumentOutOfRangeException(nameof(widthCm), "Width must be positive.");


            if (heightCm <= 0)
                throw new ArgumentOutOfRangeException(nameof(heightCm), "Height must be positive.");


            Description = description;
            WeightKg = weightKg;
            LengthCm = lengthCm;
            WidthCm = widthCm;
            HeightCm = heightCm;


        }
    }
}
