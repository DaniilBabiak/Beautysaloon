export interface CategoryModel {
    id: number,
    name: string,
    description: string,
    imageBucket: string,
    imageFileName: string,
    serviceIds: number[]
}