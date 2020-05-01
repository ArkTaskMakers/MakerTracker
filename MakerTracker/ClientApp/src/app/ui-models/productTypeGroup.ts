export interface IProductTypeGroup {
  id: number;
  name: string;
  products: IProductEntry[];
}

export interface IProductEntry {
  id: number;
  name: string;
  imageUrl: string;
}
