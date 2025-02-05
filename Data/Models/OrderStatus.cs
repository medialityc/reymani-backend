namespace reymani_web_api.Data.Models;

public enum OrderStatus : short
{
  InProcess,
  Approved,
  InPreparation,
  InPickup,
  OnTheWay,
  Delivered,
  Completed,
  Cancelled
}