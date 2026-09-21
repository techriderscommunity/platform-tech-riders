import type { AppRole } from '@core/auth/auth.service';
import { resolveIntranetWorkspace } from './intranet-nav.config';

describe('resolveIntranetWorkspace', () => {
  it('should select the administrator workspace for an administrator with other roles', () => {
    const workspace = resolveIntranetWorkspace(['ambassador', 'admin']);

    expect(workspace.role).toBe('admin');
    expect(workspace.label).toBe('Administrador');
    expect(workspace.homeRoute).toBe('/intranet');
  });

  it('should select the staff workspace over the member workspace', () => {
    const workspace = resolveIntranetWorkspace(['member', 'staff']);

    expect(workspace.role).toBe('staff');
    expect(workspace.homeRoute).toBe('/intranet/staff');
  });

  it('should provide an isolated workspace for every supported role', () => {
    const expectations: ReadonlyArray<readonly [AppRole, string]> = [
      ['admin', 'Administrador'],
      ['staff', 'Staff'],
      ['community-leader', 'Responsable de comunidad'],
      ['ambassador', 'Embajador'],
      ['center', 'Centro educativo'],
      ['community-partner', 'Entidad colaboradora'],
      ['member', 'Miembro'],
    ];

    for (const [role, label] of expectations) {
      const workspace = resolveIntranetWorkspace([role]);

      expect(workspace.role).toBe(role);
      expect(workspace.label).toBe(label);
    }
  });

  it('should not expose administrative modules in the ambassador workspace', () => {
    const workspace = resolveIntranetWorkspace(['ambassador']);
    const routes = workspace.sections.flatMap(section => section.items.map(item => item.route));

    expect(routes).not.toContain('/intranet/administration/user-roles');
    expect(routes).not.toContain('/intranet/staff');
  });

  it('should provide dedicated internal entry routes for center and community partner', () => {
    expect(resolveIntranetWorkspace(['center']).homeRoute).toBe('/intranet/center');
    expect(resolveIntranetWorkspace(['community-partner']).homeRoute).toBe('/intranet/community-partner');
  });
});
