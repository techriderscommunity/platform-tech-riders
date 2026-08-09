export interface IntranetAuditRecord {
  readonly id: string;
  readonly createdUtc: string;
  readonly actorUserId?: string | null;
  readonly actorEmail?: string | null;
  readonly module: string;
  readonly action: string;
  readonly result: string;
  readonly detail?: string | null;
}

export interface IntranetSettingRecord {
  readonly id: string;
  readonly key: string;
  readonly module: string;
  readonly value: string;
  readonly status: string;
  readonly updatedUtc: string;
  readonly updatedBy?: string | null;
}

export interface UpdateIntranetSettingPayload {
  readonly key: string;
  readonly module: string;
  readonly value: string;
  readonly status: 'activo' | 'revision';
}

export interface AuditRow {
  readonly fecha: string;
  readonly usuario: string;
  readonly modulo: string;
  readonly accion: string;
  readonly resultado: 'ok' | 'warning';
}

export interface ConfigItem {
  readonly key: string;
  readonly modulo: string;
  readonly valor: string;
  readonly estado: 'activo' | 'revision';
}
