namespace Order.Api.Models
{
    public class CargoDetails //note this is a value object, ToDo : define it as a value object on configurations settings
    {
        public string Description { get; set; }
        public double WeightKg { get; set; }
        public string Dimensions { get; set; }
    }
}
