import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminSidebarExpandIcon } from './admin-sidebar-expand-icon';

describe('AdminSidebarExpandIcon', () => {
  let component: AdminSidebarExpandIcon;
  let fixture: ComponentFixture<AdminSidebarExpandIcon>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdminSidebarExpandIcon],
    }).compileComponents();

    fixture = TestBed.createComponent(AdminSidebarExpandIcon);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
