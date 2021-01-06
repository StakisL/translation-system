using System;

namespace TranslateSystem.Data
{
    //todo Extension to many types of operations.
    public class Operation
    {
        public Guid Id { get; set; }
        public int SourceUserId { get; set; }
        public int DestinationUserId { get; set; }
        public double AmountTransaction { get; set; }
    }
}
