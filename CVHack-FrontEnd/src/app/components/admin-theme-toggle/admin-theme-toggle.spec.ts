import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminThemeToggle } from './admin-theme-toggle';

describe('AdminThemeToggle', () => {
  let component: AdminThemeToggle;
  let fixture: ComponentFixture<AdminThemeToggle>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdminThemeToggle],
    }).compileComponents();

    fixture = TestBed.createComponent(AdminThemeToggle);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
