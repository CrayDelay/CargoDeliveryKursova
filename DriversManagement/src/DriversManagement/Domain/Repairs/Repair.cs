namespace DriversManagement.Domain.Repairs;

using System.ComponentModel.DataAnnotations;
using DriversManagement.Domain.Trucks;
using System.ComponentModel.DataAnnotations.Schema;
using Destructurama.Attributed;
using DriversManagement.Exceptions;
using DriversManagement.Domain.Repairs.Models;
using DriversManagement.Domain.Repairs.DomainEvents;
using DriversManagement.Domain.Trucks;
using DriversManagement.Domain.Trucks.Models;


public class Repair : BaseEntity
{
    public string RepairDate { get; private set; }

    public string Description { get; private set; }

    public string Cost { get; private set; }

    public Truck Truck { get; private set; }

    // Add Props Marker -- Deleting this comment will cause the add props utility to be incomplete


    public static Repair Create(RepairForCreation repairForCreation)
    {
        var newRepair = new Repair();

        newRepair.RepairDate = repairForCreation.RepairDate;
        newRepair.Description = repairForCreation.Description;
        newRepair.Cost = repairForCreation.Cost;

        newRepair.QueueDomainEvent(new RepairCreated(){ Repair = newRepair });
        
        return newRepair;
    }

    public Repair Update(RepairForUpdate repairForUpdate)
    {
        RepairDate = repairForUpdate.RepairDate;
        Description = repairForUpdate.Description;
        Cost = repairForUpdate.Cost;

        QueueDomainEvent(new RepairUpdated(){ Id = Id });
        return this;
    }

    public Repair SetTruck(Truck truck)
    {
        Truck = truck;
        return this;
    }

    // Add Prop Methods Marker -- Deleting this comment will cause the add props utility to be incomplete
    
    protected Repair() { } // For EF + Mocking
}
