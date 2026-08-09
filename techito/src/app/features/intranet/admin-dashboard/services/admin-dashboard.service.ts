import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { environment } from '@env/environment';

export interface AdminDashboardApiResponse {
  Stats: {
    TotalUsers: number;
    ActiveUsers: number;
    SuperAdmins: number;
    Events: number;
    Sessions: number;
    Ambassadors: number;
    JobOffers: number;
    Applications: number;
  };
  RecentActions: Array<{
    Action: string;
    Detail: string;
    CreatedUtc: string;
  }>;
  SystemHealth: {
    Servers: string;
    Database: string;
    Uploads: string;
    Cpu: string;
  };
}

@Injectable({ providedIn: 'root' })
export class AdminDashboardService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/admin/dashboard`;

  getDashboard() {
    return this.http.get<AdminDashboardApiResponse>(this.baseUrl);
  }
}
