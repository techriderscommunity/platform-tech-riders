import { Injectable, inject, signal } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { environment } from '\.\./\.\./\.\./\.\./\.\./\.\./environments/environment';
import { Curso, Evento, OfertaJunior, PagedResult } from '../models/junior.models';

interface OfferApi {
  Id: string;
  Titulo: string;
  Empresa: string;
  Salario: number;
  Ubicacion: string;
  Modalidad: number;
  Estado: number;
  FechaPublicacion: string;
}

@Injectable({ providedIn: 'root' })
export class JuniorService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}`;

  readonly loading = signal(false);
  readonly error = signal<string | null>(null);

  readonly cursos = signal<Curso[]>([]);
  readonly eventos = signal<Evento[]>([]);

  private mapOfferStatus(status: number): string {
    if (status === 1) return 'Activa';
    if (status === 2) return 'Cerrada';
    return 'Borrador';
  }

  private mapModalidad(modalidad: number): string {
    if (modalidad === 1) return 'Hybrid';
    if (modalidad === 2) return 'On-site';
    return 'Remote';
  }

  getDashboard() {
    return this.getOfertas(1, 6);
  }

  getOfertas(page = 1, pageSize = 50, estado?: string, modalidad?: string) {
    let params = new HttpParams()
      .set('page', String(page))
      .set('pageSize', String(pageSize));

    if (estado) {
      params = params.set('estado', estado);
    }

    if (modalidad) {
      params = params.set('modalidad', modalidad);
    }

    return this.http.get<OfferApi[]>(`${this.baseUrl}/offers`, { params }).pipe(
      map((items) => {
        const mappedItems = (items ?? []).map((item): OfertaJunior => ({
          id: item.Id,
          titulo: item.Titulo,
          empresa: item.Empresa,
          salario: `${item.Salario}`,
          ubicacion: item.Ubicacion,
          modalidad: this.mapModalidad(item.Modalidad),
          estado: this.mapOfferStatus(item.Estado),
          fechaPublicacion: item.FechaPublicacion,
        }));

        return {
          items: mappedItems,
          totalCount: mappedItems.length,
          page,
          pageSize,
          totalPages: 1,
          hasNextPage: false,
          hasPreviousPage: false,
        } satisfies PagedResult<OfertaJunior>;
      })
    );
  }

  enviarSolicitud(ofertaId: string, juniorId: string, nombreJunior: string, emailJunior: string) {
    return this.http.post(`${this.baseUrl}/applications`, {
      ofertaId,
      juniorId,
      nombreJunior,
      emailJunior,
      cartaPresentacion: null,
    });
  }
}



