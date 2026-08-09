import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { environment } from '@env/environment';

@Injectable({ providedIn: 'root' })
export class IntranetHomeService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = environment.apiUrl;

  getMyCategories(userKey: string) {
    return this.http.get<string[]>(`${this.baseUrl}/intranet/mis-categorias`, {
      params: { userKey },
    });
  }

  saveMyCategories(userKey: string, categories: string[]) {
    return this.http.put(`${this.baseUrl}/intranet/mis-categorias`, {
      userKey,
      categories,
    });
  }

  emitLandingTrace() {
    return this.http.post(`${this.baseUrl}/intranet/trazas`, {
      kind: 'landing',
      route: '/intranet',
      detail: 'home_loaded',
    });
  }
}
