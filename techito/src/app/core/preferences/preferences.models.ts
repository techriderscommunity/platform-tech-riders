export interface PreferenceDimensionValueApi {
  Id: string;
  Code: string;
  Name: string;
  ParentValueId?: string | null;
}

export interface PreferenceDimensionApi {
  Id: string;
  Code: string;
  Name: string;
  Values: PreferenceDimensionValueApi[];
}

export interface PreferenceDimension {
  id: string;
  code: string;
  name: string;
  values: { id: string; code: string; name: string }[];
}
