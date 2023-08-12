import { ServiceCategory } from "./service-category";

export interface Service {
    id: number | null,
    name: string | null,
    price: number | null,
    categoryId: number | null,
    category: ServiceCategory | null
}