import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { environment } from '@env/environment';
import { EventApiDto } from '../models/public-events-api.models';
import {
  PublicEvent,
  PublicEventModality,
  PublicEventTopic,
  PublicEventType,
} from '../models/public-event.model';

@Injectable({ providedIn: 'root' })
export class PublicEventsService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/events/next`;

  getUpcomingEvents(): Observable<PublicEvent[]> {
    return this.http.get<EventApiDto[]>(this.baseUrl).pipe(
      map((events) => (events ?? []).map((event) => this.mapToPublicEvent(event)))
    );
  }

  private mapToPublicEvent(event: EventApiDto): PublicEvent {
    const title = this.getString(event.Name ?? event.name, 'Evento sin titulo');
    const summary = this.getString(event.Description ?? event.description, 'Sin descripcion disponible.');
    const place = this.getString(event.Location ?? event.location, 'Por confirmar');
    const startDate = this.getString(event.StartDate ?? event.startDate, '');
    const combinedText = `${title} ${summary} ${place}`.toLowerCase();

    return {
      title,
      summary,
      type: this.inferType(combinedText),
      modality: this.inferModality(combinedText),
      topic: this.inferTopic(combinedText),
      date: this.toIsoDate(startDate),
      place,
      url: '/events',
    };
  }

  private toIsoDate(dateText: string): string {
    if (!dateText) {
      return '';
    }

    return dateText.substring(0, 10);
  }

  private inferType(text: string): PublicEventType {
    if (text.includes('podcast')) return 'Podcast';
    if (text.includes('workshop') || text.includes('taller')) return 'Workshop';
    if (text.includes('woman tech') || text.includes('women tech')) return 'Woman Tech';
    if (text.includes('empleo') || text.includes('cv') || text.includes('entrevista')) return 'Empleabilidad';
    if (text.includes('orienta') || text.includes('itinerario') || text.includes('carrera')) return 'Orientacion';
    return 'Sesion tecnica';
  }

  private inferModality(text: string): PublicEventModality {
    if (text.includes('streaming') || text.includes('online') || text.includes('remoto')) return 'Online';
    if (text.includes('hibrid') || text.includes('híbrido')) return 'Hibrido';
    return 'Presencial';
  }

  private inferTopic(text: string): PublicEventTopic {
    if (text.includes('azure')) return 'Azure';
    if (text.includes('.net') || text.includes('dotnet') || text.includes('api')) return '.NET';
    if (text.includes('dato') || text.includes('data')) return 'Datos';
    if (text.includes('ciber') || text.includes('owasp') || text.includes('seguridad')) return 'Ciberseguridad';
    if (text.includes('soft skill') || text.includes('comunicacion') || text.includes('feedback')) return 'Soft Skills';
    if (text.includes('empleo') || text.includes('career') || text.includes('carrera')) return 'Carrera';
    return 'Comunidad';
  }

  private getString(value: string | null | undefined, fallback: string): string {
    const text = (value ?? '').trim();
    return text.length > 0 ? text : fallback;
  }
}
