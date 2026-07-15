import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminUsersHeader } from './admin-users-header';

describe('AdminUsersHeader', () => {
  let component: AdminUsersHeader;
  let fixture: ComponentFixture<AdminUsersHeader>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdminUsersHeader],
    }).compileComponents();

    fixture = TestBed.createComponent(AdminUsersHeader);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
