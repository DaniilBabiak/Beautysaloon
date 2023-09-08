import { SafeResourceUrl } from "@angular/platform-browser";
import { CategoryModel } from "./category-model";
import { ServiceModel } from "../service/service-model";

export interface CategoryWithImage {
    model: CategoryModel,
    image: string | SafeResourceUrl,
    services: ServiceModel[]
}