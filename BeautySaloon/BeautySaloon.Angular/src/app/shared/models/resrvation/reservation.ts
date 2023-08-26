import { Customer } from "../customer";
import { Master } from "../master";
import { Service } from "../service";

export interface Reservation {
  id: number;
  serviceId: number;
  service: Service | null;
  dateTime: Date;
  customerPhoneNumber: string;
  customer: Customer | null;
  masterId: number;
  master: Master | null;
}