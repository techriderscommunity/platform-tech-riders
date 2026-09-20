export interface ApprovalItemApi {
  Id: string;
  Type: string;
  Title: string;
  RequestedBy?: string | null;
  RequestedAt: string;
  Module: string;
}

export interface ApprovalItem {
  id: string;
  type: string;
  title: string;
  requestedBy: string;
  requestedAt: string;
  module: string;
}

export const APPROVAL_TYPE_LABELS: Record<string, string> = {
  CapabilityRequest: 'Solicitud de rol',
  OrganizationRelation: 'Relación con organización',
  CommunityPartnerApplication: 'Solicitud de comunera',
  OrganizationRequest: 'Solicitud de centro',
};
