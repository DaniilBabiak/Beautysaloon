import { Reservation } from "./reservation";

export interface GetAllReservationsResponse {
    totalPages: number,
    pageItems: Reservation[]
}