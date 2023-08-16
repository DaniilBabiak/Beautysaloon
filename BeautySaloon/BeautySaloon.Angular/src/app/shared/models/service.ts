import { Reservation } from "./reservation";
import { ServiceCategory } from "./service-category";

export interface Service {
  id: number | null;
  name: string | null;
  price: number | null;
  categoryId: number | null;
  category: ServiceCategory | null;
  startTime: string | null; // You can use string or number (timestamp) depending on your needs
  endTime: string | null;   // You can use string or number (timestamp) depending on your needs
  duration: string | null;  // You can use string or number (in minutes) depending on your needs
  reservations: Reservation[] | null;
}
