export interface MasterDetailedModel {
    id: number,
    scheduleId: number,
    name: string,
    serviceIds: number[],
    reservationIds: number[]
}