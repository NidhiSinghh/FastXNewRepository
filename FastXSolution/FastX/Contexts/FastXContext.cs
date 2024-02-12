﻿using FastX.Models;
using Microsoft.EntityFrameworkCore;

namespace FastX.Contexts
{
    public class FastXContext : DbContext
    {
        public FastXContext(DbContextOptions<FastXContext> options) : base(options)
        {
        }
        
        public DbSet<User> Users { get; set; }
        public DbSet<BusOperator> BusOperators { get; set; }
        public DbSet<Bus> Buses { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Amenity> Amenities { get; set; }
        public DbSet<BusAmenity> BusAmenities { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Routee> Routees { get; set; }
        public DbSet<RouteStop> RouteStops { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Stop> Stops { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<AllUser> AllUsers { get; set; }
        public DbSet<BusRoute> BusRoute { get; set; }

       

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Seat>()
            .HasKey(seat => new { seat.SeatId, seat.BusId });


            modelBuilder.Entity<Ticket>()
    .HasOne(ticket => ticket.Seat)
    .WithMany()
    .HasForeignKey(ticket => new { ticket.SeatId, ticket.BusId })
    .OnDelete(DeleteBehavior.NoAction); // Remove cascade delete

          

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.User)
                .WithMany(u => u!.Bookings)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            base.OnModelCreating(modelBuilder);
        }


    }
}
