import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminUserDetailPanel } from './admin-user-detail-panel';

describe('AdminUserDetailPanel', () => {
  let component: AdminUserDetailPanel;
  let fixture: ComponentFixture<AdminUserDetailPanel>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdminUserDetailPanel],
    }).compileComponents();

    fixture = TestBed.createComponent(AdminUserDetailPanel);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
