import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '@env/environment';

@Injectable({ providedIn: 'root' })
export class ProfileMediaService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/profile-media`;

  getMyPhoto(): Observable<Blob> {
    return this.http.get(`${this.baseUrl}/me`, { responseType: 'blob' });
  }

  replaceMyPhoto(file: File): Observable<void> {
    const formData = new FormData();
    formData.append('file', file, file.name);
    return this.http.put<void>(`${this.baseUrl}/me`, formData);
  }

  deleteMyPhoto(): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/me`);
  }

  getCommunityLogo(communityId: string): Observable<Blob> {
    return this.http.get(`${this.baseUrl}/communities/${communityId}`, { responseType: 'blob' });
  }

  replaceCommunityLogo(communityId: string, file: File): Observable<void> {
    const formData = new FormData();
    formData.append('file', file, file.name);
    return this.http.put<void>(`${this.baseUrl}/communities/${communityId}`, formData);
  }

  deleteCommunityLogo(communityId: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/communities/${communityId}`);
  }
}
