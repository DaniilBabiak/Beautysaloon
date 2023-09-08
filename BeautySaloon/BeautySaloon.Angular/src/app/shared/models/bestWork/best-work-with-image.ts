import { SafeResourceUrl } from '@angular/platform-browser';
import { BestWorkModel } from './best-work-model';
export interface BestWorkWithImage {
    model: BestWorkModel,
    image: string | SafeResourceUrl
}