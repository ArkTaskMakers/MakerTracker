import { Inject, Injectable, InjectionToken } from '@angular/core';
import { BaseLookupModel } from './lookup-model';

/** Injection token for the lookup models. */
export const LOOKUP_MODELS = new InjectionToken<BaseLookupModel[]>('Lookup Model');

@Injectable({
  providedIn: 'root'
})
export class ModelProviderService {
  models: Map<string, BaseLookupModel> = new Map<string, BaseLookupModel>();

  constructor(@Inject(LOOKUP_MODELS) models: BaseLookupModel[]) {
    models.forEach((model) => this.models.set(model.lookupName.toLocaleLowerCase(), model));
  }
}
