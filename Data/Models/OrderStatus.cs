namespace reymani_web_api.Data.Models;

public enum OrderStatus : short
{
  InProcess,
  InPreparation,
  InPickup,
  OnTheWay,
  Delivered,
  Completed,
  Cancelled
}