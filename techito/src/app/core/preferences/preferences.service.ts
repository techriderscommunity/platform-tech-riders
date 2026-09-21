import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { environment } from '@env/environment';
import { map } from 'rxjs/operators';
import { PreferenceDimension, PreferenceDimensionApi } from './preferences.models';

@Injectable({ providedIn: 'root' })
export class PreferencesService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/preferences`;

  getCatalog() {
    return this.http.get<PreferenceDimensionApi[]>(`${this.baseUrl}/catalog`).pipe(
      map((dimensions) => (dimensions ?? []).map(toDimension)),
    );
  }

  getMine() {
    return this.http.get<string[]>(`${this.baseUrl}/me`);
  }

  setMine(dimensionValueIds: string[]) {
    return this.http.put(`${this.baseUrl}/me`, { DimensionValueIds: dimensionValueIds });
  }
}

function toDimension(api: PreferenceDimensionApi): PreferenceDimension {
  return {
    id: api.Id,
    code: api.Code,
    name: api.Name,
    values: (api.Values ?? []).map((v) => ({ id: v.Id, code: v.Code, name: v.Name })),
  };
}
