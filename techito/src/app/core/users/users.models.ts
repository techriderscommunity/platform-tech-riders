export interface UserListItemApi {
  Id: string;
  Nickname: string;
  Name: string;
  LastName: string;
  Email: string;
  Roles: string[];
  MembershipStatus?: string | null;
  IsWorking: boolean;
  LastActivityDate?: string | null;
}

export interface UserListResponseApi {
  Items: UserListItemApi[];
  TotalCount: number;
  Page: number;
  PageSize: number;
}

export interface UserOrganizationSummaryApi {
  OrganizationId: string;
  OrganizationName: string;
  RelationType: string;
  Status: string;
}

export interface UserSkillSummaryApi {
  SkillId: string;
  SkillName: string;
  Level: string;
}

export interface UserDetailApi {
  Id: string;
  Nickname: string;
  Name: string;
  LastName: string;
  Email: string;
  Phone?: string | null;
  Locality?: string | null;
  About?: string | null;
  Roles: string[];
  MembershipStatus?: string | null;
  CurrentProfile?: string | null;
  ActiveCapabilities: string[];
  Organizations: UserOrganizationSummaryApi[];
  Skills: UserSkillSummaryApi[];
  CreatedAt: string;
}

export interface UserActivityApi {
  EventsRegistered: number;
  SessionsRegistered: number;
  SpeakerSessions: number;
  FPToursAsAmbassador: number;
  RecentActions: { CreatedUtc: string; Module: string; Action: string; Result: string; Detail?: string | null }[];
}

export interface UserListItem {
  id: string;
  nickname: string;
  name: string;
  lastName: string;
  email: string;
  roles: string[];
  membershipStatus: string;
  isWorking: boolean;
  lastActivityDate: string | null;
}

export interface CreateUserPayload {
  nickname: string;
  name: string;
  lastName: string;
  email: string;
  phone?: string;
  locality?: string;
  about?: string;
}

export interface UpdateUserPayload {
  name: string;
  lastName: string;
  email: string;
  phone?: string;
  locality?: string;
  about?: string;
}
