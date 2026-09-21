export interface GpfPersonLinkApi {
  Id: string;
  UserId: string;
  UserName?: string | null;
  UserEmail?: string | null;
  CodUnico: string;
  Status: string;
  LinkedAt: string;
  LastQueriedAt?: string | null;
  ValidatedByUserId?: string | null;
}

export interface GpfPersonLinkItem {
  id: string;
  userId: string;
  userName: string;
  userEmail: string;
  codUnico: string;
  status: string;
  linkedAt: string;
}

export interface PersonOrganizationApi {
  Id: string;
  UserId: string;
  UserName?: string | null;
  OrganizationId: string;
  OrganizationName: string;
  Position?: string | null;
  RelationType: string;
  Status: string;
  IsPrimaryContact: boolean;
  RequestedAt: string;
}

export interface PersonOrganizationItem {
  id: string;
  userName: string;
  organizationName: string;
  relationType: string;
  requestedAt: string;
}
