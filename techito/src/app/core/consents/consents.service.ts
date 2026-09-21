import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { environment } from '@env/environment';
import { forkJoin } from 'rxjs';
import { map } from 'rxjs/operators';
import { ConsentApi, ConsentPurposeApi, ConsentPurposeItem } from './consents.models';

@Injectable({ providedIn: 'root' })
export class ConsentsService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/consents`;

  getMyConsentPurposes() {
    return forkJoin({
      purposes: this.http.get<ConsentPurposeApi[]>(`${this.baseUrl}/purposes`),
      consents: this.http.get<ConsentApi[]>(`${this.baseUrl}/me`),
    }).pipe(
      map(({ purposes, consents }): ConsentPurposeItem[] => {
        const byCode = new Map((consents ?? []).map((c) => [c.PurposeCode, c]));
        return (purposes ?? []).map((purpose) => {
          const consent = byCode.get(purpose.Code);
          return {
            code: purpose.Code,
            name: purpose.Name,
            description: purpose.Description ?? '',
            status: consent?.Status ?? 'Pendiente',
            granted: consent?.Status === 'Otorgado',
          };
        });
      }),
    );
  }

  grant(purposeCode: string) {
    return this.http.post(`${this.baseUrl}/me/grant`, { PurposeCode: purposeCode });
  }

  withdraw(purposeCode: string) {
    return this.http.post(`${this.baseUrl}/me/withdraw`, { PurposeCode: purposeCode });
  }
}
