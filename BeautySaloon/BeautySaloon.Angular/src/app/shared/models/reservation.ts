import { Customer } from "./customer";
import { Service } from "./service";

export interface Reservation {
  Id: number;
  ServiceId: number;
  Service: Service | null;
  DateTime: string; // You can use string or Date depending on your needs
  CustomerId: string;
  Customer: Customer;
}
