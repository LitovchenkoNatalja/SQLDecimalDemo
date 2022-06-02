namespace DecimalDemoProject.Services
{
    public class DecimalValuesRequest
    {
        public decimal? Number1 { get; set; }
        public byte? Precision1 { get; set; } = 24;
        public byte? Scale1 { get; set; } = 10;
        public decimal? Number2 { get; set; }
        public byte? Precision2 { get; set; } = 24;
        public byte? Scale2 { get; set; } = 10;
    }
}
