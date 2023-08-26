export interface CreateReservationRequest {
    serviceId: number,
    dateTime: Date,
    masterId: number,
    phoneNumber: string
}