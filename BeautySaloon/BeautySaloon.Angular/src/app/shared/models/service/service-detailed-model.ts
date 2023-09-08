export interface ServiceDetailedModel {
    id: number,
    name: string,
    price: number,
    categoryId: number,
    duration: string,
    reservationIds: number[],
    masterIds: number[]
}