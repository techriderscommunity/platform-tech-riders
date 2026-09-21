export interface CapabilityRequestApi {
  Id: string;
  UserId: string;
  UserName?: string | null;
  UserEmail?: string | null;
  CapabilityName: string;
  Status: string;
  RequestedAt: string;
  ValidatedAt?: string | null;
  ValidatedByUserId?: string | null;
}

export interface CapabilityRequestItem {
  id: string;
  userId: string;
  userName: string;
  userEmail: string;
  capabilityName: string;
  status: string;
  requestedAt: string;
}

export const REQUESTABLE_CAPABILITIES = [
  { role: 'staff', label: 'Staff', capabilityName: 'Staff' },
  { role: 'community-leader', label: 'Community Leader', capabilityName: 'Community Leader' },
  { role: 'ambassador', label: 'Ambassador', capabilityName: 'Ambassador' },
  { role: 'center', label: 'Center', capabilityName: 'Center' },
  { role: 'community-partner', label: 'Community Partner', capabilityName: 'Community Partner' },
] as const;
