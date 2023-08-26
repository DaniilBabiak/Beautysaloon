import { Master } from "./master";
import { Reservation } from "./resrvation/reservation";
import { ServiceCategory } from "./service-category";

export interface Service {
  id: number | null;
  name: string | null;
  price: number | null;
  categoryId: number | null;
  category: ServiceCategory | null;
  duration: string | null;  // You can use string or number (in minutes) depending on your needs
  reservations: Reservation[] | null;
  masters: Master[] | null;
}
