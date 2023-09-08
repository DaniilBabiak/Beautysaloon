import { Pipe, PipeTransform } from '@angular/core';
import { BestWorkWithImage } from '../models/bestWork/best-work-with-image';

@Pipe({
    name: 'bestWorkWithImageFilter',
})
export class BestWorkWithImageFilter implements PipeTransform {
    transform(items: BestWorkWithImage[], searchText: string): BestWorkWithImage[] {
        if (!items || searchText === undefined || searchText === '') {
            return items;
        }

        return items.filter((item) => item.model.id.toString() === searchText);
    }
}
