import { Service } from "./service";

export interface ServiceCategory {
    id: number | null,
    name: string | null,
    description: string | null,
    imageBucket: string | null,
    imageUrl: string | null,
    services: Service[] | null,
    image: string | null
}