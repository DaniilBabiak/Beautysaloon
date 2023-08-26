import { Reservation } from "./resrvation/reservation";
import { Schedule } from "./schedule";
import { Service } from "./service";

export interface Master {
    id: number | null;
    name: string;
    services: Service[] | null;
    scheduleId: number | null;
    schedule: Schedule | null;
    reservations: Reservation[] | null;
  }