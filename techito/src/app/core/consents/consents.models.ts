export interface ConsentPurposeApi {
  Id: string;
  Code: string;
  Name: string;
  Description?: string | null;
}

export interface ConsentApi {
  PurposeId: string;
  PurposeCode: string;
  PurposeName: string;
  Status: string;
  GrantedAt?: string | null;
  WithdrawnAt?: string | null;
}

export interface ConsentPurposeItem {
  code: string;
  name: string;
  description: string;
  status: string;
  granted: boolean;
}
