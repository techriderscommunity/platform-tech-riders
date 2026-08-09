import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { forkJoin, of } from 'rxjs';
import { map, switchMap } from 'rxjs/operators';
import { environment } from '\.\./\.\./\.\./\.\./\.\./\.\./environments/environment';
import { ApplicationApi, Candidato, EmpresaDashboard, Oferta, OfferApi, PagedResult } from '../models/empresa.models';

@Injectable({ providedIn: 'root' })
export class EmpresaService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}`;

  private mapOffer(item: OfferApi): Oferta {
    return {
      id: item.Id,
      titulo: item.Titulo,
      empresa: item.Empresa,
      salario: `${item.Salario}`,
      ubicacion: item.Ubicacion,
      modalidad: this.mapModalidad(item.Modalidad),
      estado: this.mapOfferStatus(item.Estado),
      fechaPublicacion: item.FechaPublicacion,
      candidatos: 0,
    };
  }

  private mapCandidate(item: ApplicationApi): Candidato {
    return {
      id: item.Id,
      ofertaId: item.OfertaId,
      juniorId: item.JuniorId,
      nombreJunior: item.NombreJunior,
      nombre: item.NombreJunior,
      emailJunior: item.EmailJunior,
      puesto: '',
      estado: this.mapApplicationStatus(item.Estado),
      fechaSolicitud: item.FechaSolicitud,
      fecha: item.FechaSolicitud,
    };
  }

  private mapOfferStatus(status: number): string {
    if (status === 1) return 'Activa';
    if (status === 2) return 'Cerrada';
    return 'Borrador';
  }

  private mapApplicationStatus(status: number): string {
    if (status === 1) return 'entrevista';
    if (status === 2) return 'rechazado';
    if (status === 3) return 'oferta';
    return 'pendiente';
  }

  private mapModalidad(modalidad: number): string {
    if (modalidad === 1) return 'Hybrid';
    if (modalidad === 2) return 'On-site';
    return 'Remote';
  }

  getDashboard() {
    return this.getOfertas(1, 6).pipe(
      switchMap((offersResult) => {
        const latestOffers = offersResult.items.slice(0, 3);
        if (latestOffers.length === 0) {
          return of({
            nombreEmpresa: 'TechRiders Company',
            stats: { ofertasActivas: 0, candidatosTotal: 0, enProceso: 0, contratados: 0 },
            ultimasOfertas: [],
            ultimosCandidatos: [],
          } satisfies EmpresaDashboard);
        }

        return forkJoin(latestOffers.map((offer) => this.getCandidatos(offer.id))).pipe(
          map((candidatesByOffer) => {
            const allCandidates = candidatesByOffer.flat();
            return {
              nombreEmpresa: latestOffers[0]?.empresa ?? 'TechRiders Company',
              stats: {
                ofertasActivas: offersResult.items.filter((offer) => offer.estado === 'Activa').length,
                candidatosTotal: allCandidates.length,
                enProceso: allCandidates.filter((candidate) => candidate.estado === 'entrevista').length,
                contratados: allCandidates.filter((candidate) => candidate.estado === 'oferta').length,
              },
              ultimasOfertas: latestOffers,
              ultimosCandidatos: allCandidates.slice(0, 5),
            } satisfies EmpresaDashboard;
          })
        );
      })
    );
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
        const mappedItems = (items ?? []).map((item) => this.mapOffer(item));
        return {
          items: mappedItems,
          totalCount: mappedItems.length,
          page,
          pageSize,
          totalPages: 1,
          hasNextPage: false,
          hasPreviousPage: false,
        } satisfies PagedResult<Oferta>;
      })
    );
  }

  getCandidatos(ofertaId: string) {
    return this.http.get<ApplicationApi[]>(`${this.baseUrl}/applications/offer/${ofertaId}`).pipe(
      map((items) => (items ?? []).map((item) => this.mapCandidate(item)))
    );
  }

  getCandidatosGovernance() {
    return of([] as Candidato[]);
  }

  updateCandidaturaEstado(candidaturaId: string, estado: string) {
    if (estado === 'entrevista') {
      return this.http.post<void>(`${this.baseUrl}/applications/${candidaturaId}/advance`, {});
    }

    if (estado === 'rechazado') {
      return this.http.post<void>(`${this.baseUrl}/applications/${candidaturaId}/reject`, {});
    }

    if (estado === 'oferta') {
      return this.http.post<void>(`${this.baseUrl}/applications/${candidaturaId}/hire`, {});
    }

    return of(void 0);
  }

  createOferta(oferta: { titulo: string; empresa: string; salario: string; ubicacion: string; modalidad: string; }) {
    return this.http.post<string>(`${this.baseUrl}/offers`, oferta);
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



