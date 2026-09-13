import { Routes } from '@angular/router';

export const publicRoutes: Routes = [
  { path: '', loadComponent: () => import('./features/home/home').then(m => m.Home) },
  { path: 'join', loadComponent: () => import('./features/unete/unete').then(m => m.Unete) },
  { path: 'about-us', loadComponent: () => import('./features/about-us/quienes-somos').then(m => m.QuienesSomos) },
  { path: 'community-partners', loadComponent: () => import('./features/comuneras/comuneras').then(m => m.Comuneras) },
  { path: 'community-partners/apply', loadComponent: () => import('./features/comuneras/community-partner-apply').then(m => m.CommunityPartnerApply) },
  { path: 'community-partners/:id', loadComponent: () => import('./features/comuneras/community-partner-detail').then(m => m.CommunityPartnerDetail) },
  { path: 'events', loadComponent: () => import('./features/events/events').then(m => m.Events) },
  { path: 'orienta-tech', loadComponent: () => import('./features/orienta-tech/orienta-tech').then(m => m.OrientaTech) },
  { path: 'knowledge', loadComponent: () => import('./features/knowledge/knowledge').then(m => m.Knowledge) },
  { path: 'knowledge/:slug', loadComponent: () => import('./features/knowledge/knowledge-detail').then(m => m.KnowledgeDetail) },
  { path: 'tutorials', redirectTo: 'knowledge', pathMatch: 'full' },
  { path: 'contact', redirectTo: 'join', pathMatch: 'full' },
  { path: 'login', loadComponent: () => import('./features/login/login-redirect').then(m => m.LoginRedirect) },
  { path: '**', redirectTo: '' },
];
