import { Master } from "../master";
import { Service } from '../service';

export interface AvailableReservation {
    master: Master,
    service: Service,
    availableTime: Date
}