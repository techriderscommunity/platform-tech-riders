export interface DashboardNotification {
  readonly title: string;
  readonly detail: string;
}

export interface QuickAccess {
  readonly label: string;
  readonly description: string;
  readonly route: string;
}

export interface MySpaceItem {
  readonly label: string;
  readonly route: string;
}

export interface ActivitySummaryItem {
  readonly module: string;
  readonly pending: string;
}

export interface RoleHeroContent {
  readonly title: string;
  readonly subtitle: string;
  readonly contextLabel: string;
}

export interface RecentActivityItem {
  readonly label: string;
  readonly detail: string;
  readonly time: string;
}

export interface DashboardModuleCard {
  readonly title: string;
  readonly description: string;
  readonly route: string;
}
