import { ReservationModel } from "./reservation-model";

export interface GetAllReservationsResponse {
    totalPages: number,
    pageItems: ReservationModel[]
}