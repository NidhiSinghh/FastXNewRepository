﻿namespace FastX.Models.DTOs
{
    public class RefundDTO
    {
        public string? BusName { get; set; }
        //public string? BusType { get; set; }
        public int NumberOfSeats { get; set; }
        public DateTime BookedForWhichDate { get; set; }
        //public string? Origin { get; set; }
        //public string? Destination { get; set; }
        public string SeatNumbers { get; set; }
        public int BookingId { get; set; }
        public float TotalCost { get; set; }
        public string UserName { get; set; }
        public int UserId { get; set; }
    }
}
